using AIAssistantService.Domain.DTO;
using AIAssistantService.Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Text.Json;

namespace AIAssistantService.Infrastructure.Services;

public class RedisDialogHistoryService : IDialogHistoryService, IDisposable
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _database;
    private const string KeyPrefix = "dialog:survey:";

    public RedisDialogHistoryService(IConfiguration configuration)
    {
        var connectionString = configuration["Redis:ConnectionString"];
        _redis = ConnectionMultiplexer.Connect(connectionString!);
        _database = _redis.GetDatabase();
    }

    public void Dispose()
    {
        _redis?.Dispose();
    }

    public async Task SaveMessagesAsync(string surveyId, IEnumerable<DialogMessageDto> messages, CancellationToken cancellationToken = default)
    {
        var key = GetKey(surveyId);
        var values = messages.Select(m => (RedisValue)JsonSerializer.Serialize(m)).ToArray();
        if (values.Length > 0)
            await _database.ListRightPushAsync(key, values);
    }

    public async Task<List<DialogMessageDto>> GetDialogHistoryAsync(string surveyId, CancellationToken cancellationToken = default)
    {
        var key = GetKey(surveyId);
        var messages = await _database.ListRangeAsync(key);
        
        var result = new List<DialogMessageDto>();
        foreach (var messageJson in messages)
        {
            if (!string.IsNullOrEmpty(messageJson))
            {
                var message = JsonSerializer.Deserialize<DialogMessageDto>(messageJson!);
                if (message != null)
                {
                    result.Add(message);
                }
            }
        }
        
        return result;
    }

    public async Task ClearDialogHistoryAsync(string surveyId, CancellationToken cancellationToken = default)
    {
        var key = GetKey(surveyId);
        await _database.KeyDeleteAsync(key);
    }

    private static string GetKey(string surveyId)
    {
        return $"{KeyPrefix}{surveyId}";
    }
}

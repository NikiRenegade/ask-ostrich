using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using System.Collections.Concurrent;
using SurveyManageService.Domain.Interfaces.Publishers;

namespace SurveyManageService.Infrastructure.Messaging;

public class RabbitMqEventPublisher : IEventPublisher
{
    private readonly Task<IChannel> _channelTask;

    private readonly ConcurrentDictionary<string, bool> _exchanges = new();
    private readonly ConcurrentDictionary<string, bool> _queues = new();

    public RabbitMqEventPublisher(Task<IChannel> channelTask)
    {
        _channelTask = channelTask;
    }

    public async Task PublishAsync<T>(T @event, string routingKey, string exchangeName)
    {
        var channel = await _channelTask;

        if (channel is null)
            throw new InvalidOperationException("Канал RabbitMQ null");

        try
        {
            if (_exchanges.TryAdd(exchangeName, true))
            {
                await channel.ExchangeDeclareAsync(
                    exchange: exchangeName,
                    type: ExchangeType.Topic,
                    durable: true
                );
            }
            
            var json = JsonSerializer.Serialize(@event);
            var body = Encoding.UTF8.GetBytes(json);

            await channel.BasicPublishAsync(
                exchange: exchangeName,
                routingKey: routingKey,
                body: body
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка в PublishAsync: {ex}");
            throw;
        }
    }
}
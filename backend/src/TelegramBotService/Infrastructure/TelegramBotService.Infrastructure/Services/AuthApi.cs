using System.Net.Http.Json;
using TelegramBotService.Application.Interfaces;
using TelegramBotService.Domain.Dto;

namespace TelegramBotService.Infrastructure.Services;

public class AuthApi : IAuthApi
{
    private readonly HttpClient _http;

    public AuthApi(HttpClient http)
    {
        _http = http;
    }

    public async Task<string> StartTelegramAuth(long telegramUserId)
    {
        var response = await _http.PostAsJsonAsync("/security/api/TelegramAuth/start",
            new { telegramUserId });

        var result = await response.Content.ReadFromJsonAsync<AuthStartDto>();
        return result!.AuthId;
    }

    public async Task<AuthStatusDto> GetStatus(string authId)
    {
        return await _http.GetFromJsonAsync<AuthStatusDto>($"/security/api/TelegramAuth/status?authId={authId}");
    }

    public async Task<IEnumerable<PendingAuthDto>> GetPendingAsync()
    {
        var list = await _http.GetFromJsonAsync<IEnumerable<PendingAuthDto>>("/security/api/TelegramAuth/pending");
        return list ?? Enumerable.Empty<PendingAuthDto>();
    }

    public async Task ClearAsync(string authId)
    {
        await _http.PostAsJsonAsync("/security/api/TelegramAuth/clear", new { authId });
    }
}
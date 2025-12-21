using TelegramBotService.Application.Interfaces;
namespace TelegramBotService.Infrastructure.Services;

public class AuthFrontendUrlProvider : IAuthFrontendUrlProvider
{
    private readonly string _baseUrl;

    public AuthFrontendUrlProvider(string baseUrl)
    {
        _baseUrl = baseUrl;
    }

    public string GetTelegramAuthUrl(string authId) =>
        $"{_baseUrl}/auth/telegram?authId={authId}";
}
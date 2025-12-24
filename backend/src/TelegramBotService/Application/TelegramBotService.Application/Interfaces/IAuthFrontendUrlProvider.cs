namespace TelegramBotService.Application.Interfaces;

public interface IAuthFrontendUrlProvider
{
    string GetTelegramAuthUrl(string authId);
}
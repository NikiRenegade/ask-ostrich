namespace TelegramBotService.Domain.Sessions;

public enum AuthState
{
    None,
    WaitingForWebAuth,
    Authorized
}
namespace TelegramBotService.Application.Bot;

public class UserSession
{
    public AuthState AuthState { get; set; }
    public string? AuthId { get; set; }
    public Guid? UserId { get; set; }
}
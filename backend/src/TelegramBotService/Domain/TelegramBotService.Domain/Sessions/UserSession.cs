using TelegramBotService.Domain.Entities;

namespace TelegramBotService.Application.Bot;

public class UserSession
{
    public AuthState AuthState { get; set; }
    public string? AuthId { get; set; }
    public User User { get; set; }
}
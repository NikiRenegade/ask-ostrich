using TelegramBotService.Domain.Sessions;
namespace TelegramBotService.Application.Interfaces;

public interface IUserSessionStore
{
    UserSession Get(long chatId);
}
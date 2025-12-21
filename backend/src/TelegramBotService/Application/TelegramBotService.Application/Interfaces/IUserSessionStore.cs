using TelegramBotService.Application.Bot;
namespace TelegramBotService.Application.Interfaces;

public interface IUserSessionStore
{
    UserSession Get(long chatId);
}
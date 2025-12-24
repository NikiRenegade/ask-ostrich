using TelegramBotService.Application.Bot;

namespace TelegramBotService.Application.Interfaces;

public interface IUserCommand
{
    bool CanHandle(UserInput input, UserSession session);
    Task<AppResponse> HandleAsync(UserInput input, UserSession session);
}
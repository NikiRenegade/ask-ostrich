using TelegramBotService.Application.Bot;
using TelegramBotService.Application.Interfaces;

namespace TelegramBotService.Application.Commands;

public class StartCommand : IUserCommand
{
    public bool CanHandle(UserInput input, UserSession session)
        => input.IsCommand("/start");

    public Task<AppResponse> HandleAsync(UserInput input, UserSession session)
    {
        return Task.FromResult(new AppResponse
        {
            Text = "Привет! 👋\nЧто будем делать?",
            Actions =
            [
                new AppAction { Id = "menu.login", Label = "🔐 Войти" },
               
            ]
        });
    }
}
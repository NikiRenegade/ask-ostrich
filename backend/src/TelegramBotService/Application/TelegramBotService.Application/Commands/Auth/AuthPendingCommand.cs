using TelegramBotService.Application.Bot;
using TelegramBotService.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace TelegramBotService.Application.Commands.Auth;

public class AuthPendingCommand : IUserCommand
{
    private readonly IAuthApi _auth;

    public AuthPendingCommand(IAuthApi auth)
    {
        _auth = auth;
    }

    public bool CanHandle(UserInput input, UserSession session)
    {
        var can = session.AuthState == AuthState.WaitingForWebAuth 
                  && input.IsAction("check.auth");
        return can;
    }

    public async Task<AppResponse> HandleAsync(UserInput input, UserSession session)
    {
        if (string.IsNullOrWhiteSpace(session.AuthId))
        {
            session.AuthState = AuthState.None;
            return new AppResponse { Text = "Нет активной попытки входа. Нажмите Войти, чтобы начать." };
        }

        var status = await _auth.GetStatus(session.AuthId);

        if (!status.Completed)
        {
            return new AppResponse
            {
                Text = "Ожидаю вход на сайте ⏳"
            };
        }

        session.AuthState = AuthState.Authorized;
        session.UserId = status.UserId;

        if (!string.IsNullOrWhiteSpace(status.UserName))
        {
            return new AppResponse
            {
                Text = $"Вы вошли как {status.UserName} {status.FirstName} {status.LastName}  ✅",
                Actions =  [new AppAction { Id = "menu.startSurvey", Label = "📝 Пройти опрос" },
                            new AppAction { Id = "menu.mySurveys", Label = "📋 Мои опросы" }]
            };
        }

        return new AppResponse
        {
            Text = "Вы успешно вошли ✅"
        };
    }
}
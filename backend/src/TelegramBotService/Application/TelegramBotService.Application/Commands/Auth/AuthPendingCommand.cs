using TelegramBotService.Application.Bot;
using TelegramBotService.Application.Interfaces;
using Microsoft.Extensions.Logging;
using TelegramBotService.Application.Actions;
using TelegramBotService.Domain.Entities;

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
        return session.AuthState == AuthState.WaitingForWebAuth && input.IsAction("check.auth");
    }

    public async Task<AppResponse> HandleAsync(UserInput input, UserSession session)
    {
        if (string.IsNullOrWhiteSpace(session.AuthId))
        {
            session.AuthState = AuthState.None;
            return new AppResponse { Text = "Нет активной попытки входа. Нажмите Войти, чтобы начать." };
        }

        var status = await _auth.GetStatus(session.AuthId);

        if (status.Completed && !string.IsNullOrWhiteSpace(status.UserName))
        {
            if (session.User == null)
            {
                session.User = new User(status.UserId, status.UserName, status.FirstName, status.LastName);
            }
            session.AuthState = AuthState.Authorized;
            session.User.Id = status.UserId;
            session.User.UserName = status.UserName;
            session.User.FirstName = status.FirstName;
            session.User.LastName = status.LastName;
            
                
            return new AppResponse {
                    
                Text = $"Вы вошли как {status.UserName} {status.FirstName} {status.LastName}  ✅",
                Actions =  MenuActions.GetMenuActions()
                };
        }

        return new AppResponse
        {
            Text = "Ожидаю вход на сайте ⏳"
        };
    }
}
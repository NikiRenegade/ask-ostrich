
using TelegramBotService.Domain.Sessions;
using TelegramBotService.Application.Actions;
using TelegramBotService.Application.Interfaces;

namespace TelegramBotService.Application.Commands;

public class ProfileCommand : IUserCommand
{
    public bool CanHandle(UserInput input, UserSession session)
        => input.IsAction("menu.profile") && session.AuthState == AuthState.Authorized;

    public async Task<AppResponse> HandleAsync(UserInput input, UserSession session)
    {
        return await Task.FromResult(new AppResponse
        {
            Text = $"Вы - наш многоуважаемый пользователь\n" +
                   $"👤 UserName: {session.User.UserName}\n" +
                   $"🧑 Имя: {session.User.FirstName}\n" +
                   $"🧾 Фамилия: {session.User.LastName}",
            
            Actions = MenuActions.GetMenuActions()
        });
    }
}
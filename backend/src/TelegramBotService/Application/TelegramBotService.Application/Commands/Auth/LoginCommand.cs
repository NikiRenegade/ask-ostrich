using TelegramBotService.Application.Bot;
using TelegramBotService.Application.Interfaces;

namespace TelegramBotService.Application.Commands.Auth;

public class LoginCommand : IUserCommand
{
    private readonly IAuthApi _authApi;
    private readonly IAuthFrontendUrlProvider _authFrontendUrlProvider;

    public LoginCommand(IAuthApi authApi, IAuthFrontendUrlProvider authFrontendUrlProvider)
    {
        _authApi = authApi;
        _authFrontendUrlProvider = authFrontendUrlProvider;
    }

    public bool CanHandle(UserInput input, UserSession session)
        => (input.IsAction("menu.login") && session.AuthState == AuthState.None) 
           || (session.AuthState == AuthState.WaitingForWebAuth && input.IsAction("check.auth") == false);

    public async Task<AppResponse> HandleAsync(UserInput input, UserSession session)
    {
        var authId = await _authApi.StartTelegramAuth(input.ChatId);

        session.AuthId = authId;
        session.AuthState = AuthState.WaitingForWebAuth;

        return new AppResponse
        {
            Text = "Для входа перейдите на сайт 👇",
            Actions =
            [
                new AppAction
                {
                    Label = "🔐 Войти",
                    Url = _authFrontendUrlProvider.GetTelegramAuthUrl(authId)
                },
                new AppAction
                {
                    Id = "check.auth",
                    Label = "🔄 Проверить вход"
                }
            ]
        };
    }
}
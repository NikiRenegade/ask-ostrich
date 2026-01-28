using TelegramBotService.Domain.Sessions;
using TelegramBotService.Application.Interfaces;

namespace TelegramBotService.Application.Commands.Surveys;

public class StartSurveyCommand : IUserCommand
{
    public bool CanHandle(UserInput input, UserSession session)
        => input.IsAction("menu.startSurvey");

    public async Task<AppResponse> HandleAsync(UserInput input, UserSession session)
    {
        session.SurveyState = SurveyState.WaitingForSurveyGuid;

        return await Task.FromResult(new AppResponse
        {
            Text = "Введите короткий код опроса:"
        });
    }
}
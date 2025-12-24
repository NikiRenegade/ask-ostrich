using TelegramBotService.Application.Bot;
using TelegramBotService.Application.Interfaces;

namespace TelegramBotService.Application.Commands.Surveys;

public class StartSurveyCommand : IUserCommand
{

    public StartSurveyCommand()
    {
    }

    public bool CanHandle(UserInput input, UserSession session)
        => input.IsAction("menu.startSurvey");

    public async Task<AppResponse> HandleAsync(UserInput input, UserSession session)
    {
        return new AppResponse
        {
            Text = "Введите GUID опроса:"
        };
    }
}
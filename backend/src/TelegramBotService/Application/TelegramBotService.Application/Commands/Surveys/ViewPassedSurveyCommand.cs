using TelegramBotService.Application.Interfaces;
using TelegramBotService.Domain.Sessions;

namespace TelegramBotService.Application.Commands.Surveys;

public class ViewPassedSurveyCommand : IUserCommand
{
    private readonly ISurveyApi _surveyApi;

    public ViewPassedSurveyCommand(ISurveyApi surveyApi)
    {
        _surveyApi = surveyApi;
    }
    public bool CanHandle(UserInput input, UserSession session)
        => input.Action?.StartsWith("survey.passed:") == true
           && session.AuthState == AuthState.Authorized;


    public async Task<AppResponse> HandleAsync(UserInput input, UserSession session)
    {
        var surveyIdStr = input.Action!.Split(':')[1];
        var surveyId = Guid.Parse(surveyIdStr);

        var passedSurvey = await _surveyApi.GetPassedSurvey(surveyId, session.User.Id);

        if (passedSurvey == null)
        {
            return new AppResponse
            {
                Text = "Не удалось найти результаты этого опроса."
            };
        }

        return new SurveyResultPresenter().ShowPassedSurvey(passedSurvey);
    }
}
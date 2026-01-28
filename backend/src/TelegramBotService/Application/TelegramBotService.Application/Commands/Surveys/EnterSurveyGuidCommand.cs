using System.Text;
using TelegramBotService.Application.Actions;
using TelegramBotService.Application.Interfaces;
using TelegramBotService.Domain.Sessions;

namespace TelegramBotService.Application.Commands.Surveys;

public class EnterSurveyGuidCommand : IUserCommand
{
    private readonly ISurveyApi _surveyApi;

    public EnterSurveyGuidCommand(ISurveyApi surveyApi)
    {
        _surveyApi = surveyApi;
    }
    public bool CanHandle(UserInput input, UserSession session)
        => session.SurveyState == SurveyState.WaitingForSurveyGuid;

    public async Task<AppResponse> HandleAsync(UserInput input, UserSession session)
    {
        var survey = _surveyApi.GetSurveyByShortCode(input.Text);
        if ( (await survey) == null)
        {
            return await Task.FromResult(new AppResponse
            {
                Text = "Введите короткий код опроса:"
            });
        }
        var passedSurvey = await _surveyApi.GetPassedSurvey(survey.Result.Id, session.User.Id);
        if ((passedSurvey) != null)
        {
            return new SurveyResultPresenter().ShowPassedSurvey(passedSurvey);
        }

        session.CurrentSurvey = survey.Result;
        session.CurrentQuestionIndex = 0;
        session.SurveyState = SurveyState.InProgress;
        session.Answers.Clear();
        
        return SurveyRenderer.RenderQuestion(session);
        
    }
}
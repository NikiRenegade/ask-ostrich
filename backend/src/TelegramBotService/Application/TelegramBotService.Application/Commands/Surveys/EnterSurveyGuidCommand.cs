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
        => session.SurveyState == SurveyState.WaitingForSurveyGuid
           && Guid.TryParse(input.Text, out _);

    public async Task<AppResponse> HandleAsync(UserInput input, UserSession session)
    {
        var surveyId = Guid.Parse(input.Text);
        var passedSurvey = await _surveyApi.GetPassedSurvey(surveyId, session.User.Id);
        if ((passedSurvey) != null)
        {
            return new SurveyResultPresenter().ShowPassedSurvey(passedSurvey);
        }
        var survey = _surveyApi.GetSurvey(surveyId);

        if ( (await survey) == null)
        {
            return await Task.FromResult(new AppResponse
            {
                Text = "Введите GUID опроса:"
            });
        }
        session.CurrentSurvey = await survey;
        session.CurrentQuestionIndex = 0;
        session.SurveyState = SurveyState.InProgress;
        session.Answers.Clear();
        
        return SurveyRenderer.RenderQuestion(session);
        
    }
}
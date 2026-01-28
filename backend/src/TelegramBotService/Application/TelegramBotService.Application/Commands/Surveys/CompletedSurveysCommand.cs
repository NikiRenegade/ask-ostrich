using TelegramBotService.Application.Actions;
using TelegramBotService.Application.Interfaces;
using TelegramBotService.Domain.Sessions;

namespace TelegramBotService.Application.Commands.Surveys;

public class CompletedSurveysCommand : IUserCommand
{
    private readonly ISurveyApi _surveyApi;

    public CompletedSurveysCommand(ISurveyApi surveyApi)
    {
        _surveyApi = surveyApi;
    }

    public bool CanHandle(UserInput input, UserSession session)
        => input.IsAction("menu.completedSurveys")
           && session.AuthState == AuthState.Authorized;

    public async Task<AppResponse> HandleAsync(UserInput input, UserSession session)
    {
        var surveys = await _surveyApi.GetPassedSurveys(session.User.Id);

        if (surveys.Count == 0)
        {
            return new AppResponse
            {
                Text = "Вы ещё не прошли ни одного опроса.",
                Actions = MenuActions.GetMenuActions()
            };
        }

        var actions = new List<AppAction>();

        foreach (var s in surveys.OrderByDescending(x => x.DatePassed))
        {
            actions.Add(new AppAction
            {
                Id = $"survey.passed:{s.SurveyId}",
                Label = $"📋 {s.Title} ({s.DatePassed:dd.MM.yyyy})"
            });
        }

        actions.AddRange(MenuActions.GetMenuActions());

        return new AppResponse
        {
            Text = "🏁 Выберите опрос для просмотра результатов:",
            Actions = actions
        };
    }
}
using TelegramBotService.Application.Actions;
using TelegramBotService.Application.Interfaces;
using TelegramBotService.Domain.Sessions;
using System.Text;

namespace TelegramBotService.Application.Commands.Surveys;

public class MySurveysCommand : IUserCommand
{
    private readonly ISurveyApi _surveyApi;

    public MySurveysCommand(ISurveyApi surveyApi)
    {
        _surveyApi = surveyApi;
    }

    public bool CanHandle(UserInput input, UserSession session)
        => input.IsAction("menu.mySurveys")
           && session.AuthState == AuthState.Authorized;

    public async Task<AppResponse> HandleAsync(UserInput input, UserSession session)
    {
        var surveys = await _surveyApi.GetMySurveys(session.User!.Id);

        if (surveys.Count == 0)
        {
            return new AppResponse
            {
                Text = "У вас пока нет созданных опросов.",
                Actions = MenuActions.GetMenuActions()
            };
        }

        var sb = new StringBuilder();
        sb.AppendLine("🗂Ваши опросы:");
        sb.AppendLine();

        foreach (var s in surveys.OrderByDescending(s => s.CreatedAt))
        {
            var status = s.IsPublished ? "🟢" : "🟡";
            sb.AppendLine($"{status} {s.Title} ({s.QuestionCount} вопр.)");
            sb.AppendLine($"Описание: {s.Description}");
            sb.AppendLine();
        }
        sb.AppendLine();
        return new AppResponse
        {
            Text = sb.ToString(),
            Actions = MenuActions.GetMenuActions()
        };
    }
}
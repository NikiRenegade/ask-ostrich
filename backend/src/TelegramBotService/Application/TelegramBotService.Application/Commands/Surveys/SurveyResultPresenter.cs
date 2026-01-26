using System.Text;
using TelegramBotService.Application.Actions;

namespace TelegramBotService.Application.Commands.Surveys;

public class SurveyResultPresenter
{
    public AppResponse ShowPassedSurvey(PassedSurveyDto passedSurvey)
    {
        var sb = new StringBuilder();

        sb.AppendLine("📋 Опрос пройден");
        sb.AppendLine();
        sb.AppendLine($"Название: {passedSurvey.Title}");
        sb.AppendLine($"Описание: {passedSurvey.Description}");
        sb.AppendLine($"Дата прохождения: {passedSurvey.DatePassed:dd.MM.yyyy HH:mm}");
        sb.AppendLine();
        sb.AppendLine($"✅Правильных ответов: {passedSurvey.Answers.Count(a => a.IsCorrect)} " +
                      $"из {passedSurvey.Answers.Count()}");
        sb.AppendLine();

        int i = 1;
        foreach (var a in passedSurvey.Answers)
        {
            sb.AppendLine($"{i}. {a.QuestionTitle}   {(a.IsCorrect ? "✅ Верно" : "❌ Неверно")}");
            sb.AppendLine();
            i++;
        }
        sb.AppendLine();
        
        return new AppResponse
        {
            Text = sb.ToString(),
            Actions = MenuActions.GetMenuActions()
        };
    }
}
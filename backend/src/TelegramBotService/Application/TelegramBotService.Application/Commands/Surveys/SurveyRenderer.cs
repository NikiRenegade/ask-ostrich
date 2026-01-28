using TelegramBotService.Domain.Dto;
using TelegramBotService.Domain.Sessions;

namespace TelegramBotService.Application.Commands.Surveys;

public class SurveyRenderer
{
    public static AppResponse RenderQuestion(UserSession session)
    {
        var question = session.CurrentSurvey!.Questions
            .OrderBy(q => q.Order)
            .ElementAt(session.CurrentQuestionIndex);

        var actions = new List<AppAction>();
        if (question.Type == QuestionType.SingleChoice)
        {
            foreach (var option in question.Options)
            {
                actions.Add(new AppAction
                {
                    Id = $"survey.select:{option.Value}",
                    Label = option.Title
                });
            }
        }

        if (question.Type == QuestionType.MultipleChoice)
        {
            foreach (var option in question.Options)
            {
                var selected = session.CurrentMultiChoice.Contains(option.Value);
                actions.Add(new AppAction
                {
                    Id = $"survey.select:{option.Value}",
                    Label = selected ? $"✅ {option.Title}" : option.Title
                });
            }

            actions.Add(new AppAction
            {
                Id = "survey.next",
                Label = "➡️ Далее"
            });
        }

        if (session.CurrentQuestionIndex > 0)
        {
            actions.Add(new AppAction
            {
                Id = "survey.back",
                Label = "⬅️ Назад"
            });
        }

        return new AppResponse
        {
            Text = $"{question.Title}\n{question.InnerText}",
            Actions = actions
        };
    }
}
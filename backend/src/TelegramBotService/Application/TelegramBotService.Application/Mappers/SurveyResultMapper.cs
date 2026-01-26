using TelegramBotService.Domain.Dto;
using TelegramBotService.Domain.Sessions;

namespace TelegramBotService.Application.Mappers;

public static class SurveyResultMapper
{
    public static SurveyPassDto Map(UserSession session)
    {
        var survey = session.CurrentSurvey!;

        var dto = new SurveyPassDto
        {
            UserId = session.User!.Id,
            SurveyId = survey.Id,
            DatePassed = DateTime.UtcNow
        };

        foreach (var question in survey.Questions)
        {
            if (!session.Answers.TryGetValue(question.Id, out var raw))
                continue;

            var values = new List<string>();

            if (raw is string s)
                values.Add(s);
            else if (raw is List<string> list)
                values = list;

            dto.Answers.Add(new SurveyAnswerDto
            {
                QuestionId = question.Id,
                QuestionTitle = question.Title,
                Values = values,
            });
        }

        return dto;
    }

}
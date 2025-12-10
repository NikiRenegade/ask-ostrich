using SurveyResponseService.Domain.Entities;

namespace SurveyResponseService.Application.Helpers;

public static class SurveyResultCalculator
{
    public static bool IsAnswerCorrect(Survey survey, SurveyResult surveyResult, Guid questionId)
    {
        var question = survey.Questions.FirstOrDefault(q => q.Id == questionId);
        if (question == null) return false;

        var hasCorrectOptions = question.Options.Any(o => o.IsCorrect);
        if (!hasCorrectOptions)
        {
            return true;
        }

        var userAnswer = surveyResult.Answers.FirstOrDefault(a => a.QuestionId == questionId);
        if (userAnswer == null) return false;

        var correctOptionValues = question.Options
            .Where(o => o.IsCorrect)
            .Select(o => o.Value)
            .ToList();

        var userValues = userAnswer.Values ?? new List<string>();
        return question.Type == QuestionType.MultipleChoice
            ? correctOptionValues.Count == userValues.Count && 
              correctOptionValues.All(userValues.Contains) &&
              userValues.All(correctOptionValues.Contains)
            : userValues.Count == 1 && correctOptionValues.Contains(userValues[0]);
    }
}


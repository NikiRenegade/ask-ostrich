import type {Survey, SurveyEdit} from '../../types/Survey.ts';

export function surveyToSurveyEditConverter(survey: Survey): SurveyEdit {
    return {
        Title: survey.Title,
        Description: survey.Description,
        Questions: survey.Questions.map(question => ({
            Type: question.Type,
            Title: question.Title,
            Order: question.Order,
            InnerText: question.InnerText,
            Options: question.Options?.map(option => ({
                Title: option.Title,
                Order: option.Order,
                IsCorrect: option.IsCorrect,

            })).sort((a, b) => a.Order - b.Order)
        })
        ).sort((a, b) => a.Order - b.Order)
    };
}
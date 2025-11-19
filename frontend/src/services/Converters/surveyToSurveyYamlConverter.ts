import type {Survey, SurveyYaml} from '../../types/Survey.ts';

export function surveyToSurveyYamlConverter(survey: Survey): SurveyYaml {
    return {
        Title: survey.Title,
        Description: survey.Description,
        Questions: survey.Questions.map(q => ({
            Type: q.Type,
            Title: q.Title,
            Order: q.Order,
            InnerText: q.InnerText,
            Options: q.Options?.map(o => ({
                Title: o.Title,
                Order: o.Order,
                IsCorrect: o.IsCorrect,

            })).sort((a, b) => a.Order - b.Order)
        })
        ).sort((a, b) => a.Order - b.Order)
    };
}
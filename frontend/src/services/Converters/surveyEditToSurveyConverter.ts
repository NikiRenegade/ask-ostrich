import { v4 as uuidv4 } from 'uuid';
import type { Survey, SurveyEdit } from '../../types/Survey';

export function surveyEditToSurveyConverter(surveyEdit: SurveyEdit, previous: Survey): Survey {
    return {
        ...previous,

        Title: surveyEdit.Title ?? previous.Title,
        Description: surveyEdit.Description ?? previous.Description,

        Questions: surveyEdit.Questions.map((question, questionindex) => {
            const prevQ = previous.Questions[questionindex];

            return {
                QuestionId: prevQ?.QuestionId ?? uuidv4(),
                Type: question.Type,
                Title: question.Title,
                Order: question.Order ?? questionindex + 1,
                InnerText: question.InnerText,
                Options: question.Options?.map((option, optionIndex) => {
                    const prevO = prevQ?.Options?.[optionIndex];

                    return {
                        Title: option.Title,
                        Value: prevO?.Value ?? uuidv4(),
                        IsCorrect: option.IsCorrect ?? false,
                        Order: option.Order ?? optionIndex + 1,
                    };
                }) ?? [],
            };
        }),
    };
}

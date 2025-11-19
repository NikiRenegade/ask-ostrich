import { v4 as uuidv4 } from 'uuid';
import type { Survey, SurveyYaml } from '../../types/Survey';

export function surveyYamlToSurveyConverter(y: SurveyYaml, previous: Survey): Survey {
    return {
        ...previous,

        Title: y.Title ?? previous.Title,
        Description: y.Description ?? previous.Description,

        Questions: y.Questions.map((qYaml, index) => {
            const prevQ = previous.Questions[index];

            return {
                QuestionId: prevQ?.QuestionId ?? uuidv4(),
                Type: qYaml.Type,
                Title: qYaml.Title,
                Order: qYaml.Order ?? index + 1,
                InnerText: qYaml.InnerText,
                Options: qYaml.Options?.map((o, optIndex) => {
                    const prevO = prevQ?.Options?.[optIndex];

                    return {
                        Title: o.Title,
                        Value: prevO?.Value ?? uuidv4(),   // сохраняем если было
                        IsCorrect: o.IsCorrect ?? false,
                        Order: o.Order ?? optIndex + 1,
                    };
                }) ?? [],
            };
        }),
    };
}

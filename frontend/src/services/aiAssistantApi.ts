import type { GenerateSurveyRequest, GeneratedSurvey } from '../models/aiAssistantModels';
import api from '../api/axios';

export async function generateSurvey(request: GenerateSurveyRequest): Promise<GeneratedSurvey> {

    try {
        const response = await api.post("/ai-assistant/api/SurveyGenerator", {
            prompt: request.prompt,
                currentSurveyJson: request.currentSurveyJson,
                type: request.type,
        });
        return await response.data;

    } catch (error) {
        throw new Error('Ошибка получения ответа от ИИ-ассистента');
    }
}


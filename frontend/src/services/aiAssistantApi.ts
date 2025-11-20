import type { GenerateSurveyRequest, GeneratedSurvey } from '../models/aiAssistantModels';
import api from './axios';

export async function generateSurvey(request: GenerateSurveyRequest): Promise<GeneratedSurvey> {

    try {
        const response = await api.post("/ai-assistant/api/AIAssistant/generate", {
            prompt: request.prompt,
            currentSurveyJson: request.currentSurveyJson,
            type: request.type
        });
        return await response.data;

    } catch (error) {
        throw new Error('Ошибка получения ответа от ИИ-ассистента');
    }
}

export async function askLLM(request: GenerateSurveyRequest): Promise<string> {

    try {
        const response = await api.post("/ai-assistant/api/AIAssistant/ask", {
            prompt: request.prompt,
            currentSurveyJson: request.currentSurveyJson,
            type: request.type
        });
        return await response.data;

    } catch (error) {
        throw new Error('Ошибка получения ответа от ИИ-ассистента');
    }
}


import { API_BASE_URL } from '../config/api';
import type { GenerateSurveyRequest, GeneratedSurvey } from '../models/aiAssistantModels';

export async function generateSurvey(request: GenerateSurveyRequest): Promise<GeneratedSurvey> {
    const response = await fetch(`${API_BASE_URL}/ai-assistant/api/AIAssistant/generate`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            prompt: request.prompt,
            currentSurveyJson: request.currentSurveyJson,
            type: request.type,
        }),
    });

    if (!response.ok) {
        throw new Error('Ошибка получения ответа от ИИ-ассистента');
    }

    return await response.json();
}

export async function askLLM(request: GenerateSurveyRequest): Promise<string> {
    const response = await fetch(`${API_BASE_URL}/ai-assistant/api/AIAssistant/ask`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            prompt: request.prompt,
            currentSurveyJson: request.currentSurveyJson,
            type: request.type,
        }),
    });

    if (!response.ok) {
        throw new Error('Ошибка получения ответа от ИИ-ассистента');
    }

    return await response.text();
}


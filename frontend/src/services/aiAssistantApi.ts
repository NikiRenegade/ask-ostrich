export interface GenerateSurveyRequest {
    prompt: string;
    currentSurveyJson: string;
    type: 0 | 1;
}

export interface GeneratedSurvey {
    title: string;
    description: string;
    questions: QuestionDto[];
}

export interface QuestionDto {
    type: 0 | 1 | 2;
    title: string;
    order: number;
    innerText: string;
    options: OptionDto[];
}

export interface OptionDto {
    title: string;
    value: string;
    order: number;
    isCorrect?: boolean;
}

const API_BASE_URL = 'http://localhost:5110';

export async function generateSurvey(request: GenerateSurveyRequest): Promise<GeneratedSurvey> {
    const response = await fetch(`${API_BASE_URL}/ai-assistant/api/SurveyGenerator`, {
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


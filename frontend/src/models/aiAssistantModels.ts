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

export interface ChatMessage {
    id: string;
    isUserMessage: boolean;
    content: string;
    isPending?: boolean;
}

export const AssistentMode = {
    Construct: 'construct',
    Ask: 'ask'
} as const;

export type AssistentMode = typeof AssistentMode[keyof typeof AssistentMode];

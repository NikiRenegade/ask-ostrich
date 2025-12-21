import { v4 as uuidv4 } from 'uuid';
import api from './axios';
import type { Survey } from '../types/Survey';
import type { SurveyShort } from '../types/SurveyShort';

export interface SurveyResponse {
    id: string;
    title: string;
    description: string;
    isPublished: boolean;
    author: {
        id: string;
        userName: string;
        email: string;
        firstName: string;
        lastName: string;
    };
    createdAt: string;
    lastUpdateAt: string;
    shortUrl: string;
    questions: Array<{
        id: string;
        type: string;
        title: string;
        order: number;
        innerText: string;
        options: Array<{
            title: string;
            value: string;
            isCorrect: boolean;
            order: number;
        }>;
    }>;
}

interface PublishSurveyRequest {
    Id: string;
    IsPublished: boolean;
}

export interface SubmitSurveyResultRequest {
    userId: string | undefined;
    surveyId: string;
    datePassed: string;
    answers: Array<{
        questionId: string;
        questionTitle: string;
        values: string[];
    }>;
}

function mapSurveyResponseToSurvey(response: SurveyResponse, id: string): Survey {
    return {
        SurveyId: response.id || id,
        Title: response.title || '',
        Description: response.description || '',
        IsPublished: response.isPublished !== undefined ? response.isPublished : false,
        AuthorGuid: response.author?.id || '',
        CreatedAt: response.createdAt || new Date().toISOString(),
        ShortUrl: response.shortUrl || '',
        Questions: (response.questions || []).map((q: any) => ({
            QuestionId: q.id || uuidv4(),
            Type: (q.type || 'Text') as 'Text' | 'SingleChoice' | 'MultipleChoice',
            Title: q.title || '',
            Order: q.order || 1,
            InnerText: q.innerText || '',
            Options: (q.options || []).map((opt: any) => ({
                Title: opt.title || '',
                Value: opt.value || uuidv4(),
                IsCorrect: opt.isCorrect !== undefined ? opt.isCorrect : false,
                Order: opt.order || 1,
            })),
        })),
    };
}

function mapSurveyToPublishRequest(survey: SurveyShort): PublishSurveyRequest {
    return {
        Id: survey.id,
        IsPublished: survey.isPublished,
    };
}

export interface PassedSurveyResponse {
    surveyId: string;
    title: string;
    description: string;
    datePassed: string;
    totalQuestions: number;
    answers: Array<{
        questionId: string;
        questionTitle: string;
        values: string[];
        isCorrect: boolean;
    }>;
}

export async function loadSurveyById(id: string): Promise<Survey> {
    try {        
        const response = await api.get<SurveyResponse>(`/survey-response/api/Survey/${id}`);
        return mapSurveyResponseToSurvey(response.data, id);
    } catch (error) {
        throw new Error('Не удалось загрузить опрос');
    }
}

export async function submitSurveyResult(request: SubmitSurveyResultRequest): Promise<void> {
    try {
        await api.post('/survey-response/api/SurveyResult', {
            userId: request.userId,
            surveyId: request.surveyId,
            datePassed: request.datePassed,
            answers: request.answers,
        });
    } catch (error) {
        throw new Error('Не удалось отправить ответы');
    }
}

export async function getPassedSurveysByUserId(userId: string): Promise<SurveyResponse[]> {
    try {
        const response = await api.get<SurveyResponse[]>(`/survey-response/api/SurveyResult/user-surveys/${userId}`);
        return response.data;
    } catch (error) {
        throw new Error('Не удалось загрузить пройденные опросы');
    }
}

export async function getPassedSurveysWithResultsByUserId(userId: string): Promise<PassedSurveyResponse[]> {
    try {
        const response = await api.get<PassedSurveyResponse[]>(`/survey-response/api/SurveyResult/user-passed-surveys/${userId}`);
        return response.data;
    } catch (error) {
        throw new Error('Не удалось загрузить пройденные опросы');
    }
}

export interface SurveyResultResponse {
    surveyId: string;
    title: string;
    description: string;
    datePassed: string;
    totalQuestions: number;
    answers: Array<{
        questionId: string;
        questionTitle: string;
        values: string[];
        isCorrect: boolean;
    }>;
}

export async function getSurveyResultBySurveyIdAndUserId(surveyId: string, userId: string): Promise<SurveyResultResponse | null> {
    try {
        const response = await api.get<SurveyResultResponse>(`/survey-response/api/SurveyResult/survey/${surveyId}/user/${userId}`);
        return response.data;
    } catch (error) {
        throw new Error('Не удалось загрузить результат опроса');
    }
}

export async function publishSurvey(survey: SurveyShort): Promise<void> {
    try {
        const request = mapSurveyToPublishRequest(survey);
        await api.put("/survey-manage/api/survey/publish", request);
    } catch (error) {
        throw new Error('Не удалось обновить опрос');
    }
}
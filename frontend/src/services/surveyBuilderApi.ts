import { v4 as uuidv4 } from 'uuid';
import api from './axios';
import type { Survey } from '../types/Survey';

interface CreateSurveyRequest {
    Title: string;
    Description: string;
    AuthorGuid: string;
    Questions: Array<{
        Type: string;
        Title: string;
        Order: number;
        InnerText: string;
        Options: Array<{
            Title: string;
            Value: string;
            IsCorrect: boolean;
            Order: number;
        }>;
    }>;
}

interface UpdateSurveyRequest {
    Id: string;
    Title: string;
    Description: string;
    IsPublished: boolean;
    AuthorGuid: string;
    Questions: Array<{
        Type: string;
        Title: string;
        Order: number;
        InnerText: string;
        Options: Array<{
            Title: string;
            Value: string;
            IsCorrect: boolean;
            Order: number;
        }>;
    }>;
}

export interface SurveyResponse {
    id: string;
    title: string;
    description: string;
    isPublished: boolean;
    authorGuid?: string;
    author?: { id: string };
    createdAt: string;
    shortUrl: string;
    questions: Array<{
        questionId: string;
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

export interface SurveyCreatedResponse {
    id: string;
}

function mapSurveyResponseToSurvey(response: SurveyResponse, id: string, userId: string): Survey {
    return {
        SurveyId: response.id || id,
        Title: response.title || '',
        Description: response.description || '',
        IsPublished: response.isPublished !== undefined ? response.isPublished : false,
        AuthorGuid: response.authorGuid || response.author?.id || userId,
        CreatedAt: response.createdAt || new Date().toISOString(),
        ShortUrl: response.shortUrl || '',
        Questions: (response.questions || []).map((q: any) => ({
            QuestionId: q.questionId || uuidv4(),
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

function mapSurveyToCreateRequest(survey: Survey): CreateSurveyRequest {
    return {
        Title: survey.Title,
        Description: survey.Description,
        AuthorGuid: survey.AuthorGuid,
        Questions: survey.Questions.map(q => ({
            Type: q.Type,
            Title: q.Title,
            Order: q.Order,
            InnerText: q.InnerText,
            Options: q.Options.map(opt => ({
                Title: opt.Title,
                Value: opt.Value,
                IsCorrect: opt.IsCorrect,
                Order: opt.Order,
            })),
        })),
    };
}

function mapSurveyToUpdateRequest(survey: Survey): UpdateSurveyRequest {
    return {
        Id: survey.SurveyId,
        Title: survey.Title,
        Description: survey.Description,
        IsPublished: survey.IsPublished,
        AuthorGuid: survey.AuthorGuid,
        Questions: survey.Questions.map(q => ({
            Type: q.Type,
            Title: q.Title,
            Order: q.Order,
            InnerText: q.InnerText,
            Options: q.Options.map(opt => ({
                Title: opt.Title,
                Value: opt.Value,
                IsCorrect: opt.IsCorrect,
                Order: opt.Order,
            })),
        })),
    };
}

export async function loadSurveyById(id: string, userId: string): Promise<Survey> {
    try {
        const response = await api.get<SurveyResponse>(`/survey-manage/api/Survey/${id}`);
        return mapSurveyResponseToSurvey(response.data, id, userId);
    } catch (error) {
        throw new Error('Не удалось загрузить опрос');
    }
}

export async function createSurvey(survey: Survey): Promise<SurveyCreatedResponse> {
    try {
        const request = mapSurveyToCreateRequest(survey);
        const response = await api.post<SurveyCreatedResponse>("/survey-manage/api/survey", request);
        return response.data;
    } catch (error) {
        throw new Error('Не удалось создать опрос');
    }
}

export async function updateSurvey(survey: Survey): Promise<void> {
    try {
        const request = mapSurveyToUpdateRequest(survey);
        await api.put("/survey-manage/api/survey", request);
    } catch (error) {
        throw new Error('Не удалось обновить опрос');
    }
}


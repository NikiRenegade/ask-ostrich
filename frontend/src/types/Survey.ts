import type {Question, QuestionEdit} from "./Question.ts";

export interface Survey {
    SurveyId: string;
    Title: string;
    Description: string;
    IsPublished: boolean;
    AuthorID: string;
    CreatedAt: string;
    ShortUrl: string;
    Questions: Question[];
}

export interface SurveyEdit {
    Title: string;
    Description: string;
    Questions: QuestionEdit[];
}
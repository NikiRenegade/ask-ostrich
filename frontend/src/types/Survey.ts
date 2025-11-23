import type {Question} from "./Question.ts";

export interface Survey {
    SurveyId: string;
    Title: string;
    Description: string;
    IsPublished: boolean;
    AuthorGuid: string;
    CreatedAt: string;
    ShortUrl: string;
    Questions: Question[];
}
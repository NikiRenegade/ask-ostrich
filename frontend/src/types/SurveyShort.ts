export interface SurveyShort {
    id: string;
    title: string;
    description: string;
    isPublished: boolean;
    authorGuid: string;
    createdAt: string;
    shortUrl: string;
    shortUrlCode: string;
    questionCount: number;
}
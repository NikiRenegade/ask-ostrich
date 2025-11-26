export interface SurveyShort {
    id: string;
    title: string;
    description: string;
    isPublished: boolean;
    authorGuid: string;
    createdAt: string;
    questionCount: number;
}
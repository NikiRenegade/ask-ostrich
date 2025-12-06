import type {Option, OptionEdit} from "./Option.ts";
import type { QuestionType } from "./QuestionType.ts";
export interface Question {
    QuestionId: string;
    Title: string;
    Type: QuestionType;
    Order: number;
    InnerText: string;
    Options: Option[];
}

export interface QuestionEdit {
    Title: string;
    Type: QuestionType;
    Order: number;
    InnerText: string;
    Options?: OptionEdit[];
}
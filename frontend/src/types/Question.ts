import type {Option, OptionYaml} from "./Option.ts";
import type { QuestionType } from "./QuestionType.ts";
export interface Question {
    QuestionId: string;
    Type: QuestionType;
    Title: string;
    Order: number;
    InnerText: string;
    Options: Option[];
}

export interface QuestionYaml {
    Type: QuestionType;
    Title: string;
    Order: number;
    InnerText: string;
    Options?: OptionYaml[];
}
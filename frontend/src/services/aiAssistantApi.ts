import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import type { GenerateSurveyRequest, GeneratedSurvey } from '../models/aiAssistantModels';
import api from './axios';

export async function generateSurvey(request: GenerateSurveyRequest): Promise<GeneratedSurvey> {

    try {
        const response = await api.post("/ai-assistant/api/AIAssistant/generate", {
            prompt: request.prompt,
            currentSurveyJson: request.currentSurveyJson,
            type: request.type,
            surveyId: request.surveyId
        });
        return await response.data;

    } catch (error) {
        throw new Error('Ошибка получения ответа от ИИ-ассистента');
    }
}

export async function askLLM(request: GenerateSurveyRequest): Promise<string> {

    try {
        const response = await api.post("/ai-assistant/api/AIAssistant/ask", {
            prompt: request.prompt,
            currentSurveyJson: request.currentSurveyJson,
            type: request.type,
            surveyId: request.surveyId
        });
        return await response.data;

    } catch (error) {
        throw new Error('Ошибка получения ответа от ИИ-ассистента');
    }
}

export interface DialogMessage {
    content: string;
    isUserMessage: boolean;
    timestamp: string;
}

export async function getDialogHistory(surveyId: string): Promise<DialogMessage[]> {
    try {
        const response = await api.get<DialogMessage[]>(`/ai-assistant/api/AIAssistant/history/${surveyId}`);
        return response.data;
    } catch (error) {
        console.error('Failed to load dialog history:', error);
        return [];
    }
}

export type LlamaHubEvents =
  | { type: "Completed"; data: GeneratedSurvey | string }
  | { type: "Next"; data: string }
  | { type: "Progress"; data: number }
  | { type: "Error"; data: string }
  | { type: "Fatal"; data: string }
  | { type: "Connected"; }
  | { type: "Disconnected"};

type Listeners = {
  onEvent: (event: LlamaHubEvents) => void;
};

class LlamaHubClient {
    private connection: HubConnection | null = null;
    private listeners: Listeners[] = [];
    private url = "/ai-assistant/api/AIAssistant/stream";

    subscribe(listener: Listeners) {
        this.listeners.push(listener);
    }

    unsubscribe(listener: Listeners) {
        this.listeners = this.listeners.filter(l => l !== listener);
    }

    private notify(event: LlamaHubEvents) {
        this.listeners.forEach(l => l.onEvent(event));
    }

    async connect() {
        if (this.connection) return;

        this.connection = new HubConnectionBuilder()
            .withUrl(this.url, {withCredentials: true})
            .withAutomaticReconnect([0, 1000])
            .build();

        this.connection.on("Completed", (data: GeneratedSurvey) => {
            this.notify({type: "Completed", data: data});
        });

        this.connection.on("Progress", (data: number) => {
            this.notify({type: "Progress", data: data});
        });

        this.connection.on("Error", (data: string) => {
            this.notify({type: "Error", data: data});
        });

        this.connection.on("next", (data: string) => {
            this.notify({type: "Next", data: data});
        });

        this.connection.on("complete", (data: string) => {
            this.notify({type: "Next", data: data});
        });

         this.connection.on("error", (data: string) => {
            this.notify({type: "Error", data: data});
        });


        this.connection.onclose(() => this.notify({ type: "Disconnected"}));

        try {
            await this.connection.start();
            this.notify({ type: "Connected" });
        } catch (err) {
            this.notify({ type: "Fatal", data: "Не удалось подключиться к серверу"});
        }
    }

    async generateSurvey(request: GenerateSurveyRequest) {
        if (!this.connection || this.connection.state !== HubConnectionState.Connected){
            throw new Error("Не удалось подключиться к серверу");
        } 
        await this.connection.send("GenerateSurvey", {
            prompt: request.prompt,
            currentSurveyJson: request.currentSurveyJson,
            type: request.type,
            surveyId: request.surveyId
        });
    }

    askLLM(request: GenerateSurveyRequest) {
        if (!this.connection || this.connection.state !== HubConnectionState.Connected){
            throw new Error("Не удалось подключиться к серверу");
        } 
        return this.connection.invoke("AskLLMStream", {
            prompt: request.prompt,
            currentSurveyJson: request.currentSurveyJson,
            type: request.type,
            surveyId: request.surveyId
        });
    }

    async disconnect() {
        if (this.connection) {
            await this.connection.stop();
            this.connection = null;
            this.notify({type: "Disconnected"});
        }
    }
}

export const llamaHub = new LlamaHubClient();


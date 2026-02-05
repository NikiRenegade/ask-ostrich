import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import type { SurveyResultDto } from '../services/surveyResultApi';

export type SurveyHubEvent =
    | { type: "ResultsUpdated"; data: SurveyResultDto[] }
    | { type: "Connected" }
    | { type: "Disconnected" }
    | { type: "Error"; data: string };

type Listener = {
    onEvent: (event: SurveyHubEvent) => void;
};

class SurveyHubClient {
    private connection: HubConnection | null = null;
    private listeners: Listener[] = [];
    private url = "/survey-response/api/survey-result/surveyhub";
    private startPromise: Promise<void> | null = null;

    subscribe(listener: Listener) {
        this.listeners.push(listener);
    }

    unsubscribe(listener: Listener) {
        this.listeners = this.listeners.filter(l => l !== listener);
    }

    private notify(event: SurveyHubEvent) {
        this.listeners.forEach(l => l.onEvent(event));
    }

    async connect() {
        if (this.connection && this.connection.state === HubConnectionState.Connected) {
            return;
        }

        if (this.startPromise) return this.startPromise;

        this.connection = new HubConnectionBuilder()
            .withUrl(this.url, { withCredentials: true })
            .withAutomaticReconnect()
            .build();
        
        const subscribeToHub = () => {
            this.connection!.on("SurveyResultsUpdated", (data: SurveyResultDto[]) => {
                this.notify({ type: "ResultsUpdated", data });
            });
            this.connection!.onclose(() => this.notify({ type: "Disconnected" }));
        };

        subscribeToHub();
        
        this.connection.onreconnected(() => {
            subscribeToHub();
        });

        this.startPromise = this.connection.start()
            .then(() => {
                this.notify({ type: "Connected" });
            })
            .catch(() => {
                this.notify({ type: "Error", data: "Ошибка подключения к хабу" });
            })
            .finally(() => {
                this.startPromise = null;
            });

        return this.startPromise;
    }
    
    async joinSurvey(surveyId: string) {
        await this.connect();
        const guid = surveyId;

        if (!this.connection || this.connection.state !== HubConnectionState.Connected) {
            throw new Error("SignalR не подключён");
        }
        await this.connection.invoke("JoinSurveyGroup", guid);
        await this.connection.invoke("RequestSurveyResults", guid);
        console.log(`Joining survey with id ${guid}`);
    }
    
    async leaveSurvey(surveyId: string) {
        if (!this.connection || this.connection.state !== HubConnectionState.Connected)
            return;
        await this.connection.invoke("LeaveSurveyGroup", surveyId);
    }
}

export const surveyHub = new SurveyHubClient();

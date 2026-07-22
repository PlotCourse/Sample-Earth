import { HubConnection, HubConnectionState } from "@node_modules/@microsoft/signalr/dist/esm/HubConnection";
import { HubConnectionBuilder } from "@node_modules/@microsoft/signalr/dist/esm/HubConnectionBuilder";
import { IHttpConnectionOptions } from "@node_modules/@microsoft/signalr/dist/esm/IHttpConnectionOptions";
import { LogLevel } from "@node_modules/@microsoft/signalr/dist/esm/ILogger";
import { LoggableEvent, LoggableEventType } from "../../../../Earth.Shared/ts/interfaces/loggable-event.g";
import { ConfirmationExpectation } from "../../../../Earth.Shared/ts/interfaces/confirmation-expectation.g";
import { RetryPolicy } from "../../../../Earth.Shared/ts/retry-policy";
import * as ContractData from "../../../../Earth.Ocean.Contract/ts/data/definitions.g";

export abstract class BaseOceanSurfaceBroadcast {
    private _hearbeatTimerId = 0;

    protected _hubConnection: HubConnection;
    protected _handleLoggableEvent: (event: LoggableEvent) => void;

    public get connectionState(): HubConnectionState {
        return this._hubConnection.state;
    }

    constructor(
        hubUrl: string,
        handleLoggableEvent: (event: LoggableEvent) => void) {

        this._handleLoggableEvent = handleLoggableEvent;
        this._hubConnection = this.buildHubConnection(hubUrl);

        this.setupHubConnection(new RetryPolicy([0,0,5,5,30], true));
        this.connectWithRetry(false, new RetryPolicy([5,5,30], true));
    }

    protected abstract processTemperature(
        temperature: number): void;
    protected abstract processNewHurricane(
        location: ContractData.HurricaneCoordinate): void;

    protected buildHubConnectionOptions(): IHttpConnectionOptions {
        return {};
    }

    protected getHubLogLevel(): LogLevel {
        return LogLevel.Information;
    }

    protected buildHubConnection(hubUrl: string): HubConnection {
        return new HubConnectionBuilder()
            .withUrl(hubUrl, this.buildHubConnectionOptions())
            .configureLogging(this.getHubLogLevel())
            .build();
    }

    protected setupHubConnection(disconnectionRetryPolicy: RetryPolicy): void {
        this._hubConnection.onclose(async () => {

            disconnectionRetryPolicy.reset();
            this.connectWithRetry(true, disconnectionRetryPolicy);
        });


        this._hubConnection.on("InitializeObservables", (
            observables: ContractData.OceanSurfaceBroadcastObservables
        ) => {
            try {

                this.processTemperature(observables.temperature);
            } catch (err) {
                var event = {
                    eventType: LoggableEventType.BroadcastHubInitializeObservablesError,
                    status: 0,
                    error: err,
                    sourcePath: "Earth.Ocean.Client\ts\broadcasts\base\base-ocean-surface-broadcast.g.ts",
                    message: `Unexpected error in callback for InitializeObservables.`,
                } as LoggableEvent;

                this.handleLoggableEventForUnexpectedError(event);
            }
        });

        this._hubConnection.on("UpdateTemperature", (
            temperature: number
        ) => {
            try {
                this.processTemperature(temperature);
            } catch (err) {
                var event = {
                    eventType: LoggableEventType.BroadcastHubObservableUpdateError,
                    status: 0,
                    error: err,
                    sourcePath: "Earth.Ocean.Client\ts\broadcasts\base\base-ocean-surface-broadcast.g.ts",
                    message: `Unexpected error in callback for an Observable (UpdateTemperature).`,
                } as LoggableEvent;

                this.handleLoggableEventForUnexpectedError(event);
            }
        });

        this._hubConnection.on("NewHurricane", (
            location: ContractData.HurricaneCoordinate) => {
            try {
                this.processNewHurricane(
                    location);
            } catch (err) {
                var event = {
                    eventType: LoggableEventType.BroadcastHubMessageError,
                    status: 0,
                    error: err,
                    sourcePath: "Earth.Ocean.Client\ts\broadcasts\base\base-ocean-surface-broadcast.g.ts",
                    message: `Unexpected error in callback for Message (NewHurricane).`,
                } as LoggableEvent;

                this.handleLoggableEventForUnexpectedError(event);
            }
        });
    }

    protected handleLoggableEventForConnectionError(event: LoggableEvent): void {
        this._handleLoggableEvent(event);
    }

    protected handleLoggableEventForUnexpectedError(event: LoggableEvent): void {
        this._handleLoggableEvent(event);
    }

    protected async connectWithRetry(firstIsRetry: boolean, retryPolicy: RetryPolicy): Promise<void> {
        var nextDelayInMs = retryPolicy.nextRetryDelayInMs();

        if (!firstIsRetry) {
            try {
                await this._hubConnection.start();


                return;
            } catch (err) {
                if (nextDelayInMs == null) {
                    var event = {
                        eventType: LoggableEventType.BroadcastHubConnectionError,
                        status: 0,
                        error: err,
                        sourcePath: "Earth.Ocean.Client\ts\broadcasts\base\base-ocean-surface-broadcast.g.ts",
                        message: `Unable to start a connection to the hub within constraints of the retry policy.`,
                    } as LoggableEvent;

                    this.handleLoggableEventForConnectionError(event);

                    return;
                }
            }
        }

        setTimeout(() => this.connectWithRetry(false, retryPolicy), nextDelayInMs);
    }
}

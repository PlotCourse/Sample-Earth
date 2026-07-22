export enum LoggableEventType {
    FetchError,
    ResponseNotOk,
    ResponseParseFailure,
    BroadcastHubConnectionError,
    BroadcastHubConfirmReceiptError,
    BroadcastHubSendHeartbeatError,
    BroadcastHubInitializeObservablesError,
    BroadcastHubObservableUpdateError,
    BroadcastHubMessageError,
    Other
}

export interface LoggableEvent {
    eventType: LoggableEventType;
    status: number;
    error: Error;
    sourcePath: string;
    message: string;
}

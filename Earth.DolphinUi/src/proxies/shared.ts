import { LoggableEvent } from "../../../Earth.Shared/ts/interfaces/loggable-event.g";

// this can be updated as needed for any request preparation common to proxies for all dependencies.
export function prepareRequest(request: Request): Request {
    return request;
}

// this can be updated as needed for any event handling strategy common to proxies for all dependencies.
export function handleLoggableEvent(event: LoggableEvent): void {
    console.warn(event.message);
}

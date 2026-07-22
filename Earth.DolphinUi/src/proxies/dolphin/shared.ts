import { LoggableEvent } from "../../../../Earth.Shared/ts/interfaces/loggable-event.g";
import {
    prepareRequest as defaultPrepareRequest,
    handleLoggableEvent as defaultHandleLoggableEvent
} from "../shared";

// update exported function here as needed for any request preparation common to all Dolphin proxies.
export function prepareRequest(request: Request): Request {
    return defaultPrepareRequest(request);
}

// this can be updated as needed for any event handling strategy common to all Dolphin proxies.
export function handleLoggableEvent(event: LoggableEvent): void {
    defaultHandleLoggableEvent(event);
}

import { LoggableEvent } from "../../../Earth.Shared/ts/interfaces/loggable-event.g";
import { BaseTricksService } from "./base/base-tricks-service.g";

export abstract class TricksService extends BaseTricksService {
    constructor(
        proxyUri: string,
        prepareRequest: (request: Request) => Request,
        handleLoggableEvent: (event: LoggableEvent) => void) {

        super(proxyUri, prepareRequest, handleLoggableEvent);
    }
}

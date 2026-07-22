import { LoggableEvent } from "../../../Earth.Shared/ts/interfaces/loggable-event.g";
import { BaseWaterService } from "./base/base-water-service.g";

export abstract class WaterService extends BaseWaterService {
    constructor(
        proxyUri: string,
        prepareRequest: (request: Request) => Request,
        handleLoggableEvent: (event: LoggableEvent) => void) {

        super(proxyUri, prepareRequest, handleLoggableEvent);
    }
}

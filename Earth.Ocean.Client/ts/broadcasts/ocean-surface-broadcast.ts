import { LoggableEvent } from "../../../Earth.Shared/ts/interfaces/loggable-event.g";
import { BaseOceanSurfaceBroadcast } from "./base/base-ocean-surface-broadcast.g";

export abstract class OceanSurfaceBroadcast extends BaseOceanSurfaceBroadcast {
    constructor(
        hubUrl: string,
        handleLoggableEvent: (event: LoggableEvent) => void) {

        super(hubUrl, handleLoggableEvent);
    }
}

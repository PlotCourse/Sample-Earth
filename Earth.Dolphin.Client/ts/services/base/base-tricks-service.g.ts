import { LoggableEvent, LoggableEventType } from "../../../../Earth.Shared/ts/interfaces/loggable-event.g";
import { ReadOnlyRecords } from "../../../../Earth.Shared/ts/interfaces/read-only-records.g";
import * as ContractData from "../../../../Earth.Dolphin.Contract/ts/data/definitions.g";

export abstract class BaseTricksService {
    protected _proxyUri: string;
    protected _prepareRequest: (request: Request) => Request;
    protected _handleLoggableEvent: (event: LoggableEvent) => void;

    constructor(
        proxyUri: string,
        prepareRequest: (request: Request) => Request,
        handleLoggableEvent: (event: LoggableEvent) => void) {

        this._proxyUri = proxyUri;
        this._prepareRequest = prepareRequest;
        this._handleLoggableEvent = handleLoggableEvent;
    }


    protected contractBackflipPrepareRequest(
        jsFetchRequest: Request
    ): Request {
        return this._prepareRequest(jsFetchRequest);
    }

    protected handleLoggableEventForBackflip(event: LoggableEvent): void {
        this._handleLoggableEvent(event);
    }

    protected async contractBackflip(
    ): Promise<ReadOnlyRecords<ContractData.DolphinCoordinate>> {
        var jsFetchRequest = this.contractBackflipPrepareRequest(
            new Request(`${this._proxyUri}/dolphin/tricks/dolphincoordinate`, {
                method: "GET",
                cache: "no-store"
            }));

        var response: Response;

        try {
            response = await fetch(jsFetchRequest);
        } catch (e) {
            var event = {
                eventType: LoggableEventType.FetchError,
                status: 0,
                error: e,
                sourcePath: "Earth.Dolphin.Client/ts/services/base/base-tricks-service.g.ts",
                message: `Unexpected error in fetch call to Backflip.`,
            } as LoggableEvent;

            this.handleLoggableEventForBackflip(event);

            return null;
        }

        if (!response.ok) {
            var event = {
                eventType: LoggableEventType.ResponseNotOk,
                status: response.status,
                error: undefined,
                sourcePath: "Earth.Dolphin.Client/ts/services/base/base-tricks-service.g.ts",
                message: `Unexpected response code (${response.status}) from fetch for Backflip.`,
            } as LoggableEvent;

            this.handleLoggableEventForBackflip(event);

            return null;
        }

        try {
            var result = await response.json();

            return result as ReadOnlyRecords<ContractData.DolphinCoordinate>;
        } catch (e) {
            var event = {
                eventType: LoggableEventType.ResponseParseFailure,
                status: 0,
                error: e,
                sourcePath: "Earth.Dolphin.Client/ts/services/base/base-tricks-service.g.ts",
                message: `Unexpected error attempting to parse response JSON in Backflip.`,
            } as LoggableEvent;

            this.handleLoggableEventForBackflip(event);

            return null;
        }
    }
}

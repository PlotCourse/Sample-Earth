import { LoggableEvent, LoggableEventType } from "../../../../Earth.Shared/ts/interfaces/loggable-event.g";
import { Result, ResultItem } from "../../../../Earth.Shared/ts/interfaces/results.g";
import * as ContractData from "../../../../Earth.Ocean.Contract/ts/data/definitions.g";

export abstract class BaseWaterService {
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


    protected contractUpdateWaterStatePrepareRequest(
        jsFetchRequest: Request
    ): Request {
        return this._prepareRequest(jsFetchRequest);
    }

    protected handleLoggableEventForUpdateWaterState(event: LoggableEvent): void {
        this._handleLoggableEvent(event);
    }

    protected async contractUpdateWaterState(
    ): Promise<void> {
        var jsFetchRequest = this.contractUpdateWaterStatePrepareRequest(
            new Request(`${this._proxyUri}/ocean/water`, {
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
                sourcePath: "Earth.Ocean.Client/ts/services/base/base-water-service.g.ts",
                message: `Unexpected error in fetch call to UpdateWaterState.`,
            } as LoggableEvent;

            this.handleLoggableEventForUpdateWaterState(event);

            return;
        }

        if (!response.ok) {
            var event = {
                eventType: LoggableEventType.ResponseNotOk,
                status: response.status,
                error: undefined,
                sourcePath: "Earth.Ocean.Client/ts/services/base/base-water-service.g.ts",
                message: `Unexpected response code (${response.status}) from fetch for UpdateWaterState.`,
            } as LoggableEvent;

            this.handleLoggableEventForUpdateWaterState(event);

            return;
        }
    }

    protected contractGetDolphinCoordinatePrepareRequest(
        dolphinCoordinateId: number,
        jsFetchRequest: Request
    ): Request {
        return this._prepareRequest(jsFetchRequest);
    }

    protected handleLoggableEventForGetDolphinCoordinate(event: LoggableEvent): void {
        this._handleLoggableEvent(event);
    }

    protected async contractGetDolphinCoordinate(
        dolphinCoordinateId: number
    ): Promise<ResultItem<ContractData.DolphinCoordinate>> {
        var jsFetchRequest = this.contractGetDolphinCoordinatePrepareRequest(
            dolphinCoordinateId,
            new Request(`${this._proxyUri}/ocean/water/dolphincoordinate/${dolphinCoordinateId}`, {
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
                sourcePath: "Earth.Ocean.Client/ts/services/base/base-water-service.g.ts",
                message: `Unexpected error in fetch call to GetDolphinCoordinate.`,
            } as LoggableEvent;

            this.handleLoggableEventForGetDolphinCoordinate(event);

            return {
                succeeded: false,
                message: event.message,
                item: null
            };
        }

        if (!response.ok) {
            var event = {
                eventType: LoggableEventType.ResponseNotOk,
                status: response.status,
                error: undefined,
                sourcePath: "Earth.Ocean.Client/ts/services/base/base-water-service.g.ts",
                message: `Unexpected response code (${response.status}) from fetch for GetDolphinCoordinate.`,
            } as LoggableEvent;

            this.handleLoggableEventForGetDolphinCoordinate(event);

            return {
                succeeded: false,
                message: event.message,
                item: null
            };
        }

        try {
            var result = await response.json();

            return result as ResultItem<ContractData.DolphinCoordinate>;
        } catch (e) {
            var event = {
                eventType: LoggableEventType.ResponseParseFailure,
                status: 0,
                error: e,
                sourcePath: "Earth.Ocean.Client/ts/services/base/base-water-service.g.ts",
                message: `Unexpected error attempting to parse response JSON in GetDolphinCoordinate.`,
            } as LoggableEvent;

            this.handleLoggableEventForGetDolphinCoordinate(event);

            return {
                succeeded: false,
                message: event.message,
                item: null
            };
        }
    }

    protected contractAddDolphinCoordinatePrepareRequest(
        dolphinCoordinate: ContractData.DolphinCoordinate,
        jsFetchRequest: Request
    ): Request {
        return this._prepareRequest(jsFetchRequest);
    }

    protected handleLoggableEventForAddDolphinCoordinate(event: LoggableEvent): void {
        this._handleLoggableEvent(event);
    }

    protected async contractAddDolphinCoordinate(
        dolphinCoordinate: ContractData.DolphinCoordinate
    ): Promise<ResultItem<ContractData.DolphinCoordinate>> {
        var jsFetchRequest = this.contractAddDolphinCoordinatePrepareRequest(
            dolphinCoordinate,
            new Request(`${this._proxyUri}/ocean/water/dolphincoordinate`, {
                method: "POST",
                cache: "no-store",
                body: JSON.stringify(dolphinCoordinate),
                headers: {
                    "Content-Type": "application/json"
                }
            }));

        var response: Response;

        try {
            response = await fetch(jsFetchRequest);
        } catch (e) {
            var event = {
                eventType: LoggableEventType.FetchError,
                status: 0,
                error: e,
                sourcePath: "Earth.Ocean.Client/ts/services/base/base-water-service.g.ts",
                message: `Unexpected error in fetch call to AddDolphinCoordinate.`,
            } as LoggableEvent;

            this.handleLoggableEventForAddDolphinCoordinate(event);

            return {
                succeeded: false,
                message: event.message,
                item: null
            };
        }

        if (!response.ok) {
            var event = {
                eventType: LoggableEventType.ResponseNotOk,
                status: response.status,
                error: undefined,
                sourcePath: "Earth.Ocean.Client/ts/services/base/base-water-service.g.ts",
                message: `Unexpected response code (${response.status}) from fetch for AddDolphinCoordinate.`,
            } as LoggableEvent;

            this.handleLoggableEventForAddDolphinCoordinate(event);

            return {
                succeeded: false,
                message: event.message,
                item: null
            };
        }

        try {
            var result = await response.json();

            return result as ResultItem<ContractData.DolphinCoordinate>;
        } catch (e) {
            var event = {
                eventType: LoggableEventType.ResponseParseFailure,
                status: 0,
                error: e,
                sourcePath: "Earth.Ocean.Client/ts/services/base/base-water-service.g.ts",
                message: `Unexpected error attempting to parse response JSON in AddDolphinCoordinate.`,
            } as LoggableEvent;

            this.handleLoggableEventForAddDolphinCoordinate(event);

            return {
                succeeded: false,
                message: event.message,
                item: null
            };
        }
    }

    protected contractReplaceDolphinCoordinatePrepareRequest(
        dolphinCoordinateId: number,
        dolphinCoordinatePut: ContractData.DolphinCoordinatePut,
        jsFetchRequest: Request
    ): Request {
        return this._prepareRequest(jsFetchRequest);
    }

    protected handleLoggableEventForReplaceDolphinCoordinate(event: LoggableEvent): void {
        this._handleLoggableEvent(event);
    }

    protected async contractReplaceDolphinCoordinate(
        dolphinCoordinateId: number,
        dolphinCoordinatePut: ContractData.DolphinCoordinatePut
    ): Promise<ResultItem<ContractData.DolphinCoordinate>> {
        var jsFetchRequest = this.contractReplaceDolphinCoordinatePrepareRequest(
            dolphinCoordinateId,
            dolphinCoordinatePut,
            new Request(`${this._proxyUri}/ocean/water/dolphincoordinate/${dolphinCoordinateId}`, {
                method: "PUT",
                cache: "no-store",
                body: JSON.stringify(dolphinCoordinatePut),
                headers: {
                    "Content-Type": "application/json"
                }
            }));

        var response: Response;

        try {
            response = await fetch(jsFetchRequest);
        } catch (e) {
            var event = {
                eventType: LoggableEventType.FetchError,
                status: 0,
                error: e,
                sourcePath: "Earth.Ocean.Client/ts/services/base/base-water-service.g.ts",
                message: `Unexpected error in fetch call to ReplaceDolphinCoordinate.`,
            } as LoggableEvent;

            this.handleLoggableEventForReplaceDolphinCoordinate(event);

            return {
                succeeded: false,
                message: event.message,
                item: null
            };
        }

        if (!response.ok) {
            var event = {
                eventType: LoggableEventType.ResponseNotOk,
                status: response.status,
                error: undefined,
                sourcePath: "Earth.Ocean.Client/ts/services/base/base-water-service.g.ts",
                message: `Unexpected response code (${response.status}) from fetch for ReplaceDolphinCoordinate.`,
            } as LoggableEvent;

            this.handleLoggableEventForReplaceDolphinCoordinate(event);

            return {
                succeeded: false,
                message: event.message,
                item: null
            };
        }

        try {
            var result = await response.json();

            return result as ResultItem<ContractData.DolphinCoordinate>;
        } catch (e) {
            var event = {
                eventType: LoggableEventType.ResponseParseFailure,
                status: 0,
                error: e,
                sourcePath: "Earth.Ocean.Client/ts/services/base/base-water-service.g.ts",
                message: `Unexpected error attempting to parse response JSON in ReplaceDolphinCoordinate.`,
            } as LoggableEvent;

            this.handleLoggableEventForReplaceDolphinCoordinate(event);

            return {
                succeeded: false,
                message: event.message,
                item: null
            };
        }
    }

    protected contractDeleteDolphinCoordinatePrepareRequest(
        dolphinCoordinateId: number,
        jsFetchRequest: Request
    ): Request {
        return this._prepareRequest(jsFetchRequest);
    }

    protected handleLoggableEventForDeleteDolphinCoordinate(event: LoggableEvent): void {
        this._handleLoggableEvent(event);
    }

    protected async contractDeleteDolphinCoordinate(
        dolphinCoordinateId: number
    ): Promise<Result> {
        var jsFetchRequest = this.contractDeleteDolphinCoordinatePrepareRequest(
            dolphinCoordinateId,
            new Request(`${this._proxyUri}/ocean/water/dolphincoordinate/${dolphinCoordinateId}`, {
                method: "DELETE",
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
                sourcePath: "Earth.Ocean.Client/ts/services/base/base-water-service.g.ts",
                message: `Unexpected error in fetch call to DeleteDolphinCoordinate.`,
            } as LoggableEvent;

            this.handleLoggableEventForDeleteDolphinCoordinate(event);

            return {
                succeeded: false,
                message: event.message
            };
        }

        if (!response.ok) {
            var event = {
                eventType: LoggableEventType.ResponseNotOk,
                status: response.status,
                error: undefined,
                sourcePath: "Earth.Ocean.Client/ts/services/base/base-water-service.g.ts",
                message: `Unexpected response code (${response.status}) from fetch for DeleteDolphinCoordinate.`,
            } as LoggableEvent;

            this.handleLoggableEventForDeleteDolphinCoordinate(event);

            return {
                succeeded: false,
                message: event.message
            };
        }

        try {
            var result = await response.json();

            return result as Result;
        } catch (e) {
            var event = {
                eventType: LoggableEventType.ResponseParseFailure,
                status: 0,
                error: e,
                sourcePath: "Earth.Ocean.Client/ts/services/base/base-water-service.g.ts",
                message: `Unexpected error attempting to parse response JSON in DeleteDolphinCoordinate.`,
            } as LoggableEvent;

            this.handleLoggableEventForDeleteDolphinCoordinate(event);

            return {
                succeeded: false,
                message: event.message
            };
        }
    }

    protected contractGetHurricaneCoordinatePrepareRequest(
        hurricaneCoordinateId: number,
        jsFetchRequest: Request
    ): Request {
        return this._prepareRequest(jsFetchRequest);
    }

    protected handleLoggableEventForGetHurricaneCoordinate(event: LoggableEvent): void {
        this._handleLoggableEvent(event);
    }

    protected async contractGetHurricaneCoordinate(
        hurricaneCoordinateId: number
    ): Promise<ResultItem<ContractData.HurricaneCoordinate>> {
        var jsFetchRequest = this.contractGetHurricaneCoordinatePrepareRequest(
            hurricaneCoordinateId,
            new Request(`${this._proxyUri}/ocean/water/hurricanecoordinate/${hurricaneCoordinateId}`, {
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
                sourcePath: "Earth.Ocean.Client/ts/services/base/base-water-service.g.ts",
                message: `Unexpected error in fetch call to GetHurricaneCoordinate.`,
            } as LoggableEvent;

            this.handleLoggableEventForGetHurricaneCoordinate(event);

            return {
                succeeded: false,
                message: event.message,
                item: null
            };
        }

        if (!response.ok) {
            var event = {
                eventType: LoggableEventType.ResponseNotOk,
                status: response.status,
                error: undefined,
                sourcePath: "Earth.Ocean.Client/ts/services/base/base-water-service.g.ts",
                message: `Unexpected response code (${response.status}) from fetch for GetHurricaneCoordinate.`,
            } as LoggableEvent;

            this.handleLoggableEventForGetHurricaneCoordinate(event);

            return {
                succeeded: false,
                message: event.message,
                item: null
            };
        }

        try {
            var result = await response.json();

            return result as ResultItem<ContractData.HurricaneCoordinate>;
        } catch (e) {
            var event = {
                eventType: LoggableEventType.ResponseParseFailure,
                status: 0,
                error: e,
                sourcePath: "Earth.Ocean.Client/ts/services/base/base-water-service.g.ts",
                message: `Unexpected error attempting to parse response JSON in GetHurricaneCoordinate.`,
            } as LoggableEvent;

            this.handleLoggableEventForGetHurricaneCoordinate(event);

            return {
                succeeded: false,
                message: event.message,
                item: null
            };
        }
    }

    protected contractAddHurricaneCoordinatePrepareRequest(
        hurricaneCoordinate: ContractData.HurricaneCoordinate,
        jsFetchRequest: Request
    ): Request {
        return this._prepareRequest(jsFetchRequest);
    }

    protected handleLoggableEventForAddHurricaneCoordinate(event: LoggableEvent): void {
        this._handleLoggableEvent(event);
    }

    protected async contractAddHurricaneCoordinate(
        hurricaneCoordinate: ContractData.HurricaneCoordinate
    ): Promise<ResultItem<ContractData.HurricaneCoordinate>> {
        var jsFetchRequest = this.contractAddHurricaneCoordinatePrepareRequest(
            hurricaneCoordinate,
            new Request(`${this._proxyUri}/ocean/water/hurricanecoordinate`, {
                method: "POST",
                cache: "no-store",
                body: JSON.stringify(hurricaneCoordinate),
                headers: {
                    "Content-Type": "application/json"
                }
            }));

        var response: Response;

        try {
            response = await fetch(jsFetchRequest);
        } catch (e) {
            var event = {
                eventType: LoggableEventType.FetchError,
                status: 0,
                error: e,
                sourcePath: "Earth.Ocean.Client/ts/services/base/base-water-service.g.ts",
                message: `Unexpected error in fetch call to AddHurricaneCoordinate.`,
            } as LoggableEvent;

            this.handleLoggableEventForAddHurricaneCoordinate(event);

            return {
                succeeded: false,
                message: event.message,
                item: null
            };
        }

        if (!response.ok) {
            var event = {
                eventType: LoggableEventType.ResponseNotOk,
                status: response.status,
                error: undefined,
                sourcePath: "Earth.Ocean.Client/ts/services/base/base-water-service.g.ts",
                message: `Unexpected response code (${response.status}) from fetch for AddHurricaneCoordinate.`,
            } as LoggableEvent;

            this.handleLoggableEventForAddHurricaneCoordinate(event);

            return {
                succeeded: false,
                message: event.message,
                item: null
            };
        }

        try {
            var result = await response.json();

            return result as ResultItem<ContractData.HurricaneCoordinate>;
        } catch (e) {
            var event = {
                eventType: LoggableEventType.ResponseParseFailure,
                status: 0,
                error: e,
                sourcePath: "Earth.Ocean.Client/ts/services/base/base-water-service.g.ts",
                message: `Unexpected error attempting to parse response JSON in AddHurricaneCoordinate.`,
            } as LoggableEvent;

            this.handleLoggableEventForAddHurricaneCoordinate(event);

            return {
                succeeded: false,
                message: event.message,
                item: null
            };
        }
    }

    protected contractReplaceHurricaneCoordinatePrepareRequest(
        hurricaneCoordinateId: number,
        hurricaneCoordinatePut: ContractData.HurricaneCoordinatePut,
        jsFetchRequest: Request
    ): Request {
        return this._prepareRequest(jsFetchRequest);
    }

    protected handleLoggableEventForReplaceHurricaneCoordinate(event: LoggableEvent): void {
        this._handleLoggableEvent(event);
    }

    protected async contractReplaceHurricaneCoordinate(
        hurricaneCoordinateId: number,
        hurricaneCoordinatePut: ContractData.HurricaneCoordinatePut
    ): Promise<ResultItem<ContractData.HurricaneCoordinate>> {
        var jsFetchRequest = this.contractReplaceHurricaneCoordinatePrepareRequest(
            hurricaneCoordinateId,
            hurricaneCoordinatePut,
            new Request(`${this._proxyUri}/ocean/water/hurricanecoordinate/${hurricaneCoordinateId}`, {
                method: "PUT",
                cache: "no-store",
                body: JSON.stringify(hurricaneCoordinatePut),
                headers: {
                    "Content-Type": "application/json"
                }
            }));

        var response: Response;

        try {
            response = await fetch(jsFetchRequest);
        } catch (e) {
            var event = {
                eventType: LoggableEventType.FetchError,
                status: 0,
                error: e,
                sourcePath: "Earth.Ocean.Client/ts/services/base/base-water-service.g.ts",
                message: `Unexpected error in fetch call to ReplaceHurricaneCoordinate.`,
            } as LoggableEvent;

            this.handleLoggableEventForReplaceHurricaneCoordinate(event);

            return {
                succeeded: false,
                message: event.message,
                item: null
            };
        }

        if (!response.ok) {
            var event = {
                eventType: LoggableEventType.ResponseNotOk,
                status: response.status,
                error: undefined,
                sourcePath: "Earth.Ocean.Client/ts/services/base/base-water-service.g.ts",
                message: `Unexpected response code (${response.status}) from fetch for ReplaceHurricaneCoordinate.`,
            } as LoggableEvent;

            this.handleLoggableEventForReplaceHurricaneCoordinate(event);

            return {
                succeeded: false,
                message: event.message,
                item: null
            };
        }

        try {
            var result = await response.json();

            return result as ResultItem<ContractData.HurricaneCoordinate>;
        } catch (e) {
            var event = {
                eventType: LoggableEventType.ResponseParseFailure,
                status: 0,
                error: e,
                sourcePath: "Earth.Ocean.Client/ts/services/base/base-water-service.g.ts",
                message: `Unexpected error attempting to parse response JSON in ReplaceHurricaneCoordinate.`,
            } as LoggableEvent;

            this.handleLoggableEventForReplaceHurricaneCoordinate(event);

            return {
                succeeded: false,
                message: event.message,
                item: null
            };
        }
    }

    protected contractDeleteHurricaneCoordinatePrepareRequest(
        hurricaneCoordinateId: number,
        jsFetchRequest: Request
    ): Request {
        return this._prepareRequest(jsFetchRequest);
    }

    protected handleLoggableEventForDeleteHurricaneCoordinate(event: LoggableEvent): void {
        this._handleLoggableEvent(event);
    }

    protected async contractDeleteHurricaneCoordinate(
        hurricaneCoordinateId: number
    ): Promise<Result> {
        var jsFetchRequest = this.contractDeleteHurricaneCoordinatePrepareRequest(
            hurricaneCoordinateId,
            new Request(`${this._proxyUri}/ocean/water/hurricanecoordinate/${hurricaneCoordinateId}`, {
                method: "DELETE",
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
                sourcePath: "Earth.Ocean.Client/ts/services/base/base-water-service.g.ts",
                message: `Unexpected error in fetch call to DeleteHurricaneCoordinate.`,
            } as LoggableEvent;

            this.handleLoggableEventForDeleteHurricaneCoordinate(event);

            return {
                succeeded: false,
                message: event.message
            };
        }

        if (!response.ok) {
            var event = {
                eventType: LoggableEventType.ResponseNotOk,
                status: response.status,
                error: undefined,
                sourcePath: "Earth.Ocean.Client/ts/services/base/base-water-service.g.ts",
                message: `Unexpected response code (${response.status}) from fetch for DeleteHurricaneCoordinate.`,
            } as LoggableEvent;

            this.handleLoggableEventForDeleteHurricaneCoordinate(event);

            return {
                succeeded: false,
                message: event.message
            };
        }

        try {
            var result = await response.json();

            return result as Result;
        } catch (e) {
            var event = {
                eventType: LoggableEventType.ResponseParseFailure,
                status: 0,
                error: e,
                sourcePath: "Earth.Ocean.Client/ts/services/base/base-water-service.g.ts",
                message: `Unexpected error attempting to parse response JSON in DeleteHurricaneCoordinate.`,
            } as LoggableEvent;

            this.handleLoggableEventForDeleteHurricaneCoordinate(event);

            return {
                succeeded: false,
                message: event.message
            };
        }
    }
}

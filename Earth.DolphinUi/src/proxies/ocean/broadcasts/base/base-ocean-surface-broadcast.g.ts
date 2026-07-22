import { BehaviorSubject, Observable, ReplaySubject } from "rxjs";
import { skip, takeUntil } from "rxjs/operators";
import { OceanSurfaceBroadcast as BaseClientOceanSurfaceBroadcast } from "../../../../../../Earth.Ocean.Client/ts/broadcasts/ocean-surface-broadcast";
import * as ContractData from "../../../../../../Earth.Ocean.Contract/ts/data/definitions.g";
import * as Data from "../../../../data/definitions.g";
import { IMapRecord } from "../../../../../../Earth.Shared/ts/interfaces/map-record.g";
import { lazyInject } from "../../../../../../Earth.Shared/ts/ioc/ioc";
import { ProxyTypes } from "../../../dependencies/types.g";
import { handleLoggableEvent } from "../../shared";

declare const SERVER_URL_EARTH_SUPPORT: string;

export abstract class BaseOceanSurfaceBroadcast extends BaseClientOceanSurfaceBroadcast {

    @lazyInject(ProxyTypes.Ocean_MapFromContractHurricaneCoordinate)
    private hurricaneCoordinateMapperFromContract!: IMapRecord<ContractData.HurricaneCoordinate, Data.HurricaneCoordinate>;

    private _temperature$ = new BehaviorSubject<number>(null);
    private _messageNewHurricane$ = new BehaviorSubject<{
        location: Data.HurricaneCoordinate}>(null);

    get temperature$(): Observable<number> {
        return this._temperature$.asObservable();
    }
    get temperature(): number {
        return this._temperature$.value;
    }

    constructor() {
        super(
            SERVER_URL_EARTH_SUPPORT + "/ocean/oceansurfacehub",
            handleLoggableEvent
        );
    }

    public subscribeToNewHurricane(
        callback: (
            location: Data.HurricaneCoordinate) => void,
        unsubscribeSubject$: ReplaySubject<void>
    ): void {
        var messageNewHurricane$ = this._messageNewHurricane$.asObservable();
        messageNewHurricane$.pipe(skip(1), takeUntil(unsubscribeSubject$))
            .subscribe({
                next: (params: {
                    location: Data.HurricaneCoordinate}) => {

                    callback(
                        params.location);
                }
            });
    }

    protected processTemperature(temperature: number): void {
        this._temperature$.next(temperature);
    }

    protected processNewHurricane(
        location: ContractData.HurricaneCoordinate): void {

        this._messageNewHurricane$.next({
            location: this.hurricaneCoordinateMapperFromContract.mapRecord(location)
        });
    }
}

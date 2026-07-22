import { Observable, ReplaySubject } from "rxjs";
import * as Data from "../../../../../data/definitions.g";

export interface IBaseOceanSurfaceBroadcast {
    get temperature$(): Observable<number>;
    get temperature(): number;

    subscribeToNewHurricane(
        callback: (
            location: Data.HurricaneCoordinate) => void,
        unsubscribeSubject$: ReplaySubject<void>): void;
}

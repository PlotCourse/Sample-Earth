import { ReadOnlyRecords } from "../../../Earth.Shared/ts/interfaces/read-only-records.g";
export interface DolphinCoordinate {
    get dolphinCoordinateId(): number;
    get x(): number;
    get y(): number;
}

export interface DolphinCoordinatePut {
    get x(): number;
    get y(): number;
}

export interface HurricaneCoordinate {
    get hurricaneCoordinateId(): number;
    get x(): number;
    get y(): number;
}

export interface HurricaneCoordinatePut {
    get x(): number;
    get y(): number;
}

// Used for subscribers to initialize observable values.
export interface OceanSurfaceBroadcastObservables {
    get temperature(): number;
}


export enum OceanSurfaceNotificationType {
    ObservableInitialization,
    Observable_Temperature,
    Message_NewHurricane
}

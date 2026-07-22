import * as Data from "../../../../../data/definitions.g";
import { Result, ResultItem } from "../../../../../../../Earth.Shared/ts/interfaces/results.g";

export interface IBaseWaterService {

    updateWaterState(
    ): Promise<void>;

    getDolphinCoordinate(
        dolphinCoordinateId: number
    ): Promise<ResultItem<Data.DolphinCoordinate>>;

    addDolphinCoordinate(
        dolphinCoordinate: Data.DolphinCoordinate
    ): Promise<ResultItem<Data.DolphinCoordinate>>;

    replaceDolphinCoordinate(
        dolphinCoordinateId: number,
        dolphinCoordinatePut: Data.DolphinCoordinatePut
    ): Promise<ResultItem<Data.DolphinCoordinate>>;

    deleteDolphinCoordinate(
        dolphinCoordinateId: number
    ): Promise<Result>;

    getHurricaneCoordinate(
        hurricaneCoordinateId: number
    ): Promise<ResultItem<Data.HurricaneCoordinate>>;

    addHurricaneCoordinate(
        hurricaneCoordinate: Data.HurricaneCoordinate
    ): Promise<ResultItem<Data.HurricaneCoordinate>>;

    replaceHurricaneCoordinate(
        hurricaneCoordinateId: number,
        hurricaneCoordinatePut: Data.HurricaneCoordinatePut
    ): Promise<ResultItem<Data.HurricaneCoordinate>>;

    deleteHurricaneCoordinate(
        hurricaneCoordinateId: number
    ): Promise<Result>;
}

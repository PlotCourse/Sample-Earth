import { IMapRecord } from "../../../../../../../Earth.Shared/ts/interfaces/map-record.g";
import * as ContractData from "../../../../../../../Earth.Ocean.Contract/ts/data/definitions.g";
import * as InternalData from "../../../../../data/definitions.g";

export class BaseHurricaneCoordinateMapper implements IMapRecord<InternalData.HurricaneCoordinate, ContractData.HurricaneCoordinate> {
    mapRecord(from: InternalData.HurricaneCoordinate): ContractData.HurricaneCoordinate {
        return from;
    }
}

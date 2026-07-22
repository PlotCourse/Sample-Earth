import { IMapRecord } from "../../../../../../../Earth.Shared/ts/interfaces/map-record.g";
import * as ContractData from "../../../../../../../Earth.Ocean.Contract/ts/data/definitions.g";
import * as InternalData from "../../../../../data/definitions.g";

export class BaseHurricaneCoordinateMapper implements IMapRecord<ContractData.HurricaneCoordinate, InternalData.HurricaneCoordinate> {
    mapRecord(from: ContractData.HurricaneCoordinate): InternalData.HurricaneCoordinate {
        return from;
    }
}

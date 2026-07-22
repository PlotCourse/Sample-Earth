import { IMapRecord } from "../../../../../../../Earth.Shared/ts/interfaces/map-record.g";
import * as ContractData from "../../../../../../../Earth.Ocean.Contract/ts/data/definitions.g";
import * as InternalData from "../../../../../data/definitions.g";

export class BaseDolphinCoordinateMapper implements IMapRecord<ContractData.DolphinCoordinate, InternalData.DolphinCoordinate> {
    mapRecord(from: ContractData.DolphinCoordinate): InternalData.DolphinCoordinate {
        return from;
    }
}

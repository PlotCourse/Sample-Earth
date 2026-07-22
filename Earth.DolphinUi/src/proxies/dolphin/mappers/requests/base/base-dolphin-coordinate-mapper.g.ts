import { IMapRecord } from "../../../../../../../Earth.Shared/ts/interfaces/map-record.g";
import * as ContractData from "../../../../../../../Earth.Dolphin.Contract/ts/data/definitions.g";
import * as InternalData from "../../../../../data/definitions.g";

export class BaseDolphinCoordinateMapper implements IMapRecord<InternalData.DolphinCoordinate, ContractData.DolphinCoordinate> {
    mapRecord(from: InternalData.DolphinCoordinate): ContractData.DolphinCoordinate {
        return from;
    }
}

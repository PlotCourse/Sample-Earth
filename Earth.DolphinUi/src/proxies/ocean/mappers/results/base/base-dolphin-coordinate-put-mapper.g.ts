import { IMapRecord } from "../../../../../../../Earth.Shared/ts/interfaces/map-record.g";
import * as ContractData from "../../../../../../../Earth.Ocean.Contract/ts/data/definitions.g";
import * as InternalData from "../../../../../data/definitions.g";

export class BaseDolphinCoordinatePutMapper implements IMapRecord<ContractData.DolphinCoordinatePut, InternalData.DolphinCoordinatePut> {
    mapRecord(from: ContractData.DolphinCoordinatePut): InternalData.DolphinCoordinatePut {
        return from;
    }
}

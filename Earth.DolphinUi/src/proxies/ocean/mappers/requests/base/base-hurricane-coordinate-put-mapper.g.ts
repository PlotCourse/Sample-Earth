import { IMapRecord } from "../../../../../../../Earth.Shared/ts/interfaces/map-record.g";
import * as ContractData from "../../../../../../../Earth.Ocean.Contract/ts/data/definitions.g";
import * as InternalData from "../../../../../data/definitions.g";

export class BaseHurricaneCoordinatePutMapper implements IMapRecord<InternalData.HurricaneCoordinatePut, ContractData.HurricaneCoordinatePut> {
    mapRecord(from: InternalData.HurricaneCoordinatePut): ContractData.HurricaneCoordinatePut {
        return from;
    }
}

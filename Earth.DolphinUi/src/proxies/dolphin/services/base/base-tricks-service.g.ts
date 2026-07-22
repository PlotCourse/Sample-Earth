import { TricksService as BaseClientTricksService } from "../../../../../../Earth.Dolphin.Client/ts/services/tricks-service";
import * as ContractData from "../../../../../../Earth.Dolphin.Contract/ts/data/definitions.g";
import * as Data from "../../../../data/definitions.g";
import { IMapRecord } from "../../../../../../Earth.Shared/ts/interfaces/map-record.g";
import { lazyInject } from "../../../../../../Earth.Shared/ts/ioc/ioc";
import { ProxyTypes } from "../../../dependencies/types.g";
import { handleLoggableEvent, prepareRequest } from "../../shared";

export class BaseTricksService extends BaseClientTricksService {

    @lazyInject(ProxyTypes.Dolphin_MapFromContractDolphinCoordinate)
    private dolphinCoordinateMapperFromContract!: IMapRecord<ContractData.DolphinCoordinate, Data.DolphinCoordinate>;

    constructor() {
        super('/earth-main', prepareRequest, handleLoggableEvent);
    }

    public async backflip(
    ): Promise<Data.DolphinCoordinate[]> {
        var result = await this.contractBackflip(
        );
        return result?.records?.map(d => this.dolphinCoordinateMapperFromContract.mapRecord(d));
    }
}

import { WaterService as BaseClientWaterService } from "../../../../../../Earth.Ocean.Client/ts/services/water-service";
import * as ContractData from "../../../../../../Earth.Ocean.Contract/ts/data/definitions.g";
import * as Data from "../../../../data/definitions.g";
import { IMapRecord } from "../../../../../../Earth.Shared/ts/interfaces/map-record.g";
import { lazyInject } from "../../../../../../Earth.Shared/ts/ioc/ioc";
import { ProxyTypes } from "../../../dependencies/types.g";
import { handleLoggableEvent, prepareRequest } from "../../shared";
import { Result, ResultItem } from "../../../../../../Earth.Shared/ts/interfaces/results.g";

export class BaseWaterService extends BaseClientWaterService {

    @lazyInject(ProxyTypes.Ocean_MapFromContractDolphinCoordinate)
    private dolphinCoordinateMapperFromContract!: IMapRecord<ContractData.DolphinCoordinate, Data.DolphinCoordinate>;

    @lazyInject(ProxyTypes.Ocean_MapToContractDolphinCoordinate)
    private dolphinCoordinateMapperToContract!: IMapRecord<Data.DolphinCoordinate, ContractData.DolphinCoordinate>;

    @lazyInject(ProxyTypes.Ocean_MapToContractDolphinCoordinatePut)
    private dolphinCoordinatePutMapperToContract!: IMapRecord<Data.DolphinCoordinatePut, ContractData.DolphinCoordinatePut>;

    @lazyInject(ProxyTypes.Ocean_MapFromContractHurricaneCoordinate)
    private hurricaneCoordinateMapperFromContract!: IMapRecord<ContractData.HurricaneCoordinate, Data.HurricaneCoordinate>;

    @lazyInject(ProxyTypes.Ocean_MapToContractHurricaneCoordinate)
    private hurricaneCoordinateMapperToContract!: IMapRecord<Data.HurricaneCoordinate, ContractData.HurricaneCoordinate>;

    @lazyInject(ProxyTypes.Ocean_MapToContractHurricaneCoordinatePut)
    private hurricaneCoordinatePutMapperToContract!: IMapRecord<Data.HurricaneCoordinatePut, ContractData.HurricaneCoordinatePut>;

    constructor() {
        super('/earth-support', prepareRequest, handleLoggableEvent);
    }

    public updateWaterState(
    ): Promise<void> {
        return this.contractUpdateWaterState(
        );
    }

    public async getDolphinCoordinate(
        dolphinCoordinateId: number
    ): Promise<ResultItem<Data.DolphinCoordinate>> {
        var result = await this.contractGetDolphinCoordinate(
            dolphinCoordinateId
        );
        return {
            succeeded: result?.succeeded,
            message: result?.message,
            item: this.dolphinCoordinateMapperFromContract.mapRecord(result?.item)
        };
    }

    public async addDolphinCoordinate(
        dolphinCoordinate: Data.DolphinCoordinate
    ): Promise<ResultItem<Data.DolphinCoordinate>> {
        var result = await this.contractAddDolphinCoordinate(
            this.dolphinCoordinateMapperToContract.mapRecord(dolphinCoordinate)
        );
        return {
            succeeded: result?.succeeded,
            message: result?.message,
            item: this.dolphinCoordinateMapperFromContract.mapRecord(result?.item)
        };
    }

    public async replaceDolphinCoordinate(
        dolphinCoordinateId: number,
        dolphinCoordinatePut: Data.DolphinCoordinatePut
    ): Promise<ResultItem<Data.DolphinCoordinate>> {
        var result = await this.contractReplaceDolphinCoordinate(
            dolphinCoordinateId,
            this.dolphinCoordinatePutMapperToContract.mapRecord(dolphinCoordinatePut)
        );
        return {
            succeeded: result?.succeeded,
            message: result?.message,
            item: this.dolphinCoordinateMapperFromContract.mapRecord(result?.item)
        };
    }

    public async deleteDolphinCoordinate(
        dolphinCoordinateId: number
    ): Promise<Result> {
        var result = await this.contractDeleteDolphinCoordinate(
            dolphinCoordinateId
        );
        return result;
    }

    public async getHurricaneCoordinate(
        hurricaneCoordinateId: number
    ): Promise<ResultItem<Data.HurricaneCoordinate>> {
        var result = await this.contractGetHurricaneCoordinate(
            hurricaneCoordinateId
        );
        return {
            succeeded: result?.succeeded,
            message: result?.message,
            item: this.hurricaneCoordinateMapperFromContract.mapRecord(result?.item)
        };
    }

    public async addHurricaneCoordinate(
        hurricaneCoordinate: Data.HurricaneCoordinate
    ): Promise<ResultItem<Data.HurricaneCoordinate>> {
        var result = await this.contractAddHurricaneCoordinate(
            this.hurricaneCoordinateMapperToContract.mapRecord(hurricaneCoordinate)
        );
        return {
            succeeded: result?.succeeded,
            message: result?.message,
            item: this.hurricaneCoordinateMapperFromContract.mapRecord(result?.item)
        };
    }

    public async replaceHurricaneCoordinate(
        hurricaneCoordinateId: number,
        hurricaneCoordinatePut: Data.HurricaneCoordinatePut
    ): Promise<ResultItem<Data.HurricaneCoordinate>> {
        var result = await this.contractReplaceHurricaneCoordinate(
            hurricaneCoordinateId,
            this.hurricaneCoordinatePutMapperToContract.mapRecord(hurricaneCoordinatePut)
        );
        return {
            succeeded: result?.succeeded,
            message: result?.message,
            item: this.hurricaneCoordinateMapperFromContract.mapRecord(result?.item)
        };
    }

    public async deleteHurricaneCoordinate(
        hurricaneCoordinateId: number
    ): Promise<Result> {
        var result = await this.contractDeleteHurricaneCoordinate(
            hurricaneCoordinateId
        );
        return result;
    }
}

import { customElement } from "lit/decorators.js";
import { lazyInject } from "../../../../../../../Earth.Shared/ts/ioc/ioc";
import { recordStringify } from "../../../../../../../Earth.Shared/ts/dev/dev-utils.g";
import { DataInputValueRoot } from "../../../../../../../Earth.Shared/ts/dev/data-input-value-root";
import { DataInputType } from "../../../../../../../Earth.Shared/ts/dev/data-input-type.g";
import { DataInputModifier } from "../../../../../../../Earth.Shared/ts/dev/base/base-data-input.g";
import { InspectService as BaseInspectService } from "../../../../../../../Earth.Shared/ts/dev/custom-elements/inspect-service";
import * as Data from "../../../../../data/definitions.g";
import { ProxyTypes } from "../../../../../proxies/dependencies/types.g";
import { dataInputRecords, dataDefaultFactories, dataInputValuesCache, dataResultCache } from "../../../definitions.g";
import { IWaterService } from "../../../../../proxies/ocean/services/interfaces/water-service";

@customElement("dolphin-ui-ocean-water-service")
export class Water extends BaseInspectService {
    private static readonly cacheKey = {
        updateWaterState: "dolphin-ui-ocean-water-updateWaterState",
        getDolphinCoordinate: "dolphin-ui-ocean-water-getDolphinCoordinate",
        addDolphinCoordinate: "dolphin-ui-ocean-water-addDolphinCoordinate",
        replaceDolphinCoordinate: "dolphin-ui-ocean-water-replaceDolphinCoordinate",
        deleteDolphinCoordinate: "dolphin-ui-ocean-water-deleteDolphinCoordinate",
        getHurricaneCoordinate: "dolphin-ui-ocean-water-getHurricaneCoordinate",
        addHurricaneCoordinate: "dolphin-ui-ocean-water-addHurricaneCoordinate",
        replaceHurricaneCoordinate: "dolphin-ui-ocean-water-replaceHurricaneCoordinate",
        deleteHurricaneCoordinate: "dolphin-ui-ocean-water-deleteHurricaneCoordinate",
    };

    @lazyInject(ProxyTypes.Ocean_IWaterService)
    private waterService!: IWaterService;

    constructor() {
        super(dataInputRecords, dataDefaultFactories, dataInputValuesCache, dataResultCache);

        if (dataInputValuesCache[Water.cacheKey.updateWaterState] === undefined) {
            dataInputValuesCache[Water.cacheKey.updateWaterState] = [
            ];
        }

        this.allMethodsToInspect.push({
            title: ".updateWaterState(): Promise<void>",
            cacheKey: Water.cacheKey.updateWaterState,
            serviceCallMethod: () => this.updateWaterStateInspectorCall()
        });

        if (dataInputValuesCache[Water.cacheKey.getDolphinCoordinate] === undefined) {
            dataInputValuesCache[Water.cacheKey.getDolphinCoordinate] = [
                new DataInputValueRoot<number>(0, DataInputType.InputNumber, "dolphinCoordinateId", DataInputModifier.None, true)
            ];
        }

        this.allMethodsToInspect.push({
            title: ".getDolphinCoordinate(dolphinCoordinateId): Promise<ResultItem<Data.DolphinCoordinate>>",
            cacheKey: Water.cacheKey.getDolphinCoordinate,
            serviceCallMethod: () => this.getDolphinCoordinateInspectorCall()
        });

        if (dataInputValuesCache[Water.cacheKey.addDolphinCoordinate] === undefined) {
            dataInputValuesCache[Water.cacheKey.addDolphinCoordinate] = [
                new DataInputValueRoot<Data.DolphinCoordinate>(dataDefaultFactories["DolphinCoordinate"](), DataInputType.InputDataRecord, "dolphinCoordinate", DataInputModifier.None, true, "DolphinCoordinate")
            ];
        }

        this.allMethodsToInspect.push({
            title: ".addDolphinCoordinate(dolphinCoordinate): Promise<ResultItem<Data.DolphinCoordinate>>",
            cacheKey: Water.cacheKey.addDolphinCoordinate,
            serviceCallMethod: () => this.addDolphinCoordinateInspectorCall()
        });

        if (dataInputValuesCache[Water.cacheKey.replaceDolphinCoordinate] === undefined) {
            dataInputValuesCache[Water.cacheKey.replaceDolphinCoordinate] = [
                new DataInputValueRoot<number>(0, DataInputType.InputNumber, "dolphinCoordinateId", DataInputModifier.None, true),
                new DataInputValueRoot<Data.DolphinCoordinatePut>(dataDefaultFactories["DolphinCoordinatePut"](), DataInputType.InputDataRecord, "dolphinCoordinatePut", DataInputModifier.None, true, "DolphinCoordinatePut")
            ];
        }

        this.allMethodsToInspect.push({
            title: ".replaceDolphinCoordinate(dolphinCoordinateId, dolphinCoordinatePut): Promise<ResultItem<Data.DolphinCoordinate>>",
            cacheKey: Water.cacheKey.replaceDolphinCoordinate,
            serviceCallMethod: () => this.replaceDolphinCoordinateInspectorCall()
        });

        if (dataInputValuesCache[Water.cacheKey.deleteDolphinCoordinate] === undefined) {
            dataInputValuesCache[Water.cacheKey.deleteDolphinCoordinate] = [
                new DataInputValueRoot<number>(0, DataInputType.InputNumber, "dolphinCoordinateId", DataInputModifier.None, true)
            ];
        }

        this.allMethodsToInspect.push({
            title: ".deleteDolphinCoordinate(dolphinCoordinateId): Promise<Result>",
            cacheKey: Water.cacheKey.deleteDolphinCoordinate,
            serviceCallMethod: () => this.deleteDolphinCoordinateInspectorCall()
        });

        if (dataInputValuesCache[Water.cacheKey.getHurricaneCoordinate] === undefined) {
            dataInputValuesCache[Water.cacheKey.getHurricaneCoordinate] = [
                new DataInputValueRoot<number>(0, DataInputType.InputNumber, "hurricaneCoordinateId", DataInputModifier.None, true)
            ];
        }

        this.allMethodsToInspect.push({
            title: ".getHurricaneCoordinate(hurricaneCoordinateId): Promise<ResultItem<Data.HurricaneCoordinate>>",
            cacheKey: Water.cacheKey.getHurricaneCoordinate,
            serviceCallMethod: () => this.getHurricaneCoordinateInspectorCall()
        });

        if (dataInputValuesCache[Water.cacheKey.addHurricaneCoordinate] === undefined) {
            dataInputValuesCache[Water.cacheKey.addHurricaneCoordinate] = [
                new DataInputValueRoot<Data.HurricaneCoordinate>(dataDefaultFactories["HurricaneCoordinate"](), DataInputType.InputDataRecord, "hurricaneCoordinate", DataInputModifier.None, true, "HurricaneCoordinate")
            ];
        }

        this.allMethodsToInspect.push({
            title: ".addHurricaneCoordinate(hurricaneCoordinate): Promise<ResultItem<Data.HurricaneCoordinate>>",
            cacheKey: Water.cacheKey.addHurricaneCoordinate,
            serviceCallMethod: () => this.addHurricaneCoordinateInspectorCall()
        });

        if (dataInputValuesCache[Water.cacheKey.replaceHurricaneCoordinate] === undefined) {
            dataInputValuesCache[Water.cacheKey.replaceHurricaneCoordinate] = [
                new DataInputValueRoot<number>(0, DataInputType.InputNumber, "hurricaneCoordinateId", DataInputModifier.None, true),
                new DataInputValueRoot<Data.HurricaneCoordinatePut>(dataDefaultFactories["HurricaneCoordinatePut"](), DataInputType.InputDataRecord, "hurricaneCoordinatePut", DataInputModifier.None, true, "HurricaneCoordinatePut")
            ];
        }

        this.allMethodsToInspect.push({
            title: ".replaceHurricaneCoordinate(hurricaneCoordinateId, hurricaneCoordinatePut): Promise<ResultItem<Data.HurricaneCoordinate>>",
            cacheKey: Water.cacheKey.replaceHurricaneCoordinate,
            serviceCallMethod: () => this.replaceHurricaneCoordinateInspectorCall()
        });

        if (dataInputValuesCache[Water.cacheKey.deleteHurricaneCoordinate] === undefined) {
            dataInputValuesCache[Water.cacheKey.deleteHurricaneCoordinate] = [
                new DataInputValueRoot<number>(0, DataInputType.InputNumber, "hurricaneCoordinateId", DataInputModifier.None, true)
            ];
        }

        this.allMethodsToInspect.push({
            title: ".deleteHurricaneCoordinate(hurricaneCoordinateId): Promise<Result>",
            cacheKey: Water.cacheKey.deleteHurricaneCoordinate,
            serviceCallMethod: () => this.deleteHurricaneCoordinateInspectorCall()
        });
    }


    private async updateWaterStateInspectorCall(): Promise<void> {
        try {
            var result = await this.waterService.updateWaterState();

            dataResultCache[Water.cacheKey.updateWaterState] = recordStringify(result);
        } catch(ex) {
            dataResultCache[Water.cacheKey.updateWaterState] = `ERROR: ${ex.message}`;
        }

        this.requestUpdate();
    }

    private async getDolphinCoordinateInspectorCall(): Promise<void> {
        var params = dataInputValuesCache[Water.cacheKey.getDolphinCoordinate];

        try {
            var result = await this.waterService.getDolphinCoordinate(
                params[0].value);

            dataResultCache[Water.cacheKey.getDolphinCoordinate] = recordStringify(result);
        } catch(ex) {
            dataResultCache[Water.cacheKey.getDolphinCoordinate] = `ERROR: ${ex.message}`;
        }

        this.requestUpdate();
    }

    private async addDolphinCoordinateInspectorCall(): Promise<void> {
        var params = dataInputValuesCache[Water.cacheKey.addDolphinCoordinate];

        try {
            var result = await this.waterService.addDolphinCoordinate(
                params[0].value);

            dataResultCache[Water.cacheKey.addDolphinCoordinate] = recordStringify(result);
        } catch(ex) {
            dataResultCache[Water.cacheKey.addDolphinCoordinate] = `ERROR: ${ex.message}`;
        }

        this.requestUpdate();
    }

    private async replaceDolphinCoordinateInspectorCall(): Promise<void> {
        var params = dataInputValuesCache[Water.cacheKey.replaceDolphinCoordinate];

        try {
            var result = await this.waterService.replaceDolphinCoordinate(
                params[0].value,
                params[1].value);

            dataResultCache[Water.cacheKey.replaceDolphinCoordinate] = recordStringify(result);
        } catch(ex) {
            dataResultCache[Water.cacheKey.replaceDolphinCoordinate] = `ERROR: ${ex.message}`;
        }

        this.requestUpdate();
    }

    private async deleteDolphinCoordinateInspectorCall(): Promise<void> {
        var params = dataInputValuesCache[Water.cacheKey.deleteDolphinCoordinate];

        try {
            var result = await this.waterService.deleteDolphinCoordinate(
                params[0].value);

            dataResultCache[Water.cacheKey.deleteDolphinCoordinate] = recordStringify(result);
        } catch(ex) {
            dataResultCache[Water.cacheKey.deleteDolphinCoordinate] = `ERROR: ${ex.message}`;
        }

        this.requestUpdate();
    }

    private async getHurricaneCoordinateInspectorCall(): Promise<void> {
        var params = dataInputValuesCache[Water.cacheKey.getHurricaneCoordinate];

        try {
            var result = await this.waterService.getHurricaneCoordinate(
                params[0].value);

            dataResultCache[Water.cacheKey.getHurricaneCoordinate] = recordStringify(result);
        } catch(ex) {
            dataResultCache[Water.cacheKey.getHurricaneCoordinate] = `ERROR: ${ex.message}`;
        }

        this.requestUpdate();
    }

    private async addHurricaneCoordinateInspectorCall(): Promise<void> {
        var params = dataInputValuesCache[Water.cacheKey.addHurricaneCoordinate];

        try {
            var result = await this.waterService.addHurricaneCoordinate(
                params[0].value);

            dataResultCache[Water.cacheKey.addHurricaneCoordinate] = recordStringify(result);
        } catch(ex) {
            dataResultCache[Water.cacheKey.addHurricaneCoordinate] = `ERROR: ${ex.message}`;
        }

        this.requestUpdate();
    }

    private async replaceHurricaneCoordinateInspectorCall(): Promise<void> {
        var params = dataInputValuesCache[Water.cacheKey.replaceHurricaneCoordinate];

        try {
            var result = await this.waterService.replaceHurricaneCoordinate(
                params[0].value,
                params[1].value);

            dataResultCache[Water.cacheKey.replaceHurricaneCoordinate] = recordStringify(result);
        } catch(ex) {
            dataResultCache[Water.cacheKey.replaceHurricaneCoordinate] = `ERROR: ${ex.message}`;
        }

        this.requestUpdate();
    }

    private async deleteHurricaneCoordinateInspectorCall(): Promise<void> {
        var params = dataInputValuesCache[Water.cacheKey.deleteHurricaneCoordinate];

        try {
            var result = await this.waterService.deleteHurricaneCoordinate(
                params[0].value);

            dataResultCache[Water.cacheKey.deleteHurricaneCoordinate] = recordStringify(result);
        } catch(ex) {
            dataResultCache[Water.cacheKey.deleteHurricaneCoordinate] = `ERROR: ${ex.message}`;
        }

        this.requestUpdate();
    }
}

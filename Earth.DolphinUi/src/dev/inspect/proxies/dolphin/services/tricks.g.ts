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
import { ITricksService } from "../../../../../proxies/dolphin/services/interfaces/tricks-service";

@customElement("dolphin-ui-dolphin-tricks-service")
export class Tricks extends BaseInspectService {
    private static readonly cacheKey = {
        backflip: "dolphin-ui-dolphin-tricks-backflip",
    };

    @lazyInject(ProxyTypes.Dolphin_ITricksService)
    private tricksService!: ITricksService;

    constructor() {
        super(dataInputRecords, dataDefaultFactories, dataInputValuesCache, dataResultCache);

        if (dataInputValuesCache[Tricks.cacheKey.backflip] === undefined) {
            dataInputValuesCache[Tricks.cacheKey.backflip] = [
            ];
        }

        this.allMethodsToInspect.push({
            title: ".backflip(): Promise<Data.DolphinCoordinate[]>",
            cacheKey: Tricks.cacheKey.backflip,
            serviceCallMethod: () => this.backflipInspectorCall()
        });
    }


    private async backflipInspectorCall(): Promise<void> {
        try {
            var result = await this.tricksService.backflip();

            dataResultCache[Tricks.cacheKey.backflip] = recordStringify(result);
        } catch(ex) {
            dataResultCache[Tricks.cacheKey.backflip] = `ERROR: ${ex.message}`;
        }

        this.requestUpdate();
    }
}

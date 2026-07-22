import { customElement } from "lit/decorators.js";
import { takeUntil } from "rxjs/operators";
import { lazyInject } from "../../../../../../../Earth.Shared/ts/ioc/ioc";
import { recordStringify } from "../../../../../../../Earth.Shared/ts/dev/dev-utils.g";
import { BroadcastReceivedInfo } from "../../../../../../../Earth.Shared/ts/dev/broadcast-received-info";
import { InspectBroadcast as BaseInspectBroadcast } from "../../../../../../../Earth.Shared/ts/dev/custom-elements/inspect-broadcast";
import * as Data from "../../../../../data/definitions.g";
import { ProxyTypes } from "../../../../../proxies/dependencies/types.g";
import { IOceanSurfaceBroadcast } from "../../../../../proxies/ocean/broadcasts/interfaces/ocean-surface-broadcast";
import { broadcastReceivedInfoCache } from "../../../definitions.g";

@customElement("dolphin-ui-ocean-ocean-surface-broadcast")
export class OceanSurface extends BaseInspectBroadcast {
    public static observableHistoryCacheSize = 10;
    public static messagesHistoryCacheSize = 20;
    public static readonly observablesCacheKey = "dolphin-ui-ocean-ocean-surface-broadcast-observables";
    public static readonly messagesCacheKey = "dolphin-ui-ocean-ocean-surface-broadcast-messages";

    @lazyInject(ProxyTypes.Ocean_IOceanSurfaceBroadcast)
    private oceanSurfaceBroadcast!: IOceanSurfaceBroadcast;

    constructor() {
        super(broadcastReceivedInfoCache, OceanSurface.messagesCacheKey, OceanSurface.observablesCacheKey);
    }

    connectedCallback(): void {
        super.connectedCallback();
        this.connectedCallbackAsync();
    }

    private async connectedCallbackAsync(): Promise<void> {
        await this.updateComplete;

        this.oceanSurfaceBroadcast.temperature$.pipe(takeUntil(this.disconnected$))
            .subscribe({
                next: this.handleTemperature.bind(this)
            });

        this.oceanSurfaceBroadcast.subscribeToNewHurricane(
            this.messageNewHurricane.bind(this),
            this.disconnected$
        );
    }

    private handleTemperature(value: number): void {
        var cache = broadcastReceivedInfoCache[OceanSurface.observablesCacheKey];
        var info = new BroadcastReceivedInfo(
            new Date(),
            "Temperature",
            recordStringify(value),
            cache.length,
            false);

        cache.push(info);

        this.flushCache(cache, OceanSurface.observableHistoryCacheSize);
        this.selectedIndex = cache.length - 1;
        this.isSelectedMessage = false;
        this.requestUpdate();
    }

    private messageNewHurricane(
        location: Data.HurricaneCoordinate): void {

        var cache = broadcastReceivedInfoCache[OceanSurface.messagesCacheKey];
        var value = {
            message: "NewHurricane",
            parameters: [
                {
                    name: "location",
                    value: location
                }
            ]
        };

        var info = new BroadcastReceivedInfo(
            new Date(),
            "NewHurricane",
            recordStringify(value),
            cache.length,
            true);

        cache.push(info);

        this.flushCache(cache, OceanSurface.messagesHistoryCacheSize);
        this.selectedIndex = cache.length - 1;
        this.isSelectedMessage = true;
        this.requestUpdate();
    }
}

import { LitElement } from "lit";
import { takeUntil } from "rxjs/operators";
import { ObservableDisconnectedMixin } from "../../../Earth.Shared/ts/mixins/observable-disconnected-mixin";
import { lazyInject } from "../../../Earth.Shared/ts/ioc/ioc";
import * as Data from "../data/definitions.g";
import { ProxyTypes } from "../proxies/dependencies/types.g";
import { ITricksService as Dolphin_ITricksService } from "../proxies/dolphin/services/interfaces/tricks-service";
import { IOceanSurfaceBroadcast as Ocean_IOceanSurfaceBroadcast } from "../proxies/ocean/broadcasts/interfaces/ocean-surface-broadcast";

export abstract class BaseEarthOcean extends ObservableDisconnectedMixin(LitElement) {
    @lazyInject(ProxyTypes.Dolphin_ITricksService)
    protected _dolphinTricksService!: Dolphin_ITricksService;

    @lazyInject(ProxyTypes.Ocean_IOceanSurfaceBroadcast)
    protected _oceanOceanSurfaceBroadcast!: Ocean_IOceanSurfaceBroadcast;

    connectedCallback(): void {
        super.connectedCallback();
        this.connectedCallbackAsync();
    }

    protected async connectedCallbackAsync(): Promise<void> {
        await this.updateComplete;

        this._oceanOceanSurfaceBroadcast.temperature$.pipe(takeUntil(this.disconnected$))
            .subscribe({
                next: this.handleTemperature.bind(this)
            });

        this._oceanOceanSurfaceBroadcast.subscribeToNewHurricane(
            this.messageNewHurricane.bind(this),
            this.disconnected$
        );
    }

    protected handleTemperature(value: number): void {
    }

    protected messageNewHurricane(
        location: Data.HurricaneCoordinate): void {
    }
}

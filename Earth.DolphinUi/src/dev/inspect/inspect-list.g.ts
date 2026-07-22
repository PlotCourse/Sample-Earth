import { html, TemplateResult } from "lit";
import { customElement } from "lit/decorators.js";
import { InspectList as BaseInspectList } from "../../../../Earth.Shared/ts/dev/custom-elements/inspect-list";

@customElement("dolphin-ui-inspect-list")
export class InspectList extends BaseInspectList {
    renderInspector(version: string): TemplateResult {
        return html`
            <inspect-container-section title="Proxy Class Inspector (version ${version})" class="main ${this.modeClass}" .expanded=${true}>
                ${this.renderStyleToggle()}
                <inspect-container-section title="Ocean" class="dependency" .expanded=${true}>
                    <inspect-container-section title="Broadcast Proxy Classes" class="broadcast-list">
                        <inspect-container-section title="OceanSurfaceBroadcast" class="broadcast">
                            <div class="broadcast-summary">The code for this section of the proxy inspector provides a working example of how to use this proxy model.  The code can be found in this file:</div>
                            <div class="broadcast-summary-file-path">\\Earth.DolphinUi\\src\\dev\\inspect\\proxies\\ocean\\broadcasts\\ocean-surface.g.ts</div>
                            <div class="broadcast-summary">See the decorator to "@lazyInject" and how the injected model is used.</div>
                            <dolphin-ui-ocean-ocean-surface-broadcast></dolphin-ui-ocean-ocean-surface-broadcast>
                        </inspect-container-section>
                    </inspect-container-section>
                    <inspect-container-section title="Service Proxy Classes" class="service-list">
                        <inspect-container-section title="WaterService" class="service">
                            <div class="service-summary">The code for this section of the proxy inspector provides a working example of how to use this proxy model.  The code can be found in this file:</div>
                            <div class="service-summary-file-path">\\Earth.DolphinUi\\src\\dev\\inspect\\proxies\\ocean\\services\\water.g.ts</div>
                            <div class="service-summary">See the decorator to "@lazyInject" and how the injected model is used.</div>
                            <dolphin-ui-ocean-water-service></dolphin-ui-ocean-water-service>
                        </inspect-container-section>
                    </inspect-container-section>
                </inspect-container-section>
                <inspect-container-section title="Dolphin" class="dependency" .expanded=${true}>
                    <inspect-container-section title="Service Proxy Classes" class="service-list">
                        <inspect-container-section title="TricksService" class="service">
                            <div class="service-summary">The code for this section of the proxy inspector provides a working example of how to use this proxy model.  The code can be found in this file:</div>
                            <div class="service-summary-file-path">\\Earth.DolphinUi\\src\\dev\\inspect\\proxies\\dolphin\\services\\tricks.g.ts</div>
                            <div class="service-summary">See the decorator to "@lazyInject" and how the injected model is used.</div>
                            <dolphin-ui-dolphin-tricks-service></dolphin-ui-dolphin-tricks-service>
                        </inspect-container-section>
                    </inspect-container-section>
                </inspect-container-section>
            </inspect-container-section>
        `;
    }
}

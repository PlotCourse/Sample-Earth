import { html, TemplateResult, LitElement, css } from "lit";
import { customElement, state, query } from "lit/decorators.js";
import { InspectIconType } from "../../../Earth.Shared/ts/dev/custom-elements/inspect-icon";
import { readCookie, writeCookie } from "../../../Earth.Shared/ts/cookies";

@customElement("dolphin-ui-dev-dashboard")
export class DevDashboard extends LitElement {
    static styles = css`
        @media (prefers-color-scheme: dark) {
            .moded-colors {
                color: #c0c0c0;
                background-color: #070a07;
                --icon-simple-stroke: #c0c0c0;
                --icon-solid-fill: #000000;
            }
        }

        @media all and not (prefers-color-scheme: dark) {
            .moded-colors {
                color: #070a07;
                background-color: #c0c0c0;
                --icon-simple-stroke: #000000;
                --icon-solid-fill: #c0c0c0;
            }
        }

        .container {
            flex-direction: column;
            line-height: 1.25;
            font-family: Montserrat, sans-serif;
            margin: 10px;
            padding: 10px;
            border-radius: 4px;
        }

        .container-shown {
            display: flex;
        }

        .container-hidden {
            display: none;
        }

        .header {
            display: flex;
            flex-direction: row;
            justify-content: space-between;
            margin: 0 0 5px 0;
        }

        .header-text {
            font-weight: bold;
        }

        .info {
            margin: 10px 0 10px 0;
        }

        .info-list {
            display: flex;
            flex-direction: column;
            padding: 10px 0;
        }

        .info-list-item {
            margin: 5px 30px;
        }

        .icon-button {
            flex: 0 0 25px;
            padding: 1px 0 0 6px;
        }

        .float-icon {
            width: 19px;
            height: 19px;
            margin: 0;
            padding: 0;
        }
        
        #floater {
            display: flex;
            flex-direction: row;
            position: fixed;
            width: 45px;
            height: 22px;
            border: 1px solid var(--icon-simple-stroke);
            border-radius: 4px;
            z-index: 5000;
        }

        #dragHandle {
            color: white;
            cursor: move;
            flex: 0 0 14.5px;
            padding: 1px 0 0 0;
        }

        #dragIcon {
            width: 9.5px;
            height: 19px;
            margin: 0;
            padding: 0;
        }
    `;

    @state()
    protected accessor float = false;

    @query("#floater")
    private accessor floater: HTMLDivElement;

    private dragging = false;
    private floatOffsetX = 0;
    private floatOffsetY = 0;
    private lastFloaterLeft = -1;
    private lastFloaterTop = 0;

    constructor() {
        super();

        this.readDevDashCookie();
    }

    connectedCallback(): void {
        super.connectedCallback();
        this.connectedCallbackAsync();
    }

    private async connectedCallbackAsync(): Promise<void> {
        await this.updateComplete;

        document.addEventListener('mousemove', (e) => {
            if (this.dragging) {
                var f = this.floater;
                this.lastFloaterLeft = e.clientX - this.floatOffsetX;
                this.lastFloaterTop = e.clientY - this.floatOffsetY;
                f.style.left = `${this.lastFloaterLeft}px`;
                f.style.top = `${this.lastFloaterTop}px`;

                this.writeDevDashCookie();
            }
        });

        document.addEventListener('mouseup', () => {
            this.dragging = false;
            document.body.style.userSelect = '';
        });
    }

    render(): TemplateResult {
        var [floater, visibilityClass] = this.float
            ? [this.renderFloater(), "container-hidden"]
            : [html``, "container-shown"];
        
        var indexPath = '\\Earth.DolphinUi\\src\\dev\\index.ts';

        return html`
            ${floater}
            <div class="container moded-colors ${visibilityClass}">
                <div class="header">
                    <div class="header-text">DolphinUi Dev Dashboard</div>
                    <div class="icon-button">
                        <div class="float-icon" @click=${this.onFloatChange}>
                            <inspect-icon .inspectIconType=${InspectIconType.Float}></inspect-icon>
                        </div>
                    </div>
                </div>
                <div class="info">
                    <div>
                    In this dashboard:
                    </div>
                    <div class="info-list">
                        <div class="info-list-item">- Any custom dev tools specific to DolphinUi can be added as needed.</div>
                        <div class="info-list-item">- Using the proxy model inspector below you can directly test all UI proxy models generated by PlotStax for each service or broadcast dependency of DolphinUi.</div>
                        <div class="info-list-item">- To make changes to this dashboard or how it's loaded, see the entry point for the dev bundle: ${indexPath}</div>
                        <div class="info-list-item">- Use the button in the upper right to hide the dashboard from your page flow.  It can then be dragged out of the way or reopened as needed.</div>
                    </div>
                </div>
                <dolphin-ui-inspect-list></dolphin-ui-inspect-list>
            </div>
        `;
    }

    renderFloater(): TemplateResult {
        return html`
            <div id="floater" class="moded-colors" style="left:${this.lastFloaterLeft}px;top:${this.lastFloaterTop}px">
                <div class="icon-button">
                    <div class="float-icon" @click=${this.onFloatChange}>
                        <inspect-icon .inspectIconType=${InspectIconType.Float}></inspect-icon>
                    </div>
                </div>
                <div id="dragHandle" @mousedown=${this.onDragMouseDown}>
                    <div id="dragIcon">
                        <inspect-icon .inspectIconType=${InspectIconType.DragDots}></inspect-icon>
                    </div>
                </div>
            </div>
        `;
    }

    onFloatChange(event: PointerEvent): void {
        if (this.lastFloaterLeft === -1) {
            this.lastFloaterLeft = window.innerWidth - 65;
            this.lastFloaterTop = 10;
        }

        this.float = !this.float;
        this.writeDevDashCookie();
        event.stopPropagation();
    }

    onDragMouseDown(e: MouseEvent): void {
        this.dragging = true;

        var f = this.floater;
        this.floatOffsetX = e.clientX - f.offsetLeft;
        this.floatOffsetY = e.clientY - f.offsetTop;
        document.body.style.userSelect = 'none';
    }

    private readDevDashCookie(): void {
        var c = readCookie("DevDashDolphinUi");

        if (!c) {
            return;
        }

        var parts = c.split("|");

        if (parts.length !== 3) {
            return;
        }

        this.float = parts[0] === "1";

        var l = +parts[1];
        this.lastFloaterLeft = isNaN(l) ? window.innerWidth - 65 : l;

        var t = +parts[2];
        this.lastFloaterTop = isNaN(t) ? 10 : t;
    }

    private writeDevDashCookie(): void {
        var f = this.float ? "1" : "0";
        writeCookie("DevDashDolphinUi", `${f}|${this.lastFloaterLeft}|${this.lastFloaterTop}`);
    }
}

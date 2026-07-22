import { customElement, state } from "lit/decorators.js";
import { css, html, svg, TemplateResult, SVGTemplateResult } from "lit";
import { BaseEarthOcean } from "./base/base-earth-ocean.g";

//DEV_MANAGED_CODE_EXAMPLE: a LitElement that displays a simple ocean scene; demonstrates use of two types of generated proxy models:
//  1) Service "ITricksService": called to get coordinates from the server for the next dolphin backflip.
//  2) Broadcast "IOceanSurfaceBroadcast": used to display the temperature from the server in the upper right of the ocean scene.

interface DolphinState {
    id: number;
    startX: number;
    startY: number;
    endX: number;
    endY: number;
    animationStartedInMs: number;
    disobedient: boolean; // (will do a front flip)
}

@customElement("earth-ocean")
export class EarthOcean extends BaseEarthOcean {
    static styles = css`
        :host {
            display: flex;
            flex-direction: column;
        }

        .center-container {
            display: flex;
            flex-direction: row;
            justify-content: center;
            padding: 5px;
        }

        #scene {
            width: 576px;
            height: 436px;
            background-image: url("/images/ocean.png");
            background-repeat: no-repeat;
        }

        .dolphin {
            stroke: none;
            transform-box: fill-box;
            transform-origin: center;
        }

        #buttonBackflip {
            width: 100px;
            height: 30px;
            font-family: Arial, sans-serif;
            font-size: 12px;
            color: #fff;
            background-color: #007;
            padding: 15px 0 0 0;
            border-radius: 5px;
            cursor: pointer;
            text-align: center;
        }

        .temperature {
            font-family: Arial, sans-serif;
            font-size: 20px;
            text-anchor: middle;
            stroke: none;
            fill: #fff;
        }
    `;

    private static animationDurationInMs = 2000;

    @state()
    private accessor temperature = 0;

    private backflipCounter = 0;
    private allDolphins: { [id: number]: DolphinState } = {};

    private animating = false;

    render(): TemplateResult {
        var all = [];

        for (const id in this.allDolphins) {
            if (Object.prototype.hasOwnProperty.call(this.allDolphins, id)) {
                all.push(this.allDolphins[id]);
            }
        }

        return html`
            <div class="center-container">
                <div id="scene">
                    <svg viewBox="0 0 576 436" xmlns="http://www.w3.org/2000/svg">
                        <text x="550" y="30" class="temperature">${this.temperature}°F</text>
                        ${all.map(d => this.renderDolphin(d))}
                    </svg>
                </div>
            </div>
            <div class="center-container">
                <div id="buttonBackflip" @click=${this.onBackflip}>Backflip</div>
            </div>
        `;
    }

    renderDolphin(d: DolphinState): SVGTemplateResult {
        return svg`
            <g id="main_${d.id}" transform="scale(0.05)">
                <mask id="waterMask_${d.id}" mask-type="luminance">
                    <rect x="-1000" y="-10000" width="2000" height="10000" fill="white"/>
                </mask>
                <g id="dolphinShape_${d.id}" mask="url(#waterMask_${d.id})">
                    <g id="dolphin_${d.id}" class="dolphin" fill="#000">
                        <path d="M 177.74 712.50 C173.96,716.74 172.19,718.00 169.98,718.00 C168.41,718.00 166.85,717.56 166.51,717.02 C166.18,716.48 166.21,711.59 166.59,706.16 C167.77,688.94 164.03,680.27 153.51,675.92 C148.26,673.74 147.88,673.36 148.38,670.86 L 148.92 668.15 L 142.10 670.53 C130.52,674.58 123.50,675.21 98.50,674.47 C72.37,673.69 67.39,674.27 53.67,679.63 C44.89,683.06 42.26,683.23 40.00,680.50 C37.11,677.02 44.50,664.22 55.88,652.97 C73.98,635.07 93.45,624.85 121.50,618.50 C149.12,612.26 160.85,608.34 165.85,603.70 C167.15,602.49 172.18,595.65 177.03,588.50 C196.82,559.29 226.51,525.42 255.00,499.57 C315.97,444.22 373.12,407.91 443.50,379.82 C469.72,369.35 477.41,364.05 482.11,353.21 C489.66,335.77 485.33,316.46 470.15,299.80 C462.21,291.10 461.17,288.91 463.38,285.54 C465.87,281.74 471.00,280.65 483.44,281.29 C506.26,282.47 530.04,292.81 552.03,311.10 C555.43,313.93 565.76,323.59 574.99,332.55 C591.65,348.73 596.74,352.46 605.50,354.88 C607.70,355.49 620.97,357.14 635.00,358.54 C666.45,361.69 687.52,364.78 709.01,369.41 C813.00,391.79 907.75,445.01 934.41,496.00 C938.58,503.99 942.00,514.88 942.00,520.18 C942.00,524.85 943.44,526.43 956.08,535.62 C967.13,543.66 978.65,554.40 982.18,559.95 C987.62,568.52 985.23,573.51 974.75,575.41 C969.30,576.40 960.56,574.53 942.50,568.52 C909.87,557.66 883.03,553.10 833.50,550.01 C815.68,548.89 762.05,548.64 757.02,549.65 C754.87,550.08 750.82,553.07 744.85,558.64 C716.05,585.52 689.97,599.89 661.46,604.56 C647.53,606.85 625.05,606.08 616.32,603.03 C608.75,600.38 605.00,597.13 605.00,593.22 C605.00,589.23 608.31,586.48 617.26,583.03 C634.69,576.32 645.96,569.24 657.14,557.98 C657.66,557.45 658.17,556.95 658.65,556.46 C663.76,551.34 666.27,548.81 665.75,547.49 C665.19,546.08 661.19,546.04 653.20,545.68 C636.87,544.94 615.91,542.48 580.00,537.09 C545.43,531.90 529.13,530.20 503.20,529.08 C403.52,524.77 303.47,552.52 220.49,607.48 C212.78,612.58 204.59,618.80 202.28,621.31 L 198.09 625.86 L 198.66 642.68 C199.31,661.32 197.95,672.58 193.55,685.25 C190.39,694.34 183.79,705.72 177.74,712.50 Z"/>
                    </g>
                </g>
            </g>
        `;
    }

    private onBackflip(event: PointerEvent): void {
        event.stopPropagation();
        this.handleBackflip();
    }

    private async handleBackflip(): Promise<void> {
        this.backflipCounter++;
        
        var coords = await this._dolphinTricksService.backflip();
        var id = coords[0].dolphinCoordinateId;
        var nowInMs = new Date().getTime();
        var dolphin = {
            id,
            startX: coords[0].x,
            startY: coords[0].y,
            endX: coords[1].x,
            endY: coords[1].y,
            animationStartedInMs: nowInMs,
            disobedient: (nowInMs % 5 === 0) && this.backflipCounter > 10
        }

        this.allDolphins[id] = dolphin;
        this.requestUpdate();

        await this.updateComplete;

        if (!this.animating) {
            this.animating = true;
            this.doAnimation();
        }
    }

    private doAnimation(): void {
        var elementUpdate = false;
        var nowInMs = new Date().getTime();
        var count = 0;

        for (const id in this.allDolphins) {
            if (Object.prototype.hasOwnProperty.call(this.allDolphins, id)) {
                var state = this.allDolphins[id];
                var timePassedInMs = nowInMs - state.animationStartedInMs;

                if (timePassedInMs > EarthOcean.animationDurationInMs) {
                    delete this.allDolphins[id];
                    elementUpdate = true;
                } else {
                    count++;
                    var mainGroup = this.shadowRoot.querySelector(`#main_${id}`) as SVGGElement;
                    var dolphinShape = mainGroup.querySelector(`#dolphinShape_${id}`) as SVGGElement;
                    var dolphin = dolphinShape.querySelector(`#dolphin_${id}`) as SVGGElement;

                    var timePortion = timePassedInMs / EarthOcean.animationDurationInMs;
                    var mainX = 576 * (state.startX + (timePortion * (state.endX - state.startX))) / 100;
                    var mainY = 280 + (state.startY / 2);
                    var scale = .005 + (.045 * (state.startY / 100));

                    mainGroup.setAttribute("transform", `translate(${mainX}, ${mainY}) scale(${scale})`);

                    var elevation = -(Math.sin(Math.PI * timePortion) * 2500);
                    var flip = (-180 * timePortion) - 90;
                    var x = -512 * scale;
                    var invert = state.disobedient ? "scale(1, -1)" : "";
                    dolphin.setAttribute("transform", `translate(${x}, ${elevation}) rotate(${flip}) ${invert}`);
                }
            }
        }

        if (elementUpdate) {
            this.requestUpdate();
        }

        if (count === 0) {
            this.animating = false;
            return;
        }

        window.requestAnimationFrame(() => this.doAnimation());
    }

    protected handleTemperature(value: number): void {
        this.temperature = value;
    }
}

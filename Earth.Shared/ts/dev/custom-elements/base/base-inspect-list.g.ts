/*
MIT License

Copyright (c) 2025-2026 PlotCourse LLC

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
import { html, LitElement, css, TemplateResult } from "@node_modules/lit";
import { state } from "@node_modules/lit/decorators.js";
import { ObservableDisconnectedMixin } from "../../../mixins/observable-disconnected-mixin";
import { InspectIconType } from "./base-inspect-icon.g";

export abstract class BaseInspectList extends ObservableDisconnectedMixin(LitElement) {
    static styles = css`
        :host {
            line-height: 1.25;
        }

        .main {
            --title-font-weight: bold;
            --title-font-size: 15px;
            --title-margin-top: 11px;
            border: 1px solid #c0c0c0;
            border-radius: 4px;
        }

        .main-light {
            --title-bar-color: #000000;
            --background-color: #000000;
            --color: #ffffff;
            --theme-color: #1E7DFF;
            --icon-simple-stroke: #ffffff;
            --icon-copy-paste-stroke: var(--theme-color);
            --icon-copy-paste-fill: #ffffff;
            --icon-solid-fill: var(--theme-color);
            --dependency-title-bar-color: var(--theme-color);
            --dependency-background-color: var(--theme-color);
            --dependency-color: #000000;
            --dependency-list-color: #000000;
            --dependency-icon-simple-stroke: #000000;
            --broadcast-list-title-bar-color: #c0c0c0;
            --broadcast-list-background-color: #c0c0c0;
            --broadcast-title-bar-color: #aaaaaa;
            --broadcast-background-color: #dddddd;
            --service-list-title-bar-color: #c0c0c0;
            --service-list-background-color: #c0c0c0;
            --service-title-bar-color: #aaaaaa;
            --service-background-color: #aaaaaa;
            --container-data-expand-icon-solid-stroke: #000000;
            --container-data-collapse-icon-solid-stroke: #000000;
            --method-style-0-title-bar-color: #dddddd;
            --method-style-0-background-color: #dddddd;
            --method-style-0-row-hover-color: #cccccc;
            --method-style-1-title-bar-color: #cccccc;
            --method-style-1-background-color: #cccccc;
            --method-style-1-row-hover-color: #dddddd;
            --execute-button-border-color: var(--theme-color);
            --execute-button-background-color: var(--theme-color);
            --json-container-color: #000000;
            --json-container-border-color: #aaaaaa;
            --json-container-background-color: #ffffff;
        }

        .main-dark {
            --title-bar-color: #000000;
            --background-color: #000000;
            --color: #ffffff;
            --theme-color: #0C3369;
            --icon-simple-stroke: #ffffff;
            --icon-copy-paste-stroke: #ffffff;
            --icon-copy-paste-fill: #181818;
            --icon-solid-fill: #ffffff;
            --dependency-title-bar-color: var(--theme-color);
            --dependency-background-color: var(--theme-color);
            --dependency-color: #f0f0f0;
            --dependency-list-color: #f0f0f0;
            --dependency-icon-simple-stroke: #f0f0f0;
            --broadcast-list-title-bar-color: #181818;
            --broadcast-list-background-color: #181818;
            --broadcast-title-bar-color: #686868;
            --broadcast-background-color: #404040;
            --service-list-title-bar-color: #181818;
            --service-list-background-color: #181818;
            --service-title-bar-color: #404040;
            --service-background-color: #404040;
            --container-data-expand-icon-solid-stroke: #f0f0f0;
            --container-data-collapse-icon-solid-stroke: #f0f0f0;
            --method-style-0-title-bar-color: #303030;
            --method-style-0-background-color: #303030;
            --method-style-0-row-hover-color: #404040;
            --method-style-1-title-bar-color: #404040;
            --method-style-1-background-color: #404040;
            --method-style-1-row-hover-color: #303030;
            --execute-button-border-color: var(--theme-color);
            --execute-button-background-color: #1E7DFF;
            --json-container-color: #c0c0c0;
            --json-container-border-color: #404040;
            --json-container-background-color: #000000;
        }

        .dependency {
            --title-bar-color: var(--dependency-title-bar-color);
            --background-color: var(--dependency-background-color);
            --color: var(--dependency-color);
            --icon-simple-stroke: var(--dependency-icon-simple-stroke);
            --title-font-weight: normal;
            margin: 5px 0 0 0;
        }

        .broadcast-list {
            --title-bar-color: var(--broadcast-list-title-bar-color);
            --background-color: var(--broadcast-list-background-color);
            --color: var(--dependency-list-color);
            margin: 5px 0 0 0;
        }

        .broadcast {
            --title-bar-color: var(--broadcast-title-bar-color);
            --background-color: var(--broadcast-background-color);
            --title-font-family: Courier, sans serif;
            margin: 5px 0 0 0;
        }

        .service-list {
            --title-bar-color: var(--service-list-title-bar-color);
            --background-color: var(--service-list-background-color);
            --color: var(--dependency-list-color);
            margin: 5px 0 0 0;
        }

        .service {
            --title-bar-color: var(--service-title-bar-color);
            --background-color: var(--service-background-color);
            --title-font-family: Courier, sans serif;
            margin: 5px 0 0 0;
        }

        .broadcast-summary {
            font-family: Montserrat, sans-serif;
            color: var(--dependency-list-color);
            padding: 10px;
        }

        .broadcast-summary-file-path {
            color: var(--dependency-list-color);
            padding: 10px 10px 10px 50px;
        }

        .service-summary {
            font-family: Montserrat, sans-serif;
            color: var(--dependency-list-color);
            padding: 10px 10px 10px 40px;
        }

        .service-summary-file-path {
            color: var(--dependency-list-color);
            padding: 10px 10px 10px 90px;
        }

        .additional-header-controls {
            display: flex;
            flex-direction: row;
        }
        
        .icon-button {
            flex: 0 0 25px;
            padding: 1px 0 0 6px;
        }

        .style-mode-icon {
            width: 19px;
            height: 19px;
            margin: 0;
            padding: 0;
        }
    `;

    private static defaultStyle = "";

    @state()
    protected modeClass = "main-dark";

    abstract renderInspector(version: string): TemplateResult;

    connectedCallback(): void {
        super.connectedCallback();
        this.connectedCallbackAsync();
    }

    private async connectedCallbackAsync(): Promise<void> {
        await this.updateComplete;

        if (!BaseInspectList.defaultStyle) {
            BaseInspectList.defaultStyle = window.matchMedia("(prefers-color-scheme: dark)").matches
                ? "main-dark"
                : "main-light";
        }

        this.modeClass = BaseInspectList.defaultStyle;
    }


    render(): TemplateResult {
        return this.renderInspector("0.1.31");
    }

    renderStyleToggle(): TemplateResult {
        var iconType = this.isDarkMode()
            ? InspectIconType.Light
            : InspectIconType.Dark;

        return html`
            <div class="additional-header-controls" slot="additional-header-controls">
                <div class="icon-button">
                    <div class="style-mode-icon" @click=${this.onStyleChange}>
                        <inspect-icon .inspectIconType=${iconType}></inspect-icon>
                    </div>
                </div>
            </div>
        `;
    }

    onStyleChange(event: PointerEvent): void {
        this.modeClass = this.isDarkMode()
            ? "main-light"
            : "main-dark";
        BaseInspectList.defaultStyle = this.modeClass;
        event.stopPropagation();
    }

    private isDarkMode(): boolean {
        return this.modeClass === "main-dark";
    }
}

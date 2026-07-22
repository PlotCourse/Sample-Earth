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
import { html, css, TemplateResult } from "@node_modules/lit";
import { InspectIconType } from "../inspect-icon";
import { InspectContainer } from "../inspect-container";

export abstract class BaseInspectContainerSection extends InspectContainer {
    static styles = css`
        :host {
            display: block;
        }

        .main {
            background-color: var(--background-color, #ccc);
            margin: 0px;
            padding: 0px;
            font-family: var(--title-font-family, Montserrat, sans-serif);
            border-radius: 4px;
        }

        .title-bar {
            height: 38px;
            display: flex;
            flex-direction: row;
            flex-wrap: nowrap;
            justify-content: space-between;
            cursor: pointer;
            background-color: var(--title-bar-color, #ccc);
            color: var(--color, #000);
        }

        .title-bar-expanded {
            border-radius: 4px 4px 0 0;
        }

        .title-bar-collapsed {
            border-radius: 4px 4px 4px 4px;
        }

        .title {
            margin-top: var(--title-margin-top, 11px);
            margin-left: 20px;
            font-size: var(--title-font-size, 15px);
            font-weight: var(--title-font-weight, normal);
        }

        .container {
            border-width: 4px;
            border-top-style: none;
            border-right-style: solid;
            border-bottom-style: solid;
            border-left-style: solid;
            border-color: var(--title-bar-color, #ccc);
            border-radius: 0 0 4px 4px;
        }

        .all-header-controls {
            display: flex;
            flex-direction: row;
            flex-wrap: nowrap;
            justify-content: space-between;
            margin: 0;
            padding: 0;
        }

        .additional-header-controls {
            height: 19px;
            margin: 8px;
            padding: 0;
        }

        .expand-action-icon {
            width: 19px;
            height: 19px;
            margin: 8px;
            padding: 0;
        }
    `;

    render(): TemplateResult {
        var expandActionIcon = this.expanded
            ? html`<inspect-icon .inspectIconType=${InspectIconType.Collapse}></inspect-icon>`
            : html`<inspect-icon .inspectIconType=${InspectIconType.Expand}></inspect-icon>`;
        var title = this.expanded
            ? "Tap/Click to Collapse Section"
            : "Tap/Click to Expand Section";
        var titleBarClass = this.expanded
            ? "title-bar title-bar-expanded"
            : "title-bar title-bar-collapsed"
        return html`
            <div class="main">
                <div class="${titleBarClass}" @click=${this.onExpandedChange}>
                    <div class="title">
                        ${this.title}
                    </div>
                    <div class="all-header-controls">
                        <div class="additional-header-controls">
                            <slot name="additional-header-controls"></slot>
                        </div>
                        <div class="expand-action-icon" title="${title}">
                            ${expandActionIcon}
                        </div>
                    </div>
                </div>
                ${this.renderContent()}
            </div>
        `;
    }
}

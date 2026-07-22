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
import { LitElement, html, css, TemplateResult } from "@node_modules/lit";
import { property } from "@node_modules/lit/decorators.js";
import { InspectIconType } from "../inspect-icon";

export abstract class BaseInspectText extends LitElement {
    static styles = css`
        :host {
            display: block;
            overflow: hidden;
        }

        .main {
            display: flex;
            flex-direction: column;
            margin: 10px;
        }
        
        .json-container {
            width: var(--inspect-text-width, default);
            height: var(--inspect-text-height, default);
            overflow: auto;
            border-width: 2px;
            border-style: inset;
            color: var(--json-container-color);
            border-color: var(--json-container-border-color);
            background-color: var(--json-container-background-color);
        }

        .header {
            display: flex;
            flex-direction: row;
        }

        .header-text {
            font-family: Montserrat, sans-serif;
            margin-bottom: 4px;
            color: var(--color);
        }

        .copy-icon {
            width: 19px;
            height: 19px;
            margin-left: 19px;
            margin-right: 4px;
        }

        pre {
            margin: 0;
            white-space: pre-wrap;
        }
    `;

    @property({type: String})
    headerText = "";

    @property({type: String})
    json = "";

    render(): TemplateResult {
        return html`
            <div class="main">
                <div class="header">
                    <div class="header-text">
                        ${this.headerText}
                    </div>
                    <div>
                        <div class="copy-icon" @click=${this.copyClick} title="Tap/Click to Copy JSON">
                            <inspect-icon .inspectIconType=${InspectIconType.Copy}></inspect-icon>
                        </div>
                    </div>
                </div>
                <div class="json-container">
                    <pre>${this.json}</pre>
                </div>
            </div>
        `;
    }

    protected copyClick(): void {
        navigator.clipboard.writeText(this.json);
    }
}

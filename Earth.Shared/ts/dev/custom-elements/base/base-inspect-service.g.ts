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
import { ObservableDisconnectedMixin } from "../../../mixins/observable-disconnected-mixin";
import { DataInputRecord } from "../../data-input-record";
import { DataInputValue } from "../../data-input-value";
import { NeedToHideEditorFieldsEvent } from "../editors/base/base-value-editor.g";
import { BaseSetEditor } from "../editors/base/base-set-editor.g";

export interface InspectMethod {
    title: string;
    cacheKey: string;
    serviceCallMethod: () => Promise<void>;
}

export abstract class BaseInspectService extends ObservableDisconnectedMixin(LitElement) {
    static styles = css`
        .method {
            --font-family: Courier, sans serif;
            --title-font-size: 13px;
            --title-margin-top: 12px;
        }

        .method-style-0 {
            --title-bar-color: var(--method-style-0-title-bar-color);
            --background-color: var(--method-style-0-background-color);
            --row-hover-color: var(--method-style-0-row-hover-color);
        }

        .method-style-1 {
            --title-bar-color: var(--method-style-1-title-bar-color);
            --background-color: var(--method-style-1-background-color);
            --row-hover-color: var(--method-style-1-row-hover-color);
        }

        .execute-button {
            cursor: pointer;
            border-width: 2px;
            border-style: outset;
            border-color: var(--execute-button-border-color);
            background-color: var(--execute-button-background-color);
            margin: 5px 10px;
        }

        .result {
            margin: 10px;
        }
    `;

    protected allMethodsToInspect: InspectMethod[];

    constructor(
        protected dataInputRecords: { [name: string]: DataInputRecord },
        protected dataDefaultFactories: { [name: string]: () => any },
        protected dataInputValuesCache: { [key: string]: DataInputValue[] },
        protected dataResultCache: { [name: string]: string }) {

        super();

        this.allMethodsToInspect = new Array<InspectMethod>();
    }

    render(): TemplateResult {
        return html`${this.allMethodsToInspect.map((m, ix) => this.renderMethodInspector(m, ix))}`;
    }

    renderMethodInspector(method: InspectMethod, index: number): TemplateResult {
        var containerClass = `method method-style-${index % 2}`;
        var result = this.renderResult(this.dataResultCache[method.cacheKey]);
        var dataInputValues = this.dataInputValuesCache[method.cacheKey];
        var setEditor = dataInputValues.length == 0
            ? html``
            : html`
                <set-editor
                    .dataInputRecords=${this.dataInputRecords}
                    .dataDefaultFactories=${this.dataDefaultFactories}
                    .data=${dataInputValues}
                    @need-to-hide-editor-fields=${this.onNeedToHideFields}>
                </set-editor>`;

        return html`
            <inspect-container-section title="${method.title}" class="${containerClass}">
                ${setEditor}
                <button class="execute-button" @click=${method.serviceCallMethod}>Execute</button>
                ${result}
            </inspect-container-section>
        `;
    }

    renderResult(result: string): TemplateResult {
        if (!result) {
            return html``;
        }

        var headerText = "Method Call Result";
        return html`
            <div class="result">
                <inspect-text .headerText=${headerText} .json=${result}></inspect-text>
            </div>
        `;
    }

    protected onNeedToHideFields(event: NeedToHideEditorFieldsEvent): void {
        this.shadowRoot.querySelectorAll("set-editor").forEach((value: Element) => {
            (value as BaseSetEditor).hideAllFieldEditors();
        });
    }
}

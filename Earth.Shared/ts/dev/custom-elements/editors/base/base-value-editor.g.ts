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
import { property, query, state } from "@node_modules/lit/decorators.js";
import { ObservableDisconnectedMixin } from "../../../../mixins/observable-disconnected-mixin";
import { DataInputValue } from "../../../data-input-value";
import { DataInputType } from "../../../data-input-type.g";
import { InspectContainerData } from "../../inspect-container-data";
import { InspectIconType } from "../../inspect-icon";
import { BaseDataInput } from "../../../base/base-data-input.g";

export class DeleteClickEvent extends CustomEvent<DataInputValue> {
    constructor(data: DataInputValue) {
        super('delete-click', {
            detail: data,
            composed: true, 
            bubbles: true
        })
    }
}

export class EditorValueChangeEvent extends CustomEvent<DataInputValue> {
    constructor(data: DataInputValue) {
        super('editor-value-change', {
            detail: data,
            composed: true,
            bubbles: true
        })
    }
}

export class NeedToHideEditorFieldsEvent extends CustomEvent<void> {
    constructor() {
        super('need-to-hide-editor-fields', {
            composed: true, 
            bubbles: true
        })
    }
}

/**
 * A "set" might be a collection of parameters for a service method, the fields of a data record, or the elements of an array.
 */
export abstract class BaseValueEditor extends ObservableDisconnectedMixin(LitElement) {
    static styles = css`
        :host {
            display: block;
            margin: 0;
        }

        .row {
            display: flex;
            flex-direction: row;
            justify-content: flex-start;
            height: var(--line-height, 17px);
            cursor: pointer;
            color: var(--color, #000);
        }

        .row:hover {
            background-color: var(--row-hover-color, #fff);

            .visible-on-hover {
                display: block;
            }
        }

        .visible-on-hover {
            display: none;
        }

        .column {
            flex: 1;
            overflow: hidden;
        }

        .value {
            display: flex;
        }

        select {
            width: 234px;
            color: var(--color);
            background-color: var(--background-color);
        }

        select:focus {
            outline: none;
        }

        input {
            width: 234px;
            color: var(--color);
            background-color: var(--background-color);
        }

        input:focus {
            outline: none;
        }
    
        .invalid {
            color: #f00;
        }

        .delete-button-icon {
            width:9px;
            height:9px;
            margin-top: 1px;
            margin-left: 4px;
        }
    `;

    @property({type: String, reflect: true})
    id!: string;

    @property({type: Number})
    indentation = 0;

    @property({type: String})
    label!: string;

    @property({type: Object})
    data!: DataInputValue;

    @state()
    hideEditor = true;

    @query("input")
    inputElement!: HTMLInputElement;

    @query("select")
    selectElement!: HTMLSelectElement;

    render(): TemplateResult {
        var indentationPx = this.indentation * InspectContainerData.INDENTATION_AMOUNT_IN_PX;
        var hasValue = !(this.data.value === null || this.data.value === undefined);

        var deleteButton = !this.data.isRequired && hasValue
            ? html`
                <div class="visible-on-hover" @click=${this.onDeleteClick}>
                    <inspect-icon .inspectIconType=${InspectIconType.Ex} class="delete-button-icon"></inspect-icon>
                </div>
            `
            : html``;

        var editor = html`null`;

        if (hasValue || this.data.isRequired) {
            switch (this.data.dataInputType) {
                case DataInputType.InputBoolean:
                    editor = html`${this.data.valueAsString}`;
                    break;
                case DataInputType.InputDataEnum:
                    editor = this.hideEditor
                        ? html`${this.data.valueAsString}`
                        : html`
                            <select name="enumChoices" id="enumChoices" @change="${this.onSelectChange}">
                                ${this.data.dataInputEnum.valueChoices.map(v => html`<option value="${v.value}">${v.valueName}</option>`)}
                            </select>
                        `
                    break;
                default:
                    editor = this.hideEditor
                        ? html`${this.data.valueAsString}`
                        : html`<input type="text" @keyup=${this.onInputKeyUp} .value="${this.data.valueAsString}"></input>`;
                    break;
            }
        }

        var typeName = this.data.isRequired
            ? this.data.typeName
            : `${this.data.typeName}?`;

        return html`
            <div class="row" @click=${this.onRowClick}>
                <div class="column">
                    <div style="margin-left:${indentationPx}px"> 
                        ${this.label}
                    </div>
                </div>
                <div class="column value">
                    ${editor}
                    ${deleteButton}
                </div>
                <div class="column">
                    ${typeName}
                </div>
            </div>
        `;
    }

    protected async onRowClick(event: PointerEvent): Promise<void> {
        event.stopPropagation();

        if (this.data.value === null || this.data.value === undefined) {
            this.data.value = BaseDataInput.getDefaultValue(this.data.dataInputType);
        }

        if (this.data.dataInputType === DataInputType.InputBoolean) {
            this.data.value = !this.data.value;
            this.requestUpdate();
            return;
        }

        this.dispatchEvent(new NeedToHideEditorFieldsEvent());

        this.hideEditor = false;
        this.requestUpdate();
        await this.updateComplete;

        this.updateValidityCues(true);

        if (this.data.dataInputType === DataInputType.InputDataEnum) {
            this.selectElement?.focus();
            return;
        }

        this.inputElement?.focus();
    }

    protected onInputKeyUp(event: KeyboardEvent): void {
        var text = (event.target as HTMLInputElement).value;
        var isValid = false;
        var isChanged = false;
        var oldValue = this.data.value;

        if (this.data.dataInputType === DataInputType.InputNumber) {
            var n = Number(text);

            if (!isNaN(n) && typeof n === "number") {
                isValid = this.data.setFromSerializable(n, false, null);
                isChanged = isValid && oldValue !== n;
            }
        } else {
            isValid = this.data.setFromSerializable(text, false, null);
            isChanged = isValid && oldValue  !== text;
        }

        this.updateValidityCues(isValid);

        if (isChanged) {
            this.dispatchEvent(new EditorValueChangeEvent(this.data));
        }
    }

    protected onSelectChange(event: Event): void {
        this.data.value = parseInt((event.target as HTMLInputElement).value);
    }

    protected onDeleteClick(event: PointerEvent): void {
        this.dispatchEvent(new DeleteClickEvent(this.data));
        event.stopPropagation();
        this.requestUpdate();
    }

    hideValueEditor(): void {
        this.hideEditor = true;
        this.updateValidityCues(true);
    }

    // this intentionally done independently of the render b/c a re-render will interfere with input being edited
    protected updateValidityCues(isValid: boolean): void {
        var c = isValid
            ? ""
            : "invalid";
        
        this.inputElement?.setAttribute("class", c);
    }
}

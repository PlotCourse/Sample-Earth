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
import { keysOf } from "../../../../utils.g";
import { ObservableDisconnectedMixin } from "../../../../mixins/observable-disconnected-mixin";
import { DataInputValue } from "../../../data-input-value";
import { DataInputType } from "../../../data-input-type.g";
import { ExpandedChangeEvent } from "../../inspect-container";
import { DataInputRecord } from "../../../data-input-record";
import { InspectContainerClickEvent } from "../../inspect-container-data";
import { InspectIconType } from "../../inspect-icon";
import { DeleteClickEvent, EditorValueChangeEvent, NeedToHideEditorFieldsEvent, ValueEditor } from "../value-editor";
import { DataInputModifier } from "../../../base/base-data-input.g";
import { IDictionaryEntryWrapper } from "../../../base/base-data-input-value.g";

export class SetSizeChangeEvent extends CustomEvent<void> {
    constructor() {
        super('set-size-change', {
            composed: true, 
            bubbles: true
        })
    }
}

export class SetEditorValueChangeEvent extends CustomEvent<DataInputValue> {
    constructor(data: DataInputValue) {
        super('set-editor-value-change', {
            detail: data,
            composed: true,
            bubbles: true
        })
    }
}

/**
 * A "set" might be a collection of parameters for a service method, the fields of a data record, or the elements of an array.
 */
export abstract class BaseSetEditor extends ObservableDisconnectedMixin(LitElement) {
    static styles = css`
        :host {
            display: block;
            --line-height: 19px;
            color: var(--color, #000);
        }
        
        .root {
            margin-left: 38px;
        }

        .headers {
            display: flex;
            flex-direction: row;
            justify-content: flex-start;
            height: var(--line-height, 17px);
            font-family: Montserrat, sans-serif;
            font-size: 13px;
            font-weight: bold;
        }

        .header {
            flex: 1;
        }

        .input-list {
            display: flex;
            flex-direction: column;
            margin-left: 0px;
        }
    `;

    @property({type: String, reflect: true})
    id!: string;

    @property({type: Number})
    indentation = 1;

    @property({type: Object})
    dataInputRecords!: { [name: string]: DataInputRecord };

    @property({type: Object})
    dataDefaultFactories!: { [name: string]: () => any };
    
    @property({type: Array})
    data!: DataInputValue[];

    @property({type: Object})
    parentInputValue!: DataInputValue;

    @property({type: Number})
    arrayLength!: number;

    @property({type: String})
    parentDataFieldName!: string;

    render(): TemplateResult {
        var main = this.renderMain();

        if (this.indentation !== 1) {
            return main;
        }

        return html`
            <div class="root">
                <div class="headers">
                    <div class="header">Name</div>
                    <div class="header">Value</div>
                    <div class="header">Type</div>
                </div>
                <div>
                    ${main}
                </div>
            </div>
        `;
    }

    renderMain(): TemplateResult {
        return html`
            <div class="input-list">${this.data.map((val, ix) => this.renderInputValue(val, ix))}</div>
        `;
    }

    renderInputValue(inputValue: DataInputValue, index: number): TemplateResult {
        var parentIsArray = this.parentInputValue && this.parentInputValue.modifier === DataInputModifier.Array;
        var parentIsDictionary = this.parentInputValue && this.parentInputValue.modifier === DataInputModifier.Dictionary;
        var isArray = inputValue.modifier === DataInputModifier.Array;
        var isDictionary = inputValue.modifier === DataInputModifier.Dictionary;

        var title = inputValue.dataFieldName;
        
        if (parentIsArray) {
            title = `[${index}]`;
        } else if (parentIsDictionary) {
            var entryWrapper = inputValue.value as IDictionaryEntryWrapper;
            title = `[${entryWrapper.key}]`;
        }

        if (inputValue.dataInputType === DataInputType.InputDataRecord
            || (inputValue.modifier === DataInputModifier.Array && inputValue.dataInputType !== DataInputType.InputUint8Array)
            || (inputValue.modifier === DataInputModifier.Dictionary)) {

            var isSingleRecord = inputValue.dataInputType === DataInputType.InputDataRecord
                && !isArray;
            var showPasteButton = inputValue.allowCopyPaste && (isSingleRecord || isArray || isDictionary);
            var showCopyButton = showPasteButton && inputValue.value;
            var showAddButton = isArray || isDictionary;
            var showDeleteButton = isSingleRecord && (
                (!inputValue.isRequired && inputValue.value)
                || parentIsArray
                || parentIsDictionary
            );

            var expanded = (inputValue.value && inputValue.nestedData);
            var valueDesc = "";
            var arrayLength = 0;

            if (inputValue.value) {
                if (isArray) {
                    arrayLength = inputValue.value.length;
                    valueDesc = `Array (length ${arrayLength})`;
                } else if (isDictionary) {
                    var allKeys = keysOf(inputValue.value, inputValue.keyType);
                    valueDesc = `Object (key count ${allKeys.length})`;
                } else {
                    valueDesc = "Object";
                }
            } else {
                valueDesc = "null";
            }

            var appendTypeName = isArray ? "[]" : "";
            var typeName = isDictionary
                ? `[key: ${inputValue.keyTypeName}]: ${inputValue.typeName}`
                : `${inputValue.typeName}${appendTypeName}`;

            var indentation = this.indentation + 1;
            var setEditor = expanded
                ? html`
                    <set-editor
                        id="${inputValue.id}"
                        .dataInputRecords=${this.dataInputRecords}
                        .dataDefaultFactories=${this.dataDefaultFactories}
                        .data=${inputValue.nestedData}
                        .parentInputValue=${inputValue}
                        .arrayLength=${arrayLength}
                        .parentDataFieldName=${inputValue.dataFieldName}
                        .indentation=${indentation}
                        @set-size-change=${this.onSetSizeChange}
                        @set-editor-value-change=${this.onSetEditorValueChange}
                        @need-to-hide-editor-fields=${this.onNeedToHideFields}>
                    </set-editor>`
                : html``;

            var expandedChange = (event: ExpandedChangeEvent) => {
                this.onExpandedChange(inputValue, event);
            };

            var iconClick = (event: InspectContainerClickEvent) => {
                event.stopPropagation();
                this.handleInspectContainerClick(inputValue, event);
            };

            return html`
                <inspect-container-data
                    .indentation=${this.indentation}
                    .title="${title}"
                    .valueDesc="${valueDesc}"
                    ?showAddButton=${showAddButton}
                    ?showDeleteButton=${showDeleteButton}
                    ?showCopyButton=${showCopyButton}
                    ?showPasteButton=${showPasteButton}
                    .typeName="${typeName}"
                    ?expanded=${expanded}
                    @expanded-change=${expandedChange}
                    @container-icon-click=${iconClick}>
                    ${setEditor}
                </inspect-container-data>
            `;
        }

        var deleteFieldClick = (event: DeleteClickEvent) => {
            event.stopPropagation();
            this.handleDelete(event.detail);
        };

        var editorValueChange = (event: EditorValueChangeEvent) => {
            event.stopPropagation();
            this.dispatchEvent(new SetEditorValueChangeEvent(event.detail));
        }
        
        return html`
            <value-editor
                id="${inputValue.id}"
                .indentation=${this.indentation}
                .label="${title}"
                .data=${inputValue}
                @delete-click=${deleteFieldClick}
                @editor-value-change=${editorValueChange}>
            </value-editor>`;
    }

    protected onExpandedChange(inputValue: DataInputValue, event: ExpandedChangeEvent): void {
        if (!event.detail || (inputValue.value && inputValue.nestedData)) {
            this.requestUpdate();
            return;
        }

        var reset = false;

        if (!inputValue.value) {
            inputValue.value = this.dataDefaultFactories[inputValue.recordOrEnumName]();
            reset = true;
        }

        inputValue.initNestedData(this.dataInputRecords, reset, false);
        this.requestUpdate();
    }

    protected handleAdd(inputValue: DataInputValue): void {
        inputValue.initNestedData(this.dataInputRecords, true, true, this.dataDefaultFactories);
        
        this.needToHideAllFields();
        this.requestUpdate();
    }

    protected handleDelete(inputValue: DataInputValue): void {
        var parentIsArray = this.parentInputValue && this.parentInputValue.modifier === DataInputModifier.Array;
        var parentIsDictionary = this.parentInputValue && this.parentInputValue.modifier === DataInputModifier.Dictionary;

        if (parentIsArray) {
            this.parentInputValue.value = this.parentInputValue.value.filter((val: any) => val !== inputValue.value);
        }

        if (parentIsDictionary) {
            var entryWrapper = inputValue.value as IDictionaryEntryWrapper;
            this.parentInputValue.value[entryWrapper.key] = undefined;
        }

        if (parentIsArray || parentIsDictionary) {
            this.parentInputValue.initNestedData(this.dataInputRecords, true, false);

            this.dispatchEvent(new SetSizeChangeEvent());
            this.needToHideAllFields();
            this.requestUpdate();
            
            return;
        }

        inputValue.value = null;
        this.requestUpdate();
    }

    protected async handleCopy(inputValue: DataInputValue): Promise<void> {
        await inputValue.copy(this.dataInputRecords);
        this.requestUpdate();
    }

    protected async handlePaste(inputValue: DataInputValue): Promise<void> {
        await inputValue.paste(this.dataInputRecords);
        this.needToHideAllFields();
        this.requestUpdate();
    }

    protected handleInspectContainerClick(inputValue: DataInputValue, event: InspectContainerClickEvent): void {
        event.stopPropagation();
        
        switch (event.detail) {
            case InspectIconType.Plus:
                this.handleAdd(inputValue);
                return;
            case InspectIconType.Ex:
                this.handleDelete(inputValue);
                return;
            case InspectIconType.Copy:
                this.handleCopy(inputValue);
                return;
            case InspectIconType.Paste:
                this.handlePaste(inputValue);
                return;
            default:
                return;
        }
    }

    protected onSetSizeChange(event: SetSizeChangeEvent): void {
        event.stopPropagation();
        this.requestUpdate();
    }

    protected onSetEditorValueChange(event: SetEditorValueChangeEvent): void {
        event.stopPropagation();
        this.requestUpdate();
    }

    protected needToHideAllFields(): void {
        if (this.isRoot()) {
            this.hideAllFieldEditors();
        } else {
            this.dispatchEvent(new NeedToHideEditorFieldsEvent());
        }
    }

    protected onNeedToHideFields(event: NeedToHideEditorFieldsEvent): void {
        if (!this.isRoot()) {
            return;
        }

        this.hideAllFieldEditors();
    }

    protected isRoot(): boolean {
        return this.parentElement.tagName !== "INSPECT-CONTAINER-DATA";
    }

    hideAllFieldEditors(): void {
        this.shadowRoot.querySelectorAll("set-editor").forEach((value: Element) => {
            (value as BaseSetEditor).hideAllFieldEditors();
        });

        this.shadowRoot.querySelectorAll("value-editor").forEach((value: Element) => {
            (value as ValueEditor).hideValueEditor();
        });
    }
}

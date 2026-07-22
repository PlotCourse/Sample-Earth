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
import { keysOf, mapBase64FromUint8Array, mapUint8ArrayFromBase64 } from "../../utils.g";
import { DevTypes } from "../ioc/dev-types.g";
import { lazyInject } from "../../ioc/ioc";
import { DataInputEnum } from "../data-input-enum";
import { DataInputRecord } from "../data-input-record";
import { DataInputType } from "../data-input-type.g";
import { DataInput } from "../data-input";
import { IDataInputValueFactory } from "../factories/interfaces/data-input-value-factory.g";
import { DateSharp, DateSharpKind } from "../../date-sharp";
import { TimeSpan } from "../../time-span";
import { TimeOnly } from "../../time-only";
import { BaseDataInput, DataInputModifier } from "./base-data-input.g";
import { DataInputValue } from "../data-input-value";

export interface IDictionaryEntryWrapper {
    key: string | number;
    value: any;
}

/**
 * Represents access to a specific data value along with meta-data about its type represented in the base class, DataInput.
 * Allows an editor to access the value and determine how it can be edited.
 */
export abstract class BaseDataInputValue extends DataInput {
    @lazyInject(DevTypes.IDataInputValueFactory)
    protected dataInputValueFactory!: IDataInputValueFactory;

    protected _getValue: () => any;
    protected _setValue: (value: any) => void;
    protected _nestedData: BaseDataInputValue[] = null;
    protected _allowCopyPaste = true;

    get value(): any {
        return this._getValue();
    }
    set value(value: any) {
        this._setValue(value);
    }

    get valueAsString(): string {
        try {
            switch (this.dataInputType) {
                case DataInputType.InputString:
                    return this.value;
                case DataInputType.InputNumber:
                    return `${this.value}`;
                case DataInputType.InputBoolean:
                    return this.value ? "true" : "false";
                case DataInputType.InputDateTime:
                case DataInputType.InputDateTimeOffset:
                case DataInputType.InputDateOnly:
                case DataInputType.InputTimeSpan:
                case DataInputType.InputTimeOnly:
                    return this.value.serialize();
                case DataInputType.InputUint8Array:
                    return mapBase64FromUint8Array(this.value);
                case DataInputType.InputDataEnum:
                    if (!this.dataInputEnum) {
                        return "?";
                    }

                    var match = this.dataInputEnum.valueChoices.find(val => val.value === this.value);

                    if (!match) {
                        return "?";
                    }

                    return match.valueName;
                case DataInputType.InputDataRecord:
                    return this.value;
            }
        } catch {
            return "?";
        }
    }

    get nestedData(): BaseDataInputValue[] {
        return this._nestedData;
    }
    set nestedData(value: BaseDataInputValue[]) {
        this._nestedData = value;
    }
    
    get allowCopyPaste(): boolean {
        return this._allowCopyPaste;
    }
    set allowCopyPaste(value: boolean) {
        this._allowCopyPaste = value;
    }

    constructor(
        getValue: () => any,
        setValue: (value: any) => void,
        dataInputType: DataInputType,
        dataFieldName: string,
        modifier: DataInputModifier,
        isRequired: boolean,
        recordOrEnumName: string = "",
        dataInputEnum: DataInputEnum = null,
        keyType: string = "") {

        super(dataInputType, dataFieldName, modifier, isRequired, recordOrEnumName, dataInputEnum, keyType);

        this._getValue = getValue;
        this._setValue = setValue;
    }

    async copy(dataInputRecords: { [name: string]: DataInputRecord }): Promise<void> {
        var obj = this.buildSerializable(dataInputRecords);
        await navigator.clipboard.writeText(JSON.stringify(obj));
    }

    async paste(dataInputRecords: { [name: string]: DataInputRecord }): Promise<void> {
        try {
            var json = await navigator.clipboard.readText();
            var obj = JSON.parse(json);
        } catch(e) {
            console.log(`Error pasting JSON into proxy inspector editor: ${e}`);
        }

        this.setFromSerializable(obj, true, dataInputRecords);
        this.initNestedData(dataInputRecords, true, false);
    }

    initNestedData(
        dataInputRecords: { [name: string]: DataInputRecord },
        reset: boolean,
        withAddNew: boolean,
        dataDefaultFactories?: { [name: string]: () => any }
    ): void {
        if (this._nestedData && !reset) {
            return;
        }

        this._nestedData = this.buildNestedInputValues(dataInputRecords, withAddNew, dataDefaultFactories);
    }
    
    private buildDictionaryEntry(
        entryKey: string | number,
        entryValue: any): DataInputValue {

        var entryWrapper = {
            key: entryKey,
            value: entryValue
        } as IDictionaryEntryWrapper;

        var entryInput = this.dataInputValueFactory.create(
            () => {
                return entryWrapper;
            },
            (value) => { },
            DataInputType.InputDataRecord,
            "",
            DataInputModifier.None,
            true,
            "",
            null
        );

        var keyInput = this.dataInputValueFactory.create(
            () => {
                return entryWrapper.key;
            },
            (value) => {
                if (this.value[value] !== undefined) {
                    return;
                }

                this.value[entryWrapper.key] = undefined;
                entryWrapper.key = value;
                this.value[entryWrapper.key] = entryWrapper.value;
            },
            this.keyType == "string" ? DataInputType.InputString : DataInputType.InputNumber,
            "key",
            DataInputModifier.None,
            true,
            "",
            null
        );

        var valueInput = this.dataInputValueFactory.create(
            () => {
                return entryWrapper.value;
            },
            (value) => {
                entryWrapper.value = value;
                this.value[entryWrapper.key] = entryWrapper.value;
            },
            this.dataInputType,
            "value",
            DataInputModifier.None,
            true,
            this.recordOrEnumName,
            this.dataInputEnum
        );

        entryInput.nestedData = [keyInput, valueInput];
        entryInput.allowCopyPaste = false;

        return entryInput;
    }

    protected buildNestedInputValues(
        dataInputRecords: { [name: string]: DataInputRecord },
        withAddNew: boolean,
        dataDefaultFactories?: { [name: string]: () => any }
    ): BaseDataInputValue[] {
        var newItem: any;

        if (this.modifier !== DataInputModifier.None && dataDefaultFactories) {
            newItem = (this.dataInputType === DataInputType.InputDataRecord)
                ? dataDefaultFactories[this.recordOrEnumName]()
                : BaseDataInput.getDefaultValue(this.dataInputType);
        }

        switch (this.modifier) {
            case DataInputModifier.None:
                return dataInputRecords[this.recordOrEnumName].dataFields.map(f => this.dataInputValueFactory.create(
                    () => {
                        return this.value[f.dataFieldName];
                    },
                    (value) => {
                        this.value[f.dataFieldName] = value;
                    },
                    f.dataInputType,
                    f.dataFieldName,
                    f.modifier,
                    f.isRequired,
                    f.recordOrEnumName,
                    f.dataInputEnum
                ));
            case DataInputModifier.Array:
                if (withAddNew) {
                    var arr = this.value as Array<any>;
                    arr.push(newItem);
                }

                return this.value.map((element: any, ix: number) => this.dataInputValueFactory.create(
                    () => {
                        return this.value[ix];
                    },
                    (value) => {
                        this.value[ix] = value;
                    },
                    this.dataInputType,
                    "",
                    DataInputModifier.None,
                    false,
                    this.recordOrEnumName,
                    this.dataInputEnum
                ));
            case DataInputModifier.Dictionary:
                if (withAddNew) {
                    var newKey: string | number | undefined = undefined;
                    var ix = 1;

                    while (newKey === undefined || this.value[newKey] !== undefined) {
                        newKey = this.keyType === "string"
                            ? `key${ix}`
                            : ix;
                        ix++;
                    }
        
                    this.value[newKey] = newItem;
                }

                var allEntryInputs = new Array<BaseDataInputValue>();
                var allKeys = keysOf(this.value, this.keyType);

                for (const key of allKeys) {
                    var val = this.value[key];
                    var entryInput = this.buildDictionaryEntry(key, val);

                    allEntryInputs.push(entryInput);
                }

                return allEntryInputs;
        }
    }

    protected buildSerializable(dataInputRecords: { [name: string]: DataInputRecord }): any {
        if (this.modifier === DataInputModifier.Dictionary) {
            if (!this.value) {
                return {};
            }

            var indexSignature: { [key: string | number]: any } = {};

            this.initNestedData(dataInputRecords, false, false);

            for (var entryInput of this._nestedData) {
                indexSignature[entryInput.nestedData[0].value] = entryInput.nestedData[1].buildSerializable(dataInputRecords);
            }

            return indexSignature;
        } else if (this.modifier === DataInputModifier.Array && this.dataInputType !== DataInputType.InputUint8Array) {
            if (!this.value) {
                return [];
            }

            this.initNestedData(dataInputRecords, false, false);
            return this.nestedData.map(dataInputValue => {
                return dataInputValue.buildSerializable(dataInputRecords);
            });
        }

        switch (this.dataInputType) {
            case DataInputType.InputString:
            case DataInputType.InputNumber:
            case DataInputType.InputBoolean:
            case DataInputType.InputDataEnum:
                return this.value;
            case DataInputType.InputDateTime:
            case DataInputType.InputDateTimeOffset:
            case DataInputType.InputDateOnly:
            case DataInputType.InputTimeSpan:
            case DataInputType.InputTimeOnly:
                return this.value
                    ? this.value.serialize()
                    : null;
            case DataInputType.InputUint8Array:
                return this.value
                    ? mapBase64FromUint8Array(this.value)
                    : "";
            case DataInputType.InputDataRecord:
                if (!this.value) {
                    return null;
                }

                var serializable = new Object() as any;

                this.initNestedData(dataInputRecords, false, false);

                for(var field of this.nestedData) {
                    serializable[field.dataFieldName] = field.buildSerializable(dataInputRecords);
                }

                return serializable;
        }
    }

    public setFromSerializable(value: any, logErrors: boolean, dataInputRecords: { [name: string]: DataInputRecord }): boolean {
        if (value === undefined || (value === null && this.isRequired)) {
            return false;
        }

        if (this.modifier === DataInputModifier.Dictionary) {
            var newKeys = keysOf(value, this.keyType);
            var oldKeys = keysOf(this.value, this.keyType);

            for(var k of oldKeys) {
                if (!newKeys.find(f => f === k)) {
                    this.value[k] = undefined;
                }
            }

            for(var k of newKeys) {
                var val = this.value[k] === undefined
                    ? null
                    : this.value[k];
                var entryInput = this.buildDictionaryEntry(k, val);
                entryInput.nestedData[1].setFromSerializable(value[k], logErrors, dataInputRecords);
            }
        } else if (this.modifier === DataInputModifier.Array && this.dataInputType !== DataInputType.InputUint8Array) {
            if (!Array.isArray(value)) {
                return false;
            }
            
            var maxIx = Math.max(this.value.length, value.length);

            for(var ix = 0; ix < maxIx; ix++) {
                if (ix >= this.value.length) {
                    this.value.push(null);
                }

                if (ix >= value.length) {
                    this.value.pop();
                }
            }

            var nestedData = this.buildNestedInputValues(dataInputRecords, false);

            for (var ix = 0; ix < value.length; ix++) {
                nestedData[ix].setFromSerializable(value[ix], logErrors, dataInputRecords);
            }

            this.value = this.value.filter((v: any) => v !== null);

            return true;
        }

        var jsTypeName = typeof value;

        try {
            switch (this.dataInputType) {
                case DataInputType.InputString:
                    if (jsTypeName !== "string") {
                        return false;
                    }
                    this.value = value;
                    return this.value === value;
                case DataInputType.InputBoolean:
                    if (jsTypeName !== "boolean") {
                        return false;
                    }
                    this.value = value;
                    return this.value === value;
                case DataInputType.InputNumber:
                case DataInputType.InputDataEnum:
                    if (jsTypeName !== "number") {
                        return false;
                    }
                    this.value = value;
                    return this.value === value;
                case DataInputType.InputDateTime:
                    if (value instanceof Date) {
                        //forward compatibility in case JSON.parse is ever fixed to recognize Date fields serialized by its own JSON.stringify method:
                        value = value.toISOString();
                    }
                    this.value = DateSharp.strictParse(value, DateSharpKind.DateTimeUtc, DateSharpKind.Offset, DateSharpKind.Unspecified);
                    return true;
                case DataInputType.InputDateTimeOffset:
                    this.value = DateSharp.strictParse(value, DateSharpKind.Offset);
                    return true;
                case DataInputType.InputDateOnly:
                    this.value = DateSharp.strictParse(value, DateSharpKind.DateOnly);
                    return true;
                case DataInputType.InputTimeSpan:
                    this.value = TimeSpan.strictParse(value);
                    return true;
                case DataInputType.InputTimeOnly:
                    this.value = TimeOnly.strictParse(value);
                    return true;
                case DataInputType.InputUint8Array:
                    this.value = mapUint8ArrayFromBase64(value);
                    return true;
                case DataInputType.InputDataRecord:
                    if (value === null) {
                        this.value = null;
                        return true;
                    }

                    if (jsTypeName !== "object") {
                        return false;
                    }

                    if (!this.value || typeof this.value !== "object") {
                        this.value = new Object() as any;
                    }

                    var nestedData = this.buildNestedInputValues(dataInputRecords, false);

                    for(var field of nestedData) {
                        field.setFromSerializable(value[field.dataFieldName], logErrors, dataInputRecords);                        
                    }
                    return true;
                }
        } catch(e) {
            if (logErrors) {
                console.log(e);
            }
        }

        return false;
    }
}

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
import { DateSharp, DateSharpKind } from "../../date-sharp";
import { TimeOnly } from "../../time-only";
import { TimeSpan } from "../../time-span";
import { DataInputEnum } from "../data-input-enum";
import { DataInputType } from "../data-input-type.g";

export enum DataInputModifier {
    None,
    Array,
    Dictionary
}

/**
 * Represents metadata for a specific type of input, providing all info needed for how the value can be changed.
 * See sub-classes DataInputValue and DataInputValueRoot.
 */
export abstract class BaseDataInput {
    protected static idSeed = 0;

    protected _id: number;
    protected _dataInputType: DataInputType;
    protected _dataFieldName: string;
    protected _modifier: DataInputModifier;
    protected _isRequired: boolean;
    protected _recordOrEnumName: string;
    protected _dataInputEnum: DataInputEnum;
    protected _keyType: string;

    get id(): number {
        return this._id;        
    }

    get dataInputType(): DataInputType {
        return this._dataInputType;
    }

    get dataFieldName(): string {
        return this._dataFieldName;
    }

    get modifier(): DataInputModifier {
        return this._modifier;
    }

    get isRequired(): boolean {
        return this._isRequired;
    }

    get recordOrEnumName(): string {
        return this._recordOrEnumName;
    }

    get dataInputEnum(): DataInputEnum {
        return this._dataInputEnum;
    }

    get keyType(): string {
        return this._keyType;
    }

    get keyTypeName(): string {
        if (this.keyType == "string") {
            return "string";
        }

        return "number";
    }

    get typeName(): string {
        switch (this.dataInputType) {
            case DataInputType.InputString:
                return "string";
            case DataInputType.InputNumber:
                return "number";
            case DataInputType.InputBoolean:
                return "boolean";
            case DataInputType.InputDateTime:
                return "DateTime";
            case DataInputType.InputDateTimeOffset:
                return "DateTimeOffset";
            case DataInputType.InputDateOnly:
                return "DateOnly";
            case DataInputType.InputTimeSpan:
                return "TimeSpan";
            case DataInputType.InputTimeOnly:
                return "TimeOnly";
            case DataInputType.InputUint8Array:
                return "Uint8Array";
            case DataInputType.InputDataRecord:
            case DataInputType.InputDataEnum:
                return this.recordOrEnumName;
        }
    }

    public static getDefaultValue(dataInputType: DataInputType): any {
        switch(dataInputType) {
            case DataInputType.InputString:
                return "";
            case DataInputType.InputBoolean:
                return false;
            case DataInputType.InputDateTime:
                return DateSharp.now(DateSharpKind.DateTimeUtc);
            case DataInputType.InputDateTimeOffset:
                return DateSharp.now(DateSharpKind.Offset);
            case DataInputType.InputDateOnly:
                return DateSharp.now(DateSharpKind.DateOnly);
            case DataInputType.InputTimeOnly:
                return TimeOnly.now();
            case DataInputType.InputTimeSpan:
                return new TimeSpan("31.01:02:03.0000004");
            default: //enum, byte, or number:
                return 0;
        }
    }

    constructor(
        dataInputType: DataInputType,
        dataFieldName: string,
        modifier: DataInputModifier,
        isRequired: boolean,
        recordOrEnumName: string = "",
        dataInputEnum: DataInputEnum = null,
        keyType: string = "") {

        BaseDataInput.idSeed++;

        this._id = BaseDataInput.idSeed;
        this._dataInputType = dataInputType;
        this._dataFieldName = dataFieldName;
        this._modifier = modifier;
        this._isRequired = isRequired;
        this._recordOrEnumName = recordOrEnumName;
        this._dataInputEnum = dataInputEnum;
        this._keyType = keyType;
    }
}

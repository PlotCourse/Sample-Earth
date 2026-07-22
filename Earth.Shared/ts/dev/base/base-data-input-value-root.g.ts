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
import { DataInputEnum } from "../data-input-enum";
import { DataInputType } from "../data-input-type.g";
import { DataInputValue } from "../data-input-value";
import { DataInputModifier } from "./base-data-input.g";

/**
 * An DataInputValue value that also holds a reference to the actual value, not just accessors for it.
 */
export abstract class BaseDataInputValueRoot<T> extends DataInputValue {
    protected _root: T;

    constructor(
        initialValue: T,
        dataInputType: DataInputType,
        dataFieldName: string,
        modifier: DataInputModifier,
        isRequired: boolean,
        recordOrEnumName: string = "",
        dataInputEnum: DataInputEnum = null,
        keyType: string = "") {

        super(
            () => { return this._root; },
            (value: T) => { this._root = value; },
            dataInputType,
            dataFieldName,
            modifier,
            isRequired,
            recordOrEnumName,
            dataInputEnum,
            keyType
        );
        this._root = initialValue;
    }
}

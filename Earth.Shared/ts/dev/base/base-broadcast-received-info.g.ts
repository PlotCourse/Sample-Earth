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
export abstract class BaseBroadcastReceivedInfo {
    protected _received: Date;
    protected _summary: string;
    protected _detail: string;
    protected _index: number;
    protected _isMessage: boolean;

    get received(): string {
        const year = this._received.getFullYear();
        const month = String(this._received.getMonth() + 1).padStart(2, '0');
        const day = String(this._received.getDate()).padStart(2, '0');
        const hours = String(this._received.getHours()).padStart(2, '0');
        const minutes = String(this._received.getMinutes()).padStart(2, '0');
        const seconds = String(this._received.getSeconds()).padStart(2, '0');
        const ms = String(this._received.getMilliseconds()).padStart(3, '0');

        return `${year}-${month}-${day} ${hours}:${minutes}:${seconds}.${ms}`;
    }

    get summary(): string {
        return this._summary;
    }

    get detail(): string {
        return this._detail;
    }

    get index(): number {
        return this._index;
    }
    set index(value: number) {
        this._index = value;
    }

    get isMessage(): boolean {
        return this._isMessage;
    }

    constructor(
        received: Date,
        summary: string,
        detail: string,
        index: number,
        isMessage: boolean) {

        this._received = received;
        this._summary = summary;
        this._detail = detail;
        this._index = index;
        this._isMessage = isMessage;
    }
}

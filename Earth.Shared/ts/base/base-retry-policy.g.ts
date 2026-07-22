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
export abstract class BaseRetryPolicy {
    protected _intervalsInSeconds: number[];
    protected _neverGiveUp: boolean;
    protected _next = 0;

    constructor(intervalsInSeconds: number[], neverGiveUp: boolean) {
        this._intervalsInSeconds = intervalsInSeconds;
        this._neverGiveUp = neverGiveUp;
    }

    reset(): void {
        this._next = 0;
    }

    nextRetryDelayInMs(): number {
        var allUsed = this._next == this._intervalsInSeconds.length;

        if (allUsed && !this._neverGiveUp) {
            return null;
        }

        var ix = allUsed
            ? this._intervalsInSeconds.length - 1
            : this._next;

        if (this._next < this._intervalsInSeconds.length) {
            this._next++;
        }

        return this._intervalsInSeconds[ix] * 1000;
    }
}

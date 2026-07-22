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

export class TimeFrame {
    static parseHns(serialized: string): number {
        var start = serialized.indexOf('.') + 1;

        if (start === 0) {
            return 0;
        }

        var end = serialized.lastIndexOf('+');

        if (end === -1) {
            end = serialized.lastIndexOf('Z');

            if (end === -1) {
                end = serialized.lastIndexOf('-');
            }

            if (end === -1 || end < start) {
                end = serialized.length;
            }
        }

        if (end - start > 7) {
            end = start + 7;
        }

        var digits = serialized.substring(start, end);
        return parseInt(digits.padEnd(7, "0"), 10);
    }

    protected _hns: number;

    /**
     * hns = hundreds of nanoseconds representing a portion of 1 day.
     * This number represents a value in the range from "00:00:00" to
     * "23:59:59.9999999" when this object is created from serialized
     * .NET instances of TimeOnly or the remaining time portion of a
     * serialized TimeSpan following the day if any.
     */
    get hns(): number {
        return this._hns;
    }
    set hns(value: number) {
        this._hns = value;
    }
    
    constructor(serialized: string) {
        if (!serialized?.length) {
            this.hns = 0;
            return;
        }

        if (!/^[0-9|:|.|-]*$/gm.test(serialized)) {
            this.hns = NaN;
            return;
        }

        var parts = serialized.split(":");
        var h = parseInt(parts[0]);
        var m = parts.length > 0
            ? parseInt(parts[1])
            : 0;
        var s = 0;
        var hns = 0;
    
        if (parts.length > 2) {
            var secondsPart = parts[2];
            
            parts = parts[2].split('.');
            s = parseInt(parts[0]);
            hns = TimeFrame.parseHns(secondsPart);
        }

        this.hns = (h * 36000000000) + (m * 600000000) + (s * 10000000) + hns;
    }

    serialize(): string {
        var hns = this._hns;
        
        var h = `${Math.floor(hns / 36000000000)}`.padStart(2, "0");
        hns %= 36000000000;

        var m = `${Math.floor(hns / 600000000)}`.padStart(2, "0");
        hns %= 600000000;

        var s = `${Math.floor(hns / 10000000)}`.padStart(2, "0");
        hns %= 10000000;

        var hnsString = `${hns}`.padStart(7, "0");

        return `${h}:${m}:${s}.${hnsString}`;
    }
}

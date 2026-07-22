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

import { TimeFrame } from "./time-frame.g";

export abstract class BaseTimeSpan {
    protected _days: number;
    protected _timeFrame: TimeFrame;

    get days(): number {
        return this._days;
    }
    set days(value: number) {
        this._days = value;
    }

    /**
     * Note when using this field that the sign of the timespan as a whole is tracked only
     * on the "days" portion of this TimeSpan so depending on the reason for accessing hns
     * it may need to be negated if used in calculations.
     */
    get hns(): number {
        return this._timeFrame.hns;
    }
    set hns(value: number) {
        this._timeFrame.hns = value;
    }

    constructor(serialized: string) {
        if (!serialized) {
            this._days = NaN;
            this._timeFrame = new TimeFrame(null);
        }

        var dot = serialized.indexOf(".");
        var colon = serialized.indexOf(":");

        if (dot > -1 && dot < colon) {
            var daysString = serialized.substring(0, dot);

            if (/^[0-9|-]*$/gm.test(daysString)) {
                this._days = parseInt(daysString);
            } else {
                this._days = NaN;
            }
    
            this._timeFrame = new TimeFrame(serialized.substring(dot + 1));
        } else {
            this._days = 0;
            this._timeFrame = new TimeFrame(serialized);
        }

        if (this._timeFrame.hns > 864000000000) {
            this._days += Math.floor(this._timeFrame.hns / 864000000000);
            this._timeFrame.hns = this._timeFrame.hns % 864000000000;
        }
    }

    static strictParseImpl<TTimespan extends BaseTimeSpan>(serialized: string, builder: (serialized: string) => TTimespan): TTimespan {
        var ts = builder(serialized);
        ts.validate(serialized);
        return ts;
    }

    serialize(): string {
        var tfSerialized = this._timeFrame.serialize();
        return this._days != 0
            ? `${this._days}.${tfSerialized}`
            : tfSerialized;
    }

    protected validate(serialized: string): void {
        if (isNaN(this.days) || isNaN(this.hns)) {
            throw new Error(`Unable to parse BaseTimeSpan: "${serialized}"`);
        }
    }
}

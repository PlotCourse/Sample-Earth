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

export enum DateSharpKind {
    Unspecified, // from a .NET DateTime with unspecified kind
    DateTimeUtc, // from a .NET DateTime with UTC kind
    // An "Offset" DateSharp is from either a .NET DateTimeOffset or a .NET DateTime with local
    // kind (where "local" means the place that serialized it, not necessarily local to UI):
    Offset,
    DateOnly     // from a .NET DateOnly
}

/**
 * A class that allows for lossless conversion to and from serialized .NET date types: any instance
 * created from a serialized .NET DateTime, DateTimeOffset, or DateOnly when subsequently
 * serialized will match the original serialized value, as well as providing a "Date" property for
 * the closest possible Javascript representation of the date.
 */
export abstract class BaseDateSharp {
    protected _jsDate: Date;
    protected _kind: DateSharpKind;
    protected _offsetMinutes: number;
    protected _hns: number;

    get jsDate(): Date {
        return this._jsDate;
    }
    set jsDate(value: Date) {
        this._jsDate = value;
    }

    get kind(): DateSharpKind {
        return this._kind;
    }
    set kind(value: DateSharpKind) {
        if (this._kind === value) {
            return;
        }

        if (this._kind === DateSharpKind.DateTimeUtc) {
            this._offsetMinutes = -this._jsDate.getTimezoneOffset();
        } else {
            if (this._kind === DateSharpKind.Offset) {
                this._offsetMinutes = 0;
            }
        }

        this._kind = value;
    }

    /**
     * The original offset is lost when converting from a serialized .NET date to a JS date so it's preserved in this field.
     */
    get offsetMinutes(): number {
        return this._offsetMinutes;
    }
    set offsetMinutes(value: number) {
        this._offsetMinutes = value;
    }

    /**
     * hns = hundreds of nanoseconds
     * Some of this precision in the time is lost when converting from a serialized .NET date to a JS date so it's preserved in this field.
     */
    get hns(): number {
        return this._hns;
    }
    set hns(value: number) {
        this._hns = value;
    }

    constructor(serialized: string) {
        if (!serialized?.length) {
            this._jsDate = new Date();
            this._kind = DateSharpKind.DateTimeUtc;
            this._offsetMinutes = 0;
            this._hns = TimeFrame.parseHns(this._jsDate.toISOString());
            return;
        }

        if (serialized.length === 10) {
            this._jsDate = new Date(serialized);
            this._kind = DateSharpKind.DateOnly;
            this._offsetMinutes = 0;
            this._hns = 0;
            return;
        }

        if (serialized.charAt(serialized.length - 1) === "Z") {
            this._jsDate = new Date(serialized);
            this._kind = DateSharpKind.DateTimeUtc;
            this._offsetMinutes = 0;
            this._hns = TimeFrame.parseHns(serialized);
            return;
        }

        var parts = serialized.split("+");
        var offsetSign = 1;
        var offset = "";
        var baseDate = "";

        if (parts.length == 2) {
            baseDate = parts[0] + "Z";
            offset = parts[1];
        } else {
            parts = serialized.split("-");

            if (parts.length === 4) {
                offsetSign = -1;
                offset = parts[3];

                parts.pop();
                baseDate = parts.join("-") + "Z";
            }
        }

        if (offset.length == 0) {
            this._jsDate = new Date(serialized + "Z");
            this._kind = DateSharpKind.Unspecified;
            this._offsetMinutes = 0;
            this._hns = TimeFrame.parseHns(serialized);
            return;
        }

        parts = offset.split(":");
        var minutes = offsetSign * 60 * parseInt(parts[0], 10);

        if (parts.length > 1) {
            minutes += (offsetSign * parseInt(parts[1], 10));
        }

        var adjustMs = minutes * -60000;
        this._jsDate = new Date((new Date(baseDate)).getTime() + adjustMs);
        this._kind = DateSharpKind.Offset;
        this._offsetMinutes = minutes;
        this._hns = TimeFrame.parseHns(serialized);
    }

    static strictParseImpl<TDateSharp extends BaseDateSharp>(
        serialized: string,
        builder: (s: string) => TDateSharp,
        ...expectedKind: DateSharpKind[]): TDateSharp {

        var d = builder(serialized);
        d.validate(serialized, ...expectedKind);

        return d;
    }

    serialize(): string {
        if (this._kind === DateSharpKind.Offset) {
            var minutes = Math.abs(this._offsetMinutes);
            var sign = this._offsetMinutes < 0 ? "-" : "+";
            var h = `${Math.floor(minutes / 60)}`.padStart(2, "0");
            var m = `${Math.floor(minutes % 60)}`.padStart(2, "0");
            var adjustMs = this._offsetMinutes * 60000;
            var d = this.serializeBase(new Date(this._jsDate.getTime() + adjustMs), false);

            return `${d}${sign}${h}:${m}`;
        }

        switch (this._kind) {
            case DateSharpKind.Unspecified:
                return this.serializeBase(this._jsDate, false);
            case DateSharpKind.DateTimeUtc:
                return this.serializeBase(this._jsDate, true);
            case DateSharpKind.DateOnly:
                return this._jsDate.toISOString().substring(0, 10);
        }
    }

    protected serializeBase(date: Date, utc: boolean): string {
        var s = date.toISOString().split(".")[0];
        var hnsString = `${this._hns}`.padStart(7, "0");

        return utc
            ? `${s}.${hnsString}Z`
            : `${s}.${hnsString}`;
    }

    protected validate(serialized: string, ...expectedKind: DateSharpKind[]): void {
        if (isNaN(this.jsDate.getTime())) {
            throw new Error(`Unable to parse BaseDateSharp: "${serialized}"`);
        }

        if (expectedKind.indexOf(this.kind) === -1) {
            var names = expectedKind.map(k => `DateSharpKind.${DateSharpKind[k]}`);
            var lastIx = names.length - 1;

            if (lastIx > 0) {
                names[lastIx] = `or ${names[lastIx]}`;
            }

            throw new Error(`BaseDateSharp parsed from "${serialized}" to kind DateSharpKind.${DateSharpKind[this.kind]} instead of expected ${names.join(', ')}`);
        }
    }
}

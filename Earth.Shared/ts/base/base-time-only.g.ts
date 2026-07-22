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

import { DateSharp, DateSharpKind } from "../date-sharp";
import { TimeFrame } from "./time-frame.g";

export abstract class BaseTimeOnly extends TimeFrame {
    static serializedNow(): string {
        var d = DateSharp.now(DateSharpKind.Offset).serialize();
        var parts = d.split("T");
        var end = Math.max(parts[1].indexOf("+"), parts[1].indexOf("-"))
        var time = parts[1].substring(0, end);

        return time;
    }

    static strictParseImpl<TTimeOnly extends BaseTimeOnly>(serialized: string, builder: (s: string) => TTimeOnly): TTimeOnly {
        var ts = builder(serialized);
        ts.validate(serialized);
        return ts;
    }

    protected validate(serialized: string): void {
        if (isNaN(this.hns) || this.hns >= 864000000000) {
            throw new Error(`Unable to parse BaseTimeOnly: "${serialized}"`);
        }
    }
}

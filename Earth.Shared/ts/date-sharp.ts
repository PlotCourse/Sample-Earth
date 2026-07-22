import { BaseDateSharp, DateSharpKind } from "./base/base-date-sharp.g";

export { DateSharpKind }

export class DateSharp extends BaseDateSharp {
    static now(kind: DateSharpKind): DateSharp {
        var d = new DateSharp("");
        d.kind = kind;        
        return d;
    }

    static strictParse(serialized: string, ...expectedKind: DateSharpKind[]): DateSharp {
        var builder = (s: string) => {
            return new DateSharp(s);
        };

        return BaseDateSharp.strictParseImpl(serialized, builder, ...expectedKind);
    }
}

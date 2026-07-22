import { BaseTimeOnly } from "./base/base-time-only.g";
import { DateSharp, DateSharpKind } from "./date-sharp";

export class TimeOnly extends BaseTimeOnly {
    static now(): TimeOnly {
        return new TimeOnly(BaseTimeOnly.serializedNow());
    }

    static strictParse(serialized: string): TimeOnly {
        var builder = (serialized: string) => {
            return new TimeOnly(serialized);
        };
        return BaseTimeOnly.strictParseImpl(serialized, builder);
    }
}

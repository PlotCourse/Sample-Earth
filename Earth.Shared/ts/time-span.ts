
import { BaseTimeSpan } from "./base/base-time-span.g";

export class TimeSpan extends BaseTimeSpan {
    static strictParse(serialized: string): TimeSpan {
        var builder = (serialized: string) => {
            return new TimeSpan(serialized);
        };
        return BaseTimeSpan.strictParseImpl(serialized, builder);
    }
}

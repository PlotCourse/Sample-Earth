import { devUtilsImpl } from "./dev-utils-impl";

function replacer(key: string, value: any): any {
    return devUtilsImpl.replacer(key, value);
}

export function recordStringify(value: any): string {
    return devUtilsImpl.recordStringify(value);
}


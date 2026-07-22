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
import { DictionaryEntry, ReadOnlyDictionary } from "../interfaces/read-only-dictionary.g";

export abstract class BaseUtilsImpl {
    mapUint8ArrayFromBase64(source: string): Uint8Array {
        if (source === "") {
            return new Uint8Array(0);
        }

        return new Uint8Array(Array.from(atob(source), s => s.charCodeAt(0)));
    }

    mapBase64FromUint8Array(source: Uint8Array): string {
        return btoa(String.fromCharCode(...source));
    }

    toIndexSignature<TKey extends string | number, TValue>(
        from: ReadOnlyDictionary<TKey, TValue>
    ): { [key: string | number]: TValue } {
        return this.toIndexSignatureWithMap(from, v => v);
    }

    toIndexSignatureWithMap<TKey extends string | number, TFromValue, TToValue>(
        from: ReadOnlyDictionary<TKey, TFromValue>,
        mapper: (val: TFromValue) => TToValue
    ): { [key: string | number]: TToValue } {
        if (!from) {
            return null;
        }

        var ixSig: { [key: string | number]: TToValue } = {};
        from.records.forEach(element => {
            ixSig[element.key] = mapper(element.value);
        });
        return ixSig;
    }

    toReadOnlyDictionary<TKey extends string | number, TValue>(
        from: { [key: string | number]: TValue }
    ): ReadOnlyDictionary<TKey, TValue> {
        return this.toReadOnlyDictionaryWithMap(from, v => v);
    }

    toReadOnlyDictionaryWithMap<TKey extends string | number, TFromValue, TToValue>(
        from: { [key: string | number]: TFromValue },
        mapper: (val: TFromValue) => TToValue
    ): ReadOnlyDictionary<TKey, TToValue> {
        if (!from) {
            return null;
        }

        var records = new Array<DictionaryEntry<TKey, TToValue>>();

        for (const k in from) {
            records.push({
                key: k as TKey,
                value: mapper(from[k])
            });
        }

        return {
            records: records
        };
    }

    keysOf<TValue>(indexSignature: { [key: string | number]: TValue }, keyType: string): string[] | number[] {
        if (keyType === "string") {
            return this.stringKeysOf(indexSignature);
        } else {
            return this.numberKeysOf(indexSignature);
        }
    }

    stringKeysOf<TValue>(indexSignature: { [key: string]: TValue }): string[] {
        var allKeys = Object.keys(indexSignature)
            .filter(k => indexSignature[k] !== undefined);
        allKeys.sort();
        return allKeys;
    }

    numberKeysOf<TValue>(indexSignature: { [key: number]: TValue }): number[] {
        var allKeys = Object.keys(indexSignature)
            .map(k => Number(k))
            .filter(k => indexSignature[k] !== undefined);
        allKeys.sort((a,b) => a - b);

        return allKeys;
    }
}

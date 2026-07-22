import { utilsImpl } from "./utils-impl";
import { ReadOnlyDictionary } from "./interfaces/read-only-dictionary.g";

export function mapUint8ArrayFromBase64(source: string): Uint8Array {
    return utilsImpl.mapUint8ArrayFromBase64(source);
}

export function mapBase64FromUint8Array(source: Uint8Array): string {
    return utilsImpl.mapBase64FromUint8Array(source);
}

export function toIndexSignature<TKey extends string | number, TValue>(
    from: ReadOnlyDictionary<TKey, TValue>): { [key: string | number]: TValue } {

    return utilsImpl.toIndexSignature(from);
}

export function toIndexSignatureWithMap<TKey extends string | number, TFromValue, TToValue>(
    from: ReadOnlyDictionary<TKey, TFromValue>,
    mapper: (val: TFromValue) => TToValue): { [key: string | number]: TToValue } {

    return utilsImpl.toIndexSignatureWithMap(from, mapper);
}

export function toReadOnlyDictionary<TKey extends string | number, TValue>(
    from: { [key: string | number]: TValue }): ReadOnlyDictionary<TKey, TValue> {

    return utilsImpl.toReadOnlyDictionary(from);
}

export function toReadOnlyDictionaryWithMap<TKey extends string | number, TFromValue, TToValue>(
    from: { [key: string | number]: TFromValue },
    mapper: (val: TFromValue) => TToValue): ReadOnlyDictionary<TKey, TToValue> {

    return utilsImpl.toReadOnlyDictionaryWithMap(from, mapper);
}

export function keysOf<TValue>(indexSignature: { [key: string | number]: TValue }, keyType: string): string[] | number[] {
    return utilsImpl.keysOf(indexSignature, keyType);
}

export function stringKeysOf<TValue>(indexSignature: { [key: string]: TValue }): string[] {
    return utilsImpl.stringKeysOf(indexSignature);
}

export function numberKeysOf<TValue>(indexSignature: { [key: number]: TValue }): number[] {
    return utilsImpl.numberKeysOf(indexSignature);
}

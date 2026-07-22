import { ReadOnlyRecords } from "./read-only-records.g";

export interface DictionaryEntry<TKey, TValue> {
    key: TKey;
    value: TValue;
}

export interface ReadOnlyDictionary<TKey, TValue> extends ReadOnlyRecords<DictionaryEntry<TKey, TValue>> {
}

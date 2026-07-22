import { ReadOnlyRecords } from "./read-only-records.g";

export interface Result {
    succeeded: boolean;
    message: string;
}

export interface ResultItem<T> extends Result {
    item: T;
}

export interface ResultSet<T> extends Result {
    items: ReadOnlyRecords<T>;
}

export interface PagedResult<T> extends Result {
    pageItems: ReadOnlyRecords<T>;
    page: number;
    totalPages: number;
    itemsPerPage: number;
    totalItems: number;
}

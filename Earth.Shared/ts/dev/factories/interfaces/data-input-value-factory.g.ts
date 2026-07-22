import { DataInputModifier } from "../../base/base-data-input.g";
import { DataInputEnum } from "../../data-input-enum";
import { DataInputType } from "../../data-input-type.g";
import { DataInputValue } from "../../data-input-value";

export interface IDataInputValueFactory {
    create(
        getValue: () => any,
        setValue: (value: any) => void,
        dataInputType: DataInputType,
        dataFieldName: string,
        modifier: DataInputModifier,
        isRequired: boolean,
        recordOrEnumName?: string,
        dataInputEnum?: DataInputEnum): DataInputValue;
}

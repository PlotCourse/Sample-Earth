import { DataInputModifier } from "../base/base-data-input.g";
import { DataInputEnum } from "../data-input-enum";
import { DataInputType } from "../data-input-type.g";
import { DataInputValue } from "../data-input-value";
import { IDataInputValueFactory } from "./interfaces/data-input-value-factory.g";

export class DataInputValueFactory implements IDataInputValueFactory {
    create(
        getValue: () => any,
        setValue: (value: any) => void,
        dataInputType: DataInputType,
        dataFieldName: string,
        modifier: DataInputModifier,
        isRequired: boolean,
        recordOrEnumName: string = "",
        dataInputEnum: DataInputEnum = null) {

        return new DataInputValue(
            getValue,
            setValue,
            dataInputType,
            dataFieldName,
            modifier,
            isRequired,
            recordOrEnumName,
            dataInputEnum);
    }
}

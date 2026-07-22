import { DataInput } from "../../../../Earth.Shared/ts/dev/data-input";
import { DataInputType } from "../../../../Earth.Shared/ts/dev/data-input-type.g";
import { DataInputModifier } from "../../../../Earth.Shared/ts/dev/base/base-data-input.g";
import { DataInputRecord } from "../../../../Earth.Shared/ts/dev/data-input-record";
import { DataInputEnum } from "../../../../Earth.Shared/ts/dev/data-input-enum";
import { DataInputValue } from "../../../../Earth.Shared/ts/dev/data-input-value";
import { EnumInputValue } from "../../../../Earth.Shared/ts/dev/enum-input-value";
import * as Data from "../../data/definitions.g";
import { BroadcastReceivedInfo } from "../../../../Earth.Shared/ts/dev/broadcast-received-info";
export const dataInputRecords: { [name: string]: DataInputRecord } = {};
export const dataInputEnums: { [name: string]: DataInputEnum } = {};
export const dataDefaultFactories: { [name: string]: () => any } = {};
export const dataInputValuesCache: { [key: string]: DataInputValue[] } = {};
export const dataResultCache: { [name: string]: string } = {};
export const broadcastReceivedInfoCache: { [name: string]: BroadcastReceivedInfo[] } = {};
dataInputRecords["DolphinCoordinate"] = new DataInputRecord(
    "DolphinCoordinate",
    [
        new DataInput(DataInputType.InputNumber, "dolphinCoordinateId", DataInputModifier.None, true),
        new DataInput(DataInputType.InputNumber, "x", DataInputModifier.None, true),
        new DataInput(DataInputType.InputNumber, "y", DataInputModifier.None, true)
    ]);

dataDefaultFactories["DolphinCoordinate"] = (): Data.DolphinCoordinate => {
    return {
        dolphinCoordinateId: 0,
        x: 0,
        y: 0
    } as Data.DolphinCoordinate;
}

dataInputRecords["DolphinCoordinatePut"] = new DataInputRecord(
    "DolphinCoordinatePut",
    [
        new DataInput(DataInputType.InputNumber, "x", DataInputModifier.None, true),
        new DataInput(DataInputType.InputNumber, "y", DataInputModifier.None, true)
    ]);

dataDefaultFactories["DolphinCoordinatePut"] = (): Data.DolphinCoordinatePut => {
    return {
        x: 0,
        y: 0
    } as Data.DolphinCoordinatePut;
}

dataInputRecords["HurricaneCoordinate"] = new DataInputRecord(
    "HurricaneCoordinate",
    [
        new DataInput(DataInputType.InputNumber, "hurricaneCoordinateId", DataInputModifier.None, true),
        new DataInput(DataInputType.InputNumber, "x", DataInputModifier.None, true),
        new DataInput(DataInputType.InputNumber, "y", DataInputModifier.None, true)
    ]);

dataDefaultFactories["HurricaneCoordinate"] = (): Data.HurricaneCoordinate => {
    return {
        hurricaneCoordinateId: 0,
        x: 0,
        y: 0
    } as Data.HurricaneCoordinate;
}

dataInputRecords["HurricaneCoordinatePut"] = new DataInputRecord(
    "HurricaneCoordinatePut",
    [
        new DataInput(DataInputType.InputNumber, "x", DataInputModifier.None, true),
        new DataInput(DataInputType.InputNumber, "y", DataInputModifier.None, true)
    ]);

dataDefaultFactories["HurricaneCoordinatePut"] = (): Data.HurricaneCoordinatePut => {
    return {
        x: 0,
        y: 0
    } as Data.HurricaneCoordinatePut;
}


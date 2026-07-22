import * as Data from "../../../../../data/definitions.g";

export interface IBaseTricksService {

    backflip(
    ): Promise<Data.DolphinCoordinate[]>;
}

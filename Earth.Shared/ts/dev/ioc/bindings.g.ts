import { iocContainer } from "../../ioc/ioc";
import { DevTypes } from "./dev-types.g";
import { DataInputValueFactory } from "../factories/data-input-value-factory.g";

iocContainer.addSingletonForLazyConstruction(DevTypes.IDataInputValueFactory, () => new DataInputValueFactory());

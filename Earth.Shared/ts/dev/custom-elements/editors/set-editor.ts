import { customElement } from "@node_modules/lit/decorators.js";
import { BaseSetEditor } from "./base/base-set-editor.g";

export { SetSizeChangeEvent } from "./base/base-set-editor.g";

/**
 * A "set" might be a collection of parameters for a service method, the fields of a data record, or the elements of an array.
 */
@customElement("set-editor")
export class SetEditor extends BaseSetEditor {
}

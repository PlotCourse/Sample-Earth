import { customElement } from "@node_modules/lit/decorators.js";
import { BaseValueEditor } from "./base/base-value-editor.g";

export { DeleteClickEvent, EditorValueChangeEvent, NeedToHideEditorFieldsEvent} from "./base/base-value-editor.g";

@customElement("value-editor")
export class ValueEditor extends BaseValueEditor {
}

import { customElement } from "@node_modules/lit/decorators.js";
import { BaseInspectContainerData, InspectContainerClickEvent } from "./base/base-inspect-container-data.g";

export { InspectContainerClickEvent }

@customElement("inspect-container-data")
export class InspectContainerData extends BaseInspectContainerData {
}

/*
MIT License

Copyright (c) 2025-2026 PlotCourse LLC

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
import { html, css, TemplateResult } from "@node_modules/lit";
import { property } from "@node_modules/lit/decorators.js";
import { InspectIconType } from "../inspect-icon";
import { InspectContainer } from "../inspect-container";

export class InspectContainerClickEvent extends CustomEvent<InspectIconType> {
    constructor(buttonIcon: InspectIconType) {
        super('container-icon-click', {
            detail: buttonIcon,
            composed: true, 
            bubbles: true
        })
    }
}

export abstract class BaseInspectContainerData extends InspectContainer {
    static readonly INDENTATION_AMOUNT_IN_PX = 19;

    static styles = css`
        :host {
            display: block;
            color: var(--color, #000);
        }

        .main {
            background-color: var(--background-color, #cccccc);
            margin: 0px;
            padding: 0px;
        }

        .title-bar {
            display: flex;
            height: var(--line-height, 17px);
            flex-direction: row;
            flex-wrap: nowrap;
            justify-content: flex-start;
            cursor: pointer;
            background-color: var(--title-bar-color, #cccccc);
        }

        .title-bar:hover {
            background-color: var(--row-hover-color, #ffffff);

            .visible-on-hover {
                display: block;
            }
        }

        .visible-on-hover {
            display: none;
        }

        .column {
            flex: 1;
        }

        .title {
            display: flex;
        }

        .value-and-cliboard-buttons {
            display: flex;
            justify-content: space-between;
        }

        .value {
            display: flex;
            justify-content: flex-start;
        }

        .clipboard-buttons {
            display: flex;
        }

        .expand-action-icon {
            width:9px;
            height:9px;
            margin-right: 2px;
        }

        .add-button-icon {
            width:9px;
            height:9px;
            margin-left: 2px;
        }

        .delete-button-icon {
            width:9px;
            height:9px;
            margin-top: 1px;
            margin-left: 2px;
        }

        .copy-button-icon {
            width: 19px;
            height: 19px;
            margin-left: 2px;
        }

        .paste-button-icon {
            width: 19px;
            height: 19px;
            margin-left: 2px;
        }

        .expand-icon {
            --icon-solid-stroke: var(--container-data-expand-icon-solid-stroke);
            --icon-solid-fill: transparent;
        }

        .collapse-icon {
            --icon-solid-stroke: var(--container-data-collapse-icon-solid-stroke);
        }
    `;

    @property({type: Number})
    indentation = 0;

    @property({type: String})
    valueDesc = "";

    @property({type: Boolean})
    showAddButton = false;

    @property({type: Boolean})
    showDeleteButton = false;

    @property({type: Boolean})
    showCopyButton = false;

    @property({type: Boolean})
    showPasteButton = false;

    @property({type: String})
    typeName = "";

    render(): TemplateResult {
        var [title, expandActionIcon] = this.expanded
            ? ["Tap/Click to Collapse Section", html`<inspect-icon .inspectIconType=${InspectIconType.CollapseHorizontal} class="collapse-icon"></inspect-icon>`]
            : ["Tap/Click to Expand Section", html`<inspect-icon .inspectIconType=${InspectIconType.ExpandHorizontal} class="expand-icon"></inspect-icon>`];

        var indentationPx = (this.indentation * BaseInspectContainerData.INDENTATION_AMOUNT_IN_PX) - 11; //11 = icon width + its right margin
        var showCopyOrPaste = this.expanded && (this.showCopyButton || this.showPasteButton);
        var valueClasses = showCopyOrPaste
            ? "value"
            : "column value";
        var valueSection = html`
            <div class="${valueClasses}">
                <div>
                    ${this.valueDesc}
                </div>
                ${this.renderButtonIcon(InspectIconType.Plus)}
                ${this.renderButtonIcon(InspectIconType.Ex)}
            </div>
        `;
        var valueColumn = valueSection;

        if (showCopyOrPaste) {
            valueColumn = html`
                <div class="column value-and-cliboard-buttons">
                    ${valueSection}
                    <div class="clipboard-buttons">
                        ${this.renderButtonIcon(InspectIconType.Copy)}
                        ${this.renderButtonIcon(InspectIconType.Paste)}
                    </div>
                </div>
            `;
        }

        return html`
            <div class="main">
                <div class="title-bar" @click=${this.onExpandedChange}>
                    <div class="column title">
                        <div class="expand-action-icon" title="${title}" style="margin-left:${indentationPx}px">
                            ${expandActionIcon}
                        </div>
                        <div>
                            ${this.title}
                        </div>
                    </div>
                    ${valueColumn}
                    <div class="column">
                        ${this.typeName}
                    </div>
                </div>
                ${this.renderContent()}
            </div>
        `;
    }

    renderButtonIcon(buttonIcon: InspectIconType): TemplateResult {
        var show = false;
        var iconClass: string;

        switch (buttonIcon) {
            case InspectIconType.Plus:
                show = this.showAddButton;
                iconClass = "add-button-icon";
                break;
            case InspectIconType.Ex:
                show = this.showDeleteButton;
                iconClass = "delete-button-icon";
                break;
            case InspectIconType.Copy:
                show = this.showCopyButton;
                iconClass = "copy-button-icon";
                break;
            case InspectIconType.Paste:
                show = this.showPasteButton;
                iconClass = "paste-button-icon";
                break;
            default:
                break;
        }

        if (!show) {
            return html``;
        }

        var onClick = (event: PointerEvent) => {
            this.dispatchEvent(new InspectContainerClickEvent(buttonIcon));
            event.stopPropagation();
        };

        return html`
            <div @click=${onClick} class="visible-on-hover">
                <inspect-icon .inspectIconType=${buttonIcon} class="${iconClass}"></inspect-icon>
            </div>
        `;
    }
}

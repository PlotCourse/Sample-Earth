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
import { LitElement, html, css, TemplateResult } from "@node_modules/lit";
import { property } from "@node_modules/lit/decorators.js";

export class ExpandedChangeEvent extends CustomEvent<boolean> {
    constructor(expanded: boolean) {
        super('expanded-change', {
            detail: expanded,
            composed: true, 
            bubbles: true
        })
    }
}

export abstract class BaseInspectContainer extends LitElement {
    @property({type: Boolean})
    expanded = false;

    @property({type: String})
    title = "";

    renderContent(): TemplateResult {
        if (!this.expanded) {
            return html``;
        }

        return html`<div class="container"><slot></slot></div>`;
    }

    onExpandedChange(event: PointerEvent): void {
        this.expanded = !this.expanded;
        this.dispatchEvent(new ExpandedChangeEvent(this.expanded));

        event.stopPropagation();

        this.handleScrollIntoViewAsync();
    }

    private async handleScrollIntoViewAsync(): Promise<void> {
        await this.updateComplete;
        this.scrollIntoView({
            behavior: 'smooth',
            block: 'nearest'
        });
    }
}

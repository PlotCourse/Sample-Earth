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

export enum InspectIconType {
    Expand,
    Collapse,
    ExpandHorizontal,
    CollapseHorizontal,
    Plus,
    Ex,
    Copy,
    Paste,
    Light,
    Dark,
    Float,
    DragDots
}

export abstract class BaseInspectIcon extends LitElement {
    static styles = css`
        :host {
            display: block;
        }

        .main {
            border-style: none;
            margin: 0;
            padding: 0;
        }

        .main-pointer {
            cursor: pointer;
        }

        .main-move {
            cursor: move;
        }

        .simple {
            stroke: var(--icon-simple-stroke, #21571E);
            stroke-width: 1px;
            stroke-linecap: round;
            fill: none;
        }

        .copy-paste {
            stroke: var(--icon-copy-paste-stroke, #21571E);
            stroke-width: 1px;
            stroke-linecap: round;
            stroke-linejoin: round;
            fill: var(--icon-copy-paste-fill, #fff);
        }

        .toggle-mode {
            stroke: #fff;
            stroke-width: 1px;
            stroke-linecap: round;
            stroke-linejoin: round;
            fill: #fff;
        }

        .solid {
            stroke: var(--icon-solid-stroke, none);
            stroke-width: 1px;
            fill: var(--icon-solid-fill, #21571E);
        }

        .float {
            stroke: var(--icon-simple-stroke, #21571E);
            stroke-width: 4px;
            fill: var(--icon-solid-fill, #21571E);
        }
        
        .drag-dots {
            stroke-width: 0px;
            fill: var(--icon-simple-stroke, #21571E);
        }

        .drop-shadow {
            filter: drop-shadow( 1px 1px 1px rgba(0, 0, 0, .7));
        }
    `;

    @property({type: Number})
    inspectIconType!: InspectIconType;

    render(): TemplateResult {
        var svg = this.renderSvg();

        var cursorClass = this.inspectIconType === InspectIconType.DragDots
            ? "main main-move"
            : "main main-pointer";

        return html`
            <div class="${cursorClass}">
                ${svg}
            </div>
        `;
    }

    renderSvg(): TemplateResult {
        switch (this.inspectIconType) {
            case InspectIconType.Expand:
                return html`
                    <svg viewBox="0 0 20 20" xmlns="http://www.w3.org/2000/svg" class="simple">
                        <line class="drop-shadow" x1="10" y1="13" x2="5" y2="8" />
                        <line class="drop-shadow" x1="10" y1="13" x2="15" y2="8" />
                    </svg>
                `;
            case InspectIconType.Collapse:
                return html`
                    <svg viewBox="0 0 20 20" xmlns="http://www.w3.org/2000/svg" class="simple">
                        <line class="drop-shadow" x1="10" y1="7" x2="5" y2="12" />
                        <line class="drop-shadow" x1="10" y1="7" x2="15" y2="12" />
                    </svg>
                `;
            case InspectIconType.ExpandHorizontal:
                return html`
                    <svg viewBox="0 0 20 20" xmlns="http://www.w3.org/2000/svg" class="solid">
                        <path d="M 5 0 L 15 10 5 20Z" />
                    </svg>
                `;
            case InspectIconType.CollapseHorizontal:
                return html`
                    <svg viewBox="0 0 20 20" xmlns="http://www.w3.org/2000/svg" class="solid">
                        <path d="M 17 3 L 3 17 17 17Z" />
                    </svg>
                `;
            case InspectIconType.Plus:
                return html`
                    <svg viewBox="0 0 20 20" xmlns="http://www.w3.org/2000/svg" class="solid">
                        <path d="M 0 8 l 8 0 0 -8 4 0 0 8 8 0 0 4 -8 0 0 8 -4 0 0 -8 -8 0Z" />
                    </svg>
                `;
            case InspectIconType.Ex:
                return html`
                    <svg viewBox="0 0 20 20" xmlns="http://www.w3.org/2000/svg" class="solid">
                        <g transform="rotate(45 10 10)">
                            <path d="M 0 8 l 8 0 0 -8 4 0 0 8 8 0 0 4 -8 0 0 8 -4 0 0 -8 -8 0Z" />
                        </g>
                    </svg>
                `;
            case InspectIconType.Copy:
                return html`
                    <svg viewBox="0 0 20 20" xmlns="http://www.w3.org/2000/svg" class="copy-paste">
                        <rect width="10" height="12" x="5" y="2" rx="1" ry="1" />
                        <line x1="7" y1="4" x2="13" y2="4" />
                        <line x1="10" y1="19" x2="10" y2="10" />
                        <line x1="8" y1="12" x2="10" y2="10" />
                        <line x1="12" y1="12" x2="10" y2="10" />
                    </svg>
                `;
            case InspectIconType.Paste:
                return html`
                    <svg viewBox="0 0 20 20" xmlns="http://www.w3.org/2000/svg" class="copy-paste">
                        <rect width="10" height="12" x="5" y="2" rx="1" ry="1" />
                        <line x1="7" y1="4" x2="13" y2="4" />
                        <line x1="10" y1="19" x2="10" y2="10" />
                        <line x1="10" y1="19" x2="8" y2="17" />
                        <line x1="10" y1="19" x2="12" y2="17" />
                    </svg>
                `;
            case InspectIconType.Light:
                return html`
                    <svg viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg">
                        <circle cx="50" cy="50" r="20" fill="#fff" />
                        <g stroke="#fff" stroke-width="4">
                            <line x1="50" y1="5" x2="50" y2="20" />
                            <line x1="50" y1="80" x2="50" y2="95" />
                            <line x1="5" y1="50" x2="20" y2="50" />
                            <line x1="80" y1="50" x2="95" y2="50" />
                            <line x1="20" y1="20" x2="30" y2="30" />
                            <line x1="80" y1="20" x2="70" y2="30" />
                            <line x1="20" y1="80" x2="30" y2="70" />
                            <line x1="80" y1="80" x2="70" y2="70" />
                        </g>
                    </svg>
                `;
            case InspectIconType.Dark:
                return html`
                    <svg viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg">
                        <circle cx="50" cy="50" r="30" fill="#fff" />
                        <circle cx="60" cy="40" r="30" fill="#000" />
                    </svg>
                `;
            case InspectIconType.Float:
                return html`
                    <svg viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg">
                        <g class="float">
                            <rect width="50" height="50" x="25" y="20" rx="1" ry="1" />
                            <rect width="50" height="50" x="10" y="35" rx="1" ry="1" />
                        </g>
                    </svg>
                `;
            case InspectIconType.DragDots:
                return html`
                    <svg viewBox="0 0 50 100" xmlns="http://www.w3.org/2000/svg">
                        <g class="drag-dots">
                            <circle cx="5" cy="15" r="5" />
                            <circle cx="25" cy="35" r="5" />
                            <circle cx="45" cy="15" r="5" />
                            <circle cx="5" cy="50" r="5" />
                            <circle cx="25" cy="70" r="5" />
                            <circle cx="45" cy="50" r="5" />
                            <circle cx="5" cy="85" r="5" />
                            <circle cx="45" cy="85" r="5" />
                        </g>
                    </svg>
                `;
        }
    }
}

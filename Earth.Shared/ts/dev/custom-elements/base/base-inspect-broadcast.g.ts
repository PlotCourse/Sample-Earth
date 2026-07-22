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
import { state } from "@node_modules/lit/decorators.js";
import { ObservableDisconnectedMixin } from "../../../mixins/observable-disconnected-mixin";
import { BroadcastReceivedInfo } from "../../broadcast-received-info";

export abstract class BaseInspectBroadcast extends ObservableDisconnectedMixin(LitElement) {
    static styles = css`
        :host {
            font-family: Courier, sans serif;
        }

        .main {
            display: flex;
            flex-direction: row;
        }

        .notification-lists {
            display: flex;
            flex-direction: column;
        }

        .notification-list {
            display: flex;
            flex-direction: column;
            font-family: Montserrat, sans-serif;
            margin: 10px;
        }

        .notification-list-header-text {
            font-family: Montserrat, sans-serif;
            margin-bottom: 4px;
            color: var(--color);
        }

        option {
            min-width: 200px;
        }

        inspect-text {
            --inspect-text-height: 500px;
        }
    `;

    @state()
    protected isSelectedMessage = false;

    @state()
    protected selectedIndex = -1;

    protected messagesCache: BroadcastReceivedInfo[];
    protected observablesCache: BroadcastReceivedInfo[];

    constructor(
        broadcastReceivedInfoCache: { [name: string]: BroadcastReceivedInfo[] },
        messagesCacheKey: string,
        observablesCacheKey: string) {

        super();

        if (!broadcastReceivedInfoCache[messagesCacheKey]) {
            broadcastReceivedInfoCache[messagesCacheKey] = new Array<BroadcastReceivedInfo>();
        } else {
            var cache = broadcastReceivedInfoCache[messagesCacheKey];
            if (cache.length > 0) {
                this.isSelectedMessage = true;
                this.selectedIndex = cache.length - 1;
            }
        }

        if (!broadcastReceivedInfoCache[observablesCacheKey]) {
            broadcastReceivedInfoCache[observablesCacheKey] = new Array<BroadcastReceivedInfo>();
        } else {
            var cache = broadcastReceivedInfoCache[observablesCacheKey];
            if (cache.length > 0) {
                this.isSelectedMessage = false;
                this.selectedIndex = cache.length - 1;
            }
        }

        this.messagesCache = broadcastReceivedInfoCache[messagesCacheKey];
        this.observablesCache = broadcastReceivedInfoCache[observablesCacheKey];
    }

    protected flushCache(cache: BroadcastReceivedInfo[], limit: number): void {
        if (cache.length > limit) {
            cache.shift();

            for (var b of cache) {
                b.index--;
            }
        }
    }

    render(): TemplateResult {
        var detail = html``;

        if (this.selectedIndex > -1) {
            var info = this.isSelectedMessage
                ? this.messagesCache[this.selectedIndex]
                : this.observablesCache[this.selectedIndex];
            var event = this.isSelectedMessage
                ? "message received"
                : "observable received";

            var headerText = `"${info.summary}" ${event} at ${info.received}`;

            detail = html`
                <inspect-text .headerText=${headerText} .json=${info.detail}></inspect-text>
            `;
        }

        return html`
            <div class="main">
                <div class="notification-lists">
                    ${this.renderNotificationHistory(false)}
                    ${this.renderNotificationHistory(true)}
                </div>
                ${detail}
            </div>
        `;
    }

    renderNotificationHistory(isMessages: boolean): TemplateResult {
        const { history, messageForEmpty, header, handleChange } = isMessages
            ? {
                history: this.messagesCache,
                messageForEmpty: "No Messages Received",
                header: "Messages received:",
                handleChange: this.messagesListChange
            }
            : {
                history: this.observablesCache,
                messageForEmpty: "No Observable Updates",
                header: "Observable updates:",
                handleChange: this.observablesListChange
            };

        if (history.length === 0) {
            return html`
                <div class="notification-list">
                    <div class="notification-list-header-text">
                        ${messageForEmpty}
                    </div>
                </div>
            `;
        }

        return html`
            <div class="notification-list">
                <div class="notification-list-header-text">
                    ${header}
                </div>
                <select size="5" @change=${handleChange}>
                    ${history.map(h => this.renderNotificationOption(h))}
                </select>
            </div>
        `;
    }

    renderNotificationOption(info: BroadcastReceivedInfo): TemplateResult {
        var selected = (info.isMessage === this.isSelectedMessage
            && info.index === this.selectedIndex);

        return html`<option .selected=${selected}>${info.summary}</option>`;
    }

    observablesListChange(event: Event): void {
        var list = event.target as HTMLSelectElement;
        this.isSelectedMessage = false;
        this.selectedIndex = list.selectedIndex;
    }

    messagesListChange(event: Event): void {
        var list = event.target as HTMLSelectElement;
        this.isSelectedMessage = true;
        this.selectedIndex = list.selectedIndex;
    }
}

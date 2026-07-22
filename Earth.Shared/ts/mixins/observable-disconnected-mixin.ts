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
import { LitElement } from "@node_modules/lit";
import { ReplaySubject } from "@node_modules/rxjs";

type Constructor<T> = new (...args: any[]) => T;

/**
 * Exposes an observable for an element's discconnectedCallbackAsync() that simplifies setup code
 * for other subscribers that should only be subscribing when the element is connected.  This is
 * done by subscribing in the connectedCallbackAsync() using a "pipe" call with operator:
 *      takeUntil(this.disconnected$)
 */

export abstract class ObservableDisconnectedClass extends LitElement {
    disconnected$: ReplaySubject<void>;
}

export const ObservableDisconnectedMixin = <T extends Constructor<LitElement>>(base: T): T & Constructor<ObservableDisconnectedClass> => {
    return class ObservableDisconnected extends base {
        disconnected$: ReplaySubject<void>;
        connectedCallback(): void {
            super.connectedCallback();
            this.disconnected$ = new ReplaySubject<void>();
        }
        disconnectedCallback(): void {
            super.disconnectedCallback();
            this.disconnected$.next(undefined);
            this.disconnected$.complete();
        }
    };
};

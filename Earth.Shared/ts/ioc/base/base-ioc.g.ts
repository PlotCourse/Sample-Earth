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
import { IocResolver } from "../interfaces/ioc-resolver.g";

export abstract class BaseIocContainer {
    protected resolversByName: { [name: symbol]: IocResolver } = {};

    /**
     * Set up IOC resolution for a transient object that will be created on every access.
     * @param key A unique symbol that will be used to set up injection of this transient object.
     * @param factory A method that will be used to create the object every time it's used.
     */
    addTransient<T>(key: symbol, factory: () => T): void {
        this.resolversByName[key] = {
            isSingleton: false,
            factory: factory as () => object,
            value: null
        };
    }

    /**
     * Set up IOC resolution for a singleton provided now.
     * @param key A unique symbol that will be used to set up injection of this singleton.
     * @param instance The singleton that should always resolve for injection configured with the indicated key.
     */
    addSingleton<T>(key: symbol, instance: T): void {
        this.resolversByName[key] = {
            isSingleton: true,
            value: instance as object
        };
    }

    /**
     * Set up IOC resolution for a singleton that will only be constructed at the time it's used if at all.
     * @param key A unique symbol that will be used to set up injection of this singleton.
     * @param factory A method that will be used to create the singleton the first time it's injected somewhere.
     */
    addSingletonForLazyConstruction<T>(key: symbol, factory: () => T): void {
        this.resolversByName[key] = {
            isSingleton: true,
            factory: factory as () => object,
            value: null
        };
    }

    resolve(name: symbol): object {
        var r = this.resolversByName[name];

        if (!r) {
            return null;
        }

        if (r.value !== null) {
            return r.value;
        }

        var v = r.factory();

        if (r.isSingleton) {
            r.value = v;
        }

        return v;
    }
}

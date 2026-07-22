import { BaseIocContainer } from "./base/base-ioc.g";

class IocContainer extends BaseIocContainer {
}

export const iocContainer = new IocContainer();

export function lazyInject<T>(name: symbol) {
    return function actualDecorator(target: any, propertyKey: string | symbol): void {
        Object.defineProperty(target, propertyKey, {
            get(): T {
                return iocContainer.resolve(name) as T;
            },
            enumerable: true,
            configurable: true,
        });
    }
}

export function resolve<T>(name: symbol) {
    return iocContainer.resolve(name) as T;
}


export interface IocResolver {
    isSingleton: boolean;
    factory?: () => object;
    value: object;
}

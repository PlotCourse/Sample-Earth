import { BaseOceanSurfaceBroadcast } from "./base/base-ocean-surface-broadcast.g";
import { IOceanSurfaceBroadcast } from "./interfaces/ocean-surface-broadcast";

// Use this class to override methods as needed, for example, hub connection configuration could be modified as follows:
//import { IHttpConnectionOptions } from "@microsoft/signalr/dist/esm/IHttpConnectionOptions";
//...
//protected buildHubConnectionOptions(): IHttpConnectionOptions {
//    return {
//        accessTokenFactory: () => this._accessToken
//    };
//}
export class OceanSurfaceBroadcast extends BaseOceanSurfaceBroadcast implements IOceanSurfaceBroadcast {
}

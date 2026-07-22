import { BaseWaterService } from "./base/base-water-service.g";
import { IWaterService } from "./interfaces/water-service";

export class WaterService extends BaseWaterService implements IWaterService {
    // Use this class to override any proxy functionality only in this UI component.  If overriding proxy
    //  functionality for all clients is preferred, this can be done in the grandparent of this class.
}

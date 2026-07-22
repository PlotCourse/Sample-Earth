import { BaseTricksService } from "./base/base-tricks-service.g";
import { ITricksService } from "./interfaces/tricks-service";

export class TricksService extends BaseTricksService implements ITricksService {
    // Use this class to override any proxy functionality only in this UI component.  If overriding proxy
    //  functionality for all clients is preferred, this can be done in the grandparent of this class.
}

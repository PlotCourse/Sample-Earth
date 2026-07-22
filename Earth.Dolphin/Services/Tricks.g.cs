using Microsoft.Extensions.Logging;
using Earth.Dolphin.Services.Base;
using Service_Ocean_IWaterService = Earth.Dolphin.Proxies.Ocean.Services.Interfaces.IWaterService;

namespace Earth.Dolphin.Services;

internal partial class Tricks : BaseTricks
{
    // If additional dependencies are needed, a new constructor can be defined in the
    // dev-managed partial definition for this class using the attribute,
    // "ActivatorUtilitiesConstructor".
    public Tricks(
        Service_Ocean_IWaterService oceanWaterService,
        ILogger<Tricks> logger) : base(
            oceanWaterService,
            logger) { }
}

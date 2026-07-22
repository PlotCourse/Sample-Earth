using Microsoft.Extensions.Logging;
using Earth.Ocean.Services.Base;
using Broadcast_IOceanSurfaceInternalPublisher = Earth.Ocean.Broadcasts.Interfaces.IOceanSurfaceInternalPublisher;

namespace Earth.Ocean.Services;

internal partial class Water : BaseWater
{
    // If additional dependencies are needed, a new constructor can be defined in the
    // dev-managed partial definition for this class using the attribute,
    // "ActivatorUtilitiesConstructor".
    public Water(
        Broadcast_IOceanSurfaceInternalPublisher oceanOceanSurfacePublisher,
        ILogger<Water> logger) : base(
            oceanOceanSurfacePublisher,
            logger) { }
}

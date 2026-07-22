using Microsoft.Extensions.Logging;
using Earth.Dolphin.Base;
using Broadcast_Ocean_IOceanSurfaceInternalPublisher = Earth.Dolphin.Proxies.Ocean.Broadcasts.Interfaces.IOceanSurfaceInternalPublisher;

namespace Earth.Dolphin;

internal partial class MonitorOcean : BaseMonitorOcean
{
    // If additional dependencies are needed, a new constructor can be defined in the
    // dev-managed partial definition for this class using the attribute,
    // "ActivatorUtilitiesConstructor".
    public MonitorOcean(
        Broadcast_Ocean_IOceanSurfaceInternalPublisher oceanOceanSurfacePublisher,
        ILogger<MonitorOcean> logger) : base(
            oceanOceanSurfacePublisher,
            logger) { }
}

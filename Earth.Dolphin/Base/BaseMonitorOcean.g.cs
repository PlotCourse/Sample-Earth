using Microsoft.Extensions.Logging;
using Broadcast_Ocean_IOceanSurfaceInternalPublisher = Earth.Dolphin.Proxies.Ocean.Broadcasts.Interfaces.IOceanSurfaceInternalPublisher;

namespace Earth.Dolphin.Base;

internal abstract partial class BaseMonitorOcean
{
    protected Broadcast_Ocean_IOceanSurfaceInternalPublisher _oceanOceanSurfacePublisher;
    protected ILogger<MonitorOcean> _logger;

    public BaseMonitorOcean(
        Broadcast_Ocean_IOceanSurfaceInternalPublisher oceanOceanSurfacePublisher,
        ILogger<MonitorOcean> logger)
    {
        _oceanOceanSurfacePublisher = oceanOceanSurfacePublisher;
        _logger = logger;

        if (_subscribeToOceanOceanSurface)
        {
            _cancellationOceanOceanSurface = _oceanOceanSurfacePublisher.Subscribe(this);
        }
    }
}

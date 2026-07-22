using Earth.Shared.Broadcasts.InProc.Interfaces;

namespace Earth.Dolphin.Proxies.Ocean.Broadcasts.Interfaces;

internal partial interface IOceanSurfaceInternalPublisher : IPublisher<IOceanSurfaceInternalBroadcast>, IOceanSurfaceInternalBroadcast
{
}

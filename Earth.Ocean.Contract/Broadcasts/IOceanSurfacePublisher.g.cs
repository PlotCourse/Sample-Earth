using Earth.Shared.Broadcasts.InProc.Interfaces;

namespace Earth.Ocean.Contract.Broadcasts;

public partial interface IOceanSurfacePublisher : IPublisher<IOceanSurfaceBroadcast>, IOceanSurfaceBroadcast
{
}

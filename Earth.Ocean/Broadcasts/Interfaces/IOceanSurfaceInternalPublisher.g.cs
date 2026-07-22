
namespace Earth.Ocean.Broadcasts.Interfaces;

internal partial interface IOceanSurfaceInternalPublisher
    : Earth.Shared.Broadcasts.InProc.Interfaces.IPublisher<IOceanSurfaceInternalBroadcast>, IOceanSurfaceInternalBroadcast
{
}

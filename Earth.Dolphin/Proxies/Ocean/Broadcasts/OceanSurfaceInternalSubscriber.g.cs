using Earth.Dolphin.Proxies.Ocean.Broadcasts.Interfaces;
using Earth.Dolphin.Data;

namespace Earth.Dolphin.Proxies.Ocean.Broadcasts;

/// <summary>
/// Implement a sub-class of this class to receive event messages and updates to observables
/// or create some other IOceanSurfaceBroadcast that subscribes to the same publisher used
/// here.  Note that services can be automatically configured as broadcast subscribers, separately
/// from this class.
/// </summary>
internal abstract partial class OceanSurfaceInternalSubscriber : IOceanSurfaceInternalBroadcast
{
    private IDisposable _cancellation;
    protected IOceanSurfaceInternalPublisher _publisher;

    public abstract int Temperature { get; set; }

    public abstract void NewHurricane(
        HurricaneCoordinate location);

    public OceanSurfaceInternalSubscriber(IOceanSurfaceInternalPublisher publisher)
    {
        _publisher = publisher;
        _cancellation = publisher.Subscribe(this);
    }

    public void Unsubscribe()
    {
        _cancellation.Dispose();
    }
}

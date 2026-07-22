using Earth.Ocean.Data;

using Earth.Ocean.Broadcasts.Interfaces;

namespace Earth.Ocean.Broadcasts.Base;

/// <summary>
/// Implement a sub-class of this class to receive event messages and updates to observables
/// or create some other IOceanSurfaceBroadcast that subscribes to the same publisher used
/// here.  Note that services can be automatically configured as broadcast subscribers, separately
/// from this class.
/// </summary>
internal abstract partial class BaseOceanSurfaceInternalSubscriber : IOceanSurfaceInternalBroadcast
{
    private IDisposable _cancellation;
    protected IOceanSurfaceInternalPublisher _publisher;

    public abstract int Temperature { get; set; }

    public abstract void NewHurricane(
        HurricaneCoordinate location);

    public BaseOceanSurfaceInternalSubscriber(IOceanSurfaceInternalPublisher publisher)
    {
        _publisher = publisher;
        _cancellation = publisher.Subscribe(this);
    }

    public virtual void Unsubscribe()
    {
        _cancellation.Dispose();
    }
}

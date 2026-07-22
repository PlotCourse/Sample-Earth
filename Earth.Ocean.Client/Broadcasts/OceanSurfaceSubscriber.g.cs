using Earth.Shared.Broadcasts.InProc.Interfaces;
using Earth.Ocean.Contract.Broadcasts;

using Earth.Ocean.Contract.Data;


namespace Earth.Ocean.Client.Broadcasts;

/// <summary>
/// The base set of observable(s) and message(s) contained in a broadcast that can be updated
/// by a publisher.  Sub-classes of this subscriber subscribe to the same Contract record types
/// that are published in the original broadcast.
/// </summary>
public abstract class OceanSurfaceSubscriber : IOceanSurfaceBroadcast
{
    private IDisposable _cancellation;

    public abstract int Temperature { get; set; }

    public abstract void NewHurricane(
        HurricaneCoordinate location);

    public OceanSurfaceSubscriber(IPublisher<IOceanSurfaceBroadcast> publisher)
    {
        _cancellation = publisher.Subscribe(this);
    }

    public void Unsubscribe()
    {
        _cancellation.Dispose();
    }
}

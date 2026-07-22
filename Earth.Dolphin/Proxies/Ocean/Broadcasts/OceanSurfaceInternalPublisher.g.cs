using Earth.Shared.Broadcasts.InProc;
using Earth.Dolphin.Proxies.Ocean.Broadcasts.Interfaces;
using Earth.Dolphin.Data;

namespace Earth.Dolphin.Proxies.Ocean.Broadcasts;

/// <summary>
/// This singleton can be expected to have as many subscribers as the code inside this component
/// wishes to create.  For any subscribers that are transient, the consuming code should always
/// call "Unsubscribe" when finished with the subscriber.
/// </summary>
internal partial class OceanSurfaceInternalPublisher : Publisher<IOceanSurfaceInternalBroadcast>, IOceanSurfaceInternalPublisher
{
    private int _temperature = 0;

    public int Temperature
    {
        get => _temperature;
        set
        {
            _temperature = value;
            DoBroadcast(r => r.Temperature = value);
        }
    }

    public void NewHurricane(
        HurricaneCoordinate location)
    {
        DoBroadcast(r => r.NewHurricane(
            location));
    }

    protected override void InitializeNewSubscriber(IOceanSurfaceInternalBroadcast subscriber)
    {
        subscriber.Temperature = _temperature;
    }
}

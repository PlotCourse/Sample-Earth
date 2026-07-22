using Earth.Ocean.Data;

using Earth.Ocean.Broadcasts.Interfaces;
using Earth.Shared.Broadcasts.InProc;

namespace Earth.Ocean.Broadcasts.Base;

/// <summary>
/// This singleton can be expected to have as many subscribers as the code inside this component
/// wishes to create.  For any subscribers that are transient, the consuming code should always
/// call "Unsubscribe" when finished with the subscriber.
/// </summary>
internal abstract partial class BaseOceanSurfaceInternalPublisher : Publisher<IOceanSurfaceInternalBroadcast>, IOceanSurfaceInternalPublisher
{
    //TOOLINFO_SCALEOUT: support?  Instead of get/set to this private value, put
    //  this in a redis cache or...
    private int _temperature = 0;

    public virtual int Temperature
    {
        get => _temperature;
        set
        {
            _temperature = value;
            DoBroadcast(r => r.Temperature = value);
        }
    }

    public virtual void NewHurricane(
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

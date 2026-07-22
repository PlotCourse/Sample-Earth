using InternalDataType_HurricaneCoordinate = Earth.Dolphin.Data.HurricaneCoordinate;
using Earth.Dolphin.Proxies.Ocean.Broadcasts.Interfaces;

namespace Earth.Dolphin.Base;

/// <summary>
/// This part of the base class was generated so that this service can subscribe to the OceanSurface
/// broadcast. The MonitorOcean class can override these properties / methods as needed to receive
/// notifications from the OceanSurface broadcast.
/// </summary>
internal abstract partial class BaseMonitorOcean : IOceanSurfaceInternalBroadcast
{
    //This flag is generated to allow developer control over whether this service instance should
    //  subscribe.  If the Web API with this component is scaled out and the app design is such
    //  that only one instance needs observables/messages from the broadcast.  Dependencies can
    //  be added as needed to the constructor of this service to set this flag false in the
    //  additional instances.
    protected bool _subscribeToOceanOceanSurface = true;
    protected IDisposable _cancellationOceanOceanSurface;

    protected int _temperature = 0;

    public virtual int Temperature
    {
        get => _temperature;
        set
        {
            _temperature = value;
        }
    }

    public virtual void NewHurricane(
        InternalDataType_HurricaneCoordinate location)
    {
    }

    protected void UnsubscribeOceanOceanSurface()
    {
        _cancellationOceanOceanSurface?.Dispose();
    }
}

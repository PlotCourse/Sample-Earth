using Earth.Ocean.Broadcasts.Interfaces;
using Earth.Ocean.Contract.Broadcasts;
using ContractDataType_OceanSurfaceBroadcastObservables = Earth.Ocean.Contract.Data.OceanSurfaceBroadcastObservables;
using ContractDataType_HurricaneCoordinate = Earth.Ocean.Contract.Data.HurricaneCoordinate;

namespace Earth.Ocean.Broadcasts.Base;

/// <summary>
/// This singleton can be expected to have:
/// 1 subscriber per dependent component that runs inside the same Web API and acts a relay to
///     dependent component's internal publisher.  The relay maps contract record types for this
///     component to the record types used internally in the dependent component.
/// PLUS 1 more subscriber that relays messages to a SignalR hub if there's at least one dependent
///     component outside the Web API of this component, or if this broadcast was
///     configured to always publish externally.  (OceanSurfaceBroadcastRelay generated in this
///     same namespace.)
/// </summary>
internal abstract partial class BaseOceanSurfaceExternalPublisher
    : Earth.Shared.Broadcasts.InProc.Publisher<IOceanSurfaceBroadcast>, IOceanSurfaceExternalPublisher
{
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

    public virtual ContractDataType_OceanSurfaceBroadcastObservables GetAllBroadcastObservables()
    {
        return new ContractDataType_OceanSurfaceBroadcastObservables(
            Temperature);
    }

    public virtual void NewHurricane(
        ContractDataType_HurricaneCoordinate location)
    {
        DoBroadcast(r => r.NewHurricane(
            location));
    }

    protected override void InitializeNewSubscriber(IOceanSurfaceBroadcast subscriber)
    {
        subscriber.Temperature = _temperature;
    }
}

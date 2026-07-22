using ContractDataType_OceanSurfaceBroadcastObservables = Earth.Ocean.Contract.Data.OceanSurfaceBroadcastObservables;
using Earth.Ocean.Broadcasts.Base;
using Earth.Ocean.Broadcasts.Interfaces;
using Earth.Ocean.Contract.Broadcasts;

namespace Earth.Ocean.Broadcasts;

/// <summary>
/// Routes messages and changes to observables from this component's singleton publisher to
/// clients outside this Web API by using a SignalR hub.
/// </summary>
internal class OceanSurfaceRelayToHub(
    IOceanSurfaceExternalPublisher sourcePublisher,
    Earth.Shared.Broadcasts.Remote.Interfaces.IPublisher<OceanSurfaceHub, ContractDataType_OceanSurfaceBroadcastObservables, OceanSurfaceNotificationType> remotePublisher)
    : BaseOceanSurfaceRelayToHub(sourcePublisher, remotePublisher)
{
}

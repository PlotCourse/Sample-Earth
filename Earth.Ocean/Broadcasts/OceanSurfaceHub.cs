using Microsoft.Extensions.Logging;
using Earth.Ocean.Contract.Broadcasts;
using ContractDataType_OceanSurfaceBroadcastObservables = Earth.Ocean.Contract.Data.OceanSurfaceBroadcastObservables;

using Earth.Ocean.Broadcasts.Base;

namespace Earth.Ocean.Broadcasts;

public class OceanSurfaceHub(
    Earth.Shared.Broadcasts.Remote.Interfaces.IPublisher<OceanSurfaceHub, ContractDataType_OceanSurfaceBroadcastObservables, OceanSurfaceNotificationType> remotePublisher,
    ILogger<OceanSurfaceHub> logger) : BaseOceanSurfaceHub(remotePublisher, logger)
{
}

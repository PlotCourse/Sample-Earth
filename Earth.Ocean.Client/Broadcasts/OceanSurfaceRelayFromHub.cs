using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Earth.Ocean.Contract.Broadcasts;
using Earth.Ocean.Client.Broadcasts.Base;

namespace Earth.Ocean.Client.Broadcasts;

public partial class OceanSurfaceRelayFromHub : BaseOceanSurfaceRelayFromHub
{
    public OceanSurfaceRelayFromHub(
        HubConnection hubConnection,
        string clientId,
        Earth.Shared.Broadcasts.Remote.Interfaces.IClientSequenceTracker<OceanSurfaceNotificationType> sequenceTracker,
        ILogger<OceanSurfaceRelayFromHub> logger) : base(hubConnection, clientId, sequenceTracker, logger) { }
}

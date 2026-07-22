using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Earth.Ocean.Contract.Broadcasts;
using ContractDataType_OceanSurfaceBroadcastObservables = Earth.Ocean.Contract.Data.OceanSurfaceBroadcastObservables;

namespace Earth.Ocean.Broadcasts.Base;

public abstract partial class BaseOceanSurfaceHub(
    Earth.Shared.Broadcasts.Remote.Interfaces.IPublisher<OceanSurfaceHub, ContractDataType_OceanSurfaceBroadcastObservables, OceanSurfaceNotificationType> remotePublisher,
    ILogger<OceanSurfaceHub> logger) : Hub
{
    public async override Task OnConnectedAsync()
    {
        var request = Context.GetHttpContext()?.Request;
        var clientId = request?.Headers?["ClientId"];

        if (string.IsNullOrWhiteSpace(clientId) && request != null)
        {
            clientId = request.Query["ClientId"];
        }

        await remotePublisher.Subscribe(
            Clients.Caller,
            clientId,
            Context.ConnectionId,
            true);

        await base.OnConnectedAsync();
    }


}

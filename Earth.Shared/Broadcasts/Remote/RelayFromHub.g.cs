using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Earth.Shared.Broadcasts.Remote.Interfaces;
using IRetryPolicy = Earth.Shared.Broadcasts.Remote.Interfaces.IRetryPolicy;
using Earth.Shared.Broadcasts.Remote.Base;

namespace Earth.Shared.Broadcasts.Remote;

public abstract partial class RelayFromHub<TSubscriber, TNotificationType> : BaseRelayFromHub<TSubscriber, TNotificationType>
    where TNotificationType : Enum
{
    public RelayFromHub(
        HubConnection hubConnection,
        IRetryPolicy firstConnectRetryPolicy,
        IRetryPolicy disconnectionRetryPolicy,
        string clientId,
        int clientKeepAliveIntervalSeconds,
        IClientSequenceTracker<TNotificationType> sequenceTracker,
        ILogger logger,
        Action<PublisherOptions> configurePublisher)
        : base(
            hubConnection,
            firstConnectRetryPolicy,
            disconnectionRetryPolicy,
            clientId,
            clientKeepAliveIntervalSeconds,
            sequenceTracker,
            logger,
            configurePublisher)
    { }
}

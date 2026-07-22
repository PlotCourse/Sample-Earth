using Microsoft.AspNetCore.SignalR;

namespace Earth.Shared.Broadcasts.Remote.Interfaces;

public partial interface IPublisher<THub, TBroadcastObservables, TNotificationType>
    where THub : Hub
    where TBroadcastObservables : class
    where TNotificationType : Enum
{
    Task Subscribe(
        ISingleClientProxy caller,
        string clientId,
        string connectionId,
        bool isInitialConnection);

    void Send(
        string method,
        object[] args,
        TNotificationType type,
        CancellationToken cancellationToken = default);

    Task ClientHeartbeatReceived(
        ISingleClientProxy caller,
        string clientId,
        string connectionId);

    Task ConfirmationReceived(
        ISingleClientProxy caller,
        string clientId,
        string connectionId,
        string notificationId);
}

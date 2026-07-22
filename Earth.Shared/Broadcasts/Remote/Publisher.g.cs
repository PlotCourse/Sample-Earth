using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Earth.Shared.Broadcasts.Remote.Base;
using Earth.Shared.Broadcasts.Remote.Interfaces;

namespace Earth.Shared.Broadcasts.Remote;

public partial class Publisher<THub, TBroadcastObservables, TNotificationType>
    : BasePublisher<THub, TBroadcastObservables, TNotificationType>, IPublisher<THub, TBroadcastObservables, TNotificationType>
    where THub : Hub
    where TBroadcastObservables : class
    where TNotificationType : Enum
{
    public Publisher(
        IHubContext<THub> hubContext,
        Func<TBroadcastObservables> observablesProvider,
        Dictionary<TNotificationType, NotificationTypeDetails> notificationTypeDetails,
        TNotificationType notificationTypeForObservableInit,
        ILogger<Publisher<THub, TBroadcastObservables, TNotificationType>> logger,
        Action<PublisherOptions> configurePublisher)
        : base(
            hubContext,
            observablesProvider,
            notificationTypeDetails,
            notificationTypeForObservableInit,
            logger,
            configurePublisher)
    { }
}

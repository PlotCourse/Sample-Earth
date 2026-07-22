using Earth.Shared.Broadcasts.Remote.Base;

namespace Earth.Shared.Broadcasts.Remote;

public partial class Notification<TNotificationType> : BaseNotification<TNotificationType> where TNotificationType : Enum
{
    public Notification(
        string notificationId,
        string method,
        object[] args,
        TNotificationType notificationType) : base(notificationId, method, args, notificationType) { }
}

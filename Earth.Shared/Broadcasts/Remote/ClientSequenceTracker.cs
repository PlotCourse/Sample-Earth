using Earth.Shared.Broadcasts.Remote.Base;
using Earth.Shared.Broadcasts.Remote.Interfaces;

namespace Earth.Shared.Broadcasts.Remote;

public class ClientSequenceTracker<TNotificationType> : BaseClientSequenceTracker<TNotificationType>, IClientSequenceTracker<TNotificationType>
    where TNotificationType : Enum
{
}

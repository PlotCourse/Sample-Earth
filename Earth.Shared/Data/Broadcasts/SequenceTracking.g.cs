using Earth.Shared.Data.Broadcasts.Base;
namespace Earth.Shared.Data.Broadcasts;

public partial class SequenceTracking<TNotificationType> : BaseSequenceTracking<TNotificationType>
    where TNotificationType : Enum
{
    public SequenceTracking(TNotificationType notificationType, int sequenceNumber) : base(notificationType, sequenceNumber) { }
}

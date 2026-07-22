using System.Text.Json.Serialization;

namespace Earth.Shared.Data.Broadcasts.Base;

public class BaseSequenceTracking<TNotificationType>
    where TNotificationType : Enum
{
    protected TNotificationType _notificationType;
    protected int _sequenceNumber;

    [JsonPropertyName("notificationType")]
    public virtual TNotificationType NotificationType => _notificationType;

    [JsonPropertyName("sequenceNumber")]
    public virtual int SequenceNumber => _sequenceNumber;

    public BaseSequenceTracking(TNotificationType notificationType, int sequenceNumber)
    {
        _notificationType = notificationType;
        _sequenceNumber = sequenceNumber;
    }
}

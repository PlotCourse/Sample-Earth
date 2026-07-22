using Earth.Shared.Data.Broadcasts;

namespace Earth.Shared.Broadcasts.Remote.Interfaces;

public partial interface IClientSequenceTracker<TNotificationType>
    where TNotificationType : Enum
{
    /// <summary>
    /// Replaces all sequence number tracking by this tracker and starts with any values provided here.
    /// </summary>
    /// <param name="trackings"></param>
    void Reset(SequenceTracking<TNotificationType>[] trackings);

    /// <summary>
    /// Updates the current sequence number to match the number provided.
    /// </summary>
    /// <param name="notificationType">Type of observable update or message that this sequence number corresponds to.</param>
    /// <param name="sequenceNumber">Number received from hub for this notification.</param>
    /// <returns>
    /// Null if the update is to the next sequential number as expected, otherwise returns the old value of the sequence number.
    /// In any case the update is still made.
    /// </returns>
    int? UpdateSequenceNumber(TNotificationType notificationType, int sequenceNumber);
}

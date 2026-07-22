/**
 * Passed to Broadcast subscribers when sequence tracking is used.
 */
export interface SequenceTracking<TNotificationType> {
    notificationType: TNotificationType;
    sequenceNumber: number;
}

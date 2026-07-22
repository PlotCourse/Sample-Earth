/**
 * Passed to Broadcast subscribers when confirmation is needed.
 */
export interface ConfirmationExpectation {
    notificationId: string;
    confirmAsync: boolean;
}

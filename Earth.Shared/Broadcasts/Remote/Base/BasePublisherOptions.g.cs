/*
MIT License

Copyright (c) 2025-2026 PlotCourse LLC

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
namespace Earth.Shared.Broadcasts.Remote.Base;

public abstract class BasePublisherOptions
{
    /// <summary>
    /// If set, client confirmation of observable initialization is expected when a client
    /// first connects, and if confirmation is not received in the specified timespan the
    /// notification is re-sent up to the number of retries specified.
    /// If "ConfirmationOfObservableUpdates" is also set, then any update notifications
    /// are queued until observable initialization is complete or failed all retries.
    /// If "ConfirmationOfMessages" is also set, then any messages are queued until
    /// observable initialization is complete or failed all retries.
    /// </summary>
    public virtual ConfirmationTimeoutOptions ConfirmationOfObservableInitialization { get; set; }

    /// <summary>
    /// If set, client confirmation of observable updates is expected and additional updates
    /// are queued per client until confirmation is received.  If "ConfirmationOfMessages"
    /// is also set then integrity of the order of notifications is maintained regardless of
    /// notification type.  In other words, a new observable update will not be sent to a
    /// specific client while a message confirmation from that client is still pending and
    /// vice-versa.
    /// 
    /// If ConfirmationOfObservableUpdates is not set, then updates to observables will be
    /// sent asynchronously to all clients in the order they occur but without any
    /// confirmation of delivery prior to sending additional updates.
    /// </summary>
    public virtual ConfirmationTimeoutOptions ConfirmationOfObservableUpdates { get; set; }

    /// <summary>
    /// If true a sequence number is incremented per observable and sent with the update
    /// notification.  Client side gap detection is also generated.
    /// </summary>
    public virtual bool SequenceTrackingOfObservableUpdates { get; set; }

    /// <summary>
    /// If set, client confirmation of messages is expected and additional updates are queued
    /// per client until confirmation is received.  If "ConfirmationOfObservableUpdates" is
    /// also set then integrity of the order of notifications is maintained regardless of
    /// notification type.  In other words, a new message will not be sent to a specific
    /// client while an observable update confirmation from that client is still pending and
    /// vice-versa.
    /// 
    /// If ConfirmationOfMessages is not set, then messages will be sent asynchronously to
    /// all clients in the order they occur but without any confirmation of delivery prior
    /// to sending additional updates.
    /// </summary>
    public virtual ConfirmationTimeoutOptions ConfirmationOfMessages { get; set; }

    /// <summary>
    /// If true a sequence number is incremented per observable and sent with the update
    /// notification.  Client side gap detection is also generated.
    /// </summary>
    public virtual bool SequenceTrackingOfMessages { get; set; }

    /// <summary>
    /// The amount of time to allow a client to be unresponsive before purging the client and
    /// its queue of unsent messages.  This setting should be chosen carefully.  The larger
    /// the timespan chosen the more memory will be consumed for queued messages for clients
    /// that have disconnected from the broadcast.  The volume of messages, the volume of
    /// clients, and the needs related to message integrity should all be considered.  In
    /// most cases, the default value 2 can be used with a plan for observable data that
    /// captures all information that a client needs when rejoining after a disconnect.
    /// </summary>
    public virtual TimeSpan PurgeSilentClients { get; set; }

    /// <summary>
    /// This setting is only relevant if at least one of the Confirmation* settings is provided.
    /// 
    /// When true, clients are always expected to make an async call to confirm receipt of each
    /// notification.  The calling context of that confirmation is then used to send the next
    /// message (if any) in the queue for that client and the calling context is immediately
    /// released.
    /// 
    /// When false, the publisher tells clients how to confirm depending on the calling
    /// context.  If the context is a confirmation from a previous notification and additional
    /// notifications exist in that same client's queue, the next notification will be *invoked*
    /// on the caller and will include an instruction in the call to tell the client that an
    /// async confirmation is not needed.  When the invoked call completes without errors, the
    /// next notification will be sent in the same way on the same context until the client's
    /// queue is empty.
    /// However, if the calling context is a broadcast of the same message to all clients then
    /// the clients are simply instructed to confirm the notification with an async callback.
    /// In high volume scenarios, notifications requiring confirmation will typically not be
    /// elligible to send to all clients at the same time because clients will have varying
    /// queue lengths due to differences in connection speeds.
    /// 
    /// Data regarding the scenarios in which to favor one value of the setting vs. another has
    /// not been fully collected however in general a value of true will more quickly release calling
    /// context resources whereas a value of false will require less calls to be made.
    /// </summary>
    public virtual bool AlwaysConfirmAsync { get; set; }

    /// <summary>
    /// This setting is only relevant if at least one of the Confirmation* settings is provided.
    /// 
    /// When false, notifications that require a confirmation are always retried when the
    /// call to a client fails or does not confirm in the configured time limit for the number of
    /// retries specified in the confirmation configuration.
    /// 
    /// When true, a reliability state that's tracked per client per notification type is also
    /// used to determine whether or not to retry a failed call.  See the enum "ClientReliability"
    /// for more info on this logic.
    /// </summary>
    public virtual bool UseAdaptiveRetryStrategy { get; set; }

    public BasePublisherOptions()
    {
        ConfirmationOfObservableInitialization = new ConfirmationTimeoutOptions(TimeSpan.FromSeconds(15), 3);
        ConfirmationOfObservableUpdates = new ConfirmationTimeoutOptions(TimeSpan.FromSeconds(15), 3);
        ConfirmationOfMessages = null;
        PurgeSilentClients = TimeSpan.FromMinutes(2);
        AlwaysConfirmAsync = true;
        UseAdaptiveRetryStrategy = true;
    }
}

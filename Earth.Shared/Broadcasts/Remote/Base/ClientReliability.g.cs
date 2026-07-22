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

/// <summary>
/// ClientReliability is tracked per TNotificationType for each client where TNotificationType
/// is the enum of possible methods called on the client for messages or updates to
/// observables or the observable initialization.
/// </summary>
public enum ClientReliability
{
    //The initial default state for each client / notification type.
    Unknown,

    //The publisher sets this state the first time a confirmation is received from the client
    //for the notification type.  Once in this state it remains this way for as long as this client
    //is tracked by the publisher.  If retries are set to > 0 for this type of confirmation then
    //the retries will always be attempted after a failed call regardless of the value of the
    //PublisherOption.UseAdaptiveRetryStrategy.
    CanSucceed,

    //The publisher sets this state for a particular client / notification type if the first
    //notification of that type did not receive a confirmation, nor was confirmation received on all
    //retries.  Yet, while in this state, if retries are set to > 0 for this type of confirmation
    //then the retries will continue to be done if the next call fails call regardless of the value
    //of the PublisherOption.UseAdaptiveRetryStrategy.
    Failed,

    //The publisher sets this state for a particular client / notification type if the second
    //notification of that type did not receive a confirmation, nor was confirmation received on all
    //retries.  If the PublisherOption.UseAdaptiveRetryStrategy has been set to true and
    //reliability is in this state the next time the same notification type is sent and it fails
    //then no retries are attempted and the state for this client broadcast type remains "AlwaysFails".
    //However if a notification of this type for this client ever succeeds the state changes to
    //CanSucceed and retries are never again skipped for this client for this notification type.
    AlwaysFails
}

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

public abstract class BaseConfirmationTimeoutOptions
{
    /// <summary>
    /// The amount of time to wait for a confirmation to come from a client before
    /// assuming it won't happen and either resending the notification or giving up
    /// on the possibility of a proper confirmation.  See "Retries" for more info.
    /// </summary>
    public virtual TimeSpan TimeLimit { get; set; }

    /// <summary>
    /// If confirmation cannot be received after all retries have been attempted
    /// message processing continues for the client if the client remains connected.
    /// A large number of retries is not recommended because failures that aren't
    /// transient network problems are typically either due to a complete
    /// disconnection by the client or a bug in the code handling the notification
    /// that prevents proper confirmation.  Neither of these 2 scenarios are
    /// likely to be resolved with excessive retries and the message queue for this
    /// client may grow large while excessive retries are attempted.
    /// When retries are allowed it's recommended to also set the "PublisherOptions"
    /// configuration value for "UseAdaptiveRetryStrategy" to true.
    /// </summary>
    public virtual int Retries { get; set; }

    public BaseConfirmationTimeoutOptions(TimeSpan timeLimit, int retries)
    {
        TimeLimit = timeLimit;
        Retries = retries;
    }
}

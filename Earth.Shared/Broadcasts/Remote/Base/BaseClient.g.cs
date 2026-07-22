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
using System.Collections.Concurrent;

namespace Earth.Shared.Broadcasts.Remote.Base;

public abstract class BaseClient<TNotificationType>
    where TNotificationType : Enum
{
    public virtual string ClientId { get; set; }

    /// <summary>
    /// The SignalR ConnectionId.  This is treated as transient and not assumed to be the current
    /// ConnectionId for clients, however, it's used to attempt to exclude unnecessary "Identify"
    /// calls to clients we don't need to identify.  If the ConnectionId is stale the only cost
    /// is some unnecessary traffic.
    /// </summary>
    public virtual string LastConnectionId { get; set; }

    public virtual DateTime LastContactUtc { get; set; }

    /// <summary>
    /// Only set when the message that's first in the queue has been sent and pending confirmation.
    /// </summary>
    public virtual DateTime? SentUtc { get; set; }

    /// <summary>
    /// Only set when the message that's first in the queue has been sent and pending confirmation.
    /// Keeps track of the number of times that message was sent and failed before the current attempt.
    /// </summary>
    public virtual int PreviousAttempts { get; set; }

    public virtual ConcurrentQueue<Notification<TNotificationType>> NotificationQueue { get; init; }

    protected Dictionary<TNotificationType, ClientReliability> Reliability { get; init; }

    public BaseClient(string clientId, string connectionId)
    {
        ClientId = clientId;
        LastConnectionId = connectionId;
        LastContactUtc = DateTime.UtcNow;
        SentUtc = null;
        NotificationQueue = new ConcurrentQueue<Notification<TNotificationType>>();
        Reliability = new Dictionary<TNotificationType, ClientReliability>();
    }

    protected int _processing = 0;

    public virtual bool ClaimProcessing()
    {
        return 0 == Interlocked.Exchange(ref _processing, 1);
    }

    public virtual bool IsProcessing()
    {
        return _processing == 1;
    }

    public virtual void ReleaseProcessing()
    {
        Interlocked.Exchange(ref _processing, 0);
    }

    public virtual ClientReliability ReliabilityOf(TNotificationType type)
    {
        if (Reliability.ContainsKey(type))
        {
            return Reliability[type];
        }

        return ClientReliability.Unknown;
    }

    public virtual void NotificationSucceeded(TNotificationType type)
    {
        PreviousAttempts = 0;

        if (ReliabilityOf(type) == ClientReliability.CanSucceed)
        {
            return;
        }

        UpdateReliability(type, ClientReliability.CanSucceed);
    }

    public virtual void NotificationFailed(TNotificationType type, int retries)
    {
        PreviousAttempts++;

        if (PreviousAttempts > retries)
        {
            var currentState = ReliabilityOf(type);

            switch (currentState)
            {
                case ClientReliability.Unknown:
                    UpdateReliability(type, ClientReliability.Failed);
                    return;
                case ClientReliability.CanSucceed:
                    return;
                case ClientReliability.Failed:
                    UpdateReliability(type, ClientReliability.AlwaysFails);
                    return;
                case ClientReliability.AlwaysFails:
                    return;
            }
        }
    }

    protected virtual void UpdateReliability(TNotificationType type, ClientReliability reliability)
    {
        if (Reliability.ContainsKey(type))
        {
            Reliability.Remove(type);
        }

        Reliability.Add(type, reliability);
    }
}

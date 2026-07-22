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
using System.Timers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Earth.Shared.Data.Broadcasts;

namespace Earth.Shared.Broadcasts.Remote.Base;

public abstract partial class BasePublisher<THub, TBroadcastObservables, TNotificationType>
    where THub : Hub
    where TBroadcastObservables : class
    where TNotificationType : Enum
{
    const string ClientMethod_InitializeObservables = "InitializeObservables";

    protected IHubContext<THub> _hubContext;
    protected Func<TBroadcastObservables> _observablesProvider;
    protected TNotificationType _notificationTypeForObservableInit;
    protected Dictionary<TNotificationType, NotificationTypeDetails> _notificationTypeDetails;
    protected ILogger<Publisher<THub, TBroadcastObservables, TNotificationType>> _logger;
    protected PublisherOptions _options;

    protected bool ConfirmObservableInitialization => _options.ConfirmationOfObservableInitialization != null;
    protected bool ConfirmObservableUpdates => _options.ConfirmationOfObservableUpdates != null;
    protected bool ConfirmMessages => _options.ConfirmationOfMessages != null;
    protected bool ConfirmationsUsed => ConfirmObservableInitialization || ConfirmObservableUpdates || ConfirmMessages;

    protected ConcurrentDictionary<string, Client<TNotificationType>> _remoteClientsById = new();

    protected object _lockCleanup = new object();
    protected System.Timers.Timer _cleanupTimer = null;

    /// <summary>
    /// Handles sending data through SignalR, notification confirmation, resend
    /// attempts, and sequenced messaging.
    /// </summary>
    /// <param name="hubContext"></param>
    /// <param name="logger"></param>
    public BasePublisher(
        IHubContext<THub> hubContext,
        Func<TBroadcastObservables> observablesProvider,
        Dictionary<TNotificationType, NotificationTypeDetails> notificationTypeDetails,
        TNotificationType notificationTypeForObservableInit,
        ILogger<Publisher<THub, TBroadcastObservables, TNotificationType>> logger,
        Action<PublisherOptions> configurePublisher)
    {
        _hubContext = hubContext;
        _observablesProvider = observablesProvider;
        _notificationTypeDetails = notificationTypeDetails;
        _notificationTypeForObservableInit = notificationTypeForObservableInit;

        _logger = logger;

        _options = new PublisherOptions();
        configurePublisher(_options);

        if (ConfirmationsUsed)
        {
            var interval = TimeSpan.FromSeconds(
                Math.Max(30, _options.PurgeSilentClients.TotalSeconds / 2));

            _cleanupTimer = new System.Timers.Timer(interval);
            _cleanupTimer.Elapsed += OnElapsedTimeForCleanup;
            _cleanupTimer.AutoReset = true;
            _cleanupTimer.Enabled = true;
        }
    }

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "{className}.{methodName}({caller}, {clientId}, {connectionId}, {isInitialConnection})")]
    protected partial void LogSubscribe(
        string className,
        string methodName,
        ISingleClientProxy caller,
        string clientId,
        string connectionId,
        bool isInitialConnection);

    public virtual async Task Subscribe(
        ISingleClientProxy caller,
        string clientId,
        string connectionId,
        bool isInitialConnection)
    {
        if (ConfirmationsUsed && string.IsNullOrWhiteSpace(clientId))
        {
            return;
        }

        LogSubscribe(nameof(Publisher<THub, TBroadcastObservables, TNotificationType>),
            nameof(Subscribe), caller, clientId, connectionId, isInitialConnection);

        Client<TNotificationType> client = null;
        object[] args = [];

        if (_observablesProvider != null)
        {
            args = _options.SequenceTrackingOfObservableUpdates
                ? [LastSequenceNumbersForAllNotifications(), _observablesProvider()]
                : [_observablesProvider()];

            if (!ConfirmObservableInitialization)
            {
                try
                {
                    await caller.SendCoreAsync(
                        ClientMethod_InitializeObservables,
                        args,
                        CancellationToken.None);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex, "Failure during untracked client initialization of observables.");
                }
            }
        }

        if (ConfirmationsUsed && !string.IsNullOrWhiteSpace(clientId))
        {
            if (!TryGetClient(clientId, connectionId, out client))
            {
                client = new Client<TNotificationType>(clientId, connectionId);

                if (!RemoteClientCacheTryAdd(clientId, client))
                {
                    var firstConnectionId = TryGetClient(clientId, connectionId, out client)
                        ? client?.LastConnectionId
                        : "unknown";
                    _logger.LogWarning($"Multiple connections attempted to subscribe at the same time with the same client ID: {clientId}."
                        + $"  The client ID should be generated uniquely per client.  First ConnectionId {firstConnectionId}.  Second ConnectionId {connectionId}.");
                }
            }
        }

        if (!ConfirmObservableInitialization || client == null || _observablesProvider == null)
        {
            return;
        }

        var message = new Notification<TNotificationType>(
            Guid.NewGuid().ToString(),
            ClientMethod_InitializeObservables,
            args,
            _notificationTypeForObservableInit);

        client.NotificationQueue.Enqueue(message);

        await ProcessRemoteClientQueue(caller, client, isInitialConnection);
    }

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "{className}.{methodName}({method}, {originalArgs}, {type}, ...)")]
    protected partial void LogSend(
        string className,
        string methodName,
        string method,
        object[] originalArgs,
        TNotificationType type);

    public virtual void Send(
        string method,
        object[] originalArgs,
        TNotificationType type,
        CancellationToken cancellationToken = default)
    {
        LogSend(nameof(Publisher<THub, TBroadcastObservables, TNotificationType>),
            nameof(Send), method, originalArgs, type);

        var details = _notificationTypeDetails[type];
        bool confirmReceipt = false;
        bool includeSequenceTracking = false;

        switch (details.Category)
        {
            case NotificationCategory.Message:
                confirmReceipt = ConfirmMessages;
                includeSequenceTracking = _options.SequenceTrackingOfMessages;
                break;
            case NotificationCategory.ObservablesUpdate:
                confirmReceipt = ConfirmObservableUpdates;
                includeSequenceTracking = _options.SequenceTrackingOfObservableUpdates;
                break;
            default:
                _logger.LogError(
                    $"Unsupported notification category for Send method: {details.Category} of notification type: {type}");
                return;
        }

        object[] args = includeSequenceTracking
            ? [new SequenceTracking<TNotificationType>(type, details.NextSequenceNumber()), .. originalArgs]
            : originalArgs;

        if (!confirmReceipt)
        {
            _hubContext.Clients.All.SendCoreAsync(method, args, cancellationToken);
            return;
        }

        var message = new Notification<TNotificationType>(
            Guid.NewGuid().ToString(),
            method,
            args,
            type);

        foreach (var client in RemoteClientCacheAllValues())
        {
            client.NotificationQueue.Enqueue(message);
        }

        if (!TrySendAll())
        {
            var exclude = new List<string>();

            foreach (var client in RemoteClientCacheAllValues())
            {
                if (client.SentUtc != null
                    || client.IsProcessing()
                    // since a message was added above, it's unlikely the queue is empty but
                    //  it's possible if there are a lot of clients and this one was already
                    //  processing that loop could have sent the new message and received a
                    //  confirmation.
                    || client.NotificationQueue.Count == 0)
                {
                    exclude.Add(client.LastConnectionId);
                }
            }

            _hubContext.Clients.AllExcept(exclude).SendAsync("Identify");
        }
    }

    public virtual Task ClientHeartbeatReceived(
        ISingleClientProxy caller,
        string clientId,
        string connectionId)
    {
        return HandleClientCallToHub(caller, clientId, connectionId);
    }

    public virtual Task ConfirmationReceived(
        ISingleClientProxy caller,
        string clientId,
        string connectionId,
        string notificationId)
    {
        return HandleClientCallToHub(caller, clientId, connectionId, notificationId);
    }

    protected virtual bool TryGetClient(string clientId, string connectionId, out Client<TNotificationType> client)
    {
        if (!string.IsNullOrWhiteSpace(clientId))
        {
            lock (_lockCleanup)
            {
                if (RemoteClientCacheTryGetValue(clientId, out client))
                {
                    client.LastConnectionId = connectionId;
                    client.LastContactUtc = DateTime.UtcNow;

                    return true;
                }
            }
        }

        client = null;
        return false;
    }

    protected virtual void OnElapsedTimeForCleanup(object source, ElapsedEventArgs e)
    {
        var timeLimit = _options.PurgeSilentClients;
        var now = DateTime.UtcNow;
        var purge = RemoteClientCacheAllValues()
            .Where(rc => now - rc.LastContactUtc > timeLimit)
            .Select(rc => rc.ClientId)
            .ToList();

        List<string> purgedClientIds = null;

        foreach (var clientId in purge)
        {
            lock (_lockCleanup)
            {
                if (RemoteClientCacheTryGetValue(clientId, out var client))
                {
                    if (DateTime.UtcNow - client.LastContactUtc > timeLimit)
                    {
                        RemoteClientCacheTryRemove(clientId);

                        if (purgedClientIds == null)
                        {
                            purgedClientIds = new List<string>([clientId]);
                        }
                        else
                        {
                            purgedClientIds.Add(clientId);
                        }
                    }
                }
            }
        }

        if (purgedClientIds != null)
        {
            foreach (var clientId in purgedClientIds)
            {
                SilentClientPurged(clientId);
            }
        }
    }

    /// <summary>
    /// Override as needed to check for specific client IDs that have been purged due to a prolonged
    /// inability to confirm receipt of notifications.
    /// </summary>
    /// <param name="clientId">
    /// ID of the client that was just purged.
    /// </param>
    protected virtual void SilentClientPurged(string clientId) { }

    protected virtual SequenceTracking<TNotificationType>[] LastSequenceNumbersForAllNotifications()
    {
        return _notificationTypeDetails.Keys
            .Select(t => new SequenceTracking<TNotificationType>(t, _notificationTypeDetails[t].SequenceNumber))
            .ToArray();
    }

    protected virtual async Task ProcessRemoteClientQueue(
        ISingleClientProxy caller,
        Client<TNotificationType> client,
        bool isInitialConnection)
    {
        if (client.NotificationQueue.Count == 0 || !client.ClaimProcessing())
        {
            return;
        }

        try
        {
            var first = true;

            while (client.NotificationQueue.TryPeek(out var notification))
            {
                var now = DateTime.UtcNow;
                ConfirmationTimeoutOptions timeoutOptions = null;
                var type = notification.NotificationType;
                var category = type.Equals(_notificationTypeForObservableInit)
                    ? NotificationCategory.ObservablesInitialization
                    : _notificationTypeDetails[type].Category;

                switch (category)
                {
                    case NotificationCategory.Message:
                        timeoutOptions = _options.ConfirmationOfMessages;
                        break;
                    case NotificationCategory.ObservablesUpdate:
                        timeoutOptions = _options.ConfirmationOfObservableUpdates;
                        break;
                    case NotificationCategory.ObservablesInitialization:
                        timeoutOptions = _options.ConfirmationOfObservableInitialization;
                        break;
                }

                if (timeoutOptions == null)
                {
                    break;
                }

                if (first)
                {
                    // need to check if the first message is already in progress
                    if (client.SentUtc != null)
                    {
                        if (now - client.SentUtc < timeoutOptions.TimeLimit)
                        {
                            break;
                        }
                        else
                        {
                            client.NotificationFailed(type, timeoutOptions.Retries);
                        }
                    }

                    first = false;
                }

                if (client.PreviousAttempts > timeoutOptions.Retries
                    || (client.PreviousAttempts > 0
                        && _options.UseAdaptiveRetryStrategy
                        && client.ReliabilityOf(type) == ClientReliability.AlwaysFails))
                {
                    client.NotificationQueue.TryDequeue(out notification);
                    client.SentUtc = null;
                    client.PreviousAttempts = 0;
                    continue;
                }

                client.SentUtc = DateTime.UtcNow;

                var useAsync = _options.AlwaysConfirmAsync || isInitialConnection;
                object[] args = [
                    new ConfirmationExpectation(notification.NotificationId, useAsync),
                    .. notification.Args];

                if (useAsync)
                {
                    await caller.SendCoreAsync(notification.Method, args, CancellationToken.None);
                    break;
                }

                var success = false;

                try
                {
                    var task = caller.InvokeCoreAsync<bool>(notification.Method, args, CancellationToken.None);

                    if (await Task.WhenAny(task, Task.Delay(timeoutOptions.TimeLimit, CancellationToken.None)) == task)
                    {
                        await task;
                        success = true;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogDebug(ex, "Call to caller.InvokeCoreAsync in ProcessRemoteClientQueue failed.");
                }

                if (success)
                {
                    client.NotificationSucceeded(type);
                }
                else
                {
                    client.NotificationFailed(type, timeoutOptions.Retries);

                    if (client.PreviousAttempts <= timeoutOptions.Retries
                        && (!_options.UseAdaptiveRetryStrategy
                            || client.ReliabilityOf(type) != ClientReliability.AlwaysFails))
                    {
                        // Need to retry in the next run of this queue processor.
                        // That'll be done in the heartbeat response to this "Identify" instruction, or the next
                        // already scheduled heartbeat, or in a reconnection, whichever comes first.  This is all we
                        // should do at this point because the connection to this client and/or the context of this
                        // hub caller might be gone.
                        client.SentUtc = null;
                        await caller.SendAsync("Identify");
                        break;
                    }
                }

                client.NotificationQueue.TryDequeue(out notification);
                client.SentUtc = null;
                client.PreviousAttempts = 0;
            }
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Failure during processing of a client's message queue.");
        }
        finally
        {
            client.ReleaseProcessing();
        }
    }

    protected virtual async Task HandleClientCallToHub(
        ISingleClientProxy caller,
        string clientId,
        string connectionId,
        string confirmNotificationId = null)
    {
        Client<TNotificationType> client = null;

        if (!ConfirmationsUsed || string.IsNullOrWhiteSpace(clientId))
        {
            return;
        }

        if (!TryGetClient(clientId, connectionId, out client))
        {
            // this would really only happen if the hub's OnConnectedAsync did not properly process
            //  for this client.  For example, if the client ID was not sent in the header for some
            //  reason.  Auto-subscription is preferrable to the client not receiving some messages
            //  but receiving the async messages under low volume when the queue isn't needed since
            //  this would be flaky behavior and could be difficult to debug.  So we just subscribe
            //  now and log it.
            await Subscribe(caller, clientId, connectionId, false);
                
            _logger.LogInformation("Unexpectedly needed to subscribe new client that did not subscribe during its original connection.");
            return;
        }

        if (confirmNotificationId != null)
        {
            client.NotificationQueue.TryPeek(out var notification);

            if (notification != null && notification.NotificationId == confirmNotificationId)
            {
                client.NotificationQueue.TryDequeue(out notification);
                client.SentUtc = null;
                client.PreviousAttempts = 0;
            }
        }

        await ProcessRemoteClientQueue(caller, client, false);
    }

    /// <summary>
    /// Sends a message to all clients and requests an asynchronous confirmation.
    /// This is only done if:
    /// 1) no clients already have individual message processing already in progress, and
    /// 2) the same message is next in the queue for all clients and has not been sent.
    /// Note that this code does not prevent or care if other messages are added to the
    /// queues while this code is running.
    /// </summary>
    /// <returns>whether send all succeeded</returns>
    protected virtual bool TrySendAll()
    {
        var canSendAll = true;

        // This starts by making sure this method can claim processing for every
        //  client before checking the queues because if any processing is going
        //  on already the state of the next message in a queue can change while
        //  the code that checks it is running.

        var processingClients = new List<Client<TNotificationType>>();

        foreach (var client in RemoteClientCacheAllValues())
        {
            if (client.ClaimProcessing())
            {
                processingClients.Add(client);
            }
            else
            {
                canSendAll = false;
            }
        }

        if (canSendAll)
        {
            bool first = true;
            Notification<TNotificationType> nextToSend = null;

            foreach (var client in RemoteClientCacheAllValues())
            {
                if (client.SentUtc != null)
                {
                    //first item in queue has already been sent
                    canSendAll = false;
                    break;
                }

                client.NotificationQueue.TryPeek(out var notification);

                if (notification == null)
                {
                    canSendAll = false;
                    break;
                }

                if (first)
                {
                    nextToSend = notification;
                    first = false;
                }
                else
                {
                    if (notification != nextToSend)
                    {
                        canSendAll = false;
                        break;
                    }
                }
            }

            if (nextToSend != null && canSendAll)
            {
                var now = DateTime.UtcNow;

                foreach (var client in RemoteClientCacheAllValues())
                {
                    client.SentUtc = now;
                }

                object[] args = [
                    new ConfirmationExpectation(nextToSend.NotificationId, true),
                    .. nextToSend.Args];

                _hubContext.Clients.All.SendCoreAsync(nextToSend.Method, args, CancellationToken.None);
            }
            else
            {
                canSendAll = false;
            }
        }

        foreach (var processingClient in processingClients)
        {
            processingClient.ReleaseProcessing();
        }

        return canSendAll;
    }

    /// <summary>
    /// When broadcasts are configured to confirm delivery of messages or observable updates the
    /// server maintains a cache of active clients.  By default, this cache is just a
    /// ConcurrentDictionary, but this can be changed if needed (for example, if scaling out the
    /// Web API exposing this broadcast a Redis cache may be used.)  To fully replace the cache
    /// implementation all 4 of these methods should be overridden in the dev managed Publisher
    /// code:
    ///     RemoteClientCacheTryAdd
    ///     RemoteClientCacheTryGetValue
    ///     RemoteClientCacheTryRemove
    ///     RemoteClientCacheAllValues
    /// </summary>
    protected bool RemoteClientCacheTryAdd(string clientId, Client<TNotificationType> client)
    {
        return _remoteClientsById.TryAdd(clientId, client);
    }

    protected bool RemoteClientCacheTryGetValue(string clientId, out Client<TNotificationType> client)
    {
        return _remoteClientsById.TryGetValue(clientId, out client);
    }

    protected void RemoteClientCacheTryRemove(string clientId)
    {
        _remoteClientsById.TryRemove(clientId, out _);
    }

    protected ICollection<Client<TNotificationType>> RemoteClientCacheAllValues()
    {
        return _remoteClientsById.Values;
    }
}

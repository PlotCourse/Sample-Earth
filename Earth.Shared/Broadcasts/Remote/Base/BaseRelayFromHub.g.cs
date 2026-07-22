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
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Earth.Shared.Broadcasts.InProc;
using Earth.Shared.Broadcasts.Remote.Interfaces;
using Earth.Shared.Data.Broadcasts;
using IRetryPolicy = Earth.Shared.Broadcasts.Remote.Interfaces.IRetryPolicy;

namespace Earth.Shared.Broadcasts.Remote.Base;

public abstract class BaseRelayFromHub<TSubscriber, TNotificationType> : Publisher<TSubscriber>
    where TNotificationType : Enum
{
    protected ILogger _logger;
    protected HubConnection _hubConnection;

    protected string _clientId;
    protected int _clientKeepAliveIntervalSeconds;
    protected System.Timers.Timer _heartbeatTimer = null;

    protected PublisherOptions _options;

    protected bool ConfirmObservableInitialization => _options.ConfirmationOfObservableInitialization != null;
    protected bool ConfirmObservableUpdates => _options.ConfirmationOfObservableUpdates != null;
    protected bool ConfirmMessages => _options.ConfirmationOfMessages != null;
    protected bool ConfirmationsUsed => ConfirmObservableInitialization || ConfirmObservableUpdates || ConfirmMessages;

    protected IClientSequenceTracker<TNotificationType> _sequenceTracker;

    public BaseRelayFromHub(
        HubConnection hubConnection,
        IRetryPolicy firstConnectRetryPolicy,
        IRetryPolicy disconnectionRetryPolicy,
        string clientId,
        int clientKeepAliveIntervalSeconds,
        IClientSequenceTracker<TNotificationType> sequenceTracker,
        ILogger logger,
        Action<PublisherOptions> configurePublisher)
    {
        _hubConnection = hubConnection;
        _clientId = clientId;
        _clientKeepAliveIntervalSeconds = clientKeepAliveIntervalSeconds;
        _sequenceTracker = sequenceTracker;
        _logger = logger;

        _options = new PublisherOptions();
        configurePublisher(_options);

        SetupHubConnection(hubConnection, disconnectionRetryPolicy);

        ConnectWithRetry(false, firstConnectRetryPolicy);
    }

    protected virtual void SetupHubConnection(HubConnection hubConnection, IRetryPolicy disconnectionRetryPolicy)
    {
        if (ConfirmationsUsed)
        {
            // When this is called, the server wants a heartbeat from this client now.
            // Once the heartbeat is received, if there's an overdue confirmation from this client
            // the original message will be re-sent or if there are other queued messages and none
            // in progress it will send the next message.
            _hubConnection.On("Identify", ResetHeartbeatTimer);

            //  This is set to 2 seconds larger than the Keep Alive Interval setting in the tool to cut
            //  down on chatter since the client heartbeat that uses the setting can serve as the keep alive
            //  (The timer for the internal keep alive is reset when a message is sent.)
            _hubConnection.KeepAliveInterval = TimeSpan.FromSeconds(_clientKeepAliveIntervalSeconds + 2);
        }

        _hubConnection.Closed += (error) =>
        {
            if (ConfirmationsUsed)
            {
                ClearHeartbeatTimer();
            }

            disconnectionRetryPolicy.Reset();
            ConnectWithRetry(true, disconnectionRetryPolicy);

            return Task.CompletedTask;
        };
    }

    protected virtual void SetupInitializeObservables(object handler)
    {
        var sequenceTracking = _options.SequenceTrackingOfObservableUpdates
            ? typeof(SequenceTracking<TNotificationType>[])
            : null;
        SetupHubMethod("InitializeObservables", handler, ConfirmObservableInitialization, sequenceTracking);
    }

    protected virtual void SetupObservableUpdate(string methodName, object handler)
    {
        var sequenceTracking = _options.SequenceTrackingOfObservableUpdates
            ? typeof(SequenceTracking<TNotificationType>)
            : null;
        SetupHubMethod(methodName, handler, ConfirmObservableUpdates, sequenceTracking);
    }

    protected virtual void SetupMessage(string methodName, object handler)
    {
        var sequenceTracking = _options.SequenceTrackingOfMessages
            ? typeof(SequenceTracking<TNotificationType>)
            : null;
        SetupHubMethod(methodName, handler, ConfirmMessages, sequenceTracking);
    }

    protected virtual void SetupHubMethod(string methodName, object handler, bool includeConfirmation, Type sequenceTracking)
    {
        var actionType = handler.GetType();
        var invokeMethod = actionType.GetMethod("Invoke");

        if (!actionType.Name.StartsWith("Action") || invokeMethod == null)
        {
            throw new ArgumentException("Handler is expected to be an Action.");
        }

        var argTypes =actionType.GetGenericArguments();
        var additionalParams = new List<Type>();

        if (includeConfirmation)
        {
            additionalParams.Add(typeof(ConfirmationExpectation));
        }

        var init = false;

        if (sequenceTracking != null)
        {
            init = sequenceTracking.IsArray;
            additionalParams.Add(sequenceTracking);
        }

        _hubConnection.On(methodName, [.. additionalParams, .. argTypes], async Task<bool> (vals) =>
        {
            var doCallHandler = true;
            if (sequenceTracking != null)
            {
                var tracking = (includeConfirmation ? vals[1] : vals[0])!;

                if (init)
                {
                    ResetSequenceTrackingForNotifications((SequenceTracking<TNotificationType>[])tracking);
                }
                else
                {
                    doCallHandler = ProcessNotification((SequenceTracking<TNotificationType>)tracking);
                }
            }

            if (doCallHandler)
            {
                var handlerArgs = additionalParams.Count == 0
                    ? vals
                    : vals.Skip(additionalParams.Count);

                invokeMethod.Invoke(handler, handlerArgs.ToArray());
            }

            if (includeConfirmation)
            {
                await NotificationReceived((ConfirmationExpectation)vals[0]!);
            }

            return true;
        });
    }

    protected virtual Task NotificationReceived(ConfirmationExpectation expectation)
    {
        if (_options.AlwaysConfirmAsync || expectation.ConfirmAsync)
        {
            return _hubConnection.SendAsync(
                "ConfirmReceipt",
                _clientId,
                expectation.NotificationId);
        }

        return Task.CompletedTask;
    }

    protected virtual Task SendHeartbeat()
    {
        return _hubConnection.SendAsync(
            "ClientHeartbeat",
            _clientId);
    }

    /// <summary>
    ///  NB: Intentionally not using SignalR's built-in auto-reconnection functionality because that has some
    ///  conditions under which it won't bother to attempt reconnections.  Instead the code here is written to
    ///  not assume that the usage is something like a chat app where the user can just be instructed to try
    ///  again later.  Instead, the assumed use is that this is a mission critical communication mechanism
    ///  between application components and the reconnection should be attempted as it's configured even when,
    ///  for example, the server running the component with the broadcast was intentionally rebooted.
    /// </summary>
    /// <param name="firstIsRetry"></param>
    /// <param name="retryPolicy"></param>
    protected virtual void ConnectWithRetry(bool firstIsRetry, IRetryPolicy retryPolicy)
    {
        Task.Run(async () =>
        {
            if (firstIsRetry)
            {
                var initialDelay = retryPolicy.NextRetryDelay();

                if (initialDelay != null && initialDelay.Value.TotalSeconds > 0)
                {
                    await Task.Delay(initialDelay.Value);
                }
            }

            while (true)
            {
                try
                {
                    await _hubConnection.StartAsync();

                    if (ConfirmationsUsed)
                    {
                        ResetHeartbeatTimer();
                    }
                    return;
                }
                catch (Exception ex)
                {
                    var delay = retryPolicy.NextRetryDelay();
                    var className = GetType().Name;

                    if (delay == null)
                    {
                        _logger.LogException(ex, className, nameof(ConnectWithRetry), "Giving up.");
                        return;
                    }
                    else
                    {
                        _logger.LogException(ex, className, nameof(ConnectWithRetry), "Will retry.");
                    }

                    if (delay.Value.TotalSeconds > 0)
                    {
                        await Task.Delay(delay.Value);
                    }
                }
            }
        });
    }

    protected virtual void ResetHeartbeatTimer()
    {
        ClearHeartbeatTimer();

        _heartbeatTimer = new System.Timers.Timer(TimeSpan.FromSeconds(_clientKeepAliveIntervalSeconds));

        _heartbeatTimer.Elapsed += OnElapsedTimeForHeartbeat;
        _heartbeatTimer.AutoReset = true;
        _heartbeatTimer.Enabled = true;

        SendHeartbeat();
    }

    protected virtual void OnElapsedTimeForHeartbeat(object source, System.Timers.ElapsedEventArgs e)
    {
        SendHeartbeat();
    }

    protected virtual void ClearHeartbeatTimer()
    {
        if (_heartbeatTimer == null)
        {
            return;
        }
        _heartbeatTimer.Stop();
        _heartbeatTimer.Dispose();
        _heartbeatTimer = null;
    }

    protected virtual void ResetSequenceTrackingForNotifications(SequenceTracking<TNotificationType>[] trackings)
    {
        if (_sequenceTracker == null)
        {
            return;
        }
        _sequenceTracker.Reset(trackings);
    }

    protected virtual bool HandleSequenceTrackingDiscrepancy(
        TNotificationType type,
        int previousSequenceNumber,
        int currentSequenceNumber)
    {
        return true;
    }

    protected virtual bool ProcessNotification(SequenceTracking<TNotificationType> sequenceTracking)
    {
        if (_sequenceTracker == null)
        {
            return true;
        }

        var oldValueDiscrepancy = _sequenceTracker.UpdateSequenceNumber(
            sequenceTracking.NotificationType, sequenceTracking.SequenceNumber);

        if (oldValueDiscrepancy == null)
        {
            return true;
        }

        return HandleSequenceTrackingDiscrepancy(
            sequenceTracking.NotificationType,
            oldValueDiscrepancy.Value,
            sequenceTracking.SequenceNumber);
    }
}

using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using RetryPolicy = Earth.Shared.Broadcasts.Remote.RetryPolicy;
using IRetryPolicy = Earth.Shared.Broadcasts.Remote.Interfaces.IRetryPolicy;
using IOceanSurfaceBroadcast = Earth.Ocean.Contract.Broadcasts.IOceanSurfaceBroadcast;
using IOceanSurfacePublisher = Earth.Ocean.Contract.Broadcasts.IOceanSurfacePublisher;
using OceanSurfaceNotificationType = Earth.Ocean.Contract.Broadcasts.OceanSurfaceNotificationType;

using Earth.Ocean.Contract.Data;
using System;

namespace Earth.Ocean.Client.Broadcasts.Base;

/// <summary>
/// Routes messages and changes to observables for clients outside the broadcast Web API
/// by connecting to a SignalR hub of the broadcast and passing messages through a publisher
/// that can be subscribed to by subscribers in the client.
/// </summary>
public abstract partial class BaseOceanSurfaceRelayFromHub
    : Earth.Shared.Broadcasts.Remote.RelayFromHub<IOceanSurfaceBroadcast, OceanSurfaceNotificationType>, IOceanSurfacePublisher
{
    protected int _temperature;

    public virtual int Temperature
    {
        get => _temperature;
        set
        {
            DoBroadcast(r => r.Temperature = value);
        }
    }

    public BaseOceanSurfaceRelayFromHub(
        HubConnection hubConnection,
        string clientId,
        Earth.Shared.Broadcasts.Remote.Interfaces.IClientSequenceTracker<OceanSurfaceNotificationType> sequenceTracker,
        ILogger<OceanSurfaceRelayFromHub> logger) : base(
            hubConnection,
            new RetryPolicy([5,5,30], true),
            new RetryPolicy([0,0,5,5,30], true),
            clientId,
            15,
            sequenceTracker,
            logger,
            options =>
            {
                options.ConfirmationOfObservableInitialization = null;
                options.ConfirmationOfObservableUpdates = null;
                options.SequenceTrackingOfObservableUpdates = false;
                options.ConfirmationOfMessages = null;
                options.SequenceTrackingOfMessages = false;
                options.AlwaysConfirmAsync = false;
            }) { }

    protected override void SetupHubConnection(HubConnection hubConnection, IRetryPolicy disconnectionRetryPolicy)
    {
        base.SetupHubConnection(hubConnection, disconnectionRetryPolicy);

        SetupInitializeObservables((object)InitializeObservables);

        SetupObservableUpdate("UpdateTemperature", (int temperature) => { Temperature = temperature; });

        SetupMessage("NewHurricane", (object)NewHurricane);
    }

    public virtual void InitializeObservables(OceanSurfaceBroadcastObservables observables)
    {
        Temperature = observables.Temperature;
    }

    public virtual void NewHurricane(
        HurricaneCoordinate location)
    {
        DoBroadcast(r => r.NewHurricane(
            location));
    }

    protected override void InitializeNewSubscriber(IOceanSurfaceBroadcast subscriber)
    {
        subscriber.Temperature = _temperature;
    }

}

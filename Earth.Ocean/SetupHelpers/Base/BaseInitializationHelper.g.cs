using Microsoft.Extensions.DependencyInjection;
using Broadcast_IOceanSurfaceInternalPublisher = Earth.Ocean.Broadcasts.Interfaces.IOceanSurfaceInternalPublisher;
using Broadcast_IOceanSurfaceExternalPublisher = Earth.Ocean.Broadcasts.Interfaces.IOceanSurfaceExternalPublisher;
using Broadcast_OceanSurfaceRelayToExternalPublisher = Earth.Ocean.Broadcasts.OceanSurfaceRelayToExternalPublisher;
using Broadcast_OceanSurfaceBroadcastObservables = Earth.Ocean.Contract.Data.OceanSurfaceBroadcastObservables;
using Broadcast_OceanSurfaceNotificationType = Earth.Ocean.Contract.Broadcasts.OceanSurfaceNotificationType;
using Broadcast_OceanSurfaceRelayToHub = Earth.Ocean.Broadcasts.OceanSurfaceRelayToHub;
using Broadcast_OceanSurfaceHub = Earth.Ocean.Broadcasts.OceanSurfaceHub;

namespace Earth.Ocean.SetupHelpers.Base;

internal abstract partial class BaseInitializationHelper
{
    protected IServiceProvider _services;

    public BaseInitializationHelper(IServiceProvider services)
    {
        _services = services;
    }

    public virtual void RelayOceanSurfaceBroadcastToExternalPublisher()
    {
        var internalOceanSurfacePublisher = _services.GetRequiredService<Broadcast_IOceanSurfaceInternalPublisher>();
        var externalOceanSurfacePublisher = _services.GetRequiredService<Broadcast_IOceanSurfaceExternalPublisher>();
        new Broadcast_OceanSurfaceRelayToExternalPublisher(internalOceanSurfacePublisher, externalOceanSurfacePublisher);
    }

    public virtual void RelayBroadcastsToExternalPublishers()
    {
        RelayOceanSurfaceBroadcastToExternalPublisher();
    }

    public virtual void RelayOceanSurfaceBroadcastToHub()
    {
        var externalOceanSurfacePublisher = _services.GetRequiredService<Broadcast_IOceanSurfaceExternalPublisher>();
        var remoteOceanSurfacePublisher = _services.GetRequiredService<
            Earth.Shared.Broadcasts.Remote.Interfaces.IPublisher<Broadcast_OceanSurfaceHub, Broadcast_OceanSurfaceBroadcastObservables, Broadcast_OceanSurfaceNotificationType>>();
        new Broadcast_OceanSurfaceRelayToHub(externalOceanSurfacePublisher, remoteOceanSurfacePublisher);
    }

    public virtual void RelayBroadcastsToHubs()
    {
        RelayOceanSurfaceBroadcastToHub();
    }

    public virtual void Initialize()
    {
        RelayBroadcastsToExternalPublishers();
        RelayBroadcastsToHubs();
    }
}

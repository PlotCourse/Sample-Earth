using Microsoft.Extensions.DependencyInjection;
using ContractBroadcast_Ocean_IOceanSurfacePublisher = Earth.Ocean.Contract.Broadcasts.IOceanSurfacePublisher;
using ProxyBroadcast_Ocean_IOceanSurfaceInternalPublisher = Earth.Dolphin.Proxies.Ocean.Broadcasts.Interfaces.IOceanSurfaceInternalPublisher;
using ProxyBroadcast_Ocean_OceanSurfaceRelayToInternalPublisher = Earth.Dolphin.Proxies.Ocean.Broadcasts.OceanSurfaceRelayToInternalPublisher;
using SubComponent_IMonitorOcean = Earth.Dolphin.Interfaces.IMonitorOcean;

namespace Earth.Dolphin.SetupHelpers.Base;

internal abstract partial class BaseInitializationHelper
{
    protected IServiceProvider _services;

    public BaseInitializationHelper(IServiceProvider services)
    {
        _services = services;
    }

    public virtual void AddOcean_OceanSurfaceRelayToInternalPublisher()
    {
        var sourcePublisher = _services.GetService<ContractBroadcast_Ocean_IOceanSurfacePublisher>();
        var destinationPublisher = _services.GetService<ProxyBroadcast_Ocean_IOceanSurfaceInternalPublisher>();
        new ProxyBroadcast_Ocean_OceanSurfaceRelayToInternalPublisher(sourcePublisher, destinationPublisher);
    }
    public virtual SubComponent_IMonitorOcean InitializeMonitorOceanSubComponent()
    {
        return _services.GetRequiredService<SubComponent_IMonitorOcean>();
    }

    public virtual void InitializeInternalSubComponents()
    {
        InitializeMonitorOceanSubComponent();
    }

    public virtual void Initialize()
    {
        AddOcean_OceanSurfaceRelayToInternalPublisher();
        InitializeInternalSubComponents();
    }
}

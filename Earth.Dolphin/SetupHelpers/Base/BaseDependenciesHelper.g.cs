using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Earth.Shared.Data.Mappers;
using ContractService_ITricksService = Earth.Dolphin.Contract.Services.ITricksService;
using Service_TricksService = Earth.Dolphin.Services.Contractors.TricksService;
using Service_ITricks = Earth.Dolphin.Services.Interfaces.ITricks;
using Service_Tricks = Earth.Dolphin.Services.Tricks;
using Service_IMonitorOcean = Earth.Dolphin.Interfaces.IMonitorOcean;
using Service_MonitorOcean = Earth.Dolphin.MonitorOcean;
using ProxyService_Ocean_IWaterService = Earth.Dolphin.Proxies.Ocean.Services.Interfaces.IWaterService;
using ProxyService_Ocean_WaterService = Earth.Dolphin.Proxies.Ocean.Services.WaterService;
using Proxies_ComponentDefault_IDefaultServiceClientInitializer = Earth.Dolphin.Proxies.DefaultInitializers.Interfaces.IDefaultServiceClientInitializer;
using Proxies_ComponentDefault_DefaultServiceClientInitializer = Earth.Dolphin.Proxies.DefaultInitializers.DefaultServiceClientInitializer;
using Proxies_Ocean_IDefaultServiceClientInitializer = Earth.Dolphin.Proxies.Ocean.Services.Interfaces.IDefaultServiceClientInitializer;
using Proxies_Ocean_DefaultServiceClientInitializer = Earth.Dolphin.Proxies.Ocean.Services.DefaultServiceClientInitializer;
using ClientService_Ocean_WaterRestClient = Earth.Ocean.Client.Services.WaterRestClient;
using ProxyBroadcast_Ocean_IOceanSurfaceInternalPublisher = Earth.Dolphin.Proxies.Ocean.Broadcasts.Interfaces.IOceanSurfaceInternalPublisher;
using ProxyBroadcast_Ocean_OceanSurfaceInternalPublisher = Earth.Dolphin.Proxies.Ocean.Broadcasts.OceanSurfaceInternalPublisher;

namespace Earth.Dolphin.SetupHelpers.Base;

internal abstract partial class BaseDependenciesHelper
{
    protected WebApplicationBuilder _builder;

    public BaseDependenciesHelper(WebApplicationBuilder builder)
    {
        _builder = builder;
    }

    public virtual void RegisterMappers()
    {
        typeof(BaseDependenciesHelper).Assembly.RegisterMappers();
    }

    public virtual void AddDolphinDefaultServiceClientInitializer()
    {
        _builder.Services.AddSingleton<Proxies_ComponentDefault_IDefaultServiceClientInitializer, Proxies_ComponentDefault_DefaultServiceClientInitializer>();
    }
        
    public virtual void AddOceanDefaultServiceClientInitializer()
    {
        _builder.Services.AddSingleton<Proxies_Ocean_IDefaultServiceClientInitializer, Proxies_Ocean_DefaultServiceClientInitializer>();
    }

    public virtual void AddOceanWaterServiceProxy()
    {
        _builder.Services.AddSingleton<ProxyService_Ocean_IWaterService>(s =>
        {
            var factory = () =>
            {
                return s.GetService<ClientService_Ocean_WaterRestClient>();
            };

            var initializer = s.GetService<Proxies_Ocean_IDefaultServiceClientInitializer>();

            return new ProxyService_Ocean_WaterService(factory, initializer);
        });
    }

    public virtual void AddOceanOceanSurfaceBroadcastProxy()
    {
        _builder.Services.AddSingleton<ProxyBroadcast_Ocean_IOceanSurfaceInternalPublisher>(s =>
        {
            return new ProxyBroadcast_Ocean_OceanSurfaceInternalPublisher();
        });
    }

    public virtual void AddInternalTricksService()
    {
        _builder.Services.AddScoped<Service_ITricks, Service_Tricks>();
    }

    public virtual void AddInternalMonitorOceanService()
    {
        _builder.Services.AddSingleton<Service_IMonitorOcean, Service_MonitorOcean>();
    }

    public virtual void AddContractorTricksService()
    {
        _builder.Services.AddScoped<ContractService_ITricksService, Service_TricksService>();
    }

    public virtual void AddDependencies()
    {
        RegisterMappers();

        AddDolphinDefaultServiceClientInitializer();
        AddOceanDefaultServiceClientInitializer();
        AddOceanWaterServiceProxy();
        AddOceanOceanSurfaceBroadcastProxy();
        AddInternalTricksService();
        AddInternalMonitorOceanService();
        AddContractorTricksService();
    }
}

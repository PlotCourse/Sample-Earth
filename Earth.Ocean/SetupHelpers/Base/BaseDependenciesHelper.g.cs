using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Earth.Shared.Data.Mappers;
using ContractService_IWaterService = Earth.Ocean.Contract.Services.IWaterService;
using Service_WaterService = Earth.Ocean.Services.Contractors.WaterService;
using Service_IWater = Earth.Ocean.Services.Interfaces.IWater;
using Service_Water = Earth.Ocean.Services.Water;
using Broadcast_IOceanSurfaceInternalPublisher = Earth.Ocean.Broadcasts.Interfaces.IOceanSurfaceInternalPublisher;
using Broadcast_OceanSurfaceInternalPublisher = Earth.Ocean.Broadcasts.OceanSurfaceInternalPublisher;
using Broadcast_IOceanSurfacePublisher = Earth.Ocean.Contract.Broadcasts.IOceanSurfacePublisher;
using Broadcast_IOceanSurfaceExternalPublisher = Earth.Ocean.Broadcasts.Interfaces.IOceanSurfaceExternalPublisher;
using Broadcast_OceanSurfaceExternalPublisher = Earth.Ocean.Broadcasts.OceanSurfaceExternalPublisher;
using Broadcast_OceanSurfaceBroadcastObservables = Earth.Ocean.Contract.Data.OceanSurfaceBroadcastObservables;
using Broadcast_OceanSurfaceNotificationType = Earth.Ocean.Contract.Broadcasts.OceanSurfaceNotificationType;
using Broadcast_OceanSurfaceRelayToHub = Earth.Ocean.Broadcasts.OceanSurfaceRelayToHub;
using Broadcast_OceanSurfaceHub = Earth.Ocean.Broadcasts.OceanSurfaceHub;

namespace Earth.Ocean.SetupHelpers.Base;

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

    public virtual void AddInternalWaterService()
    {
        _builder.Services.AddScoped<Service_IWater, Service_Water>();
    }

    public virtual void AddContractorWaterService()
    {
        _builder.Services.AddScoped<ContractService_IWaterService, Service_WaterService>();
    }

    public virtual void AddInternalOceanSurfaceBroadcast()
    {
        _builder.Services.AddSingleton<Broadcast_IOceanSurfaceInternalPublisher>(s =>
        {
            return new Broadcast_OceanSurfaceInternalPublisher();
        });
    }

    public virtual void AddExternalOceanSurfaceBroadcast()
    {
        _builder.Services.AddSingleton<Broadcast_IOceanSurfaceExternalPublisher>(s =>
        {
            return new Broadcast_OceanSurfaceExternalPublisher();
        });
        _builder.Services.AddSingleton<Broadcast_IOceanSurfacePublisher>(s =>
        {
            return s.GetRequiredService<Broadcast_IOceanSurfaceExternalPublisher>();
        });
    }

    public virtual void AddSignalROceanSurfaceHubPublisher()
    {
        _builder.Services.AddSingleton<Earth.Shared.Broadcasts.Remote.Interfaces.IPublisher<
            Broadcast_OceanSurfaceHub, Broadcast_OceanSurfaceBroadcastObservables, Broadcast_OceanSurfaceNotificationType>>(s =>
            {
                var hubContext = s.GetRequiredService<Microsoft.AspNetCore.SignalR.IHubContext<Broadcast_OceanSurfaceHub>>();
                var sourcePublisher = s.GetRequiredService<Broadcast_IOceanSurfaceExternalPublisher>();
                var logger = s.GetRequiredService<ILogger<Earth.Shared.Broadcasts.Remote.Publisher<
                    Broadcast_OceanSurfaceHub, Broadcast_OceanSurfaceBroadcastObservables, Broadcast_OceanSurfaceNotificationType>>>();

                return new Earth.Shared.Broadcasts.Remote.Publisher<
                    Broadcast_OceanSurfaceHub, Broadcast_OceanSurfaceBroadcastObservables, Broadcast_OceanSurfaceNotificationType>(
                        hubContext,
                        sourcePublisher.GetAllBroadcastObservables,
                        Broadcast_OceanSurfaceRelayToHub.GetNotificationDetailsByType(),
                        Broadcast_OceanSurfaceNotificationType.ObservableInitialization,
                        logger,
                        options =>
                        {
                            options.ConfirmationOfObservableInitialization = null;
                            options.ConfirmationOfObservableUpdates = null;
                            options.SequenceTrackingOfObservableUpdates = false;
                            options.ConfirmationOfMessages = null;
                            options.SequenceTrackingOfMessages = false;
                            options.PurgeSilentClients = TimeSpan.FromMinutes(2);
                            options.AlwaysConfirmAsync = false;
                            options.UseAdaptiveRetryStrategy = true;
                        });
            });
    }

    public virtual void AddDependencies()
    {
        RegisterMappers();

        AddInternalWaterService();
        AddContractorWaterService();
        AddInternalOceanSurfaceBroadcast();
        AddExternalOceanSurfaceBroadcast();
        AddSignalROceanSurfaceHubPublisher();
    }
}

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Http.Connections.Client;
using Microsoft.Extensions.Logging;
using Earth.Shared.Broadcasts.Remote.Interfaces;
using Ocean_HubEndpoints = Earth.Ocean.Contract.HubEndpoints;

namespace Earth.Ocean.Client.Base;

public abstract partial class BaseClientSetupHelper
{
    private IServiceCollection _services;

    public BaseClientSetupHelper(IServiceCollection services)
    {
        _services = services;
    }


    public virtual void ConfigureWaterClient(HttpClient client)
    {
        // This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
        // Learn more about service discovery scheme resolution at https://aka.ms/dotnet/sdschemes.
        client.BaseAddress = new("https+http://earth-support");
    }

    public virtual IHttpClientBuilder AddWaterRestClient()
    {
        var httpClientBuilder = _services.AddHttpClient<Earth.Ocean.Client.Services.WaterRestClient>(ConfigureWaterClient);
        _services.AddTransient<Earth.Ocean.Client.Services.Interfaces.IWaterRestClient>(s =>
        {
            return s.GetService<Earth.Ocean.Client.Services.WaterRestClient>();
        });
        return httpClientBuilder;
    }

    protected virtual void ConfigureOceanSurfaceHttp(
        HttpConnectionOptions options,
        IHttpMessageHandlerFactory messageHandlerFactory)
    {
        options.HttpMessageHandlerFactory = _ => messageHandlerFactory.CreateHandler();
    }

    protected virtual IHubConnectionBuilder OceanSurfaceHubConnectionBuilder(
        IServiceProvider serviceProvider)
    {
        var messageHandlerFactory = serviceProvider.GetRequiredService<IHttpMessageHandlerFactory>();

        var builder = new HubConnectionBuilder()
            .WithUrl(
                "https+http://earth-support" + Ocean_HubEndpoints.OceanSurfaceHub,
                options => ConfigureOceanSurfaceHttp(options, messageHandlerFactory));
        return builder;
    }

    protected virtual void AddOceanSurfaceHubConnection()
    {
        _services.AddSingleton<Earth.Ocean.Contract.Broadcasts.IOceanSurfacePublisher>(s =>
        {
            var hubConnection = OceanSurfaceHubConnectionBuilder(s)
                .Build();

            // Intentionally not using WithAutomaticReconnect here because that built-in reconnect functionality
            // has some conditions under which it won't bother to attempt reconnections.  Instead RelayFromHub
            // handles this and is written to not assume that the usage is something like a chat app where the user
            // can just be instructed to try again later.  Instead, the assumed use (if any) is that this is a mission
            // critical communication mechanism between application components and the reconnection should be
            // attempted as its configured even when, for example, the server running the component with the broadcast
            // was intentionally rebooted.

            var logger = s.GetRequiredService<ILogger<Earth.Ocean.Client.Broadcasts.OceanSurfaceRelayFromHub>>();

            return new Earth.Ocean.Client.Broadcasts.OceanSurfaceRelayFromHub(
                hubConnection,
                null,
                null,
                logger);
        });
    }

    public virtual void AddClientDependencies()
    {
        AddWaterRestClient();
        AddOceanSurfaceHubConnection();
    }
}

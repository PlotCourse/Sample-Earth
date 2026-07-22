using Microsoft.AspNetCore.SignalR;
using Ocean_HubEndpoints = Earth.Ocean.Contract.HubEndpoints;
using Ocean_OceanSurfaceHub = Earth.Ocean.Broadcasts.OceanSurfaceHub;

namespace Earth.Support.Base;

/// <summary>
/// This partial defines SignalR Hub(s) exposed for Ocean broadcasts so that components
/// running in other Web API can connect to it.
/// </summary>
internal abstract partial class BaseSignalRSetupHelper
{
    public virtual void AddOceanSignalR()
    {
        AddOceanOceanSurfaceSignalR();
    }

    public virtual void MapOceanHubs()
    {
        MapOceanOceanSurfaceHub();
    }

    protected virtual ISignalRServerBuilder AddOceanOceanSurfaceSignalR()
    {
        //  Use SignalRSetupHelper.cs to override this method as needed.
        //  For example, this could be done to specify Azure SignalR Service for scaling by
        //  using extension method "AddNamedAzureSignalR".
        //
        //  Note that this also requires a change in the AppHost.
        //
        //  See for more info:
        //  https://learn.microsoft.com/en-us/dotnet/aspire/real-time/azure-signalr-scenario?tabs=dotnet-cli
        var signalRServerBuilder = _builder.Services.AddSignalR()
            .AddHubOptions<Ocean_OceanSurfaceHub>(ConfigureOceanOceanSurfaceSignalR);
        var supportUrl = _builder.Configuration["Support_Url"];
        var dolphinUiUrl = _builder.Configuration["DolphinUi_Url"];

        if (supportUrl != dolphinUiUrl)
        {
            if (!HubNamesWithCrossOriginUiClients.Contains("Ocean_OceanSurfaceHub"))
            {
                HubNamesWithCrossOriginUiClients.Add("Ocean_OceanSurfaceHub");
            }

            _builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins(dolphinUiUrl)
                            .AllowAnyHeader()
                            .WithMethods("GET", "POST")
                            .AllowCredentials();
                    });
            });
        }

        return signalRServerBuilder;
    }

    protected virtual void ConfigureOceanOceanSurfaceSignalR(
        Microsoft.AspNetCore.SignalR.HubOptions<Ocean_OceanSurfaceHub> options)
    {
    }

    protected virtual HubEndpointConventionBuilder MapOceanOceanSurfaceHub()
    {
        if (HubNamesWithCrossOriginUiClients.Contains("Ocean_OceanSurfaceHub"))
        {
            _app.UseCors();
        }
        return _app.MapHub<Ocean_OceanSurfaceHub>(Ocean_HubEndpoints.OceanSurfaceHub);
    }
}

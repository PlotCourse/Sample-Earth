using Earth.Ocean;

namespace Earth.Support;

public static partial class Extensions
{
    public static WebApplicationBuilder InitializeAppBuilder(this WebApplicationBuilder builder)
    {
        var webApiHelper = new WebApiHelper();
        webApiHelper.InitializeBuilder(builder);
        //Note: The departure from the extension method pattern here is intended to allow an easy
        //  override of any part of the SignalR setup.
        var signalRHelper = new SignalRSetupHelper(builder);
        signalRHelper.AddOceanSignalR();


        builder.AddOceanDependencies();

        return builder;
    }

    public static WebApplication InitializeAppForDevelopment(this WebApplication app) 
    {
        var webApiHelper = new WebApiHelper();
        return webApiHelper.InitializeAppForDevelopment(app);
    }

    public static WebApplication InitializeComponents(this WebApplication app)
    {
        //Note: The departure from the extension method pattern here is intended to allow an easy
        //  override of any part of the REST service setup.
        var restServiceHelper = new RestServiceSetupHelper(app);

        restServiceHelper.MapForOcean();

        var signalRHelper = new SignalRSetupHelper(app);
        signalRHelper.MapOceanHubs();

        app.Services.InitializeOcean();

        return app;
    }
}

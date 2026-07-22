namespace Earth.Support.Base;

internal abstract partial class BaseSignalRSetupHelper
{
    protected static readonly HashSet<string> HubNamesWithCrossOriginUiClients = new();
    protected WebApplicationBuilder _builder;
    protected WebApplication _app;

    public BaseSignalRSetupHelper(WebApplicationBuilder builder)
    {
        _builder = builder;
    }

    public BaseSignalRSetupHelper(WebApplication app)
    {
        _app = app;
    }
}

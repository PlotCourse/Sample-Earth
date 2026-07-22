namespace Earth.Main.Base;

internal abstract partial class BaseRestServiceSetupHelper
{
    protected WebApplication _app;
    protected WebApiHelper _webApiHelper;

    public BaseRestServiceSetupHelper(WebApplication app)
    {
        _app = app;
        _webApiHelper = new WebApiHelper();
    }
}

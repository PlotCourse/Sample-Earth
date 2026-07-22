using Earth.Support.Base;

namespace Earth.Support;

internal class SignalRSetupHelper : BaseSignalRSetupHelper
{
    public SignalRSetupHelper(WebApplicationBuilder builder) : base(builder) { }
    public SignalRSetupHelper(WebApplication app) : base(app) { }
}

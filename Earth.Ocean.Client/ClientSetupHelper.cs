using Microsoft.Extensions.DependencyInjection;
using Earth.Ocean.Client.Base;

namespace Earth.Ocean.Client;

public class ClientSetupHelper : BaseClientSetupHelper
{
    public ClientSetupHelper(IServiceCollection services) : base(services) { }

    //There are 2 ways to customize client setup behavior depending on whether you want to
    //  customize the default behavior for how every Web API configures itself as a client or
    //  just one Web API.  To change the default behavior for every Web API, this can be done
    //  by overriding methods here as needed.
    //
    //  To customize the client configuration for just one dependent Web API change the class
    //  "ClientSetupHelper" in the dependent Web API to use a new sub-class of this class defined
    //  there.  The new sub-class can then be used to override methods on this class as needed.
}

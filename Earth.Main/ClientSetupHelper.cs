using Earth.Main.Base;

namespace Earth.Main;

internal class ClientSetupHelper : BaseClientSetupHelper
{
    public ClientSetupHelper(IServiceCollection services) : base(services) { }

    //There are 2 ways to customize client setup behavior depending on whether you want to
    //  customize the default behavior for how every Web API configures itself as a client or
    //  just this Web API.  To change the default behavior for every Web API, this can be done
    //  without making changes here and instead overriding methods in "ClientSetupHelper" inside
    //  the client library itself (See projects for namespaces ending with ".Client").
    //
    //  To customize the client configuration for this Web API only, create a new sub-class
    //  directly in this project that derives from the "ClientSetupHelper" in the Client assembly
    //  of the dependency.  Then the corresponding method in this class can be overridden to use
    //  the new sub-class instead of the default dependency in the client assembly.
}

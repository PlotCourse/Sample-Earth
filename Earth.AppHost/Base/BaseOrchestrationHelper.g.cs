using Aspire.Hosting.JavaScript;
using Microsoft.AspNetCore.Routing;

namespace Earth.AppHost;

internal abstract partial class BaseOrchestrationHelper
{
    protected IDistributedApplicationBuilder _builder;

    public BaseOrchestrationHelper(IDistributedApplicationBuilder builder)
    {
        _builder = builder;
    }

    public virtual void AddProjects()
    {
        var support = AddProjectResourceForSupport();
        var main = AddProjectResourceForMain();

        var dolphinUi = AddViteAppResourceForDolphinUi();

        main.WithReference(support);
        dolphinUi.WithReference(support)
            .WaitFor(support);
        dolphinUi.WithReference(main)
            .WaitFor(main);
        support.WithEnvironment("DolphinUi_Url", dolphinUi.GetEndpoint("http"));
    }

    public virtual IResourceBuilder<ProjectResource> AddProjectResourceForSupport()
    {
        //  Use OrchestrationHelper.cs to override this method if you want to specify manifest
        //  publishing to include use of Azure SignalR Service for scaling.  For example:
        //
        //      public override IResourceBuilder<ProjectResource> AddProjectResourceForSupport(IDistributedApplicationBuilder builder)
        //      {
        //          var signalr = builder.ExecutionContext.IsPublishMode
        //              ? builder.AddAzureSignalR("earth-supportSignalR")
        //              : builder.AddConnectionString("earth-supportSignalR");
        //
        //          return base.AddProjectResourceForSupport(builder)
        //              .WithReference(signalr);
        //      }
        //
        //  Note that this also requires a change in the Earth.Support web app builder code.
        //
        //  See for more info:
        //  https://learn.microsoft.com/en-us/dotnet/aspire/real-time/azure-signalr-scenario?tabs=dotnet-cli
        var p = _builder.AddProject<Projects.Earth_Support>("earth-support");

        p.WithEnvironment("Support_Url", p.GetEndpoint("https"));

        return p;
    }

    public virtual IResourceBuilder<ProjectResource> AddProjectResourceForMain()
    {
        //  Use OrchestrationHelper.cs to override this method as needed for further
        //  configuration of this resource in addition to the references set in AddProjects above.
        //  For example:
        //      public override IResourceBuilder<ProjectResource> AddProjectResourceForMain(IDistributedApplicationBuilder builder)
        //      {
        //          return base.AddProjectResourceForMain(builder)
        //              .WithEnvironment(...);
        //      }
        var p = _builder.AddProject<Projects.Earth_Main>("earth-main");


        return p;
    }
    public virtual IResourceBuilder<ViteAppResource> AddViteAppResourceForDolphinUi()
    {
        return _builder.AddViteApp("dolphinui", "../Earth.DolphinUi")
            .WithNpm(true)
            .WithEnvironment("BROWSER", "none")
            .WithExternalHttpEndpoints();
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Earth.Ocean.Services;
using Earth.Ocean.SetupHelpers.Base;

namespace Earth.Ocean.SetupHelpers;

internal class DependenciesHelper : BaseDependenciesHelper
{
    public DependenciesHelper(WebApplicationBuilder builder) : base(builder) { }

    //DEV_MANAGED_CODE_EXAMPLE: DI for the background worker
    public override void AddDependencies()
    {
        base.AddDependencies();

        _builder.Services.AddHostedService<WaterUpdateHostedService>();
    }
}

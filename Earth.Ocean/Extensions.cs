using Microsoft.Extensions.DependencyInjection;
using Earth.Ocean.SetupHelpers;
using Microsoft.AspNetCore.Builder;

namespace Earth.Ocean;

public static partial class Extensions
{
    public static WebApplicationBuilder AddOceanDependencies(this WebApplicationBuilder builder)
    {
        var helper = new DependenciesHelper(builder);
        helper.AddDependencies();
        return builder;
    }

    public static IServiceProvider InitializeOcean(this IServiceProvider services)
    {
        var helper = new InitializationHelper(services);
        helper.Initialize();
        return services;
    }
}

using Microsoft.Extensions.DependencyInjection;
using Earth.Dolphin.SetupHelpers;
using Microsoft.AspNetCore.Builder;

namespace Earth.Dolphin;

public static partial class Extensions
{
    public static WebApplicationBuilder AddDolphinDependencies(this WebApplicationBuilder builder)
    {
        var helper = new DependenciesHelper(builder);
        helper.AddDependencies();
        return builder;
    }

    public static IServiceProvider InitializeDolphin(this IServiceProvider services)
    {
        var helper = new InitializationHelper(services);
        helper.Initialize();
        return services;
    }
}

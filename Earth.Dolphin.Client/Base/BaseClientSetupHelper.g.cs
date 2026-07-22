using Microsoft.Extensions.DependencyInjection;

namespace Earth.Dolphin.Client.Base;

public abstract partial class BaseClientSetupHelper
{
    private IServiceCollection _services;

    public BaseClientSetupHelper(IServiceCollection services)
    {
        _services = services;
    }


    public virtual void ConfigureTricksClient(HttpClient client)
    {
        // This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
        // Learn more about service discovery scheme resolution at https://aka.ms/dotnet/sdschemes.
        client.BaseAddress = new("https+http://earth-main");
    }

    public virtual IHttpClientBuilder AddTricksRestClient()
    {
        var httpClientBuilder = _services.AddHttpClient<Earth.Dolphin.Client.Services.TricksRestClient>(ConfigureTricksClient);
        _services.AddTransient<Earth.Dolphin.Client.Services.Interfaces.ITricksRestClient>(s =>
        {
            return s.GetService<Earth.Dolphin.Client.Services.TricksRestClient>();
        });
        return httpClientBuilder;
    }

    public virtual void AddClientDependencies()
    {
        AddTricksRestClient();
    }
}

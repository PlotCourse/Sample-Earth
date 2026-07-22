using Earth.Ocean.Client.Services.Base;
using Earth.Ocean.Client.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Earth.Ocean.Client.Services;

public class WaterRestClient : BaseWaterRestClient, IWaterRestClient
{
    public WaterRestClient(HttpClient httpClient, ILogger<WaterRestClient> logger) : base(httpClient, logger) { }
}

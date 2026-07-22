using Earth.Dolphin.Client.Services.Base;
using Earth.Dolphin.Client.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Earth.Dolphin.Client.Services;

public class TricksRestClient : BaseTricksRestClient, ITricksRestClient
{
    public TricksRestClient(HttpClient httpClient, ILogger<TricksRestClient> logger) : base(httpClient, logger) { }
}

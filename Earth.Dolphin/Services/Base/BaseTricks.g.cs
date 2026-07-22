using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using InternalDataType_DolphinCoordinate = Earth.Dolphin.Data.DolphinCoordinate;
using Service_Ocean_IWaterService = Earth.Dolphin.Proxies.Ocean.Services.Interfaces.IWaterService;


namespace Earth.Dolphin.Services.Base;

internal abstract partial class BaseTricks
{
    protected Service_Ocean_IWaterService _oceanWaterService;
    protected ILogger<Tricks> _logger;

    public BaseTricks(
        Service_Ocean_IWaterService oceanWaterService,
        ILogger<Tricks> logger)
    {
        _oceanWaterService = oceanWaterService;
        _logger = logger;
    }


    public virtual Task<InternalDataType_DolphinCoordinate[]> Backflip(
        HttpContext httpContext)
    {
        return Task.FromResult(default(InternalDataType_DolphinCoordinate[]));
    }
}

using Earth.Dolphin.Data;
using Earth.Dolphin.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Earth.Dolphin.Services;

internal partial class Tricks : ITricks
{
    private static int coordId = 1;

    //DEV_MANAGED_CODE_EXAMPLE
    public override Task<DolphinCoordinate[]> Backflip(HttpContext httpContext)
    {
        var startX = Random.Shared.Next(1, 100);
        var startY = Random.Shared.Next(1, 100);
        var endX = startX + Random.Shared.Next(-5, 5);

        var dolphinCoord = new Data.DolphinCoordinate[] {
            new DolphinCoordinate(coordId++, startX, startY),
            new DolphinCoordinate(coordId++, endX, startY)
        };

        return Task.FromResult(dolphinCoord);
    }
}

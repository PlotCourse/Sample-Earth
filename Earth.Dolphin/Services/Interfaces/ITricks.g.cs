using InternalDataType_DolphinCoordinate = Earth.Dolphin.Data.DolphinCoordinate;
using Microsoft.AspNetCore.Http;

namespace Earth.Dolphin.Services.Interfaces;

internal partial interface ITricks
{

    public Task<InternalDataType_DolphinCoordinate[]> Backflip(
        HttpContext httpContext);
}

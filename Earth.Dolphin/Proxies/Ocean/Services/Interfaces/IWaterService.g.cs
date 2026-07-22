using Microsoft.AspNetCore.Http;
using Earth.Dolphin.Data;

namespace Earth.Dolphin.Proxies.Ocean.Services.Interfaces;

internal interface IWaterService
{
    Task UpdateWaterState(
        HttpContext httpContext);
    Task<global::Earth.Shared.Data.Wrappers.Result<DolphinCoordinate>> GetDolphinCoordinate(
        int dolphinCoordinateId,
        HttpContext httpContext);
    Task<global::Earth.Shared.Data.Wrappers.Result<DolphinCoordinate>> AddDolphinCoordinate(
        DolphinCoordinate dolphinCoordinate,
        HttpContext httpContext);
    Task<global::Earth.Shared.Data.Wrappers.Result<DolphinCoordinate>> ReplaceDolphinCoordinate(
        int dolphinCoordinateId,
        DolphinCoordinatePut dolphinCoordinatePut,
        HttpContext httpContext);
    Task<global::Earth.Shared.Data.Result> DeleteDolphinCoordinate(
        int dolphinCoordinateId,
        HttpContext httpContext);
    Task<global::Earth.Shared.Data.Wrappers.Result<HurricaneCoordinate>> GetHurricaneCoordinate(
        int hurricaneCoordinateId,
        HttpContext httpContext);
    Task<global::Earth.Shared.Data.Wrappers.Result<HurricaneCoordinate>> AddHurricaneCoordinate(
        HurricaneCoordinate hurricaneCoordinate,
        HttpContext httpContext);
    Task<global::Earth.Shared.Data.Wrappers.Result<HurricaneCoordinate>> ReplaceHurricaneCoordinate(
        int hurricaneCoordinateId,
        HurricaneCoordinatePut hurricaneCoordinatePut,
        HttpContext httpContext);
    Task<global::Earth.Shared.Data.Result> DeleteHurricaneCoordinate(
        int hurricaneCoordinateId,
        HttpContext httpContext);
}

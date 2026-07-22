using InternalDataType_DolphinCoordinate = Earth.Ocean.Data.DolphinCoordinate;
using InternalDataType_DolphinCoordinatePut = Earth.Ocean.Data.DolphinCoordinatePut;
using InternalDataType_HurricaneCoordinate = Earth.Ocean.Data.HurricaneCoordinate;
using InternalDataType_HurricaneCoordinatePut = Earth.Ocean.Data.HurricaneCoordinatePut;
using Microsoft.AspNetCore.Http;

namespace Earth.Ocean.Services.Interfaces;

internal partial interface IWater
{

    public Task UpdateWaterState(
        HttpContext httpContext);

    public Task<global::Earth.Shared.Data.Wrappers.Result<InternalDataType_DolphinCoordinate>> GetDolphinCoordinate(
        int dolphinCoordinateId,
        HttpContext httpContext);

    public Task<global::Earth.Shared.Data.Wrappers.Result<InternalDataType_DolphinCoordinate>> AddDolphinCoordinate(
        InternalDataType_DolphinCoordinate dolphinCoordinate,
        HttpContext httpContext);

    public Task<global::Earth.Shared.Data.Wrappers.Result<InternalDataType_DolphinCoordinate>> ReplaceDolphinCoordinate(
        int dolphinCoordinateId,
        InternalDataType_DolphinCoordinatePut dolphinCoordinatePut,
        HttpContext httpContext);

    public Task<global::Earth.Shared.Data.Result> DeleteDolphinCoordinate(
        int dolphinCoordinateId,
        HttpContext httpContext);

    public Task<global::Earth.Shared.Data.Wrappers.Result<InternalDataType_HurricaneCoordinate>> GetHurricaneCoordinate(
        int hurricaneCoordinateId,
        HttpContext httpContext);

    public Task<global::Earth.Shared.Data.Wrappers.Result<InternalDataType_HurricaneCoordinate>> AddHurricaneCoordinate(
        InternalDataType_HurricaneCoordinate hurricaneCoordinate,
        HttpContext httpContext);

    public Task<global::Earth.Shared.Data.Wrappers.Result<InternalDataType_HurricaneCoordinate>> ReplaceHurricaneCoordinate(
        int hurricaneCoordinateId,
        InternalDataType_HurricaneCoordinatePut hurricaneCoordinatePut,
        HttpContext httpContext);

    public Task<global::Earth.Shared.Data.Result> DeleteHurricaneCoordinate(
        int hurricaneCoordinateId,
        HttpContext httpContext);
}

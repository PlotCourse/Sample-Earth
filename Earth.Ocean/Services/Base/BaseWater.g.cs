using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using InternalDataType_DolphinCoordinate = Earth.Ocean.Data.DolphinCoordinate;
using InternalDataType_DolphinCoordinatePut = Earth.Ocean.Data.DolphinCoordinatePut;
using InternalDataType_HurricaneCoordinate = Earth.Ocean.Data.HurricaneCoordinate;
using InternalDataType_HurricaneCoordinatePut = Earth.Ocean.Data.HurricaneCoordinatePut;

using Broadcast_IOceanSurfaceInternalPublisher = Earth.Ocean.Broadcasts.Interfaces.IOceanSurfaceInternalPublisher;

namespace Earth.Ocean.Services.Base;

internal abstract partial class BaseWater
{
    protected Broadcast_IOceanSurfaceInternalPublisher _oceanOceanSurfacePublisher;
    protected ILogger<Water> _logger;

    public BaseWater(
        Broadcast_IOceanSurfaceInternalPublisher oceanOceanSurfacePublisher,
        ILogger<Water> logger)
    {
        _oceanOceanSurfacePublisher = oceanOceanSurfacePublisher;
        _logger = logger;
    }


    public virtual Task UpdateWaterState(
        HttpContext httpContext)
    {
        return Task.CompletedTask;
    }

    public virtual Task<global::Earth.Shared.Data.Wrappers.Result<InternalDataType_DolphinCoordinate>> GetDolphinCoordinate(
        int dolphinCoordinateId,
        HttpContext httpContext)
    {
        return Task.FromResult(default(global::Earth.Shared.Data.Wrappers.Result<InternalDataType_DolphinCoordinate>));
    }

    public virtual Task<global::Earth.Shared.Data.Wrappers.Result<InternalDataType_DolphinCoordinate>> AddDolphinCoordinate(
        InternalDataType_DolphinCoordinate dolphinCoordinate,
        HttpContext httpContext)
    {
        return Task.FromResult(default(global::Earth.Shared.Data.Wrappers.Result<InternalDataType_DolphinCoordinate>));
    }

    public virtual Task<global::Earth.Shared.Data.Wrappers.Result<InternalDataType_DolphinCoordinate>> ReplaceDolphinCoordinate(
        int dolphinCoordinateId,
        InternalDataType_DolphinCoordinatePut dolphinCoordinatePut,
        HttpContext httpContext)
    {
        return Task.FromResult(default(global::Earth.Shared.Data.Wrappers.Result<InternalDataType_DolphinCoordinate>));
    }

    public virtual Task<global::Earth.Shared.Data.Result> DeleteDolphinCoordinate(
        int dolphinCoordinateId,
        HttpContext httpContext)
    {
        return Task.FromResult(default(global::Earth.Shared.Data.Result));
    }

    public virtual Task<global::Earth.Shared.Data.Wrappers.Result<InternalDataType_HurricaneCoordinate>> GetHurricaneCoordinate(
        int hurricaneCoordinateId,
        HttpContext httpContext)
    {
        return Task.FromResult(default(global::Earth.Shared.Data.Wrappers.Result<InternalDataType_HurricaneCoordinate>));
    }

    public virtual Task<global::Earth.Shared.Data.Wrappers.Result<InternalDataType_HurricaneCoordinate>> AddHurricaneCoordinate(
        InternalDataType_HurricaneCoordinate hurricaneCoordinate,
        HttpContext httpContext)
    {
        return Task.FromResult(default(global::Earth.Shared.Data.Wrappers.Result<InternalDataType_HurricaneCoordinate>));
    }

    public virtual Task<global::Earth.Shared.Data.Wrappers.Result<InternalDataType_HurricaneCoordinate>> ReplaceHurricaneCoordinate(
        int hurricaneCoordinateId,
        InternalDataType_HurricaneCoordinatePut hurricaneCoordinatePut,
        HttpContext httpContext)
    {
        return Task.FromResult(default(global::Earth.Shared.Data.Wrappers.Result<InternalDataType_HurricaneCoordinate>));
    }

    public virtual Task<global::Earth.Shared.Data.Result> DeleteHurricaneCoordinate(
        int hurricaneCoordinateId,
        HttpContext httpContext)
    {
        return Task.FromResult(default(global::Earth.Shared.Data.Result));
    }
}

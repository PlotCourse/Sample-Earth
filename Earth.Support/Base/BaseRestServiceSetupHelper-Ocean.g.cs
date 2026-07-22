using Microsoft.AspNetCore.Http.HttpResults;
using Earth.Ocean.Contract.Services;
using Earth.Ocean.Contract.Data;

namespace Earth.Support.Base;

internal abstract partial class BaseRestServiceSetupHelper
{
    public virtual RouteHandlerBuilder MapOceanDefault(RouteHandlerBuilder builder)
    {
        return _webApiHelper.MapDefault(builder);
    }


    public virtual RouteHandlerBuilder MapOceanDefaultWater(RouteHandlerBuilder builder)
    {
        return MapOceanDefault(builder);
    }

    public virtual RouteHandlerBuilder MapOceanWaterUpdateWaterState()
    {
        //  Use RestServiceSetupHelper.cs to override methods as needed.  For example:
        //
        //      public override RouteHandlerBuilder MapOceanWaterUpdateWaterState()
        //      {
        //          return base.MapOceanWaterUpdateWaterState().RequireAuthorization("some_policy_name");
        //      }
        //
        //  See also, the virtual "Default" methods for modifying multiple service routes at once.
        var builder = _app.MapGet("/ocean/water",
            async Task<Results<Ok, InternalServerError>> (
                IWaterService waterService,
                ILogger<IWaterService> logger,
                HttpContext httpContext) =>
            {
                try
                {
                    await waterService.UpdateWaterState(
                        httpContext);
                    return TypedResults.Ok();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, null);
                    return TypedResults.InternalServerError();
                }
            });
        return MapOceanDefaultWater(builder);
    }

    public virtual RouteHandlerBuilder MapOceanWaterGetDolphinCoordinate()
    {
        var builder = _app.MapGet("/ocean/water/dolphincoordinate/{dolphinCoordinateId}",
            async Task<Results<Ok<global::Earth.Shared.Data.Wrappers.Result<DolphinCoordinate>>, NotFound, InternalServerError>> (
                int dolphinCoordinateId,
                IWaterService waterService,
                ILogger<IWaterService> logger,
                HttpContext httpContext) =>
            {
                try
                {
                    return await waterService.GetDolphinCoordinate(
                        dolphinCoordinateId,
                        httpContext)
                        is global::Earth.Shared.Data.Wrappers.Result<DolphinCoordinate> result
                            ? TypedResults.Ok(result)
                            : TypedResults.NotFound();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, null);
                    return TypedResults.InternalServerError();
                }
            });
        return MapOceanDefaultWater(builder);
    }

    public virtual RouteHandlerBuilder MapOceanWaterAddDolphinCoordinate()
    {
        var builder = _app.MapPost("/ocean/water/dolphincoordinate",
            async Task<Results<Ok<global::Earth.Shared.Data.Wrappers.Result<DolphinCoordinate>>, NotFound, InternalServerError>> (
                DolphinCoordinate dolphinCoordinate,
                IWaterService waterService,
                ILogger<IWaterService> logger,
                HttpContext httpContext) =>
            {
                try
                {
                    return await waterService.AddDolphinCoordinate(
                        dolphinCoordinate,
                        httpContext)
                        is global::Earth.Shared.Data.Wrappers.Result<DolphinCoordinate> result
                            ? TypedResults.Ok(result)
                            : TypedResults.NotFound();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, null);
                    return TypedResults.InternalServerError();
                }
            });
        return MapOceanDefaultWater(builder);
    }

    public virtual RouteHandlerBuilder MapOceanWaterReplaceDolphinCoordinate()
    {
        var builder = _app.MapPut("/ocean/water/dolphincoordinate/{dolphinCoordinateId}",
            async Task<Results<Ok<global::Earth.Shared.Data.Wrappers.Result<DolphinCoordinate>>, NotFound, InternalServerError>> (
                int dolphinCoordinateId,
                DolphinCoordinatePut dolphinCoordinatePut,
                IWaterService waterService,
                ILogger<IWaterService> logger,
                HttpContext httpContext) =>
            {
                try
                {
                    return await waterService.ReplaceDolphinCoordinate(
                        dolphinCoordinateId,
                        dolphinCoordinatePut,
                        httpContext)
                        is global::Earth.Shared.Data.Wrappers.Result<DolphinCoordinate> result
                            ? TypedResults.Ok(result)
                            : TypedResults.NotFound();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, null);
                    return TypedResults.InternalServerError();
                }
            });
        return MapOceanDefaultWater(builder);
    }

    public virtual RouteHandlerBuilder MapOceanWaterDeleteDolphinCoordinate()
    {
        var builder = _app.MapDelete("/ocean/water/dolphincoordinate/{dolphinCoordinateId}",
            async Task<Results<Ok<global::Earth.Shared.Data.Result>, NotFound, InternalServerError>> (
                int dolphinCoordinateId,
                IWaterService waterService,
                ILogger<IWaterService> logger,
                HttpContext httpContext) =>
            {
                try
                {
                    return await waterService.DeleteDolphinCoordinate(
                        dolphinCoordinateId,
                        httpContext)
                        is global::Earth.Shared.Data.Result result
                            ? TypedResults.Ok(result)
                            : TypedResults.NotFound();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, null);
                    return TypedResults.InternalServerError();
                }
            });
        return MapOceanDefaultWater(builder);
    }

    public virtual RouteHandlerBuilder MapOceanWaterGetHurricaneCoordinate()
    {
        var builder = _app.MapGet("/ocean/water/hurricanecoordinate/{hurricaneCoordinateId}",
            async Task<Results<Ok<global::Earth.Shared.Data.Wrappers.Result<HurricaneCoordinate>>, NotFound, InternalServerError>> (
                int hurricaneCoordinateId,
                IWaterService waterService,
                ILogger<IWaterService> logger,
                HttpContext httpContext) =>
            {
                try
                {
                    return await waterService.GetHurricaneCoordinate(
                        hurricaneCoordinateId,
                        httpContext)
                        is global::Earth.Shared.Data.Wrappers.Result<HurricaneCoordinate> result
                            ? TypedResults.Ok(result)
                            : TypedResults.NotFound();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, null);
                    return TypedResults.InternalServerError();
                }
            });
        return MapOceanDefaultWater(builder);
    }

    public virtual RouteHandlerBuilder MapOceanWaterAddHurricaneCoordinate()
    {
        var builder = _app.MapPost("/ocean/water/hurricanecoordinate",
            async Task<Results<Ok<global::Earth.Shared.Data.Wrappers.Result<HurricaneCoordinate>>, NotFound, InternalServerError>> (
                HurricaneCoordinate hurricaneCoordinate,
                IWaterService waterService,
                ILogger<IWaterService> logger,
                HttpContext httpContext) =>
            {
                try
                {
                    return await waterService.AddHurricaneCoordinate(
                        hurricaneCoordinate,
                        httpContext)
                        is global::Earth.Shared.Data.Wrappers.Result<HurricaneCoordinate> result
                            ? TypedResults.Ok(result)
                            : TypedResults.NotFound();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, null);
                    return TypedResults.InternalServerError();
                }
            });
        return MapOceanDefaultWater(builder);
    }

    public virtual RouteHandlerBuilder MapOceanWaterReplaceHurricaneCoordinate()
    {
        var builder = _app.MapPut("/ocean/water/hurricanecoordinate/{hurricaneCoordinateId}",
            async Task<Results<Ok<global::Earth.Shared.Data.Wrappers.Result<HurricaneCoordinate>>, NotFound, InternalServerError>> (
                int hurricaneCoordinateId,
                HurricaneCoordinatePut hurricaneCoordinatePut,
                IWaterService waterService,
                ILogger<IWaterService> logger,
                HttpContext httpContext) =>
            {
                try
                {
                    return await waterService.ReplaceHurricaneCoordinate(
                        hurricaneCoordinateId,
                        hurricaneCoordinatePut,
                        httpContext)
                        is global::Earth.Shared.Data.Wrappers.Result<HurricaneCoordinate> result
                            ? TypedResults.Ok(result)
                            : TypedResults.NotFound();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, null);
                    return TypedResults.InternalServerError();
                }
            });
        return MapOceanDefaultWater(builder);
    }

    public virtual RouteHandlerBuilder MapOceanWaterDeleteHurricaneCoordinate()
    {
        var builder = _app.MapDelete("/ocean/water/hurricanecoordinate/{hurricaneCoordinateId}",
            async Task<Results<Ok<global::Earth.Shared.Data.Result>, NotFound, InternalServerError>> (
                int hurricaneCoordinateId,
                IWaterService waterService,
                ILogger<IWaterService> logger,
                HttpContext httpContext) =>
            {
                try
                {
                    return await waterService.DeleteHurricaneCoordinate(
                        hurricaneCoordinateId,
                        httpContext)
                        is global::Earth.Shared.Data.Result result
                            ? TypedResults.Ok(result)
                            : TypedResults.NotFound();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, null);
                    return TypedResults.InternalServerError();
                }
            });
        return MapOceanDefaultWater(builder);
    }

    public virtual void MapForOcean()
    {
        MapOceanWaterUpdateWaterState();
        MapOceanWaterGetDolphinCoordinate();
        MapOceanWaterAddDolphinCoordinate();
        MapOceanWaterReplaceDolphinCoordinate();
        MapOceanWaterDeleteDolphinCoordinate();
        MapOceanWaterGetHurricaneCoordinate();
        MapOceanWaterAddHurricaneCoordinate();
        MapOceanWaterReplaceHurricaneCoordinate();
        MapOceanWaterDeleteHurricaneCoordinate();
    }
}

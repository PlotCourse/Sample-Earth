using Microsoft.AspNetCore.Http.HttpResults;
using Earth.Dolphin.Contract.Services;
using Earth.Dolphin.Contract.Data;

namespace Earth.Main.Base;

internal abstract partial class BaseRestServiceSetupHelper
{
    public virtual RouteHandlerBuilder MapDolphinDefault(RouteHandlerBuilder builder)
    {
        return _webApiHelper.MapDefault(builder);
    }


    public virtual RouteHandlerBuilder MapDolphinDefaultTricks(RouteHandlerBuilder builder)
    {
        return MapDolphinDefault(builder);
    }

    public virtual RouteHandlerBuilder MapDolphinTricksBackflip()
    {
        //  Use RestServiceSetupHelper.cs to override methods as needed.  For example:
        //
        //      public override RouteHandlerBuilder MapDolphinTricksBackflip()
        //      {
        //          return base.MapDolphinTricksBackflip().RequireAuthorization("some_policy_name");
        //      }
        //
        //  See also, the virtual "Default" methods for modifying multiple service routes at once.
        var builder = _app.MapGet("/dolphin/tricks/dolphincoordinate",
            async Task<Results<Ok<global::Earth.Shared.Data.ReadOnlyRecords<DolphinCoordinate>>, NotFound, InternalServerError>> (
                ITricksService tricksService,
                ILogger<ITricksService> logger,
                HttpContext httpContext) =>
            {
                try
                {
                    return await tricksService.Backflip(
                        httpContext)
                        is global::Earth.Shared.Data.ReadOnlyRecords<DolphinCoordinate> result
                            ? TypedResults.Ok(result)
                            : TypedResults.NotFound();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, null);
                    return TypedResults.InternalServerError();
                }
            });
        return MapDolphinDefaultTricks(builder);
    }

    public virtual void MapForDolphin()
    {
        MapDolphinTricksBackflip();
    }
}

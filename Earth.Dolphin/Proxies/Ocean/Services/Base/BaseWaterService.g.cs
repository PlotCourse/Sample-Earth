using Microsoft.AspNetCore.Http;
using Earth.Shared.Data.Mappers;
using IWaterRestClient = Earth.Ocean.Client.Services.Interfaces.IWaterRestClient;
using IDefaultServiceClientInitializer = Earth.Dolphin.Proxies.Ocean.Services.Interfaces.IDefaultServiceClientInitializer;
using ContractDataType_DolphinCoordinate = Earth.Ocean.Contract.Data.DolphinCoordinate;
using ContractDataType_DolphinCoordinatePut = Earth.Ocean.Contract.Data.DolphinCoordinatePut;
using ContractDataType_HurricaneCoordinate = Earth.Ocean.Contract.Data.HurricaneCoordinate;
using ContractDataType_HurricaneCoordinatePut = Earth.Ocean.Contract.Data.HurricaneCoordinatePut;
using InternalDataType_DolphinCoordinate = Earth.Dolphin.Data.DolphinCoordinate;
using InternalDataType_DolphinCoordinatePut = Earth.Dolphin.Data.DolphinCoordinatePut;
using InternalDataType_HurricaneCoordinate = Earth.Dolphin.Data.HurricaneCoordinate;
using InternalDataType_HurricaneCoordinatePut = Earth.Dolphin.Data.HurricaneCoordinatePut;

namespace Earth.Dolphin.Proxies.Ocean.Services.Base;

internal abstract partial class BaseWaterService(
    Func<IWaterRestClient> RestClientFactory, IDefaultServiceClientInitializer DefaultInitializer)
{
    public virtual IWaterRestClient BuildRestClient(HttpContext httpContext)
    {
        var client = RestClientFactory();
        DefaultInitializer.Initialize(client.HttpClient, httpContext);
        return client;
    }

    public virtual async Task UpdateWaterState(
        HttpContext httpContext)
    {
        var client = BuildRestClient(httpContext);
        await client.UpdateWaterState();
    }
    public virtual async Task<global::Earth.Shared.Data.Wrappers.Result<InternalDataType_DolphinCoordinate>> GetDolphinCoordinate(
        int dolphinCoordinateId,
        HttpContext httpContext)
    {
        var client = BuildRestClient(httpContext);
        var remoteResult = await client.GetDolphinCoordinate(
            dolphinCoordinateId);

        return remoteResult?.Map<global::Earth.Shared.Data.Wrappers.Result<ContractDataType_DolphinCoordinate>, global::Earth.Shared.Data.Wrappers.Result<InternalDataType_DolphinCoordinate>>();
    }
    public virtual async Task<global::Earth.Shared.Data.Wrappers.Result<InternalDataType_DolphinCoordinate>> AddDolphinCoordinate(
        InternalDataType_DolphinCoordinate dolphinCoordinate,
        HttpContext httpContext)
    {
        var client = BuildRestClient(httpContext);
        var remoteResult = await client.AddDolphinCoordinate(
            dolphinCoordinate?.Map<InternalDataType_DolphinCoordinate, ContractDataType_DolphinCoordinate>());

        return remoteResult?.Map<global::Earth.Shared.Data.Wrappers.Result<ContractDataType_DolphinCoordinate>, global::Earth.Shared.Data.Wrappers.Result<InternalDataType_DolphinCoordinate>>();
    }
    public virtual async Task<global::Earth.Shared.Data.Wrappers.Result<InternalDataType_DolphinCoordinate>> ReplaceDolphinCoordinate(
        int dolphinCoordinateId,
        InternalDataType_DolphinCoordinatePut dolphinCoordinatePut,
        HttpContext httpContext)
    {
        var client = BuildRestClient(httpContext);
        var remoteResult = await client.ReplaceDolphinCoordinate(
            dolphinCoordinateId,
            dolphinCoordinatePut?.Map<InternalDataType_DolphinCoordinatePut, ContractDataType_DolphinCoordinatePut>());

        return remoteResult?.Map<global::Earth.Shared.Data.Wrappers.Result<ContractDataType_DolphinCoordinate>, global::Earth.Shared.Data.Wrappers.Result<InternalDataType_DolphinCoordinate>>();
    }
    public virtual async Task<global::Earth.Shared.Data.Result> DeleteDolphinCoordinate(
        int dolphinCoordinateId,
        HttpContext httpContext)
    {
        var client = BuildRestClient(httpContext);
        var remoteResult = await client.DeleteDolphinCoordinate(
            dolphinCoordinateId);

        return remoteResult;
    }
    public virtual async Task<global::Earth.Shared.Data.Wrappers.Result<InternalDataType_HurricaneCoordinate>> GetHurricaneCoordinate(
        int hurricaneCoordinateId,
        HttpContext httpContext)
    {
        var client = BuildRestClient(httpContext);
        var remoteResult = await client.GetHurricaneCoordinate(
            hurricaneCoordinateId);

        return remoteResult?.Map<global::Earth.Shared.Data.Wrappers.Result<ContractDataType_HurricaneCoordinate>, global::Earth.Shared.Data.Wrappers.Result<InternalDataType_HurricaneCoordinate>>();
    }
    public virtual async Task<global::Earth.Shared.Data.Wrappers.Result<InternalDataType_HurricaneCoordinate>> AddHurricaneCoordinate(
        InternalDataType_HurricaneCoordinate hurricaneCoordinate,
        HttpContext httpContext)
    {
        var client = BuildRestClient(httpContext);
        var remoteResult = await client.AddHurricaneCoordinate(
            hurricaneCoordinate?.Map<InternalDataType_HurricaneCoordinate, ContractDataType_HurricaneCoordinate>());

        return remoteResult?.Map<global::Earth.Shared.Data.Wrappers.Result<ContractDataType_HurricaneCoordinate>, global::Earth.Shared.Data.Wrappers.Result<InternalDataType_HurricaneCoordinate>>();
    }
    public virtual async Task<global::Earth.Shared.Data.Wrappers.Result<InternalDataType_HurricaneCoordinate>> ReplaceHurricaneCoordinate(
        int hurricaneCoordinateId,
        InternalDataType_HurricaneCoordinatePut hurricaneCoordinatePut,
        HttpContext httpContext)
    {
        var client = BuildRestClient(httpContext);
        var remoteResult = await client.ReplaceHurricaneCoordinate(
            hurricaneCoordinateId,
            hurricaneCoordinatePut?.Map<InternalDataType_HurricaneCoordinatePut, ContractDataType_HurricaneCoordinatePut>());

        return remoteResult?.Map<global::Earth.Shared.Data.Wrappers.Result<ContractDataType_HurricaneCoordinate>, global::Earth.Shared.Data.Wrappers.Result<InternalDataType_HurricaneCoordinate>>();
    }
    public virtual async Task<global::Earth.Shared.Data.Result> DeleteHurricaneCoordinate(
        int hurricaneCoordinateId,
        HttpContext httpContext)
    {
        var client = BuildRestClient(httpContext);
        var remoteResult = await client.DeleteHurricaneCoordinate(
            hurricaneCoordinateId);

        return remoteResult;
    }
}

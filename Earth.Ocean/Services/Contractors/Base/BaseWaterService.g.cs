using Earth.Shared.Data.Mappers;
using ContractDataType_DolphinCoordinate = Earth.Ocean.Contract.Data.DolphinCoordinate;
using ContractDataType_DolphinCoordinatePut = Earth.Ocean.Contract.Data.DolphinCoordinatePut;
using ContractDataType_HurricaneCoordinate = Earth.Ocean.Contract.Data.HurricaneCoordinate;
using ContractDataType_HurricaneCoordinatePut = Earth.Ocean.Contract.Data.HurricaneCoordinatePut;
using InternalDataType_DolphinCoordinate = Earth.Ocean.Data.DolphinCoordinate;
using InternalDataType_DolphinCoordinatePut = Earth.Ocean.Data.DolphinCoordinatePut;
using InternalDataType_HurricaneCoordinate = Earth.Ocean.Data.HurricaneCoordinate;
using InternalDataType_HurricaneCoordinatePut = Earth.Ocean.Data.HurricaneCoordinatePut;

namespace Earth.Ocean.Services.Contractors.Base;

internal partial class BaseWaterService
{
    protected global::Earth.Ocean.Services.Interfaces.IWater _waterImpl;

    public BaseWaterService(global::Earth.Ocean.Services.Interfaces.IWater waterImpl)
    {
        _waterImpl = waterImpl;
    }

    public virtual Task UpdateWaterState(
        global::Microsoft.AspNetCore.Http.HttpContext httpContext)
    {
        return _waterImpl.UpdateWaterState(
            httpContext);
    }

    public virtual async Task<global::Earth.Shared.Data.Wrappers.Result<ContractDataType_DolphinCoordinate>> GetDolphinCoordinate(
        int dolphinCoordinateId,
        global::Microsoft.AspNetCore.Http.HttpContext httpContext)
    {
        var result = await _waterImpl.GetDolphinCoordinate(
            dolphinCoordinateId,
            httpContext);
        return result?.Map<global::Earth.Shared.Data.Wrappers.Result<InternalDataType_DolphinCoordinate>, global::Earth.Shared.Data.Wrappers.Result<ContractDataType_DolphinCoordinate>>();
    }

    public virtual async Task<global::Earth.Shared.Data.Wrappers.Result<ContractDataType_DolphinCoordinate>> AddDolphinCoordinate(
        ContractDataType_DolphinCoordinate dolphinCoordinate,
        global::Microsoft.AspNetCore.Http.HttpContext httpContext)
    {
        var result = await _waterImpl.AddDolphinCoordinate(
            dolphinCoordinate?.Map<ContractDataType_DolphinCoordinate, InternalDataType_DolphinCoordinate>(),
            httpContext);
        return result?.Map<global::Earth.Shared.Data.Wrappers.Result<InternalDataType_DolphinCoordinate>, global::Earth.Shared.Data.Wrappers.Result<ContractDataType_DolphinCoordinate>>();
    }

    public virtual async Task<global::Earth.Shared.Data.Wrappers.Result<ContractDataType_DolphinCoordinate>> ReplaceDolphinCoordinate(
        int dolphinCoordinateId,
        ContractDataType_DolphinCoordinatePut dolphinCoordinatePut,
        global::Microsoft.AspNetCore.Http.HttpContext httpContext)
    {
        var result = await _waterImpl.ReplaceDolphinCoordinate(
            dolphinCoordinateId,
            dolphinCoordinatePut?.Map<ContractDataType_DolphinCoordinatePut, InternalDataType_DolphinCoordinatePut>(),
            httpContext);
        return result?.Map<global::Earth.Shared.Data.Wrappers.Result<InternalDataType_DolphinCoordinate>, global::Earth.Shared.Data.Wrappers.Result<ContractDataType_DolphinCoordinate>>();
    }

    public virtual async Task<global::Earth.Shared.Data.Result> DeleteDolphinCoordinate(
        int dolphinCoordinateId,
        global::Microsoft.AspNetCore.Http.HttpContext httpContext)
    {
        var result = await _waterImpl.DeleteDolphinCoordinate(
            dolphinCoordinateId,
            httpContext);
        return result;
    }

    public virtual async Task<global::Earth.Shared.Data.Wrappers.Result<ContractDataType_HurricaneCoordinate>> GetHurricaneCoordinate(
        int hurricaneCoordinateId,
        global::Microsoft.AspNetCore.Http.HttpContext httpContext)
    {
        var result = await _waterImpl.GetHurricaneCoordinate(
            hurricaneCoordinateId,
            httpContext);
        return result?.Map<global::Earth.Shared.Data.Wrappers.Result<InternalDataType_HurricaneCoordinate>, global::Earth.Shared.Data.Wrappers.Result<ContractDataType_HurricaneCoordinate>>();
    }

    public virtual async Task<global::Earth.Shared.Data.Wrappers.Result<ContractDataType_HurricaneCoordinate>> AddHurricaneCoordinate(
        ContractDataType_HurricaneCoordinate hurricaneCoordinate,
        global::Microsoft.AspNetCore.Http.HttpContext httpContext)
    {
        var result = await _waterImpl.AddHurricaneCoordinate(
            hurricaneCoordinate?.Map<ContractDataType_HurricaneCoordinate, InternalDataType_HurricaneCoordinate>(),
            httpContext);
        return result?.Map<global::Earth.Shared.Data.Wrappers.Result<InternalDataType_HurricaneCoordinate>, global::Earth.Shared.Data.Wrappers.Result<ContractDataType_HurricaneCoordinate>>();
    }

    public virtual async Task<global::Earth.Shared.Data.Wrappers.Result<ContractDataType_HurricaneCoordinate>> ReplaceHurricaneCoordinate(
        int hurricaneCoordinateId,
        ContractDataType_HurricaneCoordinatePut hurricaneCoordinatePut,
        global::Microsoft.AspNetCore.Http.HttpContext httpContext)
    {
        var result = await _waterImpl.ReplaceHurricaneCoordinate(
            hurricaneCoordinateId,
            hurricaneCoordinatePut?.Map<ContractDataType_HurricaneCoordinatePut, InternalDataType_HurricaneCoordinatePut>(),
            httpContext);
        return result?.Map<global::Earth.Shared.Data.Wrappers.Result<InternalDataType_HurricaneCoordinate>, global::Earth.Shared.Data.Wrappers.Result<ContractDataType_HurricaneCoordinate>>();
    }

    public virtual async Task<global::Earth.Shared.Data.Result> DeleteHurricaneCoordinate(
        int hurricaneCoordinateId,
        global::Microsoft.AspNetCore.Http.HttpContext httpContext)
    {
        var result = await _waterImpl.DeleteHurricaneCoordinate(
            hurricaneCoordinateId,
            httpContext);
        return result;
    }
}

using Microsoft.AspNetCore.Http;
using ContractDataType_DolphinCoordinate = Earth.Ocean.Contract.Data.DolphinCoordinate;
using ContractDataType_DolphinCoordinatePut = Earth.Ocean.Contract.Data.DolphinCoordinatePut;
using ContractDataType_HurricaneCoordinate = Earth.Ocean.Contract.Data.HurricaneCoordinate;
using ContractDataType_HurricaneCoordinatePut = Earth.Ocean.Contract.Data.HurricaneCoordinatePut;

namespace Earth.Ocean.Contract.Services;

public partial interface IWaterService
{

    Task UpdateWaterState(
        HttpContext httpContext);

    Task<global::Earth.Shared.Data.Wrappers.Result<ContractDataType_DolphinCoordinate>> GetDolphinCoordinate(
        int dolphinCoordinateId,
        HttpContext httpContext);

    Task<global::Earth.Shared.Data.Wrappers.Result<ContractDataType_DolphinCoordinate>> AddDolphinCoordinate(
        ContractDataType_DolphinCoordinate dolphinCoordinate,
        HttpContext httpContext);

    Task<global::Earth.Shared.Data.Wrappers.Result<ContractDataType_DolphinCoordinate>> ReplaceDolphinCoordinate(
        int dolphinCoordinateId,
        ContractDataType_DolphinCoordinatePut dolphinCoordinatePut,
        HttpContext httpContext);

    Task<global::Earth.Shared.Data.Result> DeleteDolphinCoordinate(
        int dolphinCoordinateId,
        HttpContext httpContext);

    Task<global::Earth.Shared.Data.Wrappers.Result<ContractDataType_HurricaneCoordinate>> GetHurricaneCoordinate(
        int hurricaneCoordinateId,
        HttpContext httpContext);

    Task<global::Earth.Shared.Data.Wrappers.Result<ContractDataType_HurricaneCoordinate>> AddHurricaneCoordinate(
        ContractDataType_HurricaneCoordinate hurricaneCoordinate,
        HttpContext httpContext);

    Task<global::Earth.Shared.Data.Wrappers.Result<ContractDataType_HurricaneCoordinate>> ReplaceHurricaneCoordinate(
        int hurricaneCoordinateId,
        ContractDataType_HurricaneCoordinatePut hurricaneCoordinatePut,
        HttpContext httpContext);

    Task<global::Earth.Shared.Data.Result> DeleteHurricaneCoordinate(
        int hurricaneCoordinateId,
        HttpContext httpContext);
}

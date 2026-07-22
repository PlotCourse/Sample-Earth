using Microsoft.AspNetCore.Http;
using ContractDataType_DolphinCoordinate = Earth.Dolphin.Contract.Data.DolphinCoordinate;

namespace Earth.Dolphin.Contract.Services;

public partial interface ITricksService
{

    Task<global::Earth.Shared.Data.ReadOnlyRecords<ContractDataType_DolphinCoordinate>> Backflip(
        HttpContext httpContext);
}

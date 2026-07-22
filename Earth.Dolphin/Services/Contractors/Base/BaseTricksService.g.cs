using Earth.Shared.Data.Mappers;
using ContractDataType_DolphinCoordinate = Earth.Dolphin.Contract.Data.DolphinCoordinate;
using InternalDataType_DolphinCoordinate = Earth.Dolphin.Data.DolphinCoordinate;

namespace Earth.Dolphin.Services.Contractors.Base;

internal partial class BaseTricksService
{
    protected global::Earth.Dolphin.Services.Interfaces.ITricks _tricksImpl;

    public BaseTricksService(global::Earth.Dolphin.Services.Interfaces.ITricks tricksImpl)
    {
        _tricksImpl = tricksImpl;
    }

    public virtual async Task<global::Earth.Shared.Data.ReadOnlyRecords<ContractDataType_DolphinCoordinate>> Backflip(
        global::Microsoft.AspNetCore.Http.HttpContext httpContext)
    {
        var result = await _tricksImpl.Backflip(
            httpContext);
        return result?.Map<InternalDataType_DolphinCoordinate[], global::Earth.Shared.Data.ReadOnlyRecords<ContractDataType_DolphinCoordinate>>();
    }
}

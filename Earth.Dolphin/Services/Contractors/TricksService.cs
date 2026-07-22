
namespace Earth.Dolphin.Services.Contractors;

internal class TricksService : global::Earth.Dolphin.Services.Contractors.Base.BaseTricksService, global::Earth.Dolphin.Contract.Services.ITricksService
{
    public TricksService(global::Earth.Dolphin.Services.Interfaces.ITricks tricksImpl) : base(tricksImpl) { }

}

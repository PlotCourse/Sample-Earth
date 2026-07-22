
namespace Earth.Ocean.Services.Contractors;

internal class WaterService : global::Earth.Ocean.Services.Contractors.Base.BaseWaterService, global::Earth.Ocean.Contract.Services.IWaterService
{
    public WaterService(global::Earth.Ocean.Services.Interfaces.IWater waterImpl) : base(waterImpl) { }

}

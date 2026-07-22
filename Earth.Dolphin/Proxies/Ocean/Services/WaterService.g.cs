using BaseWaterService = Earth.Dolphin.Proxies.Ocean.Services.Base.BaseWaterService;
using ProxyService_Ocean_IWaterService = Earth.Dolphin.Proxies.Ocean.Services.Interfaces.IWaterService;
using IWaterRestClient = Earth.Ocean.Client.Services.Interfaces.IWaterRestClient;
using IDefaultServiceClientInitializer = Earth.Dolphin.Proxies.Ocean.Services.Interfaces.IDefaultServiceClientInitializer;

namespace Earth.Dolphin.Proxies.Ocean.Services;

internal partial class WaterService
    : BaseWaterService, ProxyService_Ocean_IWaterService
{
    public WaterService(
        Func<IWaterRestClient> restClientFactory,
        IDefaultServiceClientInitializer defaultInitializer) : base(restClientFactory, defaultInitializer) { }
}

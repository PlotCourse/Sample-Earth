using BaseDefaultServiceClientInitializer = Earth.Dolphin.Proxies.Ocean.Services.Base.BaseDefaultServiceClientInitializer;
using IDefaultServiceClientInitializer = Earth.Dolphin.Proxies.Ocean.Services.Interfaces.IDefaultServiceClientInitializer;
using Dolphin_IDefaultServiceClientInitializer = Earth.Dolphin.Proxies.DefaultInitializers.Interfaces.IDefaultServiceClientInitializer;

namespace Earth.Dolphin.Proxies.Ocean.Services;

internal partial class DefaultServiceClientInitializer : BaseDefaultServiceClientInitializer, IDefaultServiceClientInitializer
{
    public DefaultServiceClientInitializer(Dolphin_IDefaultServiceClientInitializer DefaultInitializer)
        : base(DefaultInitializer) { }
}

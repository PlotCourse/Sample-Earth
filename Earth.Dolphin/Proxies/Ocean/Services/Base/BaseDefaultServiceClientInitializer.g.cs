using Microsoft.AspNetCore.Http;
using Dolphin_IDefaultServiceClientInitializer = Earth.Dolphin.Proxies.DefaultInitializers.Interfaces.IDefaultServiceClientInitializer;

namespace Earth.Dolphin.Proxies.Ocean.Services.Base;

internal abstract partial class BaseDefaultServiceClientInitializer(Dolphin_IDefaultServiceClientInitializer DefaultInitializer)
{
    public virtual void Initialize(HttpClient client, HttpContext fromContext)
    {
        DefaultInitializer.Initialize(client, fromContext);
    }
}

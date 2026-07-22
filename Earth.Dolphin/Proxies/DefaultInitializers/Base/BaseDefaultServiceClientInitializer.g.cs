using Microsoft.AspNetCore.Http;

namespace Earth.Dolphin.Proxies.DefaultInitializers.Base;

internal abstract partial class BaseDefaultServiceClientInitializer
{
    public virtual void Initialize(HttpClient client, HttpContext fromContext) { }
}

using Microsoft.AspNetCore.Http;

namespace Earth.Dolphin.Proxies.DefaultInitializers.Interfaces;

internal partial interface IDefaultServiceClientInitializer
{
    void Initialize(HttpClient client, HttpContext fromContext);
}

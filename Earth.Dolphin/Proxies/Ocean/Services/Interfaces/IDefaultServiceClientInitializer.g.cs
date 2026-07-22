using Microsoft.AspNetCore.Http;

namespace Earth.Dolphin.Proxies.Ocean.Services.Interfaces;

internal partial interface IDefaultServiceClientInitializer
{
    void Initialize(HttpClient client, HttpContext fromContext);
}

using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Earth.Dolphin.Contract.Data;

namespace Earth.Dolphin.Client.Services.Base;

public abstract partial class BaseTricksRestClient
{
    protected HttpClient _httpClient;
    protected ILogger<TricksRestClient> _logger;

    public HttpClient HttpClient => _httpClient;

    public BaseTricksRestClient(HttpClient httpClient, ILogger<TricksRestClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }


    public Task<global::Earth.Shared.Data.ReadOnlyRecords<DolphinCoordinate>> Backflip()
    {
        return Backflip(
            default(CancellationToken));
    }

    public async Task<global::Earth.Shared.Data.ReadOnlyRecords<DolphinCoordinate>> Backflip(
        CancellationToken cancellationToken)
    {
        var recordForBackflip = await _httpClient.GetFromJsonAsync<global::Earth.Shared.Data.ReadOnlyRecords<DolphinCoordinate>>
            ($"/dolphin/tricks/dolphincoordinate", cancellationToken);
        return recordForBackflip;
    }

    protected virtual void HandleFailedResponseCode(string method, System.Net.HttpStatusCode statusCode)
    {
        _logger.LogError($"HttpStatusCode HttpStatusCode {(int)statusCode} ({statusCode}) received in call made by {nameof(TricksRestClient)}.{method}");
    }
}

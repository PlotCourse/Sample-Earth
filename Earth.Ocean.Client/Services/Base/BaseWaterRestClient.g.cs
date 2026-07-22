using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Earth.Ocean.Contract.Data;

namespace Earth.Ocean.Client.Services.Base;

public abstract partial class BaseWaterRestClient
{
    protected HttpClient _httpClient;
    protected ILogger<WaterRestClient> _logger;

    public HttpClient HttpClient => _httpClient;

    public BaseWaterRestClient(HttpClient httpClient, ILogger<WaterRestClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }


    public Task UpdateWaterState()
    {
        return UpdateWaterState(
            default(CancellationToken));
    }

    public async Task UpdateWaterState(
        CancellationToken cancellationToken)
    {
        var responseForUpdateWaterState = await _httpClient.GetAsync
            ($"/ocean/water", cancellationToken);

        if (!responseForUpdateWaterState.IsSuccessStatusCode)
        {
            HandleFailedResponseCode(nameof(UpdateWaterState), responseForUpdateWaterState.StatusCode);
        }
    }

    public Task<global::Earth.Shared.Data.Wrappers.Result<DolphinCoordinate>> GetDolphinCoordinate(
        int dolphinCoordinateId)
    {
        return GetDolphinCoordinate(
            dolphinCoordinateId,
            default(CancellationToken));
    }

    public async Task<global::Earth.Shared.Data.Wrappers.Result<DolphinCoordinate>> GetDolphinCoordinate(
        int dolphinCoordinateId,
        CancellationToken cancellationToken)
    {
        var recordForGetDolphinCoordinate = await _httpClient.GetFromJsonAsync<global::Earth.Shared.Data.Wrappers.Result<DolphinCoordinate>>
            ($"/ocean/water/dolphincoordinate/{dolphinCoordinateId}", cancellationToken);
        return recordForGetDolphinCoordinate;
    }

    public Task<global::Earth.Shared.Data.Wrappers.Result<DolphinCoordinate>> AddDolphinCoordinate(
        DolphinCoordinate dolphinCoordinate)
    {
        return AddDolphinCoordinate(
            dolphinCoordinate,
            default(CancellationToken));
    }

    public async Task<global::Earth.Shared.Data.Wrappers.Result<DolphinCoordinate>> AddDolphinCoordinate(
        DolphinCoordinate dolphinCoordinate,
        CancellationToken cancellationToken)
    {
        var responseForAddDolphinCoordinate = await _httpClient.PostAsJsonAsync<DolphinCoordinate>
            ($"/ocean/water/dolphincoordinate", dolphinCoordinate, cancellationToken);

        if (!responseForAddDolphinCoordinate.IsSuccessStatusCode)
        {
            HandleFailedResponseCode(nameof(AddDolphinCoordinate), responseForAddDolphinCoordinate.StatusCode);
            return default;
        }

        var recordForAddDolphinCoordinate = await responseForAddDolphinCoordinate.Content
            .ReadFromJsonAsync<global::Earth.Shared.Data.Wrappers.Result<DolphinCoordinate>>(cancellationToken);

        return recordForAddDolphinCoordinate;
    }

    public Task<global::Earth.Shared.Data.Wrappers.Result<DolphinCoordinate>> ReplaceDolphinCoordinate(
        int dolphinCoordinateId,
        DolphinCoordinatePut dolphinCoordinatePut)
    {
        return ReplaceDolphinCoordinate(
            dolphinCoordinateId,
            dolphinCoordinatePut,
            default(CancellationToken));
    }

    public async Task<global::Earth.Shared.Data.Wrappers.Result<DolphinCoordinate>> ReplaceDolphinCoordinate(
        int dolphinCoordinateId,
        DolphinCoordinatePut dolphinCoordinatePut,
        CancellationToken cancellationToken)
    {
        var responseForReplaceDolphinCoordinate = await _httpClient.PutAsJsonAsync<DolphinCoordinatePut>
            ($"/ocean/water/dolphincoordinate/{dolphinCoordinateId}", dolphinCoordinatePut, cancellationToken);

        if (!responseForReplaceDolphinCoordinate.IsSuccessStatusCode)
        {
            HandleFailedResponseCode(nameof(ReplaceDolphinCoordinate), responseForReplaceDolphinCoordinate.StatusCode);
            return default;
        }

        var recordForReplaceDolphinCoordinate = await responseForReplaceDolphinCoordinate.Content
            .ReadFromJsonAsync<global::Earth.Shared.Data.Wrappers.Result<DolphinCoordinate>>(cancellationToken);

        return recordForReplaceDolphinCoordinate;
    }

    public Task<global::Earth.Shared.Data.Result> DeleteDolphinCoordinate(
        int dolphinCoordinateId)
    {
        return DeleteDolphinCoordinate(
            dolphinCoordinateId,
            default(CancellationToken));
    }

    public async Task<global::Earth.Shared.Data.Result> DeleteDolphinCoordinate(
        int dolphinCoordinateId,
        CancellationToken cancellationToken)
    {
        var responseForDeleteDolphinCoordinate = await _httpClient.DeleteAsync
            ($"/ocean/water/dolphincoordinate/{dolphinCoordinateId}", cancellationToken);

        if (!responseForDeleteDolphinCoordinate.IsSuccessStatusCode)
        {
            HandleFailedResponseCode(nameof(DeleteDolphinCoordinate), responseForDeleteDolphinCoordinate.StatusCode);
            return default;
        }

        var recordForDeleteDolphinCoordinate = await responseForDeleteDolphinCoordinate.Content
            .ReadFromJsonAsync<global::Earth.Shared.Data.Result>(cancellationToken);

        return recordForDeleteDolphinCoordinate;
    }

    public Task<global::Earth.Shared.Data.Wrappers.Result<HurricaneCoordinate>> GetHurricaneCoordinate(
        int hurricaneCoordinateId)
    {
        return GetHurricaneCoordinate(
            hurricaneCoordinateId,
            default(CancellationToken));
    }

    public async Task<global::Earth.Shared.Data.Wrappers.Result<HurricaneCoordinate>> GetHurricaneCoordinate(
        int hurricaneCoordinateId,
        CancellationToken cancellationToken)
    {
        var recordForGetHurricaneCoordinate = await _httpClient.GetFromJsonAsync<global::Earth.Shared.Data.Wrappers.Result<HurricaneCoordinate>>
            ($"/ocean/water/hurricanecoordinate/{hurricaneCoordinateId}", cancellationToken);
        return recordForGetHurricaneCoordinate;
    }

    public Task<global::Earth.Shared.Data.Wrappers.Result<HurricaneCoordinate>> AddHurricaneCoordinate(
        HurricaneCoordinate hurricaneCoordinate)
    {
        return AddHurricaneCoordinate(
            hurricaneCoordinate,
            default(CancellationToken));
    }

    public async Task<global::Earth.Shared.Data.Wrappers.Result<HurricaneCoordinate>> AddHurricaneCoordinate(
        HurricaneCoordinate hurricaneCoordinate,
        CancellationToken cancellationToken)
    {
        var responseForAddHurricaneCoordinate = await _httpClient.PostAsJsonAsync<HurricaneCoordinate>
            ($"/ocean/water/hurricanecoordinate", hurricaneCoordinate, cancellationToken);

        if (!responseForAddHurricaneCoordinate.IsSuccessStatusCode)
        {
            HandleFailedResponseCode(nameof(AddHurricaneCoordinate), responseForAddHurricaneCoordinate.StatusCode);
            return default;
        }

        var recordForAddHurricaneCoordinate = await responseForAddHurricaneCoordinate.Content
            .ReadFromJsonAsync<global::Earth.Shared.Data.Wrappers.Result<HurricaneCoordinate>>(cancellationToken);

        return recordForAddHurricaneCoordinate;
    }

    public Task<global::Earth.Shared.Data.Wrappers.Result<HurricaneCoordinate>> ReplaceHurricaneCoordinate(
        int hurricaneCoordinateId,
        HurricaneCoordinatePut hurricaneCoordinatePut)
    {
        return ReplaceHurricaneCoordinate(
            hurricaneCoordinateId,
            hurricaneCoordinatePut,
            default(CancellationToken));
    }

    public async Task<global::Earth.Shared.Data.Wrappers.Result<HurricaneCoordinate>> ReplaceHurricaneCoordinate(
        int hurricaneCoordinateId,
        HurricaneCoordinatePut hurricaneCoordinatePut,
        CancellationToken cancellationToken)
    {
        var responseForReplaceHurricaneCoordinate = await _httpClient.PutAsJsonAsync<HurricaneCoordinatePut>
            ($"/ocean/water/hurricanecoordinate/{hurricaneCoordinateId}", hurricaneCoordinatePut, cancellationToken);

        if (!responseForReplaceHurricaneCoordinate.IsSuccessStatusCode)
        {
            HandleFailedResponseCode(nameof(ReplaceHurricaneCoordinate), responseForReplaceHurricaneCoordinate.StatusCode);
            return default;
        }

        var recordForReplaceHurricaneCoordinate = await responseForReplaceHurricaneCoordinate.Content
            .ReadFromJsonAsync<global::Earth.Shared.Data.Wrappers.Result<HurricaneCoordinate>>(cancellationToken);

        return recordForReplaceHurricaneCoordinate;
    }

    public Task<global::Earth.Shared.Data.Result> DeleteHurricaneCoordinate(
        int hurricaneCoordinateId)
    {
        return DeleteHurricaneCoordinate(
            hurricaneCoordinateId,
            default(CancellationToken));
    }

    public async Task<global::Earth.Shared.Data.Result> DeleteHurricaneCoordinate(
        int hurricaneCoordinateId,
        CancellationToken cancellationToken)
    {
        var responseForDeleteHurricaneCoordinate = await _httpClient.DeleteAsync
            ($"/ocean/water/hurricanecoordinate/{hurricaneCoordinateId}", cancellationToken);

        if (!responseForDeleteHurricaneCoordinate.IsSuccessStatusCode)
        {
            HandleFailedResponseCode(nameof(DeleteHurricaneCoordinate), responseForDeleteHurricaneCoordinate.StatusCode);
            return default;
        }

        var recordForDeleteHurricaneCoordinate = await responseForDeleteHurricaneCoordinate.Content
            .ReadFromJsonAsync<global::Earth.Shared.Data.Result>(cancellationToken);

        return recordForDeleteHurricaneCoordinate;
    }

    protected virtual void HandleFailedResponseCode(string method, System.Net.HttpStatusCode statusCode)
    {
        _logger.LogError($"HttpStatusCode HttpStatusCode {(int)statusCode} ({statusCode}) received in call made by {nameof(WaterRestClient)}.{method}");
    }
}

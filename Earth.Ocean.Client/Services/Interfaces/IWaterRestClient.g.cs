using Earth.Ocean.Contract.Data;

namespace Earth.Ocean.Client.Services.Interfaces;

/// <summary>
/// Because each proxy is for a dependency of a specific component on another
/// specific component the generated proxy code will refer to a factory for
/// this interface when the dependency crosses a Web API boundary and the
/// prefered means of communication is REST.
/// </summary>
public partial interface IWaterRestClient
{
    HttpClient HttpClient { get; }

    Task UpdateWaterState();

    Task UpdateWaterState(
        CancellationToken cancellationToken);

    Task<global::Earth.Shared.Data.Wrappers.Result<DolphinCoordinate>> GetDolphinCoordinate(
        int dolphinCoordinateId);

    Task<global::Earth.Shared.Data.Wrappers.Result<DolphinCoordinate>> GetDolphinCoordinate(
        int dolphinCoordinateId,
        CancellationToken cancellationToken);

    Task<global::Earth.Shared.Data.Wrappers.Result<DolphinCoordinate>> AddDolphinCoordinate(
        DolphinCoordinate dolphinCoordinate);

    Task<global::Earth.Shared.Data.Wrappers.Result<DolphinCoordinate>> AddDolphinCoordinate(
        DolphinCoordinate dolphinCoordinate,
        CancellationToken cancellationToken);

    Task<global::Earth.Shared.Data.Wrappers.Result<DolphinCoordinate>> ReplaceDolphinCoordinate(
        int dolphinCoordinateId,
        DolphinCoordinatePut dolphinCoordinatePut);

    Task<global::Earth.Shared.Data.Wrappers.Result<DolphinCoordinate>> ReplaceDolphinCoordinate(
        int dolphinCoordinateId,
        DolphinCoordinatePut dolphinCoordinatePut,
        CancellationToken cancellationToken);

    Task<global::Earth.Shared.Data.Result> DeleteDolphinCoordinate(
        int dolphinCoordinateId);

    Task<global::Earth.Shared.Data.Result> DeleteDolphinCoordinate(
        int dolphinCoordinateId,
        CancellationToken cancellationToken);

    Task<global::Earth.Shared.Data.Wrappers.Result<HurricaneCoordinate>> GetHurricaneCoordinate(
        int hurricaneCoordinateId);

    Task<global::Earth.Shared.Data.Wrappers.Result<HurricaneCoordinate>> GetHurricaneCoordinate(
        int hurricaneCoordinateId,
        CancellationToken cancellationToken);

    Task<global::Earth.Shared.Data.Wrappers.Result<HurricaneCoordinate>> AddHurricaneCoordinate(
        HurricaneCoordinate hurricaneCoordinate);

    Task<global::Earth.Shared.Data.Wrappers.Result<HurricaneCoordinate>> AddHurricaneCoordinate(
        HurricaneCoordinate hurricaneCoordinate,
        CancellationToken cancellationToken);

    Task<global::Earth.Shared.Data.Wrappers.Result<HurricaneCoordinate>> ReplaceHurricaneCoordinate(
        int hurricaneCoordinateId,
        HurricaneCoordinatePut hurricaneCoordinatePut);

    Task<global::Earth.Shared.Data.Wrappers.Result<HurricaneCoordinate>> ReplaceHurricaneCoordinate(
        int hurricaneCoordinateId,
        HurricaneCoordinatePut hurricaneCoordinatePut,
        CancellationToken cancellationToken);

    Task<global::Earth.Shared.Data.Result> DeleteHurricaneCoordinate(
        int hurricaneCoordinateId);

    Task<global::Earth.Shared.Data.Result> DeleteHurricaneCoordinate(
        int hurricaneCoordinateId,
        CancellationToken cancellationToken);
}

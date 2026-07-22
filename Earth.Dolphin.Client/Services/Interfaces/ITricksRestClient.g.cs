using Earth.Dolphin.Contract.Data;

namespace Earth.Dolphin.Client.Services.Interfaces;

/// <summary>
/// Because each proxy is for a dependency of a specific component on another
/// specific component the generated proxy code will refer to a factory for
/// this interface when the dependency crosses a Web API boundary and the
/// prefered means of communication is REST.
/// </summary>
public partial interface ITricksRestClient
{
    HttpClient HttpClient { get; }

    Task<global::Earth.Shared.Data.ReadOnlyRecords<DolphinCoordinate>> Backflip();

    Task<global::Earth.Shared.Data.ReadOnlyRecords<DolphinCoordinate>> Backflip(
        CancellationToken cancellationToken);
}

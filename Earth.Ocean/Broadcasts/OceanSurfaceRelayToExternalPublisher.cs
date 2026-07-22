using Earth.Ocean.Broadcasts.Base;
using Earth.Ocean.Broadcasts.Interfaces;

namespace Earth.Ocean.Broadcasts;

/// <summary>
/// Note that this singleton is used as needed by code inside this component to
/// update the root publisher for the broadcast.
/// </summary>
internal class OceanSurfaceRelayToExternalPublisher(
    IOceanSurfaceInternalPublisher sourcePublisher,
    IOceanSurfaceExternalPublisher externalPublisher)
    : BaseOceanSurfaceRelayToExternalPublisher(sourcePublisher, externalPublisher)
{
}

using Earth.Ocean.Broadcasts.Base;

namespace Earth.Ocean.Broadcasts;

/// <summary>
/// This singleton can be expected to have as many subscribers as the code inside this component
/// wishes to create.  For any subscribers that are transient, the consuming code should always
/// call "Unsubscribe" when finished with the subscriber.
/// </summary>
internal class OceanSurfaceInternalPublisher : BaseOceanSurfaceInternalPublisher
{
}

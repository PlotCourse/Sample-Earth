using Earth.Ocean.Broadcasts.Base;

namespace Earth.Ocean.Broadcasts;

/// <summary>
/// This singleton can be expected to have:
/// 1 subscriber per dependent component that runs inside the same Web API and acts a relay to
///     dependent component's internal publisher.  The relay maps contract record types for this
///     component to the record types used internally in the dependent component.
/// PLUS 1 more subscriber that relays messages to a SignalR hub if there's at least one dependent
///     component outside the Web API of this component, or if this broadcast was
///     configured to always publish externally.  (OceanSurfaceBroadcastRelay generated in this
///     same namespace.)
/// </summary>
internal class OceanSurfaceExternalPublisher : BaseOceanSurfaceExternalPublisher
{
}

using Earth.Ocean.Broadcasts.Base;
using Earth.Ocean.Broadcasts.Interfaces;

namespace Earth.Ocean.Broadcasts;

/// <summary>
/// Implement a sub-class of this class to receive event messages and updates to observables
/// or create some other IOceanSurfaceBroadcast that subscribes to the same publisher used
/// here.  Note that services can be automatically configured as broadcast subscribers, separately
/// from this class.
/// </summary>
internal abstract class OceanSurfaceInternalSubscriber : BaseOceanSurfaceInternalSubscriber
{
    public OceanSurfaceInternalSubscriber(IOceanSurfaceInternalPublisher publisher) : base(publisher) { }
}

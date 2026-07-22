using Earth.Ocean.Contract.Broadcasts;
using Earth.Ocean.Contract.Data;

namespace Earth.Ocean.Broadcasts.Interfaces;

public partial interface IOceanSurfaceExternalPublisher : IOceanSurfacePublisher
{
    public OceanSurfaceBroadcastObservables GetAllBroadcastObservables();
}

using Earth.Ocean.Data;

namespace Earth.Ocean.Broadcasts.Interfaces;

internal partial interface IOceanSurfaceInternalBroadcast
{
    int Temperature { get; set; }

    void NewHurricane(
        HurricaneCoordinate location);
}

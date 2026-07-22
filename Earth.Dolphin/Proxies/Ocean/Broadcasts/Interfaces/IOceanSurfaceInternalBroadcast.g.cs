using Earth.Dolphin.Data;

namespace Earth.Dolphin.Proxies.Ocean.Broadcasts.Interfaces;

internal partial interface IOceanSurfaceInternalBroadcast
{
    int Temperature { get; set; }

    void NewHurricane(
        HurricaneCoordinate location);
}

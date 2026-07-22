using Earth.Ocean.Data;

namespace Earth.Ocean.Broadcasts.Interfaces;

internal interface IOceanSurfaceRelayToExternalPublisher
{
    int Temperature { get; set; }

    void NewHurricane(
        HurricaneCoordinate location);
}

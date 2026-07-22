using Earth.Ocean.Contract.Data;

namespace Earth.Ocean.Contract.Broadcasts;

public interface IOceanSurfaceBroadcast
{
    int Temperature { get; set; }

    void NewHurricane(
        HurricaneCoordinate location);

}

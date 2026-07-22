
namespace Earth.Ocean.Data;

internal record DolphinCoordinate
{
    public int DolphinCoordinateId { get; set; }

    public int X { get; set; }

    public int Y { get; set; }

    public DolphinCoordinate(
        int dolphinCoordinateId,
        int x,
        int y)
    {
        DolphinCoordinateId = dolphinCoordinateId;
        X = x;
        Y = y;
    }
}

internal record DolphinCoordinatePut
{
    public int X { get; set; }

    public int Y { get; set; }

    public DolphinCoordinatePut(
        int x,
        int y)
    {
        X = x;
        Y = y;
    }
}

internal record HurricaneCoordinate
{
    public int HurricaneCoordinateId { get; set; }

    public int X { get; set; }

    public int Y { get; set; }

    public HurricaneCoordinate(
        int hurricaneCoordinateId,
        int x,
        int y)
    {
        HurricaneCoordinateId = hurricaneCoordinateId;
        X = x;
        Y = y;
    }
}

internal record HurricaneCoordinatePut
{
    public int X { get; set; }

    public int Y { get; set; }

    public HurricaneCoordinatePut(
        int x,
        int y)
    {
        X = x;
        Y = y;
    }
}


using System.Text.Json.Serialization;

namespace Earth.Ocean.Contract.Data;

public record DolphinCoordinate(
    [property: JsonPropertyName("dolphinCoordinateId")] int DolphinCoordinateId,
    [property: JsonPropertyName("x")] int X,
    [property: JsonPropertyName("y")] int Y);

public record DolphinCoordinatePut(
    [property: JsonPropertyName("x")] int X,
    [property: JsonPropertyName("y")] int Y);

public record HurricaneCoordinate(
    [property: JsonPropertyName("hurricaneCoordinateId")] int HurricaneCoordinateId,
    [property: JsonPropertyName("x")] int X,
    [property: JsonPropertyName("y")] int Y);

public record HurricaneCoordinatePut(
    [property: JsonPropertyName("x")] int X,
    [property: JsonPropertyName("y")] int Y);

/// <summary>
/// Used for subscribers to initialize observable values.
/// </summary>
public record OceanSurfaceBroadcastObservables(
    [property: JsonPropertyName("temperature")] int Temperature);


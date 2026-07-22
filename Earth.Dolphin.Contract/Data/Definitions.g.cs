using System.Text.Json.Serialization;

namespace Earth.Dolphin.Contract.Data;

public record DolphinCoordinate(
    [property: JsonPropertyName("dolphinCoordinateId")] int DolphinCoordinateId,
    [property: JsonPropertyName("x")] int X,
    [property: JsonPropertyName("y")] int Y);


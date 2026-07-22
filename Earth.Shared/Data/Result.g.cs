using System.Text.Json.Serialization;

namespace Earth.Shared.Data;

public record Result(
    [property: JsonPropertyName("succeeded")] bool Succeeded,
    [property: JsonPropertyName("message")] string Message);

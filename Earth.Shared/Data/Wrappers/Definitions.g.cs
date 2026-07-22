using System.Text.Json.Serialization;

namespace Earth.Shared.Data.Wrappers;

public record Result<T>(
    bool Succeeded,
    string Message,
    [property: JsonPropertyName("item")] T Item)
    : Result(Succeeded, Message);

public record ResultSet<T>(
    bool Succeeded,
    string Message,
    [property: JsonPropertyName("items")] ReadOnlyRecords<T> Items)
    : Result(Succeeded, Message);

public record PagedResult<T>(
    bool Succeeded,
    string Message,
    [property: JsonPropertyName("pageItems")] ReadOnlyRecords<T> PageItems,
    [property: JsonPropertyName("page")] int Page,
    [property: JsonPropertyName("totalPages")] int TotalPages,
    [property: JsonPropertyName("itemsPerPage")] int ItemsPerPage,
    [property: JsonPropertyName("totalItems")] int TotalItems)
    : Result(Succeeded, Message);

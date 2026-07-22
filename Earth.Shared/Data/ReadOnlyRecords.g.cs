using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Earth.Shared.Data;

/// <summary>
/// Serves as a serializable/deserializable immutable list of records since ReadOnlyCollection is
/// not deserializable.
/// </summary>
/// <typeparam name="T"></typeparam>
public class ReadOnlyRecords<T>
{
    public ReadOnlyRecords()
    {
        _records = [];
    }

    public ReadOnlyRecords(List<T> records)
    {
        _records = records;
    }

    [JsonIgnore]
    public int Count
    {
        get
        {
            lock (_lock)
            {
                return _records.Count;
            }
        }
    }

    [JsonInclude, JsonPropertyName("records")]
    protected List<T> _records { get; set; }

    protected object _lock = new object();

    [JsonIgnore]
    public ReadOnlyCollection<T> Records
    {
        get
        {
            lock (_lock)
            {
                return _records.AsReadOnly();
            }
        }
    }

    public List<T> ToList()
    {
        return new List<T>(Records);
    }
}

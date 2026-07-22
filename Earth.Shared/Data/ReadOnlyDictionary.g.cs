namespace Earth.Shared.Data;

public record DictionaryEntry<TKey, TValue>(
    TKey Key,
    TValue Value);

/// <summary>
/// Serves as a serializable/deserializable immutable list of records since ReadOnlyCollection is
/// not deserializable.
/// </summary>
/// <typeparam name="T"></typeparam>
public class ReadOnlyDictionary<TKey, TValue> : ReadOnlyRecords<DictionaryEntry<TKey, TValue>>
{
    public ReadOnlyDictionary() : base() { }

    public ReadOnlyDictionary(List<DictionaryEntry<TKey, TValue>> records) : base(records) { }
}

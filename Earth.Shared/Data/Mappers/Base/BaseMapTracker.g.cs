/*
MIT License

Copyright (c) 2025-2026 PlotCourse LLC

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
namespace Earth.Shared.Data.Mappers.Base;

/// <summary>
/// A transient data structure used during record mapping.
/// Holds objects that have map targets already created so that they will not
/// be recreated if the same object is encountered again during the same mapping
/// due to multiple or circular references.  In the case of circular references,
/// the MapTracker makes it possible for the Map extension to intentionally not
/// map any nested back-reference on contracts so that JSON can be serialized.
/// </summary>
public abstract class BaseMapTracker
{
    /// <summary>
    /// The dictionary is to optimize retrieval but the value is a list because we won't assume
    /// the object's hash code is unique.  A secondary match is therefore done based on the
    /// exact source object reference.
    /// </summary>
    protected Dictionary<object, List<KeyValuePair<object, object>>> _mapped = new();

    public virtual void AddOrReplace(object source, object target)
    {
        _mapped.GetValueOrCreate(
            source,
            out List<KeyValuePair<object, object>> matches);

        var pair = matches.FirstOrDefault(kvp => kvp.Key == source);
        if (pair.Key != null)
        {
            matches.Remove(pair);
        }

        matches.Add(new KeyValuePair<object, object>(source, target));
    }

    public virtual bool TryGetMap(object source, out object target)
    {
        if (!_mapped.ContainsKey(source))
        {
            target = null;
            return false;
        }

        var matches = _mapped[source];
        var pair = matches.FirstOrDefault(kvp => kvp.Key == source);

        target = pair.Value;
        return pair.Key != null;
    }
}

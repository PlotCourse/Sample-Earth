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
using Earth.Shared.Data.Wrappers;
using Microsoft.Extensions.Logging;

namespace Earth.Shared;

public static partial class Utils
{
    private static readonly UtilsImpl _utils = new UtilsImpl();

    /// <summary>
    /// Used to ensure that schema ID collisions do not occur based on Swashbuckle's
    /// default ID implementation.
    /// </summary>
    public static string GetSwaggerSchemaId(Type type)
    {
        return _utils.GetSwaggerSchemaId(type);
    }

    /// <summary>
    /// A simple method for retrieving a specific page of records from a larger number of items
    /// provided by an IQueryable.  Intended for use in scenarios where not a lot of data exists or
    /// queries are infrequent and don't need to be optimized.
    /// </summary>
    /// <returns></returns>
    public static Task<PagedResult<T>> GetPagedResult<T>(IQueryable<T> allItems, int page, int itemsPerPage)
    {
        return _utils.GetPagedResult<T>(allItems, page, itemsPerPage);
    }

    /// <summary>
    /// Used when additional details other than class and method are not needed.
    /// </summary>
    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "{className}.{methodName} failed.")]
    public static partial void LogException(
        this ILogger logger,
        Exception ex,
        string className,
        string methodName);

    /// <summary>
    /// Used in scenarios where LogException defined above would be sufficient but
    /// there exists a need for more than one exception to be logged in the same
    /// method so a brief message is included to distinguish them.  It's assumed the
    /// message is a hard-coded string that can be compiled in for optimal performance,
    /// not a separately built string.
    /// </summary>
    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "{className}.{methodName} failed.  {message}")]
    public static partial void LogException(
        this ILogger logger,
        Exception ex,
        string className,
        string methodName,
        string message);

    /// <summary>
    /// Get a value from a dictionary if there or if not there: create it, add it, and return the
    /// new value.
    /// </summary>
    public static bool GetValueOrCreate<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary,
        TKey key,
        out TValue value) where TValue : new()
    {
        return _utils.GetValueOrCreate<TKey, TValue>(dictionary, key, out value);
    }

    /// <summary>
    /// Get a value from a dictionary if there or if not there: create it, add it, and return the
    /// new value.
    /// </summary>
    public static bool GetValueOrCreate<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary,
        TKey key,
        Func<TValue> factory,
        out TValue value)
    {
        return _utils.GetValueOrCreate(dictionary, key, factory, out value);
    }
}

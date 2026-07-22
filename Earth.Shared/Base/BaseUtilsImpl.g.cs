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
using Earth.Shared.Data;
using Microsoft.EntityFrameworkCore;

namespace Earth.Shared.Base;

internal abstract class BaseUtilsImpl
{
    public virtual string GetSwaggerSchemaId(Type type)
    {
        if (type.IsGenericType)
        {
            int index = type.Name.IndexOf('`');
            var name = (index == -1)
                ? type.Name
                : type.Name.Substring(0, index);

            var innerTypes = type.GetGenericArguments().Select(
                t => GetSwaggerSchemaId(t));

            return $"{type.Namespace}.{name}<{string.Join(",", innerTypes)}>";
        }

        return $"{type.Namespace}.{type.Name}";
    }

    public virtual async Task<PagedResult<T>> GetPagedResult<T>(IQueryable<T> allItems, int page, int itemsPerPage)
    {
        var totalItems = await allItems.CountAsync();
        var totalPages = (totalItems / itemsPerPage)
            + ((totalItems % itemsPerPage > 0) ? 1 : 0);

        var items = (page > 0 && page <= totalPages)
            ? await allItems.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToListAsync()
            : new List<T>();

        return new PagedResult<T>(
            true,
            "",
            new ReadOnlyRecords<T>(items),
            page,
            totalPages,
            itemsPerPage,
            totalItems);
    }

    public virtual bool GetValueOrCreate<TKey, TValue>(
        IDictionary<TKey, TValue> dictionary,
        TKey key,
        out TValue value) where TValue : new()
            => GetValueOrCreate(dictionary, key, () => new(), out value);

    public virtual bool GetValueOrCreate<TKey, TValue>(
        IDictionary<TKey, TValue> dictionary,
        TKey key,
        Func<TValue> factory,
        out TValue value)
    {
        if (dictionary is null)
        {
            throw new ArgumentNullException();
        }

        dictionary.TryGetValue(key, out TValue existingValue);

        if (existingValue != null)
        {
            value = existingValue;
            return true;
        }

        value = factory();
        dictionary.Add(key, value);
        return false;
    }
}

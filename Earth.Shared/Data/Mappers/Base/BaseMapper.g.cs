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
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Reflection;

namespace Earth.Shared.Data.Mappers.Base;

internal abstract class BaseMapper
{
    protected MethodInfo _mapMethod = typeof(Extensions)
        .GetMethods(BindingFlags.Static | BindingFlags.Public)
        .Where(m => m.Name == nameof(Extensions.Map) && m.GetParameters().Length == 2)
        .First();

    protected MethodInfo _mapEnumMethod = typeof(Extensions)
        .GetMethods(BindingFlags.Static | BindingFlags.Public)
        .Where(m => m.Name == nameof(Extensions.MapEnum))
        .First();

    protected Dictionary<string, IMapRecord> _recordMappers = new();
    protected Dictionary<string, IMapEnum> _enumMappers = new();

    public virtual void RegisterRecordMappers(Assembly assembly)
    {
        RegisterMappers(assembly, "MapRecord", _recordMappers);
    }

    public virtual void RegisterEnumMappers(Assembly assembly)
    {
        RegisterMappers(assembly, "MapEnumValue", _enumMappers);
    }

    /// <summary>
    /// Maps an enum value using a specific mapper if there is one otherwise using MapEnumByName
    /// below.
    /// </summary>
    public virtual TTo MapEnum<TFrom, TTo>(TFrom source)
        where TFrom : Enum
        where TTo : Enum
    {
        var fromType = typeof(TFrom);
        var toType = typeof(TTo);

        if (fromType == toType)
        {
            return (TTo)((object)source);
        }

        var key = Key(fromType, toType);

        if (_enumMappers.ContainsKey(key))
        {
            var m = (IMapEnum<TFrom, TTo>)_enumMappers[key];
            return m.MapEnumValue(source);
        }

        return MapEnumByName<TFrom, TTo>(source);
    }

    /// <summary>
    /// This is the fallback when either a mapper for the specific enum isn't used or its used but
    /// not overridden with any special mapping functionality so it calls this method.
    /// </summary>
    public virtual TTo MapEnumByName<TFrom, TTo>(TFrom source)
        where TFrom : Enum
        where TTo : Enum
    {
        var sourceAsString = source.ToString();

        if (Enum.TryParse(typeof(TTo), sourceAsString, true, out object r))
        {
            return (TTo)r;
        }

        return default;
    }

    /// <summary>
    /// For use outside a record mapper class to map an internal type to its corresponding contract
    /// type or vice-versa.  If a special mapper exists for the types, it will be found and used.
    /// For mappings inside a mapper class use this instead:
    ///     Map<TFrom, TTo>(TFrom source, MapTracker tracker)
    /// </summary>
    public virtual TTo Map<TFrom, TTo>(TFrom source)
        where TFrom : class
        where TTo : class
    {
        return source?.Map<TFrom, TTo>(new MapTracker());
    }

    /// <summary>
    /// For use inside a record mapper class to map a nested record.
    /// </summary>
    public virtual TTo Map<TFrom, TTo>(TFrom source, MapTracker tracker)
        where TFrom : class
        where TTo : class
    {
        if (source == null)
        {
            return null;
        }

        var fromType = source.GetType();
        var toType = typeof(TTo);
        var polymorph = false;

        if (fromType != typeof(TFrom))
        {
            polymorph = true;

            var scanType = fromType;
            var ns = toType.Namespace;

            while (scanType != null)
            {
                var possible = toType.Assembly.GetType($"{ns}.{scanType.Name}", false);

                if (possible != null)
                {
                    toType = possible;
                    break;
                }

                scanType = scanType.BaseType;
            }
        }

        if (fromType == toType)
        {
            return source as TTo;
        }

        if (tracker.TryGetMap(source, out var target))
        {
            return (TTo)target;
        }

        // If a mapping tries to access this entry before the final result is created at the
        // end of this method, then it must be a circular reference since it's nested inside
        // the properties of the object being built here.  The assumption in that case is that
        // it must be mapping from a mutable object (internal; circular references allowed) to
        // an immutable object (contract; circular references are impossible), and we can only
        // handle it by setting the reference to null in the mapped object.
        tracker.AddOrReplace(source, null);

        var key = Key(fromType, toType);

        if (_recordMappers.ContainsKey(key))
        {
            if (polymorph)
            {
                var recordMapper = _recordMappers[key];
                foreach (var mapRecordMethod in recordMapper.GetType()
                    .GetMethods()
                    .Where(m => m.Name == nameof(IMapRecord<TFrom, TTo>.MapRecord)))
                {
                    var p = mapRecordMethod.GetParameters();

                    if (p.Count() == 2
                        && p[0].ParameterType == fromType
                        && p[1].ParameterType == typeof(MapTracker))
                    {
                        return (TTo)mapRecordMethod.Invoke(recordMapper, [source, tracker]);
                    }
                }

                return null;
            }

            var mapped = ((IMapRecord<TFrom, TTo>)_recordMappers[key])
                .MapRecord(source, tracker);
            tracker.AddOrReplace(source, mapped);

            return mapped;
        }

        if (fromType.IsArray || toType.IsArray) // (other should be generic ReadOnlyRecords)
        {
            var mappedArray = MapArray<TFrom, TTo>(source, tracker, fromType, toType);
            tracker.AddOrReplace(source, mappedArray);

            return mappedArray;
        }

        if (IsGenericList(fromType) && IsGenericList(toType))
        {
            var mappedMulti = MapGenericList<TFrom, TTo>(source, tracker, fromType, toType);
            tracker.AddOrReplace(source, mappedMulti);

            return mappedMulti;
        }

        if (IsGenericDictionary(fromType) && IsGenericDictionary(toType))
        {
            var mappedDictionary = MapGenericDictionary<TFrom, TTo>(source, tracker, fromType, toType);
            tracker.AddOrReplace(source, mappedDictionary);

            return mappedDictionary;
        }

        var constructed = (TTo)Construct(source, tracker, fromType, toType);
        tracker.AddOrReplace(source, constructed);

        return constructed;
    }

    protected virtual string Key(Type from, Type to)
    {
        return $"{from.GetHashCode()}_{to.GetHashCode()}";
    }

    protected virtual bool IsGenericList(Type type)
    {
        if (!type.IsGenericType)
        {
            return false;
        }

        var t = type.GetGenericTypeDefinition();

        return t == typeof(ReadOnlyRecords<>) || t == typeof(List<>);
    }

    protected TTo MapArray<TFrom, TTo>(TFrom source, MapTracker tracker, Type fromType, Type toType)
        where TFrom : class //assumption: one is array and the other is ReadOnlyRecords with inner type mappable to/from array element type
        where TTo : class
    {
        var isToArray = IsGenericList(fromType);

        IList fromList;
        Type innerFromType;
        Type innerToType;

        if (isToArray)
        {
            innerFromType = fromType.GetGenericArguments()[0];
            innerToType = toType.GetElementType();

            fromList = (IList)fromType.GetProperty("Records").GetValue(source);
        }
        else
        {
            innerFromType = fromType.GetElementType();
            innerToType = toType.GetGenericArguments()[0];

            var listType = typeof(List<>).MakeGenericType(innerFromType);
            fromList = Activator.CreateInstance(listType, source) as IList;
        }

        var toListType = typeof(List<>).MakeGenericType(innerToType);
        var mappedList = MapList(tracker, innerFromType, innerToType, fromList, toListType);

        if (isToArray)
        {
            var b = BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.IgnoreCase;
            return (TTo)toListType.InvokeMember("ToArray", b, null, mappedList, []);
        }

        var readOnlyListType = typeof(ReadOnlyRecords<>).MakeGenericType(innerToType);
        var readOnlyList = Activator.CreateInstance(readOnlyListType, mappedList);

        return (TTo)readOnlyList;
    }

    protected TTo MapGenericList<TFrom, TTo>(TFrom source, MapTracker tracker, Type fromType, Type toType)
        where TFrom : class
        where TTo : class
    {
        var innerFromType = fromType.GetGenericArguments()[0];
        var innerToType = toType.GetGenericArguments()[0];

        IList fromList;

        if (fromType.GetGenericTypeDefinition() == typeof(ReadOnlyRecords<>))
        {
            fromList = (IList)fromType.GetProperty("Records").GetValue(source);
        }
        else
        {
            fromList = (IList)source;
        }

        var listType = typeof(List<>).MakeGenericType(innerToType);
        var mappedList = MapList(tracker, innerFromType, innerToType, fromList, listType);

        if (toType.GetGenericTypeDefinition() == typeof(List<>))
        {
            return (TTo)mappedList;
        }

        var readOnlyListType = typeof(ReadOnlyRecords<>).MakeGenericType(innerToType);
        var readOnlyList = Activator.CreateInstance(readOnlyListType, mappedList);

        return (TTo)readOnlyList;
    }

    private IList MapList(MapTracker tracker, Type innerFromType, Type innerToType, IList fromList, Type toListType)
    {
        var mappedList = Activator.CreateInstance(toListType, fromList.Count) as IList;

        foreach (var sourceItem in fromList)
        {
            var destItem = NestedMap(sourceItem, innerFromType, innerToType, tracker);
            mappedList.Add(destItem);
        }

        return mappedList;
    }

    protected virtual bool IsGenericDictionary(Type type)
    {
        if (!type.IsGenericType)
        {
            return false;
        }

        var t = type.GetGenericTypeDefinition();

        return t == typeof(ReadOnlyDictionary<,>) || t == typeof(Dictionary<,>);
    }

    protected TTo MapGenericDictionary<TFrom, TTo>(TFrom source, MapTracker tracker, Type fromType, Type toType)
        where TFrom : class
        where TTo : class
    {
        var keyType = fromType.GetGenericArguments()[0];
        var innerFromType = fromType.GetGenericArguments()[1];
        var innerToType = toType.GetGenericArguments()[1];
        var addCallBindings = BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.IgnoreCase;
        var itemPropBindings = BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.DeclaredOnly;

        if (fromType.GetGenericTypeDefinition() == typeof(ReadOnlyDictionary<,>))
        {
            var dictionary = Activator.CreateInstance(toType);
            var fromList = (IList)fromType.GetProperty("Records").GetValue(source);

            if (fromList.Count > 0)
            {
                var dictionaryEntryType = fromList[0].GetType();

                foreach (var dictionaryEntry in fromList)
                {
                    var key = dictionaryEntryType.GetProperty("Key").GetValue(dictionaryEntry);
                    var sourceItem = dictionaryEntryType.GetProperty("Value").GetValue(dictionaryEntry);
                    var destItem = NestedMap(sourceItem, innerFromType, innerToType, tracker);

                    toType.InvokeMember(
                        "Add",
                        addCallBindings,
                        null,
                        dictionary,
                        [key, destItem]);
                }
            }

            return (TTo)dictionary;
        }
        else
        {
            var entryType = typeof(DictionaryEntry<,>).MakeGenericType(keyType, innerToType);
            var entryListType = typeof(List<>).MakeGenericType(entryType);
            var entryList = Activator.CreateInstance(entryListType);

            var fromKeys = (IEnumerable)fromType.GetProperty("Keys").GetValue(source);
            foreach (var key in fromKeys)
            {
                var sourceItem = fromType.InvokeMember(
                    "Item",
                    itemPropBindings,
                    null,
                    source,
                    [key]);
                var destItem = NestedMap(sourceItem, innerFromType, innerToType, tracker);

                var entry = Activator.CreateInstance(entryType, key, destItem);

                entryListType.InvokeMember(
                    "Add",
                    addCallBindings,
                    null,
                    entryList,
                    [entry]);
            }

            var readOnlyDictionary = Activator.CreateInstance(toType, entryList);

            return (TTo)readOnlyDictionary;
        }
    }

    protected virtual object Construct<TFrom>(TFrom source, MapTracker tracker, Type fromType, Type toType) where TFrom : class
    {
        var allConstructors = toType.GetConstructors();
        var constructor = allConstructors.FirstOrDefault(c => c.GetCustomAttribute(typeof(MapExtensionConstructorAttribute)) != null);

        if (constructor == null)
        {
            constructor = allConstructors[0];
        }

        var paramsInfo = constructor.GetParameters();
        var paramValues = new object[paramsInfo.Length];
        var paramValuesIndex = 0;

        foreach (var param in paramsInfo)
        {
            var fromProp = fromType.GetProperty(param.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            if (fromProp != null)
            {
                var fromVal = fromProp.GetValue(source);

                if (fromProp.PropertyType == param.ParameterType)
                {
                    paramValues[paramValuesIndex] = fromVal;
                }
                else
                {
                    paramValues[paramValuesIndex] = NestedMap(fromVal, fromProp.PropertyType, param.ParameterType, tracker);
                }
            }
            else
            {
                var paramType = param.GetType();
                paramValues[paramValuesIndex] = paramType.IsValueType
                    ? Activator.CreateInstance(paramType)
                    : null;
            }

            paramValuesIndex++;
        }

        var result = Activator.CreateInstance(toType, paramValues);
        return result;
    }

    protected virtual object NestedMap(
        object source,
        Type fromType,
        Type toType,
        MapTracker tracker)
    {
        if (fromType == toType)
        {
            return source;
        }

        if (fromType.IsEnum)
        {
            var mapEnum = _mapEnumMethod.MakeGenericMethod(fromType, toType);
            return mapEnum.Invoke(null, [source]);
        }

        var map = _mapMethod.MakeGenericMethod(fromType, toType);
        return map.Invoke(null, [source, tracker]);
    }

    protected virtual void RegisterMappers<TMapInterface>(
        Assembly assembly,
        string mapMethodName,
        Dictionary<string, TMapInterface> recordMappers)
        where TMapInterface : class
    {
        var mapRecordType = typeof(TMapInterface);
        var mappers = assembly.GetTypes()
            .Where(t => mapRecordType.IsAssignableFrom(t) && !t.IsAbstract)
            .Select(t => (TMapInterface)Activator.CreateInstance(t))
            .ToArray();

        foreach (var m in mappers)
        {
            var methodMapRecord = m.GetType().GetMethod(mapMethodName);
            var fromType = methodMapRecord.GetParameters()[0].ParameterType;
            var toType = methodMapRecord.ReturnType;
            var key = Key(fromType, toType);

            if (recordMappers.ContainsKey(key))
            {
                recordMappers.Remove(key);
            }

            recordMappers.Add(key, m);
        }
    }
}

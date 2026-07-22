namespace Earth.Shared.Data.Mappers;

// A mapper is used if 1 or more fields needed to create the
//  record are not available.  Otherwise the record is created by the
//  extension method for doing the mapping.  However, if a mapper is
//  defined for matching source and target types it will be used even
//  if not required to provide field defaults.

/// <summary>
/// An interface for easy automatic registration of all mappers in a given assembly.
/// </summary>
public interface IMapRecord { }

public interface IMapRecord<TFrom, TTo> : IMapRecord
    where TFrom : class
    where TTo : class
{
    TTo MapRecord(TFrom from, MapTracker tracker);
}

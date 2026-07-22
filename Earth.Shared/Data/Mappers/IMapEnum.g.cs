namespace Earth.Shared.Data.Mappers;

// A mapper is used if 1 or more enum values in the source enum type
//  have no match in the target enum type.  Otherwise the enum is created
//  by the extension method for doing the mapping.  However, if a mapper
//  is defined for matching source and target enum types it will be used
//  even if not required to provide defaults.

public interface IMapEnum { }

public interface IMapEnum<TFrom, TTo> : IMapEnum
    where TFrom : Enum
    where TTo : Enum
{
    TTo MapEnumValue(TFrom from);
}

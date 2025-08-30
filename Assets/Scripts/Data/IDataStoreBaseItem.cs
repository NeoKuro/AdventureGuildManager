using System;

public interface IDataStoreBaseItem<out TYPE> where TYPE : Enum
{
    public int DataStoreID { get; }
    public TYPE DataStoreType { get; }
}
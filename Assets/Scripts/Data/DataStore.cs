using System;
using System.Collections;
using System.Collections.Generic;

using Hzn.Framework;

public class DataStore<T, TQ> where T : class, IDataStoreBaseItem<TQ> where TQ : Enum
{
    private Dictionary<int, T> _primaryDataStore = new Dictionary<int, T>();
    
    public int Count => _primaryDataStore.Count;
    
    public bool TryAdd(T data)
    {
        if (_primaryDataStore.ContainsKey(data.DataStoreID))
        {
            Dbg.Error(Logging.DataStore, $"Data Store Item with ID {data.DataStoreID} already exists!");
            return false;
        }

        _primaryDataStore.Add(data.DataStoreID, data);
        return true;
    }

    public bool TryRemove(T data)
    {
        if (!_primaryDataStore.ContainsKey(data.DataStoreID))
        {
            Dbg.Error(Logging.DataStore, $"Data Store Item with ID {data.DataStoreID} does not exist!");
            return false;
        }
        
        _primaryDataStore.Remove(data.DataStoreID);
        return true;
    }
    
    public bool TryLookUp_ID(int id, out T data)
    {
        if (!_primaryDataStore.TryGetValue(id, out data))
        {
            return false;
        }
        return true;
    }

    public bool TryLookUp_Type(TQ type, out T data)
    {
        foreach (KeyValuePair<int,T> dataItems in _primaryDataStore)
        {
            if (type.Equals(dataItems.Value.DataStoreType))
            {
                data = dataItems.Value;
                return true;
            }
        }

        data = null;
        return false;
    }



    // Implement the IEnumerable<T> interface
    public IEnumerator<T> GetEnumerator()
    {
        return _primaryDataStore.Values.GetEnumerator();
    }
}
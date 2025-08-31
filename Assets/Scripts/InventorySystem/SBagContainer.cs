using System.Collections.Generic;

using Hzn.Framework;

using UnityEditorInternal;

public struct SBagContainer
{
    private int                                              _maxCapacity;
    private int                                              _currentCapacity;
    private Dictionary<EInventoryType, List<SInventoryItem>> _itemsByType;
    
    public Dictionary<EInventoryType, List<SInventoryItem>> ItemsByType => _itemsByType;
    

    private bool                 _hasChanged;
    private List<SInventoryItem> _cachedItems;

    public List<SInventoryItem> AllItems
    {
        get
        {
            if (!_hasChanged)
            {
                return _cachedItems;
            }

            foreach (List<SInventoryItem> itemList in _itemsByType.Values)
            {
                _cachedItems.AddRange(itemList);
            }

            _hasChanged = false;
            return _cachedItems;
        }
    }

    public SBagContainer(int maxCapacity)
    {
        _maxCapacity     = maxCapacity;
        _currentCapacity = 0;
        _cachedItems     = new List<SInventoryItem>();
        _itemsByType     = new Dictionary<EInventoryType, List<SInventoryItem>>();
        _hasChanged      = true;
    }

    public void ChangeCapacity(int newCapacity)
    {
        _maxCapacity = newCapacity;
    }

    public void AddItem(SInventoryItem item)
    {
        if (!_itemsByType.ContainsKey(item.InventoryType))
        {
            _itemsByType.Add(item.InventoryType, new List<SInventoryItem>());
        }

        int toAdd = _currentCapacity + item.Amount > _maxCapacity ? _maxCapacity - _currentCapacity : item.Amount;
        if (toAdd != item.Amount)
        {
            Dbg.Log(Logging.Inventory, $"Bag is full! Will only add [{toAdd.ToString()}] out of [{item.Amount.ToString()}] items");
            item.Amount = toAdd;
        }

        _currentCapacity += toAdd;

        _hasChanged = true;
        int index = _itemsByType[item.InventoryType].FindIndex(x => x.Name == item.Name);
        if (index == -1)
        {
            _itemsByType[item.InventoryType].Add(item);
            return;
        }

        _itemsByType[item.InventoryType][index].AddItem(toAdd);
    }

    public void RemoveItem(string itemName, int amount = 1)
    {
        (EInventoryType type, SInventoryItem item)? pair = null;
        foreach (KeyValuePair<EInventoryType, List<SInventoryItem>> itemList in _itemsByType)
        {
            for (int i = itemList.Value.Count - 1; i >= 0; i--)
            {
                if (itemList.Value[i].Name != itemName)
                {
                    continue;
                }

                pair = (itemList.Key, itemList.Value[i]);
                break;
            }
        }

        if (!pair.HasValue)
        {
            Dbg.Error(Logging.Inventory, $"Tried to remove item [{itemName}] from bag, but it was not found");
            return;
        }

        _hasChanged = true;
        RemoveItem(pair.Value.type, pair.Value.item, amount);
    }

    private void RemoveItem(EInventoryType type, SInventoryItem item, int amount)
    {
        if (amount > item.Amount)
        {
            Dbg.Error(Logging.Inventory, $"Tried to remove {amount} items from bag, but only {item.Amount} are available");
            _itemsByType[type].Remove(item);
            _currentCapacity -= item.Amount;
            return;
        }

        item.RemoveItem(amount);
        _currentCapacity -= amount;
    }
}
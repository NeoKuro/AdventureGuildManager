using System;

using Hzn.Framework;

using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public struct SInventoryItem : IEquatable<SInventoryItem>
{
    [SerializeField]
    private string _name;

    [SerializeField]
    private int _level;

    [SerializeField]
    private int _maxDurability;

    [SerializeField]
    private EItemRarity _rarity;

    [SerializeField]
    private int _copperValuePerItem;

    [SerializeField]
    private EInventoryType _inventoryType;

    [SerializeField]
    private EInventorySubType _inventorySubType;


    public string Name
    {
        get { return _name; }
    }

    public int Level
    {
        get { return _level; }
    }

    public int OriginalMaxDurability
    {
        get { return _maxDurability; }
    }
    public int MaxDurability
    {
        get { return OriginalMaxDurability * Mathf.FloorToInt(1 - RepairAttempts * 0.2f); }
    }

    public EItemRarity Rarity
    {
        get { return _rarity; }
    }

    public int CopperValuePerItem
    {
        get { return _copperValuePerItem; }
    }

    public EInventoryType InventoryType
    {
        get { return _inventoryType; }
    }

    public EInventorySubType InventorySubType
    {
        get { return _inventorySubType; }
    }

    public int Amount         { get; set; }
    public int Durability     { get; private set; }
    public int RepairAttempts { get; private set; }

    public void DamageItem(int damage)
    {
        Durability = Mathf.Clamp(Durability - damage, 0, MaxDurability);
    }

    public void RepairItem()
    {
        RepairAttempts++;
        Durability = MaxDurability;
    }

    public SInventoryItem AddItem(int amount)
    {
        Amount += amount;
        return this;
    }

    public SInventoryItem RemoveItem(int amount)
    {
        Amount = Mathf.Clamp(Amount - amount, 0, int.MaxValue);
        return this;
    }

    public bool Equals(SInventoryItem other)
    {
        return _name == other._name && _copperValuePerItem == other._copperValuePerItem && _inventoryType == other._inventoryType;
    }

    public override bool Equals(object obj)
    {
        return obj is SInventoryItem other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_name, _copperValuePerItem, (int)_inventoryType);
    }
}
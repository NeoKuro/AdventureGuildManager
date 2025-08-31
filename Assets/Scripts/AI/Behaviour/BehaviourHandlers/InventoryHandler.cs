using System.Collections.Generic;

public class InventoryHandler
{
    protected SCurrencyWallet _wallet;
    protected SBagContainer   _bag;

    public InventoryHandler()
    {
        _wallet        = new SCurrencyWallet(0);
        _bag           = new SBagContainer(8);
    }

    public void UpgradeBag(int newCapacity)
    {
        _bag.ChangeCapacity(newCapacity);
    }

    public Dictionary<EInventoryType, List<SInventoryItem>> GetInventory()
    {
        return _bag.ItemsByType;
    }
}
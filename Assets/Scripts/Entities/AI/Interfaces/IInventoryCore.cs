using System.Collections.Generic;

public interface IInventoryCore
{
    public SAdventurerData AdventurerData     { get; }

    Dictionary<EInventoryType, List<SInventoryItem>> GetInventory();

    float EvaluateConfidenceNormalized();

    float EvaluateWealth();
}
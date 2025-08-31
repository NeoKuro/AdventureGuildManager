using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class AIInventoryHandler : InventoryHandler
{
    private const int MAX_ACCESSORIES_COUNT = 3;
    private const int MAX_CONSUMABLES_COUNT = 3;
    private const int MAX_CONFIDENCE_SCORE  = 18;   // Can add in a way to auto-calc later. But can use this to get a normalised value
    
    private int            _confidence;
    private int            _wealthExpectation;
    private IInventoryCore _inventoryCore;

    public AIInventoryHandler(IInventoryCore inventoryCore) : base()
    {
        _inventoryCore = inventoryCore;
    }

    /// <summary>
    /// Max score (not-auto calculated yet) is 18
    /// Use <see cref="CalculateConfidenceNormalized"/> to get a normalized value between 0 and 1 of how 'good' the
    /// adventurer's confidence is
    /// TODO Confidence is relative to the task, in this case a quest most likely. So we should pass that in in the future to compare
    ///     IE if they've taken a low ranked quest (or same ranked but low-difficulty-rated) then the confidence score shouldn't apply
    ///     as much (IE threshold can be lower, where '0.6f' is the normal average confidence needed for this AI to start, an easier
    ///     quest might have a '0.3f' threshold. This is all estimated by the user as well based on what they or staff put on the quest)
    /// </summary>
    /// <returns>Returns raw Confidence score (out of 18)</returns>
    public int CalculateConfidence()
    {
        int confidence  = 0;
        SAdventurerData data = _inventoryCore.AdventurerData;
        
        // MAX SCORE = 1
        //  Only count 1 weapon (may need to account for dual wielding later?)
        int weaponScore = 0;
        foreach (SInventoryItem item in _bag.ItemsByType[EInventoryType.Weapon])
        {
            if (!CalculateItemScore(item, data.Level, out int thisScore))
            {
                continue;
            }
            
            if (thisScore <= weaponScore)
            {
                continue;
            }
            
            weaponScore = thisScore;
        }

        // MAX SCORE = 5
        //  Head, Body, Legs, Gloves, Boots
        Dictionary<EInventorySubType, int> armourSubTypeScores = new Dictionary<EInventorySubType, int>();
        foreach (SInventoryItem armourItem in _bag.ItemsByType[EInventoryType.Armor])
        {
            if (!CalculateItemScore(armourItem, data.Level, out int thisScore))
            {
                continue;
            }

            armourSubTypeScores.TryAdd(armourItem.InventorySubType, 0);
            
            if (thisScore <= armourSubTypeScores[armourItem.InventorySubType])
            {
                continue;
            }
            
            armourSubTypeScores[armourItem.InventorySubType] = thisScore;
        }
        
        // MAX SCORE = 3
        //  3x accessories
        List<int> accessorySubTypeScores = new List<int>();
        foreach (SInventoryItem accessory in _bag.ItemsByType[EInventoryType.Accessory])
        {
            if (!CalculateItemScore(accessory, data.Level, out int thisScore))
            {
                continue;
            }

            if (accessorySubTypeScores.Count < MAX_ACCESSORIES_COUNT)
            {
                accessorySubTypeScores.Add(thisScore);
                continue;
            }

            for (int i = 0; i < accessorySubTypeScores.Count; i++)
            {
                if (thisScore <= accessorySubTypeScores[i])
                {
                    continue;
                }
                
                accessorySubTypeScores[i] = thisScore;
            }
        }
        
        // MAX SCORE = 9
        //  3x Health, 3x Mana, 3x Energy
        Dictionary<EInventorySubType, List<int>> consumablesScores = new Dictionary<EInventorySubType, List<int>>();
        foreach (SInventoryItem consumable in _bag.ItemsByType[EInventoryType.Consumable])
        {
            if (!CalculateItemScore(consumable, data.Level, out int thisScore))
            {
                continue;
            }

            consumablesScores.TryAdd(consumable.InventorySubType, new List<int>());

            if (consumablesScores[consumable.InventorySubType].Count < MAX_CONSUMABLES_COUNT)
            {
                consumablesScores[consumable.InventorySubType].Add(thisScore);
                continue;
            }

            for (int i = 0; i < consumablesScores[consumable.InventorySubType].Count; i++)
            {
                if (thisScore <= consumablesScores[consumable.InventorySubType][i])
                {
                    continue;
                }

                consumablesScores[consumable.InventorySubType][i] = thisScore;
            }
        }

        confidence  += weaponScore;
        confidence  += armourSubTypeScores.Values.Sum();
        confidence  += accessorySubTypeScores.Sum();
        confidence  += consumablesScores.Values.Sum(list => list.Sum());
        _confidence =  confidence;
        return _confidence;
    }

    /// <summary>
    /// Calculates the confidence of the AI of their equipment, and normalizes it against the <see cref="MAX_CONFIDENCE_SCORE"/>
    /// NOTE: In future can also take into account the Ai's stats, skills and level (see what the Quest creator looks like)
    /// TODO Confidence is relative to the task, in this case a quest most likely. So we should pass that in in the future to compare
    ///     IE if they've taken a low ranked quest (or same ranked but low-difficulty-rated) then the confidence score shouldn't apply
    ///     as much (IE threshold can be lower, where '0.6f' is the normal average confidence needed for this AI to start, an easier
    ///     quest might have a '0.3f' threshold. This is all estimated by the user as well based on what they or staff put on the quest)
    /// </summary>
    /// <returns>Value between 1 and 0 of how good the confidence is</returns>
    public float CalculateConfidenceNormalized()
    {
        float conf = (float)_confidence / MAX_CONFIDENCE_SCORE;
        return conf;
    }

    private bool CalculateItemScore(SInventoryItem item, int advLevel, out int score)
    {
        score = 0;
        if (item.Level > advLevel)
        {
            return false;
        }

        int levelScore = item.Level / advLevel;

        int dur      = item.Durability / item.MaxDurability;
        int maxDur   = item.MaxDurability / item.OriginalMaxDurability;
        int durScore = dur * maxDur;

        // The item can boost it's score, but the maximum score will ALWAYS be 1.0
        //  UNCOMMON is considered the 'standard' quality/rarity
        //  COMMON is sub-par, and reduces score slightly
        //  But all other rarities boost the score
        // LOGIC - Better rarity typically means better gear/values
        float rarityScore = item.Rarity.GetRarityScoringValue();
        score = Mathf.FloorToInt(Mathf.Clamp01(levelScore * durScore * rarityScore));
        return true;
    }

    public void UpdateWealthExpectation()
    {
        // TODO: Implement Economic adaptation. Figure out how much a room costs on average in the town and use that
        //  to determine the wealth expectation (this will penalise players who try setting their prices to ludicrous levels)

        // For now, wealth expectation is based on their assigned rank. 
        // Purpose is to maintain a wallet balance around this value. If they go higher frequently, great they are happier
        //  Otherwise, if they struggle to reach this value they will become unhappy and consider leaving

        // LONG-TERM: Tie this into the above-mentioned living costs (rooms, food etc)
        switch (_inventoryCore.AdventurerData.AssignedRank)
        {
            default:
            case EAdventurerRank.None:
            case EAdventurerRank.F:
                _wealthExpectation = 50; // 50 coppers   
                break;
            case EAdventurerRank.E:
                _wealthExpectation = 200; // 2 Silvers
                break;
            case EAdventurerRank.D:
                _wealthExpectation = 1000; // 10 Silvers
                break;
            case EAdventurerRank.C:
                _wealthExpectation = 5000; // 50 Silvers
                break;
            case EAdventurerRank.B:
                _wealthExpectation = 10000; // 1 Gold
                break;
            case EAdventurerRank.A:
                _wealthExpectation = 50000; // 5 Gold
                break;
            case EAdventurerRank.S:
                _wealthExpectation = 100000; // 10 Gold
                break;
        }
    }
}
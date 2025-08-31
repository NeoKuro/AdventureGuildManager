using System;
using System.Collections.Generic;

using UnityEngine;

public class StatsHandler
{
    private IAICore _aiCore;
    private float        _confidence = 1f; // Not included in the stats struct because this is more on-demand than persistent data
    private SEntityStats _coreStats  = new SEntityStats();

    public StatsHandler(IAICore aiCore)
    {
        _aiCore = aiCore;
    }
    
    /// <summary>
    /// Evaluate all stats and rate the priority from 0 -> 1 to be used to decide what to do
    /// IE if is bored (max priority) but is also hungry, will drop bored and raise hungry priority
    ///
    /// Will scale the priorities of each stat exponentially if a higher priority stat is lower. For example,
    /// if Health is not 100% but still high (90%) it will reduce the priorities of all other stats by a small amount
    /// But if Health continues to drop to 30% the other priorities will be affected much more.
    /// </summary>
    /// <returns></returns>
    public Dictionary<EEntityPriorities, float> EvaluateStats()
    {
        Dictionary<EEntityPriorities, float> priorities = new Dictionary<EEntityPriorities, float>();
        int                                  statsCount = System.Enum.GetValues(typeof(EEntityStats)).Length;

        // Calculate base priorities
        priorities[EEntityPriorities.Health]  = 1 - _coreStats.HealthPercent;
        priorities[EEntityPriorities.Energy]  = 1 - _coreStats.EnergyPercent;
        priorities[EEntityPriorities.Social]  = 1 - _coreStats.Social;
        priorities[EEntityPriorities.Boredom] = 1 - _coreStats.Boredom;
        priorities[EEntityPriorities.Hunger]  = 1 - _coreStats.Hunger;
        priorities[EEntityPriorities.Thirst]  = 1 - _coreStats.Thirst;

        // Adjust priorities using exponential scaling
        for (int i = 0; i < statsCount; i++)
        {
            EEntityPriorities currentStat     = (EEntityPriorities)i;
            float             currentPriority = priorities[currentStat];

            // Calculate the exponential scaling factor based on the current stat's priority
            float curveExponent = MathF.Exp(currentPriority * 1.25f);
            float exponentialFactor =
                Mathf.Exp(-currentPriority *
                          Mathf.Max(curveExponent - 1f, 0f)); // Adjust the multiplier (5f) to control the curve steepness
            exponentialFactor = Mathf.Clamp(exponentialFactor, 0.3f, 1f);
            // Apply exponential scaling to all lower-priority stats
            for (int j = i + 1; j < statsCount; j++)
            {
                EEntityPriorities lowerPriorityStat = (EEntityPriorities)j;
                priorities[lowerPriorityStat] *= exponentialFactor;
            }
        }

        return priorities;
    }

    private float CalculateConfidence()
    {
        return 1f;
    }

    private float CalculateMoneyStat()
    {
        return 1f;
    }
}
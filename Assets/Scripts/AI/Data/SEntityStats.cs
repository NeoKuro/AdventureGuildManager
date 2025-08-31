using System.Collections.Generic;

using UnityEngine;

public struct SEntityStats
{
    private const float SOCIAL_DEPLETION_RATE  = 0.0015f;  // At 50 FPS, will take ~11 minutes to deplete
    private const float BOREDOM_DEPLETION_RATE = 0.00125f; // At 50 FPS, will take ~13.3 minutes to deplete
    private const float HUNGER_DEPLETION_RATE  = 0.0015f;  // At 50 FPS, will take ~11 minutes to deplete
    private const float THIRST_DEPLETION_RATE  = 0.002f;   // At 50 FPS, will take ~8.3 minutes to deplete
    private const float ENERGY_DEPLETION_RATE  = 0.001f;   // At 50 FPS, will take ~17 minutes to deplete


    private Dictionary<EEntityStats, float> stats;

    public float HealthPercent
    {
        get { return stats[EEntityStats.Health] / _maxHp; }
    }

    public float HealthVal
    {
        get { return stats[EEntityStats.Health]; }
    }

    public float EnergyPercent
    {
        get { return stats[EEntityStats.Energy] / _maxEnergy; }
    }

    public float EnergyVal
    {
        get { return stats[EEntityStats.Energy]; }
    }

    public float Social
    {
        get { return stats[EEntityStats.Social]; }
    }

    public float Boredom
    {
        get { return stats[EEntityStats.Boredom]; }
    }

    public float Hunger
    {
        get { return stats[EEntityStats.Hunger]; }
    }

    public float Thirst
    {
        get { return stats[EEntityStats.Thirst]; }
    }


    private readonly float _maxHp;
    private readonly float _maxEnergy;

    public SEntityStats(float sHp, float sEnergy)
    {
        _maxHp     = sHp;
        _maxEnergy = sEnergy;
        stats      = new Dictionary<EEntityStats, float>();
        stats.Add(EEntityStats.Health, sHp);
        stats.Add(EEntityStats.Energy, sEnergy);
        stats.Add(EEntityStats.Social, 1);
        stats.Add(EEntityStats.Boredom, 0);
        stats.Add(EEntityStats.Hunger, 0);
        stats.Add(EEntityStats.Thirst, 0);
    }

    public void UpdateStat(EEntityStats stat, float value)
    {
        float maxVal = stat == EEntityStats.Health ? _maxHp : stat == EEntityStats.Energy ? _maxEnergy : 1;
        stats[stat] = Mathf.Clamp(stats[stat] + value, 0, maxVal);
    }

    public void TickStats()
    {
        // TODO: LONG-TERM: Add personalities (some are less social so soc stat drops slower, others are less prone to boredom etc)
        foreach (KeyValuePair<EEntityStats, float> stat in stats)
        {
            if (stat.Key == EEntityStats.Health)
            {
                continue;
            }

            switch (stat.Key)
            {
                case EEntityStats.Social:
                    stats[stat.Key] -= Time.fixedDeltaTime * SOCIAL_DEPLETION_RATE;
                    break;
                case EEntityStats.Boredom:
                    stats[stat.Key] -= Time.fixedDeltaTime * BOREDOM_DEPLETION_RATE;
                    break;
                case EEntityStats.Hunger:
                    stats[stat.Key] -= Time.fixedDeltaTime * HUNGER_DEPLETION_RATE;
                    break;
                case EEntityStats.Thirst:
                    stats[stat.Key] -= Time.fixedDeltaTime * THIRST_DEPLETION_RATE;
                    break;
                case EEntityStats.Energy:
                    stats[stat.Key] -= Time.fixedDeltaTime * ENERGY_DEPLETION_RATE;
                    break;
            }
        }
    }
}
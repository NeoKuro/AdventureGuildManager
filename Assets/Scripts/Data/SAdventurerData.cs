using System;
using System.Collections.Generic;

using Hzn.Framework;

using UnityEngine;

using Random = UnityEngine.Random;

[Serializable]
public struct SAdventurerData : IEquatable<SAdventurerData>
{
    public string Name
    {
        get { return _name; }
    }

    public ECharacterRace Race
    {
        get
        {
            return _race;
        }
    }

    public EAdventurerClass Class
    {
        get { return _class; }
    }

    public int Level
    {
        get { return _level; }
    }

    public string Nickname
    {
        get { return _nickname; }
    }

    public EAdventurerRank AssignedRank
    {
        get { return _assignedRank; }
    }

    public SAdventurerStats Stats
    {
        get { return _stats; }
    }

    private string           _name;
    private ECharacterRace   _race;
    private int              _level;
    private EAdventurerRank  _assignedRank;
    private EAdventurerRank  _hiddenRank;
    private string           _nickname;
    private SAdventurerStats _stats;
    private EAdventurerClass _class;

    /// <summary>
    /// Use this to create a NEW adventurer rather than loading an existing adventurer
    /// </summary>
    /// <param name="name"></param>
    public SAdventurerData(string name, string nickname = "")
    {
        _name         = name;
        _nickname     = nickname;
        _class        = EAdventurerClass.Warrior;
        _race         = ECharacterRace.Human;
        _level        = 1;
        _assignedRank = EAdventurerRank.None;
        _hiddenRank   = EAdventurerRank.F;
        _stats        = new SAdventurerStats();
        SetRandomRace();
        GenerateStatsForNewAdventurer();
    }

    /// <summary>
    /// Use this to load an adventurer from file (IE from JSON)
    /// </summary>
    public SAdventurerData(string name,
                           EAdventurerClass adventurerClass,
                           ECharacterRace race,
                           int level,
                           string nickname,
                           EAdventurerRank assignedRank,
                           EAdventurerRank hiddenRank,
                           SAdventurerStats stats)
    {
        _name         = name;
        _class        = adventurerClass;
        _race         = race;
        _level        = level;
        _nickname     = nickname;
        _assignedRank = assignedRank;
        _hiddenRank   = hiddenRank;
        _stats        = stats;
    }

    public void SetAssignedRank(EAdventurerRank rank)
    {
        _assignedRank = rank;
    }

    /// <summary>
    /// Determines how close / accurately the player assigned the adventurer's rank
    /// Right now just calculates the delta (closer to 0 is GOOD)
    /// In future can account for skillpoint totals, and apply worse modifiers if over-estimated
    /// rank as it is more dangerous (whereas underestimating is just 'boring' for adventurerer etc)
    /// </summary>
    /// <returns>Delta from hidden rank to assigned rank. A value as close to 0 as possible is best</returns>
    public int AssessRank()
    {
        if (_hiddenRank == _assignedRank)
        {
            return 0;
        }

        int hiddenRankIndex   = (int)_hiddenRank;
        int assignedRankIndex = (int)_assignedRank;
        return Mathf.Abs(hiddenRankIndex - assignedRankIndex);
    }

    private void GenerateStatsForNewAdventurer()
    {
        int maxStatPoints = 100;
        int rankIndex     = Random.Range(0, 7);
        switch (rankIndex)
        {
            case 6:
                _level        = Random.Range(95, 101);
                _hiddenRank   = EAdventurerRank.S;
                maxStatPoints = 10000;
                break;
            case 5:
                _level        = Random.Range(85, 95);
                _hiddenRank   = EAdventurerRank.A;
                maxStatPoints = 3200;
                break;
            case 4:
                _level        = Random.Range(70, 85);
                _hiddenRank   = EAdventurerRank.B;
                maxStatPoints = 1600;
                break;
            case 3:
                _level        = Random.Range(50, 70);
                _hiddenRank   = EAdventurerRank.C;
                maxStatPoints = 800;
                break;
            case 2:
                _level        = Random.Range(30, 50);
                _hiddenRank   = EAdventurerRank.D;
                maxStatPoints = 400;
                break;
            case 1:
                _level        = Random.Range(10, 30);
                _hiddenRank   = EAdventurerRank.E;
                maxStatPoints = 200;
                break;
            case 0:
            default:
                _level        = Random.Range(1, 10);
                _hiddenRank   = EAdventurerRank.F;
                maxStatPoints = 100;
                break;
        }

        int healthPoints   = Random.Range(1, maxStatPoints / 3);
        int speedPoints    = Random.Range(1, maxStatPoints / 3);
        int magicPoints    = Random.Range(1, maxStatPoints / 3);
        int strengthPoints = Random.Range(1, maxStatPoints / 3);

        int totalPoints = healthPoints + speedPoints + magicPoints + strengthPoints;
        if (totalPoints > maxStatPoints)
        {
            int surplus   = totalPoints - maxStatPoints;
            int randomCap = Random.Range(0, 4);
            switch (randomCap)
            {
                default:
                case 0:
                    healthPoints -= surplus;
                    break;
                case 1:
                    speedPoints -= surplus;
                    break;
                case 2:
                    magicPoints -= surplus;
                    break;
                case 3:
                    strengthPoints -= surplus;
                    break;
            }
        }

        _stats = new SAdventurerStats(healthPoints,
                                      speedPoints,
                                      magicPoints,
                                      strengthPoints);
        _class = _stats.GetNewClassFromHighestStat();
    }

    public bool Equals(SAdventurerData other)
    {
        return _name == other._name && _level == other._level && _assignedRank == other._assignedRank && _hiddenRank == other._hiddenRank && _nickname == other._nickname && _stats.Equals(other._stats) && _class == other._class;
    }

    public override bool Equals(object obj)
    {
        return obj is SAdventurerData other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_name, _level, _assignedRank, _hiddenRank, _nickname, _stats, (int)_class);
    }

    private void SetRandomRace()
    {
        int randVal = Random.Range(0, Enum.GetValues(typeof(ECharacterRace)).Length);
        _race = (ECharacterRace)randVal;
    }
}

[Serializable]
public struct SAdventurerStats
{
    public int Health
    {
        get { return Mathf.Max(_statValues[EAdventurerStats.Health], 10); }
    }

    public int Speed
    {
        get { return Mathf.Max(_statValues[EAdventurerStats.Speed], 10); }
    }

    public int Magic
    {
        get { return Mathf.Max(_statValues[EAdventurerStats.Magic], 10); }
    }

    public int Strength
    {
        get { return Mathf.Max(_statValues[EAdventurerStats.Strength], 10); }
    }

    [SerializeField]
    private List<SAdventurerStatContainer> _stats;

    private int _totalPoints;

    [NonSerialized]
    private Dictionary<EAdventurerStats, int> _statValues;

    public SAdventurerStats(int health, int speed, int magic, int strength)
    {
        _stats = new List<SAdventurerStatContainer>();
        _stats.Add(new SAdventurerStatContainer { Stat = EAdventurerStats.Health, Value   = health });
        _stats.Add(new SAdventurerStatContainer { Stat = EAdventurerStats.Speed, Value    = speed });
        _stats.Add(new SAdventurerStatContainer { Stat = EAdventurerStats.Magic, Value    = magic });
        _stats.Add(new SAdventurerStatContainer { Stat = EAdventurerStats.Strength, Value = strength });

        _statValues = new Dictionary<EAdventurerStats, int>();
        _statValues.Add(EAdventurerStats.Health, health);
        _statValues.Add(EAdventurerStats.Speed, speed);
        _statValues.Add(EAdventurerStats.Magic, magic);
        _statValues.Add(EAdventurerStats.Strength, strength);

        _totalPoints = health + speed + magic + strength;
    }

    public SAdventurerStatContainer GetStat(EAdventurerStats stat)
    {
        foreach (SAdventurerStatContainer container in _stats)
        {
            if (container.Stat == stat)
            {
                return container;
            }
        }

        Dbg.Error(Logging.Entities, $"Failed to find stat: {stat}");
        return default;
    }

    public EAdventurerClass GetNewClassFromHighestStat()
    {
        EAdventurerStats highestStat = EAdventurerStats.Health;
        foreach (KeyValuePair<EAdventurerStats, int> stat in _statValues)
        {
            if (stat.Value > _statValues[highestStat])
            {
                highestStat = stat.Key;
            }
        }

        switch (highestStat)
        {
            case EAdventurerStats.Health:
                return EAdventurerClass.Ranger;
            case EAdventurerStats.Speed:
                return EAdventurerClass.Rogue;
            case EAdventurerStats.Magic:
                int index = Random.Range(0, 2);
                return index == 0 ? EAdventurerClass.Mage : EAdventurerClass.Healer;
            case EAdventurerStats.Strength:
                return EAdventurerClass.Warrior;
            default:
                Dbg.Error(Logging.Entities, $"Failed to determine new class from highest stat: {highestStat}");
                return EAdventurerClass.Warrior;
        }
    }

    public void LevelUp(int skillPointsToDistribute, EAdventurerStats[] skillPointsPriorityIndices)
    {
        bool distributeEvenly = false;
        if (skillPointsPriorityIndices.Length != 4)
        {
            Dbg.Error(Logging.Entities,
                      $"Skill points priority indices must be of length 4 to define the order of skill points distribution. Will distribute evenly. We got: [{skillPointsPriorityIndices.Length}]");
            distributeEvenly = true;
        }

        if (distributeEvenly)
        {
            skillPointsToDistribute                /= 4;
            _statValues[EAdventurerStats.Health]   += skillPointsToDistribute;
            _statValues[EAdventurerStats.Strength] += skillPointsToDistribute;
            _statValues[EAdventurerStats.Magic]    += skillPointsToDistribute;
            _statValues[EAdventurerStats.Speed]    += skillPointsToDistribute;
            return;
        }

        for (int i = 0; i < skillPointsPriorityIndices.Length; i++)
        {
            float multiplier = 1f;
            switch (i)
            {
                case 0:
                    multiplier = 0.4f;
                    break;
                case 1:
                case 2:
                    multiplier = 0.25f;
                    break;
                case 3:
                    multiplier = 0.1f;
                    break;
            }
            
            switch (skillPointsPriorityIndices[i])
            {
                case EAdventurerStats.Health:
                    _statValues[EAdventurerStats.Health] += (int)(skillPointsToDistribute * multiplier);
                    break;
                case EAdventurerStats.Magic:
                    _statValues[EAdventurerStats.Magic] += (int)(skillPointsToDistribute * multiplier);
                    break;
                case EAdventurerStats.Strength:
                    _statValues[EAdventurerStats.Strength] += (int)(skillPointsToDistribute * multiplier);
                    break;
                case EAdventurerStats.Speed:
                    _statValues[EAdventurerStats.Speed] += (int)(skillPointsToDistribute * multiplier);
                    break;
                default:
                    Dbg.Error(Logging.Entities,
                              $"Invalid skill point priority index: {skillPointsPriorityIndices[i]}, distributing evenly");
                    skillPointsToDistribute                /= 4;
                    _statValues[EAdventurerStats.Health]   += skillPointsToDistribute;
                    _statValues[EAdventurerStats.Strength] += skillPointsToDistribute;
                    _statValues[EAdventurerStats.Magic]    += skillPointsToDistribute;
                    _statValues[EAdventurerStats.Speed]    += skillPointsToDistribute;
                    return;
            }
        }

        _totalPoints = 0;
        foreach (KeyValuePair<EAdventurerStats, int> stats in _statValues)
        {
            _totalPoints += stats.Value;
        }
    }

    /// <summary>
    /// Can be used later to see how close a user actually was to guessing the right rank
    /// Especially in cases where the RNG somehow grants higher skill points than expected
    /// </summary>
    /// <param name="rank">Assigned rank given to adventurer</param>
    /// <returns>How close the skillpoints matches expectations to the upper and lower bounds.
    /// Can be negative, if negative, it indicates the skillpoints don't match what is expected
    /// for this rank (IE -0.1 upper, means the skillpoints actually cross into the rank above etc)
    /// </returns>
    public (float upperThresholdPercentage, float lowerThresholdPercentage) CalculateProximityToLevelThresholds(string rank)
    {
        float targetMaxPoints = 100;
        float targetMinPoints = 0;
        switch (rank)
        {
            case "S":
                targetMaxPoints = 10000;
                targetMinPoints = 3200;
                break;
            case "A":
                targetMaxPoints = 3200;
                targetMinPoints = 1600;
                break;
            case "B":
                targetMaxPoints = 1600;
                targetMinPoints = 800;
                break;
            case "C":
                targetMaxPoints = 800;
                targetMinPoints = 400;
                break;
            case "D":
                targetMaxPoints = 400;
                targetMinPoints = 200;
                break;
            case "E":
                targetMaxPoints = 200;
                targetMinPoints = 100;
                break;
            default:
            case "F":
                targetMaxPoints = 100;
                targetMinPoints = 0;
                break;
        }

        float maxDelta = targetMaxPoints - _totalPoints;
        float minDelta = targetMinPoints - _totalPoints;
        return (maxDelta / targetMaxPoints, minDelta / targetMinPoints);
    }

}

public struct SAdventurerStatContainer
{
    public EAdventurerStats Stat;
    public int              Value;
}
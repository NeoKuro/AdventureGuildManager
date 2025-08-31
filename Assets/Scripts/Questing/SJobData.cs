using System.Collections.Generic;
using System.Linq;

public struct SJobData
{
    public readonly int                  JobID;
    public readonly string               Name;
    public readonly string               Issuer;
    public readonly string               Location;
    public readonly string               Description;
    public readonly int                  Level;
    public readonly EJobType             JobType;
    public readonly EAdventurerRank      Rank;
    public readonly SGuildDateTime       Deadline;
    public readonly SCurrencyWallet      MoneyReward;
    public readonly List<SInventoryItem> ItemReward;

    public readonly List<STaskObjective> TaskObjectives;

    public readonly string RewardString;

    public SJobData(int jobID,
                    string name,
                    string issuer,
                    string location,
                    int level,
                    EAdventurerRank rank,
                    SGuildDateTime deadline,
                    SCurrencyWallet moneyReward,
                    List<SInventoryItem> itemReward,
                    List<STaskObjective> taskObjectives) : this()
    {
        JobID    = jobID;
        Name     = name;
        Issuer   = issuer;
        Location = location;
        Level    = level;


        Rank           = rank;
        Deadline       = deadline;
        MoneyReward    = moneyReward;
        ItemReward     = itemReward;
        TaskObjectives = taskObjectives;

        // Combine all job types using bitwise OR
        JobType = TaskObjectives?.Aggregate(EJobType.None,
                                            (current, objective) => current | objective.JobType)
                  ?? EJobType.None;

        Description = GenerateDescription();
        RewardString = GenerateRewardString();
    }

    private string GenerateDescription()
    {
        string desc = "To complete this job, you must:\n\n";
        int    idx  = 1;
        foreach (STaskObjective obj in TaskObjectives)
        {
            desc += $"{idx.ToString()}. {obj.GetDescription()}\n\n";
        }

        desc += $"Within {Deadline.ToFormattedString()}";
        return desc;
    }

    private string GenerateRewardString()
    {
        string rew = "Rewards for successfully completing this job:\n\n";
        rew += $"Money: {MoneyReward.ToString()}\n";
        rew += $"Items:";
        foreach (SInventoryItem item in ItemReward)
        {
            rew += $"\n- {item.Name} {((item.Amount > 1) ? $"x{item.Amount}" : string.Empty)}";
        }
        
        return rew;
    }
}
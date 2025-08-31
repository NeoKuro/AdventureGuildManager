public struct STaskObjective
{
    public readonly EJobType         JobType; // Type of task (Hunt/kill, gather something, explore an area etc)
    public readonly IObjectiveTarget Target;  // Target of the task - this can be an Entity (monster to hunt, NPC to find etc) or an item
    public readonly int              Amount;  // Amount of the target required to complete the task (Hunt 5 monsters, find 100 herbs etc)
    
    // Optional specific location of task (hunt orcs in the Forest, orcs in the desert won't count), can also be start location
    public readonly string Location;


    public STaskObjective(EJobType jobType, IObjectiveTarget target, int amount, string location)
    {
        JobType  = jobType;
        Target   = target;
        Amount   = amount;
        Location = location;
    }

    public string GetDescription()
    {
        string description = "";

        switch (JobType)
        {
            // Multiple targets possible
            case EJobType.Hunting:
            case EJobType.Gathering:
                description = $"<b>{JobType.GetJobVerb()}</b> {Amount.ToString()} {Target.GetName(Amount)}";
                if (!string.IsNullOrEmpty(Location))
                {
                    description += $" in <b>{Location}</b>";
                }

                break;
            // Can only do these one time (per quest - could re-explore etc)
            case EJobType.Exploring:
            case EJobType.Investigating:
                description = $"<b>{JobType.GetJobVerb()}</b> the {Target.GetName()}";
                break;
            case EJobType.Escorting:
                description = $"<b>{JobType.GetJobVerb()}</b> {Target.GetName()} from {Location} to {Target.GetName()}";
                break;
            case EJobType.Rescuing:
                description = $"<b>{JobType.GetJobVerb()}</b> {Target.GetName()}. Their last known location was {Location}";
                break;
            // The amount value reflects how long to patrol for (in minutes)
            case EJobType.Patrolling:
                description = $"<b>{JobType.GetJobVerb()}</b> for {Amount.ToString()} minutes around the {Target.GetName()}";
                break;
        }

        return description;
    }
}
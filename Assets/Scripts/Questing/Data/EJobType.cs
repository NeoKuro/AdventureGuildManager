using System;

[Flags]
public enum EJobType
{
    None          = 0,
    Gathering     = 1 << 0,
    Hunting       = 1 << 1,
    Patrolling    = 1 << 2,
    Escorting     = 1 << 3,
    Exploring     = 1 << 4,
    Investigating = 1 << 5,
    Rescuing      = 1 << 6,

}

public static class EJobTypeExtensions
{
    public static bool HasFlag(this EJobType jobType, EJobType flag)
    {
        return (jobType & flag) == flag;
    }

    public static string GetJobVerb(this EJobType jobType)
    {
        switch (jobType)
        {
            default:
            case EJobType.None:
                return "None";
            case EJobType.Gathering:
                return "Gather";
            case EJobType.Hunting:
                return "Kill";
            case EJobType.Patrolling:
                return "Patrol";
            case EJobType.Escorting:
                return "Escort";
            case EJobType.Exploring:
                return "Explore";
            case EJobType.Investigating:
                return "Investigate";
            case EJobType.Rescuing:
                return "Rescue";
        }
    }
}
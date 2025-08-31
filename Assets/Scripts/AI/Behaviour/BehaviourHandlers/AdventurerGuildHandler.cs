using Hzn.Framework;

using UnityEngine;

public class AdventurerGuildHandler : GuildHandler
{
    // Try to limit to at most 1 day between jobs. The closer the time becomes to this, the more urgent the need
    private static SGuildDateTime MaxTimeBetweenJobs = new SGuildDateTime(0, 0, 1, 0, 0);
    
    private Adventurer_AIEntity _adventurer;

    private SJobData?      _currentJob;
    private SGuildDateTime _lastJobCompletion;

    public SJobData? CurrentJob
    {
        get { return _currentJob; }
    }

    public bool HasActiveJob
    {
        get { return _currentJob.HasValue; }
    }

    public AdventurerGuildHandler(Adventurer_AIEntity adventurer) : base(adventurer)
    {
        _adventurer        = adventurer;
        _lastJobCompletion = SGuildDateTime.Zero;
    }

    public void SetCurrentJob(SJobData job)
    {
        _currentJob = job;
    }

    public float JobPriority()
    {
        SGuildDateTime currentTime = TimeManager.CurrentTime;

        // Get time since last job in minutes
        int timeSinceLastJob = (currentTime.GetTimeAsDurationInMinutes() - _lastJobCompletion.GetTimeAsDurationInMinutes());

        // Get max time between jobs in minutes
        int maxTimeInMinutes = MaxTimeBetweenJobs.GetTimeAsDurationInMinutes();

        // Normalize and clamp the value between 0.2f and 1
        //  This will always give a 20% need for jobs as this is a quest/adventurer game after all
        //  TODO: Confirm this is appropriate levels. Its good to not have them questing 24/7, and the AI spending time relaxing in town
        //      or the guild hall. But also don't want them hanging around TOO much
        return Mathf.Clamp((float)timeSinceLastJob / maxTimeInMinutes, 0.2f, 1f);
    }

    public bool ShouldGetJob()
    {
        float wealth = _adventurer.EvaluateWealth();
        return wealth <= 0.5f || JobPriority() > 0.5f;
    }

    public override void EvaluateSatisfaction()
    {
        Dbg.LogVerbose(Log.AI, $"NOT YET IMPLEMENTED - Adventurerer Satisfaction [{nameof(EvaluateSatisfaction)}]. See Notes.");
        // TODO: For adventurers, we should evaluate if all their needs are met;
        //  - Wealth expectations being met?
        //  - Services expectations met (for their rank - remember higher ranks = expect more services like bars, rooms, training etc)
        //  - Able to get enough jobs frequently enough?
        //  - Jobs challenging enough?
        //  - Accurately listed jobs (listing them with wrong difficulty is either dangerous or boring)
        //  - Guild prices competitive?

        // This can impact multiple things. First off, it will influence how often someone is to pick the Guild services over public 
        //  services. If the player is charging 10000 gold for a piece of bread, the AI will instead go to the town for food.
        // Long-term, it will impact whether the adventurer will get fed up and leave the guild/town entirely
        //
        // TODO: Evaluate once per day? - Will need to implement a TimeManager and register into the 'Start Day' callback or something.


        Dbg.LogOnce(Log.Manager,
                       $"NOTE: create a new TimeManager and register into the 'Start Day' callback and provide other callbacks");

        _satisfaction = 1f;
    }
}
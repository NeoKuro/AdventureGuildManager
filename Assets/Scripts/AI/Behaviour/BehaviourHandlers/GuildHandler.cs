public abstract class GuildHandler
{
    protected IGuildCore _guildCore;
    protected float      _satisfaction;

    public float Satisfaction
    {
        get { return _satisfaction; }
    }
    
    public GuildHandler(IGuildCore guildCore)
    {
        _guildCore    = guildCore;
        _satisfaction = 1f;
        TimeManager.RegisterNewDayEvent(EvaluateSatisfaction);
    }
    
    public abstract void EvaluateSatisfaction();
}
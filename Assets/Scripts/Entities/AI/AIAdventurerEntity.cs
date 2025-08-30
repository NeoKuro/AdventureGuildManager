using System;

public class AIAdventurerEntity : AdventurerEntity, IAICore
{
    private Action _onArriveAtGuildCallback;

    public void InitialiseBehaviour()
    {
        
    }

    public void UpdateBehaviour()
    {
        
    }

    public void RegisterArrivedAtGuildCallback(Action onArrive)
    {
        _onArriveAtGuildCallback += onArrive;
    }

    public void UnregisterArrivedAtGuildCallback(Action onArrive)
    {
        _onArriveAtGuildCallback -= onArrive;
    }
}
using System;

public class StaffEntity : Entity, IAICore
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
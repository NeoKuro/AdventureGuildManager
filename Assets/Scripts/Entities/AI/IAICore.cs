using System;

public interface IAICore
{
    void InitialiseBehaviour();
    
    void UpdateBehaviour();

    void RegisterArrivedAtGuildCallback(Action onArrive);
    
    void UnregisterArrivedAtGuildCallback(Action onArrive);
}
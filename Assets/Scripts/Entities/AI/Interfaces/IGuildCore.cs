using System;

public interface IGuildCore : IAICore
{
    void RegisterArrivedAtGuildCallback(Action onArrive);

    void UnregisterArrivedAtGuildCallback(Action onArrive);

    float GetGuildSatisfaction();

}
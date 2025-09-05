using System;

public interface IGuildCore : IAICore
{
    void RegisterArrivedAtGuildCallback(Action onArrive);

    void UnregisterArrivedAtGuildCallback(Action onArrive);

    void RegisterFrontOfQueueCallback(Action onFront);
    void UnregisterFrontOfQueueCallback(Action onFront);

    float GetGuildSatisfaction();

}
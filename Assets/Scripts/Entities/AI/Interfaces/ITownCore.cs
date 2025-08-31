using System;

public interface ITownCore : IAICore
{
    /// <summary>
    /// Set destination to go to a location in town. It should choose the closest of
    /// the 'town' waypoints (IE Gatehouse, plaza, market etc)
    /// </summary>
    void SetDestinationToTown();

    /// <summary>
    /// Set destination to the NPC's home location in town.
    /// This could be; a house, flat, rented tavern/Inn room, or even a rented room in the guild
    /// </summary>
    void SetDestinationToHome();
    
    void RegisterArriveAtTownDestinationCallback(Action onArrive);
    void UnregisterArriveAtTownDestinationCallback(Action onArrive);
}
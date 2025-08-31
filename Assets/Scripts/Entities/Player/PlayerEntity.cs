using Hzn.Framework;

/// <summary>
/// This class will be used to differentiate between the LOCAL player
/// And networked / remote / other players
/// </summary>
public abstract class PlayerEntity : Entity
{
    // TODO: 
    //      - Add Networking support
    //      - Create general functions (movement) that can be implemented by either LocalPlayerEntity or NetworkedPlayerEntity etc



    public virtual void SpawnAdventurer(bool isNewAdventurer)
    {
        Dbg.Warn(Log.Player, $"TODO: Implement loading player-adventurer data / stats etc");
    }
}
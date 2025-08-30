/// <summary>
///  Every player is an adventurer (or can be if they want)
/// This class will be used to differentiate between the LOCAL player
/// And networked / remote / other players
/// </summary>
public abstract class PlayerEntity : AdventurerEntity
{
    // TODO: 
    //      - Add Networking support
    //      - Create general functions (movement) that can be implemented by either LocalPlayerEntity or NetworkedPlayerEntity etc
}
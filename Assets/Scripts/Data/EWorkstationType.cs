public enum EWorkstationType
{
    Reception                 = 0, // If we want to COMBINE receptionists - handle new + existing members
    Questboard                = 1, // Activated to add pre-made quests to the quest board
    Reception_NewMembers      = 2, // If we want to split out Receptionists - mostly to simplify the resulting UI
    Reception_ExistentMembers = 3, // If we want to split out receptionists - mostly to simplify the resulting UI
    Reception_Host            = 4, // Activated to handle check-in / lodging requests
    Shop                      = 5, // If shops are added, can serve as checkout / purchases
    Bartender                 = 6, // Serve guests drinks / food
    Cook                      = 7, // Discover recipes, cook food / prepare recipes
    GuildMasterDesk           = 8, // Used to create quests en-masse to add to the quest board later - other MGMT stuff too?
}
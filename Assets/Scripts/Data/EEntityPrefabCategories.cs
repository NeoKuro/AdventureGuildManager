using System;

[Flags]
public enum EEntityPrefabCategories
{
    None       = 0,
    Player     = 1 << 1,
    Adventurer = 1 << 2,
    Staff      = 1 << 3,
    NPC        = 1 << 4,
    Monster    = 1 << 5
}
using System;

    [Flags]
    public enum EEntityType
    {
        None = 0,
        Player = 1 << 0,
        Adventurer = 1<< 1,
        Staff = 1<<2,
        NPC = 1<<3,
    }
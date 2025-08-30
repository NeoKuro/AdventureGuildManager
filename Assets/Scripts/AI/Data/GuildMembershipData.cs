using System;

[Serializable]
public struct GuildMembershipData
{
    public bool            IsGuildMember  { get; private set; }
    public GuildDateTime   JoinDate       { get; private set; }
    public SAdventurerData AdventurerData { get; private set; }

    // Used for interacting with a NEW adventurer, who is not yet a member of the guild
    public GuildMembershipData(bool isGuildMember = false)
    {
        IsGuildMember  = false;
        JoinDate       = new GuildDateTime(0, 0, 0, 0, 0);
        AdventurerData = default(SAdventurerData);
    }

    // Load existing guild member data
    // NOTE: Long-term can be used to track if a member of another guild (in which case 'bool' won't suffice)
    //  Could introduce poaching/transfer mechanics 
    public GuildMembershipData(bool isGuildMember, GuildDateTime joinDate, SAdventurerData adventurerData)
    {
        IsGuildMember  = isGuildMember;
        JoinDate       = joinDate;
        AdventurerData = adventurerData;
    }
}
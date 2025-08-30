using System;

using Hzn.Framework;

public class AdventurerEntity : Entity
{
    public SAdventurerData AdventurerData{ get; private set; }

    public virtual void SpawnAdventurer(bool isNewAdventurer)
    {
        if (isNewAdventurer)
        {
            AdventurerData = new SAdventurerData(AdventurerManager.GenerateNewAdventurerName());
            return;
        }
    }

    public virtual void OnAdventurerSigned(SAdventurerData newAdventurerData)
    {
        Dbg.Log(Logging.Entities, $"Adventurer [{AdventurerData.Name}] has been Registered!");
        Dbg.Log(Logging.Entities, $"TODO: Move them on / find a new task (see notes)");
     
        // We update the data - as this will include the appropriate ranking etc
        AdventurerData = newAdventurerData;
        
        // TODO : Post-sign events;
        // --> Have a happy / emoji icon above their heads - this can be used to indicate how accurate the player was
        // Post-sign options;
        //  1. Go to quest baord and find quests - Get work 
        //  2. Use other services / facilities in the guild
        //  3. Socialise (away from the reception table - prevent overcrowding) with other adventurers
        //  4. Leave building
    }

    public virtual void OnAdventurerRejected()
    {
        Dbg.Log(Logging.Entities, $"Adventurer [{AdventurerData.Name}] has been REJECTED!");
        Dbg.Log(Logging.Entities, $"TODO: Move them on / find a new task (see notes)");

        // TODO : Post-reject events;
        // --> Have a happy / emoji icon above their heads - this can be used to indicate how accurate the player was
        // Post-reject options;
        //  MVP:
        //      -- Leave building
        //
        // FUTURE:
        //  1. Use PUBLIC services / facilities in the guild
        //      -- Player can restrict services/facilities based on rank with guild (IE if they get too popular can prioritise members etc)
        //      -- If adventurers cannot access enough facilities, even because busy, that will impact mood
        //      -- 2 things to track? "Does the guild offer it? Can I access it? If the first is yes then minimum 50% happiness?)
        //  2. Socialise (away from the reception table - prevent overcrowding) with other adventurers
        //      -- Could improve skills (so they can come back later and re-apply) -- "TRAINING"
        //      -- Could spread gossip (then you should kick them out)
        //      -- Could chat with others but not affect mood?
        
    }
}
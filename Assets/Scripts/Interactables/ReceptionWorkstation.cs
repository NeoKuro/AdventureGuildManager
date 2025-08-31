using Hzn.Framework;

using UnityEngine;

public class ReceptionWorkstation : WorkstationInteractable
{
    [SerializeField]
    private GameObject _formPosition;

    [SerializeField]
    private NewAdventurerFormController _newMemberForm;

    private Adventurer_AIEntity activeAdventurerAI;
    
    public void OnSignNewAdventurer(SAdventurerData newAdventurerData)
    {
        AdventurerManager.AddAdventurer(newAdventurerData);
        activeAdventurerAI.OnAdventurerSigned(newAdventurerData);
        Dbg.Log(Logging.Interactables, "NEXT UP -- Need to process to the NEXT adventurer in queue");
    }

    public void OnRejectCurrentAdventurer()
    {
        activeAdventurerAI.OnAdventurerRejected();
        Dbg.Log(Logging.Interactables, "ReceptionWorkstation: Rejected current adventurer");
        Dbg.Log(Logging.Interactables, "NEXT UP -- Need to process to the NEXT adventurer in queue");
    }

    protected override void BeginInteract()
    {
        // TODO: 
        //      MVP: Turn off / on the correct UI ('NewMember UI' or 'ExistingMember UI')
        //      MMP: Have basic animation of the parchment going from under the desk to on top of it
        //          PLUS any 'extras' (quills, Magic-testing orbs etc)


        // Dbg.Log(Logging.Interactables, "ReceptionWorkstation: Interacting" );
        // Activate the 'Receptionist UI'
        // Process the next adventurer (if any) in queue;
        //      -- If no adventurer in queue, then do nothing, but wait for an adventurer to show up
        //      -- If an adventurer is in queue, then process the adventurer
        //          -- If the adventurer is a new member, then process the new member form
        //          -- If the adventurer is an existing member, then process the existing member form

        Dbg.Error(Logging.Interactables, "ReceptionWorkstation: Interacting -- TODO: IMPLEMENT BEGIN INTERACT");
        if (_adventurersQueue.Count == 0)
        {
            Dbg.Log(Logging.Interactables, "ReceptionWorkstation: No adventurers in queue");
            return;
        }

        activeAdventurerAI = _adventurersQueue.Dequeue();
        _newMemberForm.StartNewAdventurerForm(activeAdventurerAI.AdventurerData);
        _newMemberForm.RegisterReceptionWorkstation(this);
    }

    protected override void ProcessInteract()
    {
        // Receptionists;
        //      -- Sign up / evaluate / process new members:
        //          --> Evaluate skills / level 
        //              ----> Game in backend will assign a random rank to a member
        //              ----> Skills will then be generated to match
        //              ----> Player must then evaluate rank + the game scores based on how close
        //              ----> Actions - player provides item for skill evaluation
        //                      member already lists their name, age + class
        //          --> Assign a Ranking
        //          --> Provide guild card  -- Purely fluff(?) - players can design own guild cards -- NOT MVP
        //      -- Process existing members
        //          --> Process quest turn-ins
        //              ----> MVP: just acknowledge + pay adventurers the reward listed
        //              ----> FUTURE: Parties/adventurers may offer additional items the guild can buy (IE rare items, ingredients etc)
        //          --> Process Party Requests
        //              ----> MVP: no 'smart' party forming, just random members (one-time deals as well)
        //
        //          ---------- NOT MVP ------------
        //
        //          --> Random enquiries ('busy work')  -- NOT MVP
        //              ----> Asking about locations, info, quests etc
        //              ----> Some enquiries could be interact and based on if the player / staff knows the answer
        //                      IE "Where can I find X monsters?" - Player will only know if they've found them, Staff by knowledge/skill
        //              ----> WHY? - Can increase happiness of members, improve guild rep, draw in new members etc
        //              ----> FUTURE FEATURE NOT MVP
        //          --> Process Lodging requests -- NOT MVP
        //              ----> Listen to offer / haggle with member to set a per-night price (include extras like food etc)
        //              ----> Assign an empty room on offer accepted
        //              ----> FUTURE FEATURE NOT MVP    
        //          --> Process quest cancellations -- NOT MVP - on failure the quest just ends
        //              ----> Quests go back into the pool on fail for the GM (or clerk) to re-add to board
        //              ----> As logn as there is time left in the quest? (see GDD how we decided on the time thing)
        //              ----> FUTURE FEATURE NOT MVP
    }

    protected override void InitialiseInteractable()
    {
        Dbg.Log(Logging.Interactables, "ReceptionWorkstation: Initialised");
        RegisterCommands();
    }
    
    #if UNITY_EDITOR
    private void RegisterCommands()
    {
        Console.AddCommand("reception.queue-adventurer", Dbg_QueueAdventurer, "Queue an adventurer for the receptionist");
    }

    private void Dbg_QueueAdventurer(string[] args)
    {
        Adventurer_AIEntity adventurerAI = null;
        if(!EntityManager.Get()?.TryCreateAdventurerEntity(out adventurerAI) ?? false)
        {
            Dbg.Error(Logging.Interactables, "Failed to create adventurer entity");
            return;
        }
        
        Interact(adventurerAI);
    }
    #endif
}
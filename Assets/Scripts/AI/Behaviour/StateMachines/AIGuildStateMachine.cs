using Hzn.Framework;

using UnityEngine;

public class AIGuildStateMachine : StateMachine
{
    private IGuildCore _adventurer;
    private Adventurer_AIEntity _adventurerEntity;

    private State VisitGuild;
    private State UseServices;
    private State RegisterGuild;
    private State ChooseQuest;
    private State TurnInQuest;
    private State FinishGuildVisit;
    
    private GuildServiceController _guildServiceController;

    public AIGuildStateMachine(IGuildCore aiAdventurer, bool tickable = false) : base(nameof(AIGuildStateMachine), tickable)
    {
        _adventurer = aiAdventurer;
        _adventurerEntity = _adventurer as Adventurer_AIEntity;

        CreateAndAddState(ref VisitGuild, nameof(VisitGuild));
        CreateAndAddState(ref UseServices, nameof(UseServices));
        CreateAndAddState(ref RegisterGuild, nameof(RegisterGuild));
        CreateAndAddState(ref ChooseQuest, nameof(ChooseQuest));
        CreateAndAddState(ref TurnInQuest, nameof(TurnInQuest));
        CreateAndAddState(ref FinishGuildVisit, nameof(FinishGuildVisit));

        // ENTRY STATE
        //  Visit Guild is used to get the AI to go to the guild
        //  On arrival (Callback?) - SM will decide what to do from there
        //  Any/All adventurers can have this State (Registered or not - IE Unregistered may go here to register etc)
        VisitGuild
            .DeclareStartState()
            .OnEntry(OnEnterVisitGuild)           // Move to the guild and decide what to do on arrival
            .AllowTransitionTo(UseServices)       // Use services on arrival
            .AllowTransitionTo(RegisterGuild)     // Register guild on arrival
            .AllowTransitionTo(ChooseQuest)       // Choose quest on arrival
            .AllowTransitionTo(TurnInQuest)       // Turn in quest on arrival
            .AllowTransitionTo(FinishGuildVisit); // Finish Visit (Likely failed to resolve a valid State in this SM)

        // INTERMEDIARY
        //  One or more stats are low enough to prioritise using services
        //      - Food/Drink = Use Bar
        //      - Entertainment = Use Bar, Training etc
        //      - Low Energy = Use Bed (room for the night? Long-term rental?)
        //      - High money/Bored = Use Shops (Potions, Armour, Weapons, Consumables?)
        //  We decide on ARRIVAL at the guild on what to do, rather than before (the extra state above) because as the
        //      AI travels to the guild, their stats may drop to the point where they pick Services over another thing
        //      For instance: Head to guild originally to get a quest, by time they arrive, they may be hungry so
        //          Instead use Services (Food) instead BEFORE they go get a quest - bit extra income for player etc
        //  NOTE: Will be able to enter this state even if that service is not provided. If it isn't provided (or available)
        //      Then the AI will get upset depending on how 'critical' that service is considered. Whilst in that state
        //      the AI can play various animations, wander around, and have thought bubbles telling the player they're upset
        UseServices
            .OnEntry(OnEnterUseServices)          // Choose a service and go to it
            .OnUpdate(UpdateUseServices)          // Wait for services to be used to decide what to do next
            .AllowTransitionTo(ChooseQuest)       // Choose quest after using services
            .AllowTransitionTo(TurnInQuest)       // Turn in quest after using services to unwind
            .AllowTransitionTo(FinishGuildVisit); // Finish visit after using services (IE start quest, go home)

        // INTERMEDIARY
        //  The AI must register with the guild before they can use any services. Be it food, drink, quests etc.
        //      - This is a one-time process (FOR NOW). Whilst members can leave, generally they disapepar from the game
        //  Exiting this state does NOT guarantee that an AI has successfully registered with the guild. If the player
        //      rejects the AI, or in some way fails registration (too expensive? too low skilled or something else?) then
        //      they will exit this state as a non-member still.
        //
        //  LONG-TERM: FAILED MEMBERSHIP:
        //      Why fail?
        //          Cost too high - (does player set fee?)
        //          Player can just choose?
        //          Add tricks (fraud/cheating) on applications which must be caught by player for extra challenge or face penalty?
        //
        //  LONG-TERM: FORMER-MEMBERS:
        //      Add in a feature to bring back former members? (Transfers, poaching, or just naturally returning former members?)
        //      May return after player has improved services (Member may have levelled up high enough and player couldn't offer 
        //      the right services, so they left. But then later return if player meets demands - could also grow ranks/levels
        //      Nice little continuity there and feeling that the world is alive and persistent)
        //
        //  LONG TERM: UNLOCKABLE-SERVICES:
        //      Maybe make it unlockable where the player can set it to where certain services are open to all (Research?)
        //      WHY? More customers = more income, non-members can still generate $$$. But higher demand = lower availability for
        //      registered members, which may impact ratings etc (so players may opt to restrict access - which could also increase
        //      chance for a non-member to register slightly? IE if they really need to sleep and are a few % away from agreeing, it
        //      may push them to actually register because of premium/restricted (wanted) services?
        RegisterGuild
            .OnEntry(OnEnterRegisterGuild)        // Move to registration area and queue up if needed
            .OnUpdate(UpdateRegisterGuild)        // Wait for registration to complete
            .AllowTransitionTo(UseServices)       // Successfully registered, relax and use services
            .AllowTransitionTo(ChooseQuest)       // Successfully registered, choose a quest
            .AllowTransitionTo(FinishGuildVisit); // Finish visit (either successfully registered or did not)

        // INTERMEDIARY
        //  Once registered (or arrived, if already a member) the AI may go to the quest board to pick an available quest
        //      - Quests are available to all members, but there are rank restrictions set by the player on quest/task EVAL
        //      - Only ONE quest per AI (or party) for the time being
        //  Only members can take quests (this will never change)
        //  Members may then choose to either use services (chill/refresh before going on the quest), or leave to start it
        //      go home, go shopping or something else around town.
        //
        // LONG TERM: MULTIPLE-QUESTS:
        //      Allow multiple quests per adventurer? Player/Guild must approve the quest selection anyway, so up to them to
        //      set 'rules'? 
        //      Too many quests, and AI may get overwhelmed and fail/miss some quest deadlines
        //      May also try completing multiple at once, and increase risk to themselves / party
        //      Leave it to the player/guild to decide? (once staff implemented - player can set guild rules IE No more than 2 quests etc)
        ChooseQuest
            .OnEntry(OnEnterChooseQuest)          // Move to quest board to choose quest
            .OnUpdate(UpdateChooseQuest)          // Wait for quest to be selected
            .AllowTransitionTo(UseServices)       // Got a quest, now relax before starting it
            .AllowTransitionTo(FinishGuildVisit); // Got a quest, finish visit (IE to start quest)

        // INTERMEDIARY
        //  Once a quest is completed, the AI must turn it in to the player/Guild to receive the rewards
        //      At this point the player/guild ALSO receive their cut as well
        //  Only members can take quests (this will never change)
        //  Members may then choose to use services, pick another quest, or go home/elsewhere in the town afterwards
        TurnInQuest
            .OnEntry(OnEnterTurnInQuest)          // Move to the reception area to turn in quest
            .OnUpdate(UpdateTurnInQuest)          // Wait to turn in quest
            .AllowTransitionTo(UseServices)       // Once finished, use services to relax/spend reward etc
            .AllowTransitionTo(ChooseQuest)       // Once finished, choose another quest
            .AllowTransitionTo(FinishGuildVisit); // Once finished, finish the visit and go elsewhere

        // EXIT
        //  Guild visit is complete. 
        //      Could mean registration was unsuccessful so the AI is leaving
        //      Could be the registration was successful but AI didn't want to do anything else (IE late so going home etc)
        //      AI could have turned in a quest or finished using services and want to go home, or shopping etc
        //      Or could have obtained a new quest and intends to start the quest (or not) 
        //  Decision on what to do next is handled by the parent state machine. All this SM cares about is they are done with the guild
        FinishGuildVisit
            .DeclareExitState()
            .OnEntry(OnEnterFinishGuildVisit);



        if (_adventurerEntity == null)
        {
            Dbg.Error(Log.AI, "AI Adventurer is not an AIEntity");
            ChangeState(FinishGuildVisit);
            return;
        }

        ChangeState(VisitGuild);
    }

    private void OnEnterVisitGuild()
    {
        // Set a destination to the guild
        // Register 'On Arrived at guild' callback

        Dbg.LogVerbose(Log.AI, "ENTERED: Visit Guild State");
        Dbg.Log(Log.AI, "TODO: Set Destination to Guild + start moving there");
        _adventurer.RegisterArrivedAtGuildCallback(OnArrivedAtGuild);
    }

    private void OnArrivedAtGuild()
    {
        Dbg.LogVerbose(Log.AI, "TODO: Refactor to IGuildBehaviour or something instead of dumping everything in 'IAICore'?");
        // TODO: Turn this into an 'IGuildBehaviour' interface? IAICore should be very basic (init, update etc)
        //  so we can use it for most/all AI. But this is specific to anyone visiting the guild
        //      - current/registered adventurers
        //      - prospective guild members (looking to register - still adventurers though)
        //      - Staff (though unlikely to use this State Machine - do we want staff to visit IE on days off or something?)
        //      - MAYBE Townsfolk? - Only if we allow players to open services to non-members (locals may use some services?)
        _adventurer.UnregisterArrivedAtGuildCallback(OnArrivedAtGuild);

        Dbg.LogVerbose(Log.AI, "CALLBACK: Has arrived at Guild hall. Deciding next state");
        // Check what the AI will want to do
        //  Look at what the current states are;
        //      - Is a guild member?
        //          - Food/Drink/Sleep/Entertainment lacking = Use Services (bar, bed etc)
        //          - Has a quest?
        //              - Is it complete?
        //                  - YES = Turn it in
        //                  - NO  = Use Services? (cancel quest? - should this be a thing?)
        //              - No = Choose Quest
        //          - NO = Try Register at guild
        //  Should be no case where the AI enters, just to leave
        //      Above all else, if something like that were to be possible, they should just default to using a service
    }

    private void OnEnterUseServices()
    {
        Dbg.LogVerbose(Log.AI, "ENTERED: Use Services state");
        // TODO: Make this a state machine
        //  - Pick which service to use - depending on availability in the guild
        // Decide which service to use
        //      Should this be a new State machine to make it more easily extendable as we get more services/features added?
        //      Make the services generic (IGuildService.UseService()) for example?
        //          - Could have the guild services use enums to track what stats are improved, and compare it with what 
        //          stats caused the AI to enter this state?
        //          - 'Shops' could be --Money, but ++entertainment or something? (But --Money is a good thing as the AI wants to spend it?)
    }

    private void UpdateUseServices(float deltaT)
    {
        // Monitor the progress of the stat / time at the service
        //  Eating could be a timed service (IE to consume the food) for example, after which the AI will re-evaluate
        //  Shopping etc might just be an 'on Arrive' at the shop
        //      - Could be it's own State Machine (all this assumes we have vendors/shops inside the guild hall instead of just the town)
    }

    private void OnEnterRegisterGuild()
    {
        Dbg.LogVerbose(Log.AI, "ENTERED: Register Guild state");

        _guildServiceController = GuildManager.GetReceptionBalanced();
        ReceptionController receptionController = _guildServiceController as ReceptionController;
        if (receptionController == null)
        {
            Dbg.Error(Logging.Guild, "Could not get ReceptionController from GuildManager");
            ChangeState(FinishGuildVisit);
            return;
        }

        // This may happen if the queue has reached it's absolute maximum
        //  TODO: FUTURE-FEATURE: Instead of leaving immediately, enter a 'Wandering state' - for now just leave
        //      early. Maybe keep it like this as its probably going to mean the queue(s) are quite big anyway
        if (!receptionController.TryEnqueueEntity(_adventurerEntity, out Vector3 destination))
        {
            Dbg.LogVerbose(Logging.Guild, "Could not enqueue AIEntity to ReceptionController, leaving the guild");
            ChangeState(FinishGuildVisit);
            return;
        }
        _adventurer.SetDestination(destination);
        _adventurer.RegisterFrontOfQueueCallback(OnRegisterAtGuild);
        // Queue up at the reception area (if space + needed) - Is possible for queue to be too long and 'no more room' missing
        //      out on potential members - if that's the case they should wander around for a moment then if still unable to, leave?
        //      - Maybe just leave (if we want the 'wander' behaviour will need that as a state)
        // Once served, they will trigger the Registration flow.
        //  After which, they either use other services, or leave (if rejected, they can only leave - for now)
    }

    private void UpdateRegisterGuild(float deltaT)
    {
        Dbg.LogOnce(Log.AI, "TODO: Turn this into a Callback?");
        // Wait around for the result / player's decision on their registration
        //  TODO - This could be a CALLBACK? (Instead of adding such callback to IAICore, add it to an IAdventurerCore? since only Adv. will
        //      be registering at the guild?)
    }

    private void OnEnterChooseQuest()
    {
        Dbg.Log(Log.AI,
                "TODO: Turn 'ChooseQuest' into a State Machine? Can more easily add additional behaviours like go back to reception to get approval etc");
        // Set Destination + move over to the quest board
        // TODO: Turn 'ChooseQuest' into a state machine, to accommodate future behaviours like 'Pick quest' -> 'Go to reception' -> 'Get approval' etc
    }

    private void UpdateChooseQuest(float deltaT)
    {
        // Idle around a bit (Idle anim?) then pick a quest they are eligible for
        //  Turn this into it's own State machine? - Especially if after they've 'chosen' a quest, they have to go to reception to
        //  get it approved or something?
        //  
        //  Also, turn this into a CALLBACK (even if MVP is to not have them get the quest approved by reception?)
    }

    private void OnEnterTurnInQuest()
    {
        // Set destination + move to the reception (or queue up)
        //  - If unable to queue, wander around / idle until able to
        //      - Could transition to Use Service or something instead of idling, and as soon as able to queue they switch back?
        //  - If too long a wait, this should greatly impact their satisfaction (want to get paid etc - especially if it means the difference
        //      between handing in a quest on time, and failing because there was a long queue -- Maybe add extra penalties if this happens?)
        //      -- IE: Still hands it in, reward won't be paid by questgiver, but player can still choose to pay adventurer out of own pocket
        //          This would greatly reduce the impact of long queues / delays (still got paid - but still had to wait a while etc)
        //          Or if doesn't want to, or just can't afford to, then it will be a HUGE satisfaction/reputation hit (all that work for nothing)
        //      - Encourages the player to be better at time management, and to get staff if too many members to handle on their own
        //      - Do not want a 'meta' where you can sign everyone, but there's no real issue if you've oversubscribed etc 
        
        // Turn into it's own State machine because of the Idle/wandering pattern?
    }

    private void UpdateTurnInQuest(float deltaT)
    {
        // Progressing through the queue
        //  -- TODO: Setup as CALLBACK (IE 'OnQueueProgressed' etc) to set new destination/move up one etc?
        // Once being served, wait around for the result
        //  -- Evaluate what they got (IE did the player mismanage the money and was unable to pay them properly etc?)
        //  -- TODO: Setup callacbk to process the Turn-in result?
    }

    private void OnEnterFinishGuildVisit()
    {
        // Exit this state machine
        //  -- Successfully concluded business at the Guild Hall, so now do something else
        //      -- Could go hunting/training, do the quest they picked up, go home, go shopping, socialise in town etc
        CompleteAndPause();
    }

    private void OnRegisterAtGuild()
    {
        ReceptionController reception = _guildServiceController as ReceptionController;
        if (reception == null)
        {
            Dbg.Error(Logging.Guild, $"Could not get ReceptionController from [{nameof(_guildServiceController)}]");
            ChangeState(FinishGuildVisit);
            return;
        }
        
        reception.RegisterWithGuild(_adventurerEntity);
    }

    private void OnTurnInQuest()
    {
        ReceptionController reception = _guildServiceController as ReceptionController;
        if (reception == null)
        {
            Dbg.Error(Logging.Guild, $"Could not get ReceptionController from [{nameof(_guildServiceController)}]");
            ChangeState(FinishGuildVisit);
            return;
        }
        reception.TurnInQuest(_adventurerEntity);
    }

    private void OnAcceptQuest()
    {
        ReceptionController reception = _guildServiceController as ReceptionController;
        if (reception == null)
        {
            Dbg.Error(Logging.Guild, $"Could not get ReceptionController from [{nameof(_guildServiceController)}]");
            ChangeState(FinishGuildVisit);
            return;
        }
        reception.AcceptQuest(_adventurerEntity);
    }
}
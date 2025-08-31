using Hzn.Framework;

using UnityEngine;

public class AdventurerStateMachine : AICoreStateMachine
{
    private StateMachine        _wildernessStateMachine; // SM to handle behaviours/activities outside the town
    private StateMachine        _townStateMachine;       // SM to handle behaviours/activities around the town 
    private AIGuildStateMachine _guildStateMachine;      // SM to handle behaviours/activities around the guild

    private State VisitGuild;      // State controlling visiting the Guild (_guildStateMachine dependency)
    private State VisitWilderness; // State controlling visiting the wilderness (_wildernessStateMachine dependency)
    private State VisitTown;       // State controlling visiting the town (_townStateMachine dependency)

    public AdventurerStateMachine(bool tickable = false) : base(nameof(AdventurerStateMachine), tickable)
    {
    }

    protected override void SetBehaviours()
    {
        base.SetBehaviours();
        CreateAndAddState(ref VisitWilderness, nameof(VisitWilderness));
        CreateAndAddState(ref VisitGuild, nameof(VisitGuild));
        CreateAndAddState(ref VisitTown, nameof(VisitTown));

        AwakeState
            .AllowTransitionTo(VisitGuild)
            .AllowTransitionTo(VisitWilderness)
            .AllowTransitionTo(VisitTown);

        IdleState
            .AllowTransitionTo(VisitGuild)
            .AllowTransitionTo(VisitWilderness)
            .AllowTransitionTo(VisitTown);

        VisitGuild
            .OnEntry(OnEnterVisitGuild)         // Enter the _guildStateMachine SM
            .AllowTransitionTo(VisitWilderness) // Go off and do their quest
            .AllowTransitionTo(VisitTown);      // Visit the town (shopping, socialising, go home etc)

        VisitTown
            .OnEntry(OnEnterVisitTown)          // Enter the _townStateMachine SM
            .AllowTransitionTo(VisitWilderness) // Go off and do their quest
            .AllowTransitionTo(VisitGuild)      // Visit the guild (training(?), get quest, use services, socialising, register)
            .AllowTransitionTo(DestroyState);   // Destroy the AI (IE entered home to sleep so destroy/disable)

        VisitWilderness
            .OnEntry(OnEnterVisitWilderness)          // Enter the _wildernessStateMachine SM
            .AllowTransitionTo(VisitGuild)    // Go back to the Guild
            .AllowTransitionTo(VisitTown)     // Visit the town (shopping, socialising, go home etc)
            .AllowTransitionTo(DestroyState); // Destroy the AI (IE Exited town, Died etc)

        Dbg.Log(Log.AI,
                "SET BEHAVIOURS. TODO: Lets look into making Destroy etc a bit smarter. We want to remove components to improve performance, but we want entities (esp. adventurers) to persist");
        // TODO: DestroyState depends if we want all AI to persist (high perf. impact?) or if we want to hide them for later
        //      Like when questing, do we want to keep them 'in the world' since eventually the player will also be able to
        //      go out and adventure? Would be nice to be able to bump into them
        //      -- PERF. Solution would be to just 'hide' them. Destroy the entity, but stick them in some sort of AI Manager 
        //              with a timer, which spawns them back in after X minutes when their quest/task is done
        //              - Could also monitor the player's position, and if they get close to where the AI is/was, spawn them back in
        //              - Not all SMs/Entities need to be around 100% of the time esp. if the player can't see them. We can fake it
    }

    protected override void OnEnterAwake()
    {
        Dbg.Log(Log.AI, "ENTERED: Awake State");
        // Decide which State to enter - are we going to the town, or are we going to the guild, or going questing etc?
    }

    private void OnEnterVisitGuild()
    {
        Dbg.LogVerbose(Log.AI, "ENTERED: Visit Guild State");
        // Enter the Visit Guild State Machine
        EnterSubStateMachine(_guildStateMachine, _ => OnCompleteGuildStateMachine());
    }

    private void OnCompleteGuildStateMachine()
    {
        Dbg.LogVerbose(Log.AI, "COMPLETE: Visit Guild State. Selecting next state");
        // We've left the guild - lets see what we should go into next;
        //  - Town (does AI want to go shopping? Socialising? Home?)
        //      -- Time, Money and mood affect this choice
        //          - Too late to venture outside?
        //          - Wants to go shopping (mood + money?)
        //          - Wants to socialise? (mood)
        //  - Quest (does AI want to start their quest?)
        //      -- Has quest? Mood and Time all affect this choice
        //          - If not quest then we can't use this one anyway
        //          - Is it too late in the day to start the quest?
        //          - Low mood? Go socialise first?
        //          - Poor equipment, go shopping? (has the money?)

        // TODO: Should we change the 'Quest' State Machine into a 'Wilderness' State Machine. So these 3 SMs are actually
        //  more related to where they are going next?
        //  -- As part of 'Wilderness', quest could be one set of actions/behaviours
        //  -- But could also include travelling (exploring)
        //  -- Or training
        //  And could more easily and dynamically shift between these behaviours whilst outside the town instead of
        //  having to revert to this SM, just to load up a new SM.
    }

    private void OnEnterVisitTown()
    {
        Dbg.LogVerbose(Log.AI, "ENTERED: Visit Town State");
        // Enter the Visit Town State machine to control behaviours/actions around the town
        //  Typical actions include;
        //  - Wandering
        //  - Patrolling
        //  - Socliaising (parks, taverns, Inns, alleys etc)
        //  - Shopping
        //  - Going Home / Sleeping

        EnterSubStateMachine(_townStateMachine, _ => OnCompletedTownStateMachine());
    }

    private void OnCompletedTownStateMachine()
    {
        Dbg.LogVerbose(Log.AI, "COMPLETE: Visit Town State. Selecting next state");
        // We've left the town - lets see what we should go into next;
        //  - Wilderness (does AI want to venture outside? Training? Questing? Exploring)
        //      -- Time, Confidence, Quests
        //          - Too late to venture outside?
        //          - Are they confident (Equipment, level, threats/dangers - dynamic events)
        //          - Have active quests?
        //  - Guild (go back to the guild? Register? Get quest? Use Services? Turn in quests etc?)
        //      -- Money, Mood, Quests
        //          - Has registered yet? (wants to try? Wants to try - again?)
        //          - Low mood? (use services)
        //          - Confident + wants a quest? (may go for harder quests?)
        //          - Needs Money? (pick up quest)
        //          - has completed quest and wants to turn it in
        //              -- This should always be the priority no matter what - exception would be if its too late in which case they'll do it next day
    }

    private void OnEnterVisitWilderness()
    {
        Dbg.LogVerbose(Log.AI, "COMPLETE: Visit Town State. Selecting next state");
        // Enter the Visit Wilderness State machine to control behaviours/actions outside the town
        //  Typical actions include;
        //      - Training (gain exp, loot, fight monsters etc) - Training = doesn't care what they fight
        //      - Hunting (gain exp, hunt animals, fight monsters etc) - Hunting = looking for specific resources
        //      - Exploring (gain exp, loot, intel, fight monsters) - Exploring = not looking to fight, but will if necessary, focus on gaining intel
        //      - Questing (gain exp, loot, fight monster, complete quest) - Questing = priority is completing quest objectives

        // TODO: Should we change the 'Quest' State Machine into a 'Wilderness' State Machine. So these 3 SMs are actually
        //  more related to where they are going next?
        //  -- As part of 'Wilderness', quest could be one set of actions/behaviours
        //  -- But could also include travelling (exploring)
        //  -- Or training
        //  And could more easily and dynamically shift between these behaviours whilst outside the town instead of
        //  having to revert to this SM, just to load up a new SM.
    }
}
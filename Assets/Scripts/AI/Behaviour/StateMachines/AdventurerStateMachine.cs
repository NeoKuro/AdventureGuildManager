using System.Collections.Generic;

using Hzn.Framework;

using Unity.VisualScripting;

using UnityEngine;

using State = Hzn.Framework.State;
using StateMachine = Hzn.Framework.StateMachine;

public class AdventurerStateMachine : AICoreStateMachine
{
    private StateMachine        _wildernessStateMachine; // SM to handle behaviours/activities outside the town
    private StateMachine        _townStateMachine;       // SM to handle behaviours/activities around the town 
    private AIGuildStateMachine _guildStateMachine;      // SM to handle behaviours/activities around the guild

    private State VisitGuild;      // State controlling visiting the Guild (_guildStateMachine dependency)
    private State VisitWilderness; // State controlling visiting the wilderness (_wildernessStateMachine dependency)
    private State VisitTown;       // State controlling visiting the town (_townStateMachine dependency)

    protected Adventurer_AIEntity AdventurerAI;

    public AdventurerStateMachine(Adventurer_AIEntity ai, bool tickable = false) : base(nameof(AdventurerStateMachine), tickable)
    {
        AdventurerAI = ai;
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
            .OnEntry(OnEnterVisitWilderness)  // Enter the _wildernessStateMachine SM
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
        bool                                 hasJob       = AdventurerAI.HasJob();
        Dictionary<EEntityPriorities, float> stats        = AdventurerAI.EvaluateStats();
        float                                confidence   = AdventurerAI.EvaluateConfidenceNormalized();
        float                                satisfaction = AdventurerAI.GetGuildSatisfaction();

        List<(EEntityPriorities priority, float value)> priorities = new List<(EEntityPriorities, float)>();
        foreach (KeyValuePair<EEntityPriorities, float> stat in stats)
        {
            priorities.Add((stat.Key, stat.Value));
        }

        priorities.Sort((a, b) => b.value.CompareTo(a.value));

        for (int i = 0; i < priorities.Count; i++)
        {
            Dbg.Log(Log.AI, $"Stat: {priorities[i].priority} - {priorities[i].value}");
            switch (priorities[i].priority)
            {
                case EEntityPriorities.Health:
                    if (priorities[i].value >= 0.33f)
                    {
                        ChangeState(VisitTown);
                        return;
                    }
                    break;
                case EEntityPriorities.Energy:
                    if (priorities[i].value >= 0.9f)
                    {
                        ChangeState(VisitTown);
                        return;
                    }
                    break;
                case EEntityPriorities.Hunger:
                case EEntityPriorities.Thirst:
                    if (priorities[i].value >= 0.6f)
                    {
                        // As Satisfaction decreases, the range the RNG will select from increases
                        //  At 100% satisfaction, there is a 20% chance to not choose the guild services still
                        //  At 0% satisfaction there is a 80% chance to choose town services over guild
                        //  At 50% satisfaction, there is a 68% chance to choose town over guild services
                        //  This will encourage players to maintain higher satisfaction rates as the drop off in going to town is
                        //      significant beyond the 50% mark (~12% drop from 0% -> 50%, but a 48% drop from 50% -> 100%)
                        float satisfactionPoint = UsefulMethods.Map(satisfaction, 0f, 1f, 0.0f, 0.75f);
                        bool  goTown            = Random.Range(satisfactionPoint, 1f) < 0.8f;
                        ChangeState(goTown ? VisitTown : VisitGuild);
                        return;
                    }
                    break;
                case EEntityPriorities.Social:
                case EEntityPriorities.Boredom:
                    if (priorities[i].value >= 0.7f)
                    {
                        // As Satisfaction decreases, the range the RNG will select from increases
                        //  At 100% satisfaction, there is a 20% chance to not choose the guild services still
                        //  At 0% satisfaction there is a 80% chance to choose town services over guild
                        //  At 50% satisfaction, there is a 68% chance to choose town over guild services
                        //  This will encourage players to maintain higher satisfaction rates as the drop off in going to town is
                        //      significant beyond the 50% mark (~12% drop from 0% -> 50%, but a 48% drop from 50% -> 100%)
                        float satisfactionPoint = UsefulMethods.Map(satisfaction, 0f, 1f, 0.0f, 0.75f);
                        bool  goTown            = Random.Range(satisfactionPoint, 1f) < 0.8f;
                        ChangeState(goTown ? VisitTown : VisitGuild);
                        return;
                    }
                    break;
            }
        }

        if (AdventurerAI.AdventurerData.AssignedRank == EAdventurerRank.None)
        {
            ChangeState(VisitGuild);
            return;
        }

        // Check if they think they have the money to upgrade their gear
        float wealth = AdventurerAI.EvaluateWealth();

        // Check if they want to go get a job - If they need money
        if (!hasJob)
        {
            // If their Wealth : Expected Wealth ratio is getting low, they will want to go get a job to earn more money
            if (AdventurerAI.ShouldGetJob())
            {
                ChangeState(VisitGuild);
                return;
            }
        }

        // Are they confidence enough to complete the job? And do they have the money to improve their confidence?
        if (confidence < 0.35f)
        {
            if (wealth > 0.66f)
            {
                ChangeState(VisitTown);
                return;
            }
        }

        // Minimum value is 0.25f
        // Decide whether - after all other evaluations are complete - if the AI should go get a new job from the guild anyway
        float jobPriority = AdventurerAI.GetJobPriority();
        bool getJobAnyway = Random.Range(jobPriority, 1f) > 0.8f;
        if (getJobAnyway)
        {
            ChangeState(VisitGuild);
            return;
        }
        
        // Otherwise - all stats are met (or cannot be met) so go to the Wilderness state to either do a job, explore or go training
        //  All can earn money (selling loot, selling intel, completing jobs)
        ChangeState(VisitWilderness);
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
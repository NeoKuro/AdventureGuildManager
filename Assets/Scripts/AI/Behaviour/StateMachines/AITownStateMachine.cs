using Hzn.Framework;

/// <summary>
/// State Machine to control in-town behaviours
/// This can be re-used by any NPC not just adventurers (everyone can venture around town, shop, socialise etc)
/// </summary>
public class AITownStateMachine : StateMachine
{
    private ITownCore _townAICore;
    
    private StateMachine _exploreTownStateMachine;  // Includes wandering, exploring and patrolling behaviours
    private StateMachine _socialiseTownStateMachine; // Includes socialising behaviours
    private StateMachine _shoppingTownStateMachine;  // Includes shopping behaviours
    
    private State VisitTownState;
    // private State WanderingState;
    // private State PatrollingState;
    private State ExploreState;
    private State SocialisingState;
    private State ShoppingState;
    private State HospitalState;
    private State HomeState;
    private State FinishVisitState;
    
    public AITownStateMachine(ITownCore townAICore, bool tickable = false) : base(nameof(AITownStateMachine), tickable)
    {
        _townAICore = townAICore;
        
        CreateAndAddState(ref VisitTownState, nameof(VisitTownState));
        // CreateAndAddState(ref WanderingState, nameof(WanderingState));
        // CreateAndAddState(ref PatrollingState, nameof(PatrollingState));
        CreateAndAddState(ref ExploreState, nameof(ExploreState));
        CreateAndAddState(ref SocialisingState, nameof(SocialisingState));
        CreateAndAddState(ref ShoppingState, nameof(ShoppingState));
        CreateAndAddState(ref HospitalState, nameof(HospitalState));
        CreateAndAddState(ref HomeState, nameof(HomeState));
        CreateAndAddState(ref FinishVisitState, nameof(FinishVisitState));
        
        // ENTRY
        //  Used to decide what behaviour to do first in town
        VisitTownState
            .DeclareStartState()
            .OnEntry(OnEnterVisitTown)
            // .AllowTransitionTo(WanderingState)
            // .AllowTransitionTo(PatrollingState)
            .AllowTransitionTo(ExploreState)
            .AllowTransitionTo(SocialisingState)
            .AllowTransitionTo(ShoppingState)
            .AllowTransitionTo(HomeState);
        
        // WanderingState
        //     .OnEntry(OnEnterWandering)
        //     .OnUpdate(UpdateWandering)
        //     .AllowTransitionTo(PatrollingState)
        //     .AllowTransitionTo(SocialisingState)
        //     .AllowTransitionTo(ShoppingState)
        //     .AllowTransitionTo(HomeState)
        //     .AllowTransitionTo(FinishVisitState);
        //
        // PatrollingState
        //     .OnEntry(OnEnterPatrolling)
        //     .OnUpdate(UpdatePatrolling)
        //     .AllowTransitionTo(WanderingState)
        //     .AllowTransitionTo(SocialisingState)
        //     .AllowTransitionTo(ShoppingState)
        //     .AllowTransitionTo(HomeState)
        //     .AllowTransitionTo(FinishVisitState);
        
        // INTERMEDIARY
        //  Handles the starting of the Explore Town State Machine
        //      - Wander (idle)
        //      - Patrol (guards)
        //      - Explore (hunt for specific shops if not known?)
        ExploreState
            .OnEntry(OnEnterExplore)
            .AllowTransitionTo(SocialisingState)
            .AllowTransitionTo(ShoppingState)
            .AllowTransitionTo(HomeState)
            .AllowTransitionTo(FinishVisitState);
        
        // INTERMEDIARY
        //  Handles the starting of the Socialise Town State Machine
        //  Picks a location (park, market, taverns, Inns etc) and navigates to them
        //  Will then stay for a while improving social stat
        //  Can also affect relationships with other NPCs (++ Persistence feeling)
        SocialisingState
            .OnEntry(OnEnterSocialising)
            // .AllowTransitionTo(WanderingState)
            // .AllowTransitionTo(PatrollingState)
            .AllowTransitionTo(ExploreState)
            .AllowTransitionTo(ShoppingState)
            .AllowTransitionTo(HomeState)
            .AllowTransitionTo(FinishVisitState);
        
        // INTERMEDIARY
        //  Hnadles the starting of the Shopping State Machine
        //  Decides what they need and will navigate to shop(s) that sell those items
        //      - Potions
        //      - Weapons
        //      - Skills
        //      - Armour
        //  On arrival, will try purchase things if they have enough money or stock
        //  May also enter this state to SELL wares (works in reverse)
        ShoppingState
            .OnEntry(OnEnterShopping)
            // .AllowTransitionTo(WanderingState)
            // .AllowTransitionTo(PatrollingState)
            .AllowTransitionTo(ExploreState)
            .AllowTransitionTo(SocialisingState)
            .AllowTransitionTo(HomeState)
            .AllowTransitionTo(FinishVisitState);
        
        // INTERMEDIARY
        //  Go to the hospital to recover. Faster than recovering at home, can heal serious wounds, but costs money
        HospitalState
            .OnEntry(OnEnterHospital)
            .AllowTransitionTo(ExploreState)
            .AllowTransitionTo(SocialisingState)
            .AllowTransitionTo(ShoppingState)
            .AllowTransitionTo(HomeState)
            .AllowTransitionTo(FinishVisitState);
        
        // EXIT
        //  State to control the time spent at home. This is an exit state
        //  Once at home, we despawn them, in the BG they are on a 'timer' before they respawn
        //  At that point, the SM re-evaluates what to do
        //  Entering this state relies on;
        //      - Time
        //      - Energy
        //      - Mood (Socialising etc)
        //  If its late, and don't need to socialise, they will go home
        //  Otherwise they may choose to go out to an inn/tavern
        //  If they are tired and its late (even low mood) they may choose to go home early
        //  After X o'Clock, once the game re-evals behaviours, they should always choose to go home
        //  NOTE: HOME can mean a house in town, a room in the tavern, OR even a room at the guild hall
        HomeState
            .DeclareExitState()
            .OnEntry(OnEnterHome)
            // .AllowTransitionTo(WanderingState)
            // .AllowTransitionTo(PatrollingState)
            .AllowTransitionTo(ExploreState)
            .AllowTransitionTo(SocialisingState)
            .AllowTransitionTo(ShoppingState)
            .AllowTransitionTo(FinishVisitState);

        // EXIT
        //  Once the AI has finished their activities in Town, and conditions aren't met to go home, they will
        //  enter this state to exit the Town State Machine. Likely either entering the Wilderness SM, or Guild SM
        FinishVisitState
            .DeclareExitState()
            .OnEntry(OnEnterFinishVisit);
    }

    private void OnEnterVisitTown()
    {
        // Pick a location/action to perform in town and enter the appropriate state
        Dbg.LogVerbose(Log.AI, "ENTERED: Visit Town State");
        ChooseNextState();
    }

    private void OnEnterExplore()
    {
        EnterSubStateMachine(_exploreTownStateMachine, _ => ChooseNextState());
    }

    private void OnEnterSocialising()
    {
        EnterSubStateMachine(_socialiseTownStateMachine, _ => ChooseNextState());
    }

    private void OnEnterShopping()
    {
        EnterSubStateMachine(_shoppingTownStateMachine, _ => ChooseNextState());
    }

    private void OnEnterHospital()
    {
        // Set a destination to the hospital
        // Register 'On Arrived at hospital' callback
    }

    private void OnEnterHome()
    {
        // Set a destination to the home
        // Register 'On Arrived at home' callback
        _townAICore.RegisterArriveAtTownDestinationCallback(OnArriveAtHome);
    }

    private void OnArriveAtHome()
    {
        Dbg.LogVerbose(Log.AI, "Arrived at home");
        CompleteAndPause();
    }

    private void OnEnterFinishVisit()
    {
        // Exit this state machine
        //  -- Successfully concluded business in Town, so now do something else
        //      -- Could go hunting/training, start a quest, go to the Guild Hall
        CompleteAndPause();
    }

    private void ChooseNextState()
    {
        // Evaluate the AI's stats / feelings and determine what state to enter next
        Dbg.LogVerbose(Log.AI, "Trying to Evaluate Needs (TODO: Implement EvaluateNeeds)");
        _townAICore.EvaluateStats();
    }
}
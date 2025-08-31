using Hzn.Framework;

public class AIIdleStateMachine : StateMachine
{
    protected State OnEnterState;
    protected State IdleState;
    protected State OnExitState;

    public AIIdleStateMachine(bool tickable = false) : base(nameof(AIIdleStateMachine), tickable)
    {
        CreateAndAddState(ref OnEnterState, nameof(OnEnterState));
        CreateAndAddState(ref IdleState, nameof(IdleState));
        CreateAndAddState(ref OnExitState, nameof(OnExitState));

        OnEnterState
            .DeclareStartState()
            .OnEntry(EnterInitialState)
            .AllowTransitionTo(IdleState);
        
        IdleState
            .OnEntry(EnterIdleState)
            .OnUpdate(UpdateIdleState)
            .OnExit(ExitIdleState)
            .AllowTransitionTo(IdleState)
            .AllowTransitionTo(OnExitState);

        OnExitState
            .OnEntry(EnterFinalState)
            .DeclareExitState();
    }
    
    protected virtual void EnterInitialState()
    {
        // Select initial animations
    }
    protected virtual void EnterIdleState()
    {
        // Select idle animation duration
    }
    
    protected virtual void UpdateIdleState(float deltaT)
    {
        // Choose new animation every X seconds etc
    }
    protected virtual void ExitIdleState()
    {
        
    }
    
    protected virtual void EnterFinalState()
    {
        // Terminate any animations
    }
    
    protected virtual void ExitFinalState()
    {
        
    }
}
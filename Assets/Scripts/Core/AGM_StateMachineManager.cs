using Hzn.Framework;

public class AGM_StateMachineManager : StateMachineManager
{
    public enum EAGM_StateMachineTypes
    {
        None = 10,  // Start with 10 to avoid conflicts with other enums
    }

    protected override void PostManagerCreated()
    {
        base.PostManagerCreated();
        InitialiseAGMStateMachines();
    }


    /// <summary>
    /// Can be used to toggle the default state machine creation function on or off - allows user to provide custom
    /// implementation or bypass default creation altogether.
    /// </summary>
    protected override void SetInitialiseDefaultStateMachines()
    {
        _initialiseDefaultStateMachines = false;
    }

    private void InitialiseAGMStateMachines()
    {
        AddUniqueStateMachine(UniqueStateMachines.GAME, new AGM_CoreStateMachine());
        AddUniqueStateMachine(UniqueStateMachines.IN_GAME, new AGM_InGameStateMachine());
        AddUniqueStateMachine(UniqueStateMachines.LOADING, new LoadingStateMachine());
    }
}
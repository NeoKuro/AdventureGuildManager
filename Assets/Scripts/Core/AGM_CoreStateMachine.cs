using Hzn.Framework;

public class AGM_CoreStateMachine : GameStateMachine
{

    /// <summary>
    /// Provides additional context to the GSM for loading game-specific states after the loading screen has finished
    /// </summary>
    /// <param name="intPayload">The next state to transition to on successfully loading the scene - this should not be any int-values of <see cref="ELevelLoad"/></param>
    protected override State OnLoadingComplete_CheckGameStates(StatePayload_Int intPayload)
    {
        Dbg.Error(Log.GameFlow,
                  $"The provided value [{intPayload.IntVal.ToString()}] could not be mapped to any State. [{nameof(OnLoadingComplete_CheckGameStates)}] is not implemented");
        return MainMenu;
    }

}
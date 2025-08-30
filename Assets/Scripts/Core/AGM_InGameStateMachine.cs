using Hzn.Framework;

public class AGM_InGameStateMachine : InGameStateMachine
{
    protected override void EnterActiveGame()
    {
        base.EnterActiveGame();
        AICoreManager.ActivateAIManagers();
    }
}
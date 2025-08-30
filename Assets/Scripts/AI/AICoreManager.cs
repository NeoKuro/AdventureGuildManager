using Hzn.Framework;

public class AICoreManager : Manager<AICoreManager>
{
    private ICoreAIManager[] _aiManagers;


    #region -- MANAGER SETUP --

    protected override void PostManagerCreated()
    {
        base.PostManagerCreated();
        CreateAIManagers();
    }

    public override void PreShutdown()
    {
        base.PreShutdown();
        ShutdownAIManagers();
    }
    
    private void CreateAIManagers()
    {
        _aiManagers = new ICoreAIManager[]
                      {
                          AISpawnManager.Create(),
                          WaypointManager.Create(),
                          AIBehaviourManager.Create()
                      };
    }

    private void ShutdownAIManagers()
    {
        foreach (ICoreAIManager manager in _aiManagers)
        {
            manager.Shutdown();
        }
    }

    #endregion


    #region -- PUBLIC API --

    public static void ActivateAIManagers()
    {
        Get()?.ActiveAIManagersInternal();
    }

    #endregion


    #region -- PRIVATE IMPLEMENTATION --

    private void ActiveAIManagersInternal()
    {
        foreach (ICoreAIManager manager in _aiManagers)
        {
            manager.ActivateAIManager();
        }
    }

    #endregion

}
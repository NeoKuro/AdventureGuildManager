using Hzn.Framework;

using UnityEngine;
using UnityEngine.SceneManagement;

public class AISpawnManager : Manager<AISpawnManager>, ICoreAIManager
{
    protected override void PostManagerCreated()
    {
        base.PostManagerCreated();
    }


    public void ActivateAIManager()
    {
        Dbg.Log(Log.AI, "Activating AI Manager");
    }
}
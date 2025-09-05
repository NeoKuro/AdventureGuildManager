using System.Collections.Generic;

using Hzn.Framework;

using Managers;

using UnityEngine;

public class GameBoot
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void InitializeBoot()
    {
        Dbg.Log(Log.Boot, ">>> UNITY BOOT -1- SubsystemRegistration: BEGIN [CORE]");
        Boot.RegisterCoreBootStartupFunction(CoreBootStartup);
        Boot.RegisterCoreManagerRegistrationFunction(CreateCoreManagers);
        Boot.RegisterCoreBootShutdownFunction(CoreShutdown);
        Dbg.Log(Log.Boot, ">>> UNITY BOOT -1- SubsystemRegistration: COMPLETE [CORE]");
        
        Dbg.Log(Log.Boot, ">>> UNITY BOOT -1- SubsystemRegistration: BEGIN [MONO]");
        Boot.RegisterMonoManagerRegistrationFunction(CreateMonoManagers);
        Dbg.Log(Log.Boot, ">>> UNITY BOOT -1- SubsystemRegistration: COMPLETE [MONO]");
    }

    private static void CoreBootStartup()
    {

    }

    private static void CreateCoreManagers()
    {
        Dbg.Log(Log.Boot, $"Creating core managers");
        Boot.RegisterCoreManagerInstances(new IManager[]
                                          { 
                                              TimeManager.Create(),
                                              GuildDataManager.Create(),
                                              AICoreManager.Create(),
                                              AIBehaviourManager.Create(),
                                          });
    }

    private static void CoreShutdown()
    {
        Dbg.Log(Log.Boot, $"Core Shutting down");
    }

    private static void CreateMonoManagers()
    {
        Dbg.Log(Log.Boot, $"Creating Mono managers");
        Boot.RegisterMonoManagerInstances( new Dictionary<string, IManager>()
                                           {
                                               { nameof(EntityManager), EntityManager.Create() },
                                               { nameof(InteractableManager), InteractableManager.Create() },
                                               { nameof(SpawnManager), SpawnManager.Create() },
                                           });
        
    }
}
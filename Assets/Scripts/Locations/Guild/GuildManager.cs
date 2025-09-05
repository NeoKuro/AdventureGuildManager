using System;
using System.Collections.Generic;
using System.Linq;

using Hzn.Framework;

using TMPro;

using UnityEngine;

using Random = UnityEngine.Random;

public class GuildManager : Singleton<GuildManager>
{
    [SerializeField]
    private Transform _visitGuildPoint;
    
    private List<ReceptionController> _allReceptions = new List<ReceptionController>();
    private Dictionary<Type, List<GuildServiceController>> _services = new Dictionary<Type, List<GuildServiceController>>();

    protected override void Awake()
    {
        base.Awake();
        _services = new Dictionary<Type, List<GuildServiceController>>();
    }

    public static void RegisterNewService<T>(T service) where T : GuildServiceController
    {
        if (Instance == null)
        {
            Dbg.Error(Logging.Guild, "GuildManager is null");
            return;
        }
        
        Instance._services.TryAdd(typeof(T), new List<GuildServiceController>() );

        if (Instance._services[typeof(T)].Contains(service))
        {
            Dbg.Error(Logging.Guild, "Service already registered");
            return;
        }
        
        Instance._services[typeof(T)].Add(service);
    }

    /// <summary>
    /// Retrieve any service of type T
    /// Use this if balancing services isn't important.
    /// Can optionally specify what access method to use (defaults to Random)
    /// </summary>
    /// <param name="access">How to retrieve one of the services</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetAnyService<T>(EAccessType access = EAccessType.Random) where T : GuildServiceController
    {
        if (Instance == null)
        {
            Dbg.Error(Log.AI, "GuildManager is null");
            return null;
        }

        if (!Instance._services.ContainsKey(typeof(T)))
        {
            Dbg.Error(Logging.Guild, $"Service of type [{typeof(T).Name}] is not registered");
            return null;
        }

        if (Instance._services[typeof(T)].Count == 0)
        {
            Dbg.Error(Logging.Guild, $"No services of type [{typeof(T).Name}] registered");
            return null;
        }

        if (Instance._services[typeof(T)].Count == 1)
        {
            return Instance._services[typeof(T)][0] as T;
        }

        switch (access)
        {
            default:
            case EAccessType.Random:
                int index = Random.Range(0, Instance._services[typeof(T)].Count);
                return Instance._services[typeof(T)][index] as T;
            case EAccessType.First:
                return Instance._services[typeof(T)].First() as T;
            case EAccessType.Last:
                return Instance._services[typeof(T)].Last() as T;
        }
    }

    /// <summary>
    /// Returns the reception with the smallest queue size
    /// Rather than randomly choosing one - this will seem more natural and help
    /// make the guild busier
    /// </summary>
    /// <returns></returns>
    public static ReceptionController GetReceptionBalanced()
    {
        if (Instance == null)
        {
            Dbg.Error(Log.AI, "GuildManager is null");
            return null;
        }

        if (!Instance._services.ContainsKey(typeof(ReceptionController)))
        {
            Dbg.Error(Logging.Guild, $"Service of type [{nameof(ReceptionController)}] is not registered");
            return null;
        }

        if (Instance._services[typeof(ReceptionController)].Count == 0)
        {
            Dbg.Error(Logging.Guild, $"No services of type [{nameof(ReceptionController)}] registered");
            return null;
        }

        int lowestCount = int.MaxValue;
        ReceptionController lowestCountReception = null;
        foreach (GuildServiceController serviceController in Instance._services[typeof(ReceptionController)])
        {
            ReceptionController reception = serviceController as ReceptionController;
            if (reception == null)
            {
                Dbg.Error(Logging.Guild, $"Service of type [{nameof(ReceptionController)}] is not of type [{nameof(ReceptionController)}]");
                continue;
            }

            int count = reception.QueueSize;
            if (count == 0)
            {
                return reception;
            }
            
            if (count < lowestCount)
            {
                lowestCount = count;
                lowestCountReception = reception;
            }
        }
        
        return lowestCountReception;
    }

    public static Transform GetGuildEntranceLocation()
    {
        if (Instance == null)
        {
            Dbg.Error(Log.AI, "GuildManager is null");
            return null;
        }

        return Instance._visitGuildPoint;
    }
}
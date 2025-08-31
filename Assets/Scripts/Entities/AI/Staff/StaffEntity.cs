using System;
using System.Collections.Generic;

using UnityEngine;

public class StaffEntity : Entity, IGuildStaff, ITownCore
{
    private Action _onArriveAtGuildCallback;
    private Action _onArriveAtTownCallback;
    
    private StatsHandler _statsHandler;

    public override bool CreateNewEntity(Transform root, GameObject prefab)
    {
        bool result = base.CreateNewEntity(root, prefab);
        InitialiseBehaviour();
        return result;
    }

    public void InitialiseBehaviour()
    {
        AIBehaviourManager.RegisterNewAI(this);
        _statsHandler = new StatsHandler(this);
    }

    public void FixedUpdateBehaviour()
    {
        
    }

    public Dictionary<EEntityPriorities, float> EvaluateStats()
    {
        return _statsHandler.EvaluateStats();
    }

    public void GetInventory()
    {
        throw new NotImplementedException();
    }

    public void SetDestinationToTown()
    {
        throw new NotImplementedException();
    }

    public void SetDestinationToHome()
    {
        throw new NotImplementedException();
    }


    #region -- CALLBACK MANAGEMENT --

    public void RegisterArrivedAtGuildCallback(Action onArrive)
    {
        _onArriveAtGuildCallback += onArrive;
    }

    public void UnregisterArrivedAtGuildCallback(Action onArrive)
    {
        _onArriveAtGuildCallback -= onArrive;
    }

    public void RegisterArriveAtTownDestinationCallback(Action onArrive)
    {
        _onArriveAtTownCallback += onArrive;
    }

    public void UnregisterArriveAtTownDestinationCallback(Action onArrive)
    {
        _onArriveAtTownCallback -= onArrive;
    }

    #endregion
}
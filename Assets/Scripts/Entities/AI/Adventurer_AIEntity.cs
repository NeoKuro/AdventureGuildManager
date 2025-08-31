using System;
using System.Collections.Generic;

using Hzn.Framework;

using UnityEngine;

public class Adventurer_AIEntity : AIEntity, ITownCore, IGuildAdventurer, IInventoryCore
{
    protected AdventurerGuildHandler _adventurerGuildHandler;
    protected AIInventoryHandler     _inventoryHandler;
    private   Action                 _onArriveAtTownCallback;
    private   Action                 _onArriveAtGuildCallback;

    public SAdventurerData AdventurerData { get; private set; }

    public virtual void SpawnAdventurer(bool isNewAdventurer)
    {
        if (isNewAdventurer)
        {
            AdventurerData = new SAdventurerData(AdventurerManager.GenerateNewAdventurerName());
            return;
        }
    }

    public override bool CreateNewEntity(Transform root, GameObject prefab)
    {
        bool result = base.CreateNewEntity(root, prefab);
        InitialiseBehaviour();
        return result;
    }

    public override void InitialiseBehaviour()
    {
        base.InitialiseBehaviour();
        _inventoryHandler       = new AIInventoryHandler(this);
        _adventurerGuildHandler = new AdventurerGuildHandler(this);
    }

    /// <summary>
    /// Updates periodically at a fixed rate. Called centrally by the AIBehaviourManager
    /// </summary>
    public override void FixedUpdateBehaviour()
    {
        _inventoryHandler.UpdateWealthExpectation();
    }


    public virtual void OnAdventurerSigned(SAdventurerData newAdventurerData)
    {
        Dbg.Log(Logging.Entities, $"Adventurer [{AdventurerData.Name}] has been Registered!");
        Dbg.Log(Logging.Entities, $"TODO: Move them on / find a new task (see notes)");

        // We update the data - as this will include the appropriate ranking etc
        AdventurerData = newAdventurerData;

        // TODO : Post-sign events;
        // --> Have a happy / emoji icon above their heads - this can be used to indicate how accurate the player was
        // Post-sign options;
        //  1. Go to quest baord and find quests - Get work 
        //  2. Use other services / facilities in the guild
        //  3. Socialise (away from the reception table - prevent overcrowding) with other adventurers
        //  4. Leave building
    }

    public virtual void OnAdventurerRejected()
    {
        Dbg.Log(Logging.Entities, $"Adventurer [{AdventurerData.Name}] has been REJECTED!");
        Dbg.Log(Logging.Entities, $"TODO: Move them on / find a new task (see notes)");

        // TODO : Post-reject events;
        // --> Have a happy / emoji icon above their heads - this can be used to indicate how accurate the player was
        // Post-reject options;
        //  MVP:
        //      -- Leave building
        //
        // FUTURE:
        //  1. Use PUBLIC services / facilities in the guild
        //      -- Player can restrict services/facilities based on rank with guild (IE if they get too popular can prioritise members etc)
        //      -- If adventurers cannot access enough facilities, even because busy, that will impact mood
        //      -- 2 things to track? "Does the guild offer it? Can I access it? If the first is yes then minimum 50% happiness?)
        //  2. Socialise (away from the reception table - prevent overcrowding) with other adventurers
        //      -- Could improve skills (so they can come back later and re-apply) -- "TRAINING"
        //      -- Could spread gossip (then you should kick them out)
        //      -- Could chat with others but not affect mood?
    }

    public Dictionary<EInventoryType, List<SInventoryItem>> GetInventory()
    {
        return _inventoryHandler.GetInventory();
    }

    public float EvaluateConfidenceNormalized()
    {
        return _inventoryHandler.CalculateConfidenceNormalized();
    }

    public float EvaluateWealth()
    {
        return _inventoryHandler.EvaluateWealth();
    }

    public void SetDestinationToTown()
    {
        Dbg.LogVerbose(Log.AI, "TO BE IMPLEMENTED: Setting destination to town");
        Dbg.LogVerbose(Log.AI,
                       "Call the Navigation/Waypoint manager and find the closest town key-waypoint (Plaza, market, gatehouse etc)");
        throw new NotImplementedException();
    }

    public void SetDestinationToHome()
    {
        Dbg.LogVerbose(Log.AI, "TO BE IMPLEMENTED: Setting destination to Home");
        // TODO: needs to track the AI's home location (a house in town, which house, or a taven/inn room, or guild room?)
        throw new NotImplementedException();
    }


    public bool HasJob()
    {
        return _adventurerGuildHandler.HasActiveJob;
    }

    public float GetJobPriority()
    {
        return _adventurerGuildHandler.JobPriority();
    }

    public bool ShouldGetJob()
    {
        return _adventurerGuildHandler.ShouldGetJob();
    }

    public float GetGuildSatisfaction()
    {
        return _adventurerGuildHandler.Satisfaction;
    }

    public SJobData GetJob()
    {
        if (!_adventurerGuildHandler.CurrentJob.HasValue)
        {
            Dbg.Error(Log.AI, $"{nameof(GetJob)}: No job assigned!");
            return default;
        }

        return _adventurerGuildHandler.CurrentJob.Value;
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
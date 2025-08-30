using System;
using System.Collections;
using System.Collections.Generic;

using Data;

using Hzn.Framework;

using UnityEngine;

public abstract class WorkstationInteractable : BaseInteractable
{
    [SerializeField]
    protected EWorkstationType _workstationType;

    [SerializeField]
    private Transform _workstationAnchorpoint;

    private bool                    _isManned         = false;
    protected Queue<AdventurerEntity> _adventurersQueue = new Queue<AdventurerEntity>();

    protected abstract void ProcessInteract();
    protected abstract void BeginInteract();
    
    private void Update()
    {
        if(!_isManned)
        {
            return;
        }
        
        ProcessInteract();
    }

    public override void Interact(Entity interactingEntity)
    {
        if (interactingEntity is not AdventurerEntity)
        {
            Dbg.Log(Logging.Interactables, "WorkstationInteractable: Interact called by a non-adventurer entity");
            return;
        }
        
        if (interactingEntity is LocalPlayerEntity)
        {
            // Teleport player to the 'workstation position'
            // Swap the UI based on the workstation type
            _isManned = true;
            BeginInteract();
            LocalPlayerEntity.LocalPlayer.transform.position = _workstationAnchorpoint.position;
            LocalPlayerEntity.LocalPlayer.transform.rotation = _workstationAnchorpoint.rotation;
            LocalPlayerEntity.LocalPlayer.RegisterOnPlayerMove(OnPlayerMovedOff);
            return;
        }
        
        OnNPCAdventurerInteracted(interactingEntity as AdventurerEntity);
    }

    private void OnPlayerMovedOff()
    {
        _isManned = false;
        LocalPlayerEntity.LocalPlayer.DeregisterOnPlayerMove(OnPlayerMovedOff);
    }

    private void OnNPCAdventurerInteracted(AdventurerEntity adventurer)
    {
        _adventurersQueue.Enqueue(adventurer);
    }
}
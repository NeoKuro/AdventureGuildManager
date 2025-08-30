using System;
using System.Collections.Generic;
using System.Linq;

using Hzn.Framework;

using Unity.VisualScripting;

using UnityEngine;

public class InteractableManager : MonoManager<InteractableManager, InteractableManagerData>, ITickable, ITickableUpdate
{
    private float _interactableRange = 5f;
    private float _raycastMoveThreshold = 0.1f;
    private float _raycastRotationThreshold = 5f;
    private LayerMask _raycastLayerMask;

    private static DataStore<IInteractable, EInteractableType> _interactableDataStore =
        new DataStore<IInteractable, EInteractableType>();

    private static DataStore<IInteractable, EInteractableType> _relevantInteractableDataStore =
        new DataStore<IInteractable, EInteractableType>();

    private int _interactablesInRangeCount = 0;
    
    private Vector3 _lastRaycastPosition;
    private Quaternion _lastPlayerRotation;
    
    public static IInteractable ActiveInteractable {get; private set;}
    
    private static int _nextInteractableId = 0;
    public static  int NextInteractableId
    {
        get { return _nextInteractableId++; }
    }

    protected override void PostManagerCreated()
    {
        base.PostManagerCreated();
        _nextInteractableId = 0;
        _interactableDataStore = new DataStore<IInteractable, EInteractableType>();
    }

    public void Update(float deltaT)
    {
        if (_interactableDataStore.Count == 0)
        {
            Dbg.Log(Log.Debug, "No interactables to raycast");
            return;
        }

        RaycastFromLocalPlayer();
        
        // RaycastOnMove();
    }

    private void UpdateRelevantInteractables()
    {
        _interactablesInRangeCount = 0;
        if (_interactableDataStore.Count == 0)
        {
            return;
        }

        foreach (IInteractable interactable in _interactableDataStore)
        {
            if (!interactable.IsInRange(_interactableRange, LocalPlayerEntity.LocalPlayer.transform.position))
            {
                continue;
            }
            
            _interactablesInRangeCount++;
        }
    }

    private void RaycastOnMove()
    {
        float deltaPosition = Vector3.Distance(_lastRaycastPosition, LocalPlayerEntity.LocalPlayer.transform.position);
        float deltaRotation = Quaternion.Angle(LocalPlayerEntity.LocalPlayer.transform.rotation, _lastPlayerRotation);
        if (deltaPosition > _raycastMoveThreshold)
        {
            _lastRaycastPosition = LocalPlayerEntity.LocalPlayer.transform.position;
            RaycastFromLocalPlayer();
        }
        else if (deltaRotation > _raycastRotationThreshold)
        {
            _lastPlayerRotation = LocalPlayerEntity.LocalPlayer.transform.rotation;
            RaycastFromLocalPlayer();
        }
    }

    private void RaycastFromLocalPlayer()
    {
        // Dbg.Log(Log.Debug, "Raycasting from local player");
        Transform playerLookTransform = LocalPlayerEntity.LocalPLayerLookTransform;
        Debug.DrawRay(playerLookTransform.position, playerLookTransform.forward * _interactableRange, Color.red);
        if (!Physics.Raycast(playerLookTransform.position, playerLookTransform.forward, out RaycastHit hit, _interactableRange, _raycastLayerMask))
        {
            ActiveInteractable?.DeactivateInteractable();
            ActiveInteractable = null;
            return;
        }
        
        IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();

        if (interactable == ActiveInteractable)
        {
            return;
        }
        
        ActiveInteractable?.DeactivateInteractable();
        if (interactable == null)
        {
            Dbg.Error(Logging.Interactables, "Raycast hit interactable but it doesn't have an IInteractable component");
            
        }
        
        ActiveInteractable = interactable;
        if (ActiveInteractable == null)
        {
            return;
        }
        
        ActiveInteractable.ActivateInteractable();
    }


    #region -- API IMPLEMENTATION --

    private void RegisterInteractable_Instance(IInteractable interactable)
    {
        if (!_interactableDataStore.TryAdd(interactable))
        {
            Dbg.Error(Logging.Interactables, $"Failed to register interactable {interactable.GetType().Name}");
            return;
        }
    }

    private void DeregisterInteractable_Instance(IInteractable interactable)
    {
        if (!_interactableDataStore.TryRemove(interactable))
        {
            Dbg.Error(Logging.Interactables, $"Failed to remove interactable {interactable.GetType().Name}");
        }
    }

    #endregion


    #region -- PUBLIC API --

    public static void RegisterInteractable(IInteractable interactable)
    {
        // This one is ok - it's not using any of the data from the sync
        // if (!Get().IsReady)
        // {
        //     Dbg.Error(Logging.Interactables, $"Manager is not ready, data has not been sync'd. Cannot register interactable {interactable.GetType().Name}");
        //     return;
        // }
        
        Get().RegisterInteractable_Instance(interactable);
    }

    public static void DeregisterInteractable(IInteractable interactable)
    {
        // This one is ok - it's not using any of the data from the sync
        // if (!Get().IsReady)
        // {
        //     Dbg.Error(Logging.Interactables, $"Manager is not ready, data has not been sync'd. Cannot register interactable {interactable.GetType().Name}");
        //     return;
        // }
        Get().DeregisterInteractable_Instance(interactable);
    }

    #endregion


    protected override bool SyncMonoComponentData(InteractableManagerData component)
    {
        if (component == null)
        {
            Dbg.Error(Log.Spawning, "InteractableManagerData is null");
            return false;
        }

        if (component.RaycastLayerMask == 0)
        {
            Dbg.Error(Log.Spawning, $"[{nameof(InteractableManagerData)}.{nameof(InteractableManagerData.RaycastLayerMask)}] is not set, nothing will be able to spawn!");
            return false;
        }
        
        _interactableRange = component.InteractableRange;
        _raycastMoveThreshold = component.RaycastMoveThreshold;
        _raycastRotationThreshold = component.RaycastRotationThreshold;
        _raycastLayerMask = component.RaycastLayerMask;
        return true;
    }
}
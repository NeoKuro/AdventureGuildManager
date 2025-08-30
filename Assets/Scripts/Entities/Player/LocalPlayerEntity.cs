using System;

using Hzn.Framework;

using UnityEngine;

public class LocalPlayerEntity : PlayerEntity
{
    public static LocalPlayerEntity LocalPlayer { get; private set; }

    public static Transform LocalPLayerLookTransform
    {
        get
        {
            return Camera.main.transform;
        }
    }
    
    private Action _onPlayerMoveCallback;
    private PlayerMovementController _playerMovementController;
    
    public override bool CreateNewEntity(Transform root, GameObject prefab)
    {
        if (LocalPlayer != null)
        {
            Dbg.Error(Log.Player, $"Local Player already exists!");
            return false;
        }
        
        if (!base.CreateNewEntity(root, prefab))
        {
            Dbg.Error(Log.Player, $"Failed to create new Local Player entity");
            return false;
        }
        
        SetupCamera();
        IsLocalPlayer = true;
        LocalPlayer = this;
        return true;
    }

    public override void SpawnAdventurer(bool isNewAdventurer)
    {
        Dbg.Warn(Log.Player, $"TODO: Implement loading player-adventurer data / stats etc");
    }


    #region -- CALLBACK REGISTRATION --

    public void RegisterOnPlayerMove(Action onPlayerMoveCallback)
    {
        _playerMovementController.SetOnMoveCallback(onPlayerMoveCallback);
    }

    public void DeregisterOnPlayerMove(Action onPlayerMoveCallback)
    {
        _playerMovementController.UnsetOnMoveCallback(onPlayerMoveCallback);
    }

    public void RegisterOnPlayerRotate(Action onPlayerRotateCallback)
    {
        _playerMovementController.SetOnRotateCallback(onPlayerRotateCallback);
    }

    public void DeregisterOnPlayerRotate(Action onPlayerRotateCallback)
    {
        _playerMovementController.UnsetOnRotateCallback(onPlayerRotateCallback);
    }

    #endregion

    public void OnInteract()
    {
        // Dbg.Log(Log.Player, $"NOT YET IMPLEMENTED - See comments for ntoes");
        if (InteractableManager.ActiveInteractable == null)
        {
            return;
        }
        
        InteractableManager.ActiveInteractable.Interact(this);
        // TODO: Need to raycast to find what we are looking at and if its an interactable execute Interact on it
        //  BETTER IDEA -- Since we will want highlight interactables (shader) we can track what interactable is
        //  currently active / highlighted, and whatever is active we just trigger that one
    }

    protected override void SetupEntityMovement()
    {
        _entityMovementController = _playerMovementController = gameObject.AddComponent<PlayerMovementController>();
    }

    private void SetupCamera()
    {
        Camera cam = Camera.main;
        cam.transform.SetParent(transform);

        Transform cameraPointTransform = transform.FindObjectWithTagRecursive(TagManager.CAMERA_POINT_TAG);

        if (cameraPointTransform == null)
        {
            Dbg.Error(Log.Player, $"Failed to find camera point transform in BasePlayerEntity object: [{name}]");
            return;
        }
        
        cam.transform.position = cameraPointTransform.position;
        cam.transform.localRotation = cameraPointTransform.localRotation;
    }
}
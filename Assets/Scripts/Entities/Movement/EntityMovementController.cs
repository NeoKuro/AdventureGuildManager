using System.Diagnostics;

using UnityEngine;

public class EntityMovementController : MonoBehaviour
{

    private void Update()
    {
        DoRotation();
        DoMovement();
        DoActions();
    }

    protected virtual void DoMovement()
    {
        
    }

    protected virtual void DoRotation()
    {
    }

    protected virtual void DoActions()
    {
    }

    protected virtual void DoInteract()
    {
        
    }
}
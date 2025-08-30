using Data;

using Managers;

using UnityEngine;

    public interface IInteractable : IDataStoreBaseItem<EInteractableType>
    {
        public void Interact(Entity interactingEntity);

        public void DeactivateInteractable();

        public void ActivateInteractable();

        public bool IsInRange(float maxDist, Vector3 position);
        
        // protected virtual void OnTriggerEnter(Collider other)
        // {
        //     bool isPlayer = other.gameObject.GetComponentInChildren<Entity>()?.IsLocalPlayer ?? false;
        //     if (!isPlayer)
        //     {
        //         return;
        //     }
        //     
        //     InteractableManager.RegisterInteractable(this);
        // }
        //
        // protected virtual void OnTriggerExit(Collider other)
        // {
        //     bool isPlayer = other.gameObject.GetComponentInChildren<Entity>()?.IsLocalPlayer ?? false;
        //     if (!isPlayer)
        //     {
        //         return;
        //     }
        //     
        //     InteractableManager.DeregisterInteractable(this);
        // }
    }
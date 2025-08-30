using System;

using EPOOutline;

using Hzn.Framework;

using Sirenix.OdinInspector;

using UnityEngine;
using UnityEngine.UI;

public abstract class BaseInteractable : MonoBehaviour, IInteractable
{
    [SerializeField]
    private LayerMask _playerMask;
    [SerializeField]
    private Material _activeInteractableMaterial;

    [SerializeField, Required("[BASE_INTERACTABLE] Outlinable component required!")]
    private Outlinable _outlineRenderer;


    public int               DataStoreID   { get; protected set; }
    public EInteractableType DataStoreType { get; protected set; }

    private Material _normalMaterial;
    private Renderer _renderer;

    public abstract void Interact(Entity interactingEntity);
    protected abstract void InitialiseInteractable();

    private async void Awake()
    {
        if (_outlineRenderer == null)
        {
            Dbg.Error(Logging.Interactables, $"[{nameof(_outlineRenderer)}] is null! {nameof(BaseInteractable)}");
            return;
        }

        DataStoreID   = InteractableManager.NextInteractableId;
        DataStoreType = EInteractableType.Workstation;
        
        _outlineRenderer = GetComponentInChildren<Outlinable>();
        InteractableManager.RegisterInteractable(this);
        InitialiseInteractable();
    }

    private void Start()
    {
        
    }

    private void OnDestroy()
    {
        InteractableManager.DeregisterInteractable(this);
    }

    public bool IsInRange(float maxDist, Vector3 position)
    {
        return Vector3.Distance(transform.position, position) < maxDist;
    }

    protected void OnTriggerEnter(Collider other)
    {
        bool isPlayer = other.transform.root.GetComponentInChildren<Entity>()?.IsLocalPlayer ?? false;
        if (!isPlayer)
        {
            return;
        }
    
        InteractableManager.RegisterInteractable(this);
    }
    
    protected void OnTriggerExit(Collider other)
    {
        bool isPlayer = other.transform.root.GetComponentInChildren<Entity>()?.IsLocalPlayer ?? false;
        if (!isPlayer)
        {
            return;
        }
    
        InteractableManager.DeregisterInteractable(this);
    }


    public virtual void ActivateInteractable()
    {
        _outlineRenderer.enabled = true;
    }

    public virtual void DeactivateInteractable()
    {
        _outlineRenderer.enabled = false;
    }
}
//    ADVENTURE GUILD MANAGER    
//      Author: Josh Hughes


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Hzn.Framework;

using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField]
    private Transform _entityGeometryRoot;

    public int        EntityID        { get; private set; } = -1;
    public GameObject EntityPrefab    { get; private set; }
    public Transform  EntityTransform { get; private set; }
    
    public Transform GeometryRoot
    {
        get { return _entityGeometryRoot; }
    }
    
    public bool IsLocalPlayer { get; protected set; } = false;
    
    protected EntityMovementController _entityMovementController;

    public virtual bool CreateNewEntity(Transform root, GameObject prefab)
    {
        _entityGeometryRoot = root;
        EntityID = EntityManager.NextEntityID;
        Dbg.Log(Logging.Entities, $"Creating new entity (ID: [{EntityID}] with prefab [{prefab.name}]");
        SetupEntityMovement();
        return ConstructEntityPrefab(prefab);
    }

    public void MoveToPosition(Vector3 position, Vector3 rotation)
    {
        transform.position = position;
        transform.rotation = Quaternion.Euler(rotation);
    }

    protected virtual bool ConstructEntityPrefab(GameObject prefab)
    {
        if (_entityGeometryRoot == null)
        {
            Dbg.Error(Logging.Entities, $"[{nameof(_entityGeometryRoot)}] is null and must be set on the Entity base prefab.");
            return false;
        }
        
        GameObject obj = Instantiate(prefab, _entityGeometryRoot);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        
        EntityTransform = obj.transform;
        return true;
    }

    protected virtual void SetupEntityMovement()
    {
        _entityMovementController = gameObject.AddComponent<EntityMovementController>();
    }
}
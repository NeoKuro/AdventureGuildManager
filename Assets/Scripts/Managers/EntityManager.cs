using System;
using System.Collections.Generic;
using System.Linq;

using Hzn.Framework;

using UnityEngine;

using Object = UnityEngine.Object;

public class EntityManager : MonoManager<EntityManager, EntityManagerData>
{
    #region -- MANAGER DATA --

    private Entity                      _baseEntityPrefab;
    private List<SEntityPrefabData>     _entityPrefabs;
    private Dictionary<int, GameObject> _entityPrefabsList = new Dictionary<int, GameObject>();

    #endregion
    
    #region -- PROPERTIES --

    private static object _lock = new object();
    private static int    _entityID;

    public static int NextEntityID
    {
        get
        {
            lock (_lock)
            {
                return _entityID++;
            }
        }
    }

    #endregion

    public static LocalPlayerEntity LocalPlayerEntity { get; private set; }

    protected override void PostManagerCreated()
    {
        base.PostManagerCreated();
        _lock = new object();
        lock (_lock)
        {
            _entityID = 0;
        }
    }



    #region -- ABSTRACT IMPLEMENTATION --

    protected override bool SyncMonoComponentData(EntityManagerData component)
    {
        if (component == null)
        {
            Dbg.Error(Log.Player, "EntityManagerData is null");
            return false;
        }

        if (component.BaseEntityPrefab == null)
        {
            Dbg.Error(Log.Player, $"[{nameof(EntityManagerData)}.{nameof(EntityManagerData.BaseEntityPrefab)}] is null");
            return false;
        }

        if (component.EntityPrefabs == null || component.EntityPrefabs.Count == 0)
        {
            Dbg.Error(Log.Player, $"[{nameof(EntityManagerData)}.{nameof(EntityManagerData.EntityPrefabs)}] list is null or empty");
            return false;
        }

        _baseEntityPrefab = component.BaseEntityPrefab;
        _entityPrefabs    = component.EntityPrefabs;
        return true;
    }

    #endregion


    #region -- PUBLIC API --

    public bool CreateLocalPlayerEntity(out LocalPlayerEntity player)
    {
        player = null;
        if (!IsReady)
        {
            Dbg.Error(Logging.Entities, $"Manager is not ready, data has not been sync'd. Cannot create new LocalPlayerEntity");
            return false;
        }
        
        Entity entityObj = Object.Instantiate(_baseEntityPrefab);
        if (entityObj == null)
        {
            Dbg.Error(Log.Player, $"Failed to instantiate base prefab [{_baseEntityPrefab}]");
            Object.DestroyImmediate(entityObj); // ???
            return false;
        }

        GameObject obj  = entityObj.gameObject;
        Transform  root = entityObj.GeometryRoot;

        player = obj.AddComponent<LocalPlayerEntity>();
        List<GameObject> prefabs = _entityPrefabs
                                   .Where(x => (x.category & EEntityPrefabCategories.Player) == EEntityPrefabCategories.Player)
                                   .SelectMany(x => x.prefabs)
                                   .ToList();
        GameObject prefab = prefabs.GetRandom();

        if (!VerifyEntity(root, player, prefab))
        {
            Dbg.Error(Logging.Entities, "Failed to create player entity");
            return false;
        }

        LocalPlayerEntity = player;
        if (LocalPlayerEntity == null)
        {
            Dbg.Error(Log.Player, "Failed to cast player entity to LocalPlayerEntity");
            return false;
        }

        return true;
    }

    public bool CreateStaffEntity(out StaffEntity staff)
    {
        staff = null;
        if (!IsReady)
        {
            Dbg.Error(Logging.Entities, $"Manager is not ready, data has not been sync'd. Cannot create new StaffEntity");
            return false;
        }
        
        Entity entityObj = Object.Instantiate(_baseEntityPrefab);
        if (entityObj == null)
        {
            Dbg.Error(Log.Player, $"Failed to instantiate base prefab [{_baseEntityPrefab}]");
            Object.DestroyImmediate(entityObj);
            return false;
        }

        GameObject obj  = entityObj.gameObject;
        Transform  root = entityObj.GeometryRoot;

        StaffEntity staffObj = obj.AddComponent<StaffEntity>();
        List<GameObject> prefabs = _entityPrefabs
                                   .Where(x => (x.category & EEntityPrefabCategories.Staff) == EEntityPrefabCategories.Staff)
                                   .SelectMany(x => x.prefabs)
                                   .ToList();
        GameObject prefab = prefabs.GetRandom();

        if (!VerifyEntity(root, staffObj, prefab))
        {
            Dbg.Error(Logging.Entities, "Failed to create staff entity");
            return false;
        }

        staff = staffObj;
        return true;
    }

    public bool TryCreateAdventurerEntity(out Adventurer_AIEntity adventurerAI, Transform spawnPoint = null)
    {
        adventurerAI = null;
        if (!IsReady)
        {
            Dbg.Error(Logging.Entities, $"Manager is not ready, data has not been sync'd. Cannot create new AdventurerEntity");
            return false;
        }
        
        Entity entityObj = Object.Instantiate(_baseEntityPrefab);
        if (entityObj == null)
        {
            Dbg.Error(Log.Player, $"Failed to instantiate base prefab [{_baseEntityPrefab}]");
            Object.DestroyImmediate(entityObj); // ???
            return false;
        }

        if (spawnPoint != null)
        {
            entityObj.transform.position = spawnPoint.position;
            entityObj.transform.rotation = spawnPoint.rotation;
        }

        GameObject obj  = entityObj.gameObject;
        Transform  root = entityObj.GeometryRoot;

        adventurerAI = obj.AddComponent<Adventurer_AIEntity>();
        adventurerAI.SpawnAdventurer(true);
        List<GameObject> prefabs = _entityPrefabs
                                   .Where(x => (x.category & EEntityPrefabCategories.Adventurer) == EEntityPrefabCategories.Adventurer)
                                   .SelectMany(x => x.prefabs)
                                   .ToList();
        GameObject prefab = prefabs.GetRandom();

        if (!VerifyEntity(root, adventurerAI, prefab))
        {
            Dbg.Error(Logging.Entities, "Failed to create adventurer entity");
            return false;
        }

        return true;
    }

    #endregion

    private bool VerifyEntity(Transform root, Entity entityObj, GameObject prefab)
    {
        Dbg.Log(Logging.Entities, $"Verifying entity - root: [{root?.name}] | obj: [{entityObj?.name}] | prefab: [{prefab?.name}]");

        if (root == null)
        {
            Dbg.Error(Logging.Entities, "Root transform is null, has it been destroyed or was it not retrieved correctly?");
            return false;
        }

        if (prefab == null)
        {
            Dbg.Error(Logging.Entities, "No prefab found - has it been configured?");
            Object.Destroy(entityObj);
            return false;
        }

        if (entityObj == null)
        {
            Dbg.Error(Logging.Entities, "Failed to create entity");
            return false;
        }

        if (!entityObj.CreateNewEntity(root, prefab))
        {
            Dbg.Error(Logging.Entities, "Failed to create entity");
            Object.Destroy(entityObj);
            return false;
        }

        if (!_entityPrefabsList.TryAdd(entityObj.EntityID, entityObj.EntityPrefab))
        {
            Dbg.Error(Logging.Entities, "Failed to add entity to list");
            Object.Destroy(entityObj);
            return false;
        }

        return true;
    }
}
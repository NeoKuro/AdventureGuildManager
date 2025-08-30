using System;
using System.Collections.Generic;

using Hzn.Framework;

using UnityEngine;

[Serializable]
public class EntityManagerData : MonoComponentData
{
    [SerializeField]
    private Entity _baseEntityPrefab;

    [SerializeField]
    private List<SEntityPrefabData> _entityPrefabs;
    
    public Entity BaseEntityPrefab => _baseEntityPrefab;
    public List<SEntityPrefabData> EntityPrefabs => _entityPrefabs;
}
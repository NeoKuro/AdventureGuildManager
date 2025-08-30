using System;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public struct SSpawnPointData
{
    public EEntityPrefabCategories spawnPointForTypes;
    public List<Transform>         spawnPoint;
}
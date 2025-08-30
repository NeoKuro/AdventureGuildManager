using System;
using System.Collections.Generic;

using Hzn.Framework;

using UnityEngine;

[Serializable]
public class SpawnManagerData : MonoComponentData
{
    [SerializeField]
    private List<SSpawnPointData> _spawnPoints = new List<SSpawnPointData>();
    
    public List<SSpawnPointData> SpawnPoints => _spawnPoints;
}
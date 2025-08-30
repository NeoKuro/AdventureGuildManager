using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Hzn.Framework;

using UnityEngine;

namespace Managers
{
    public class SpawnManager : MonoManager<SpawnManager, SpawnManagerData>
    {
        [SerializeField]
        private List<SSpawnPointData> _spawnPoints = new List<SSpawnPointData>();

        protected override void PostManagerCreated()
        {
            base.PostManagerCreated();

            LocalPlayerEntity player = null;
            if (!EntityManager.Get().CreateLocalPlayerEntity(out player))
            {
                Dbg.Error(Log.Spawning, "Failed to create local player entity");
                return;
            }

            if (player == null)
            {
                Dbg.Error(Log.Spawning, "Local Player Entity is null");
                return;
            }

            if (!TryGetSpawnPoint(EEntityPrefabCategories.Player, out Transform spawnPoint))
            {
                Dbg.Error(Log.Spawning, $"Failed to get spawn point for player entity");
                return;
            }
            
            player.MoveToPosition(spawnPoint.position, spawnPoint.rotation.eulerAngles);
            InputContextManager.Get().SetContextPriority(EInputContext.Game);
        }

        public bool TryGetSpawnPoint(EEntityPrefabCategories type, out Transform spawnPoint)
        {
            List<Transform> spawnPoints = _spawnPoints
                                          .Where(x => x.spawnPointForTypes.HasFlag(type))
                                          .SelectMany(x => x.spawnPoint)
                                          .ToList();
            spawnPoint = spawnPoints.GetRandom();
            return spawnPoint != null;
        }

        protected override bool SyncMonoComponentData(SpawnManagerData component)
        {
            if (component == null)
            {
                Dbg.Error(Log.Spawning, "SpawnManagerData is null");
                return false;
            }

            if (component.SpawnPoints == null || component.SpawnPoints.Count == 0)
            {
                Dbg.Error(Log.Spawning, $"[{nameof(SpawnManagerData)}.{nameof(SpawnManagerData.SpawnPoints)}] list is null or empty");
                return false;
            }
            
            _spawnPoints = component.SpawnPoints;
            return true;
        }
    }
}
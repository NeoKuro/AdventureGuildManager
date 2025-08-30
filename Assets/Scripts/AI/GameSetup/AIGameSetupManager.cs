using System;
using System.Collections.Generic;

using Hzn.Framework;

using Managers;

using UnityEngine;

namespace Hzn.AI.Core
{
    /// <summary>
    /// Purpose: Try load data from file.
    ///     Will then trigger all spawning of AI in the correct locations
    /// </summary>
    public class AIGameSetupManager : MonoManager<AIGameSetupManager, AIGameSetupData>, ICoreAIManager
    {
        private Dictionary<EEntityType, int> _newGameEntitiesToSpawn = new Dictionary<EEntityType, int>();

        public void ActivateAIManager()
        {
            if (LoadGameDataFromFile())
            {
                SpawnAIForExistingGame();
                return;
            }

            Dbg.Log(Log.AI, $"Activating [{nameof(AIGameSetupManager)}]");
            SpawnAIForNewGame();
        }

        /// <summary>
        /// Will be used for loading savefiles
        /// </summary>
        private bool LoadGameDataFromFile()
        {
            Dbg.Error(Log.AI, $"NOT YET IMPLEMENTED -- {nameof(LoadGameDataFromFile)}");
            return false;
        }

        private void SpawnAIForExistingGame()
        {
            Dbg.Error(Log.AI, $"NOT YET IMPLEMENTED -- {nameof(SpawnAIForExistingGame)}");
        }

        private void SpawnAIForNewGame()
        {
            Dbg.Log(Log.AI, $"Spawning AI For new Game {nameof(SpawnAIForNewGame)}");
            foreach (KeyValuePair<EEntityType, int> entityCountToSpawn in _newGameEntitiesToSpawn)
            {
                switch (entityCountToSpawn.Key)
                {
                    default:
                    case EEntityType.None:
                        Dbg.Error(Log.AI, $"No entity type was provided for [{nameof(EEntityType.None)}]");
                        break;
                    case EEntityType.Player:
                        Dbg.Error(Log.AI, $"Invalid AI Spawn Type for [{nameof(EEntityType.Player)}]");
                        break;
                    case EEntityType.Adventurer:
                        for (int i = 0; i < entityCountToSpawn.Value; i++)
                        {
                            SpawnManager.Get().TryGetSpawnPoint(EEntityPrefabCategories.Adventurer, out Transform spawnPointTransform);
                            if (EntityManager.Get().TryCreateAdventurerEntity(out AdventurerEntity adventurerEntity, spawnPointTransform))
                            {
                                Dbg.Log(Log.AI, $"Spawned Adventurer Entity at {spawnPointTransform.position}");
                                continue;
                            }
                            Dbg.Error(Log.AI, $"Failed to spawn Adventurer Entity at {spawnPointTransform.position}");
                        }

                        break;
                    case EEntityType.Staff:
                        break;
                    case EEntityType.NPC:
                        break;
                }
            }
        }

        protected override bool SyncMonoComponentData(AIGameSetupData component)
        {
            _newGameEntitiesToSpawn = component.EntityNewGameSpawnSettings;
            return true;
        }
    }
}

    using System;
    using System.Collections.Generic;

    using Hzn.Framework;

    using UnityEngine;

    namespace Hzn.AI.Core
    {
         public class AIGameSetupData : MonoComponentData
         {
             [SerializeField]
             private Dictionary<EEntityType, int> _entityNewGameSpawnSettings = new Dictionary<EEntityType, int>();
             
             public Dictionary<EEntityType, int> EntityNewGameSpawnSettings => _entityNewGameSpawnSettings;
         }
    }

    using System.Collections.Generic;

    using Hzn.Framework;

    using UnityEngine;

    public class AIBehaviourManager : Manager<AIBehaviourManager>, ITickable, ITickableUpdate, ICoreAIManager
    {
        private const float         BEHAVIOUR_UPDATE_PERIOD = 5f;

        private float         _nextUpdateTime = 0f;
        private List<IAICore> _aiCore         = new List<IAICore>();
        
        public void ActivateAIManager()
        {
            Dbg.Log(Log.AI, "Activating AI Behaviour Manager");
        }
        
        public void Update(float deltaT)
        {
            if (Time.time < _nextUpdateTime)
            {
                return;
            }
            
            for (int i = 0; i < _aiCore.Count; i++)
            {
                _aiCore[i]?.FixedUpdateBehaviour();
            }
            
            _nextUpdateTime = Time.time + BEHAVIOUR_UPDATE_PERIOD;
        }

        public static void RegisterNewAI(IAICore ai)
        {
            if (Get() == null)
            {
                return;
            }
            
            Get()._aiCore.Add(ai);
        }

        public static void UnregisterAI(IAICore ai)
        {
            if (Get() == null)
            {
                return;
            }
            
            Get()._aiCore.Remove(ai);
        }
    }
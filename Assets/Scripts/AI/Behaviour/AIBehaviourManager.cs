
    using System.Collections.Generic;

    using Hzn.Framework;

    public class AIBehaviourManager : Manager<AIBehaviourManager>, ITickable, ITickableUpdate, ICoreAIManager
    {
        private List<IAICore> _aiCore = new List<IAICore>();
        
        public void ActivateAIManager()
        {
            Dbg.Log(Log.AI, "Activating AI Behaviour Manager");
        }
        
        public void Update(float deltaT)
        {
            for (int i = 0; i < _aiCore.Count; i++)
            {
                _aiCore[i]?.UpdateBehaviour();
            }
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
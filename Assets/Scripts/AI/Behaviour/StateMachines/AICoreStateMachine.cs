
    using Hzn.Framework;

    public abstract class AICoreStateMachine : StateMachine
    {
        protected AIIdleStateMachine _idleStateMachine;

        protected State AwakeState;
        protected State IdleState;
        protected State DestroyState;
        
        public AICoreStateMachine(string stateMachineName = nameof(AICoreStateMachine), bool tickable = false) : base(stateMachineName, tickable)
        {
            SetupAIBehaviours();
        }

        private void SetupAIBehaviours()
        {
            SetBehaviours();
        }

        protected virtual void SetBehaviours()
        {
            CreateAndAddState(ref AwakeState, nameof(AwakeState));
            CreateAndAddState(ref DestroyState, nameof(DestroyState));
            CreateAndAddState(ref IdleState, nameof(IdleState));
            
            _idleStateMachine = new AIIdleStateMachine();
            
            AwakeState
                .DeclareStartState()
                .OnEntry(OnEnterAwake)
                .AllowTransitionTo(IdleState);

            IdleState
                .DeclareIdleState()
                .OnEntry(OnEnterIdle)
                .OnExit(OnExitIdle)
                .AllowTransitionTo(DestroyState);

            DestroyState
                .DeclareExitState()
                .OnEntry(OnEnterDestroy);
        }
        
        protected virtual void OnEnterAwake()
        {
            Dbg.Log(Log.AI, "ENTERED: Awake State");
        }
        
        protected virtual void OnEnterIdle()
        {
            Dbg.Log(Log.AI, "ENTERED: Idle State");
            EnterSubStateMachine(_idleStateMachine, _ => SelectNextStateAfterIdle());
        }

        protected virtual void SelectNextStateAfterIdle()
        {
            Dbg.Log(Log.AI, "COMPLETE: Idle State. Selecting next state");
        }
        
        protected virtual void OnExitIdle()
        {
            Dbg.Log(Log.AI, "EXITING: Idle State");
        }
        
        protected virtual void OnEnterDestroy()
        {
            Dbg.Log(Log.AI, "ENTERED: Destroy State");
        }
    }
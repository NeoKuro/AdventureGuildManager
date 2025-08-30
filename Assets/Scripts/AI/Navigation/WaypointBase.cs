
    using Hzn.Framework;

    using UnityEngine;

    public abstract class WaypointBase : MonoBehaviour
    {
        [SerializeField]
        protected EEntityType _entityAvailability;
        
        public EEntityType EntityAvailability => _entityAvailability;

        protected WaypointManager _waypointManager;
        
        private void Awake()
        {
            _waypointManager = WaypointManager.Get();
            if (_waypointManager == null)
            {
                Dbg.Error(Log.AI, "LandmarkWaypoint: WaypointManager not found!");
                return;
            }
            RegisterNewWaypoint();
        }

        protected virtual void RegisterNewWaypoint()
        {
            WaypointManager.RegisterNewWaypoint(this);
        }
        
    }
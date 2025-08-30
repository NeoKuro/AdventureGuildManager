
    using System;
    using System.Collections.Generic;

    using Hzn.Framework;

    public class WaypointManager : Manager<WaypointManager>, ICoreAIManager
    {
        
        private Dictionary<EEntityType, List<LandmarkWaypoint>> _allWaypoints = new Dictionary<EEntityType, List<LandmarkWaypoint>>();


        #region -- MANAGER IMPLEMENTATION --

        private void RegisterWaypoint(WaypointBase waypoint)
        {
            foreach (EEntityType flag in Enum.GetValues(typeof(EEntityType)))
            {
                if (flag == EEntityType.None)
                {
                    continue;
                }

                if ((waypoint.EntityAvailability & flag) != flag)
                {
                    continue;
                }
                
                if (!_allWaypoints.ContainsKey(flag))
                {
                    _allWaypoints.Add(flag, new List<LandmarkWaypoint>());
                }
                
                _allWaypoints[flag].Add((LandmarkWaypoint)waypoint);
                Dbg.LogVerbose(Log.AI, $"WaypointManager: Registered waypoint {waypoint.name} for entity type {flag.ToString()}");
            }
        }

        #endregion
        
        #region -- PUBLIC STATIC API --

        public static void RegisterNewWaypoint(WaypointBase waypoint)
        {
            Get()?.RegisterWaypoint(waypoint);
        }

        #endregion


        public void ActivateAIManager()
        {
            Dbg.Log(Log.Manager, "Activating AI Waypoint Manager");
        }
    }
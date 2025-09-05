using Hzn.Framework;

using UnityEngine;

public static class Dbg_Categories
{
    public static readonly string[] ActiveProjectCategories = new string[]
                                                              {
                                                                  Logging.DataStore,
                                                                  Logging.Entities,
                                                                  Logging.Guild,
                                                                  Logging.Interactables,
                                                                  Logging.Inventory,
                                                                  Logging.Town,
                                                                  Logging.Utils,
                                                                  Logging.Wilderness
                                                              };

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void RegisterCategories()
    {
        Dbg.ActiveCategories.AddRange(ActiveProjectCategories);
    }
}

namespace Hzn.Framework
{
    public static partial class Logging
    {
        public static readonly string DataStore     = "[DATASTORE]";
        public static readonly string Entities      = "[ENTITY]";
        public static readonly string Guild         = "[GUILD]";
        public static readonly string Interactables = "[INTERACTABLE]";
        public static readonly string Inventory     = "[INVENTORY]";
        public static readonly string Town          = "[TOWN]";
        public static readonly string Utils         = "[UTILS]";
        public static readonly string Wilderness    = "[WILDERNESS]";
    }
}
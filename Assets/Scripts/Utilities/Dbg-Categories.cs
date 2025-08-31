using Hzn.Framework;

using UnityEngine;

public static class Dbg_Categories
{
    public static readonly string[] ActiveProjectCategories = new string[]
                                                              {
                                                                  Logging.DataStore,
                                                                  Logging.Entities,
                                                                  Logging.Interactables,
                                                                  Logging.Utils
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
        public static readonly string Interactables = "[INTERACTABLE]";
        public static readonly string Inventory     = "[INVENTORY]";
        public static readonly string Utils         = "[UTILS]";
    }
}
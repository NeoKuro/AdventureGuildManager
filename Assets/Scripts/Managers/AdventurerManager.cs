using System.Collections.Generic;
using System.Threading.Tasks;

using Hzn.Framework;

public class AdventurerManager : Manager<AdventurerManager>
{
    private int                   _dbgAdventurersGenerated = 0;
    private List<SAdventurerData> _adventurers             = new List<SAdventurerData>();


    #region -- API IMPLEMENTATION --

    private void AddAdventurer_Instance(SAdventurerData adventurer)
    {
        if (_adventurers.Contains(adventurer))
        {
            Dbg.Error(Logging.Entities, $"Adventurer {adventurer.Name} already exists!");
            return;
        }

        _adventurers.Add(adventurer);
        Dbg.Log(Logging.Entities, $"Added Adventurer {adventurer.Name}");
    }

    private void RemoveAdventurer_Instance(SAdventurerData adventurer)
    {
        if (!_adventurers.Contains(adventurer))
        {
            Dbg.Error(Logging.Entities, $"Adventurer {adventurer.Name} does not exist!");
            return;
        }

        _adventurers.Remove(adventurer);
        Dbg.Log(Logging.Entities, $"Removed Adventurer {adventurer.Name}");
    }

    private string GenerateNewAdventurerName_Instance()
    {
        return $"Adventurer Heroson {++_dbgAdventurersGenerated}";
    }

    #endregion


    #region -- API --

    public static void AddAdventurer(SAdventurerData adventurer)
    {
        Get().AddAdventurer_Instance(adventurer);
    }

    public static void RemoveAdventurer(SAdventurerData adventurer)
    {
        Get().RemoveAdventurer_Instance(adventurer);
    }

    public static string GenerateNewAdventurerName()
    {
        return Get().GenerateNewAdventurerName_Instance();
    }

    #endregion


}
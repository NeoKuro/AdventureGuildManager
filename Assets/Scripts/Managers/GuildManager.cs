using System.Collections.Generic;
using System.Threading.Tasks;

using Hzn.Framework;

public class GuildManager : Manager<GuildManager>
{
    private List<AdventurerEntity> _registeredAdventurers = new List<AdventurerEntity>();

    public async override void PreInitialSceneLoaded()
    {
        base.PreInitialSceneLoaded();
        // Load all registered adventurers from file async
        await LoadAdventurersFromFile();
    }


    #region -- DATA PROCESS --

    private async Task LoadAdventurersFromFile()
    {
        Dbg.Error(Logging.Entities, "TODO - IMPLEMENT LOAD ADVENTURERS");
    }

    private void ProcessNewAdventurer(AdventurerEntity adventurer)
    {
        if (_registeredAdventurers.Contains(adventurer))
        {
            Dbg.Error(Logging.Entities, $"Trying to register new Adventurer {adventurer.AdventurerData.Name} already registered!");
            return;
        }
        
        _registeredAdventurers.Add(adventurer);
    }

    #endregion
    

    #region -- DATA ACCESS --

    public bool TryGetAdventurer_ByName(string name, out AdventurerEntity adventurer)
    {
        adventurer = null;
        for (int i = 0; i < _registeredAdventurers.Count; i++)
        {
            if (_registeredAdventurers[i].AdventurerData.Name == name)
            {
                adventurer = _registeredAdventurers[i];
                return true;
            }
        }

        return false;
    }

    public bool TryGetAdventurers_ByClass(EAdventurerClass adventurerClass, out AdventurerEntity adventurer)
    {
        adventurer = null;
        for (int i = 0; i < _registeredAdventurers.Count; i++)
        {
            if (_registeredAdventurers[i].AdventurerData.Class == adventurerClass)
            {
                adventurer = _registeredAdventurers[i];
                return true;
            }
        }
        
        return false;
    }

    public bool TryGetAdventurers_ByRace(ECharacterRace race, out AdventurerEntity adventurer)
    {
        adventurer = null;
        for (int i = 0; i < _registeredAdventurers.Count; i++)
        {
            if (_registeredAdventurers[i].AdventurerData.Race == race)
            {
                adventurer = _registeredAdventurers[i];
                return true;
            }
        }
        
        return false;
    }

    #endregion


    #region -- PUBLIC API --

    public static void RegisterNewAdventurer(AdventurerEntity adventurer)
    {
        Get()?.ProcessNewAdventurer(adventurer);
    }

    #endregion
}
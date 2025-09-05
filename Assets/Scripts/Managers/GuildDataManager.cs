using System.Collections.Generic;
using System.Threading.Tasks;

using Hzn.Framework;

public class GuildDataManager : Manager<GuildDataManager>
{
    private List<Adventurer_AIEntity> _registeredAdventurers = new List<Adventurer_AIEntity>();

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

    private void ProcessNewAdventurer(Adventurer_AIEntity adventurerAI)
    {
        if (_registeredAdventurers.Contains(adventurerAI))
        {
            Dbg.Error(Logging.Entities, $"Trying to register new Adventurer {adventurerAI.AdventurerData.Name} already registered!");
            return;
        }
        
        _registeredAdventurers.Add(adventurerAI);
    }

    #endregion
    

    #region -- DATA ACCESS --

    public bool TryGetAdventurer_ByName(string name, out Adventurer_AIEntity adventurerAI)
    {
        adventurerAI = null;
        for (int i = 0; i < _registeredAdventurers.Count; i++)
        {
            if (_registeredAdventurers[i].AdventurerData.Name == name)
            {
                adventurerAI = _registeredAdventurers[i];
                return true;
            }
        }

        return false;
    }

    public bool TryGetAdventurers_ByClass(EAdventurerClass adventurerClass, out Adventurer_AIEntity adventurerAI)
    {
        adventurerAI = null;
        for (int i = 0; i < _registeredAdventurers.Count; i++)
        {
            if (_registeredAdventurers[i].AdventurerData.Class == adventurerClass)
            {
                adventurerAI = _registeredAdventurers[i];
                return true;
            }
        }
        
        return false;
    }

    public bool TryGetAdventurers_ByRace(ECharacterRace race, out Adventurer_AIEntity adventurerAI)
    {
        adventurerAI = null;
        for (int i = 0; i < _registeredAdventurers.Count; i++)
        {
            if (_registeredAdventurers[i].AdventurerData.Race == race)
            {
                adventurerAI = _registeredAdventurers[i];
                return true;
            }
        }
        
        return false;
    }

    #endregion


    #region -- PUBLIC API --

    public static void RegisterNewAdventurer(Adventurer_AIEntity adventurerAI)
    {
        Get()?.ProcessNewAdventurer(adventurerAI);
    }

    #endregion
}
using System.Text;

using Hzn.Framework;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class NewAdventurerFormController : WorldspaceUI
{
    [Header("UI Element References")]
    [SerializeField]
    private TMP_Text _basicStatsText;

    [SerializeField]
    private TMP_Text _classText;
    [SerializeField]
    private Image _classIcon;
    [SerializeField]
    private TMP_Text _statsText;
    [SerializeField]
    private GameObject _setRankPrompt;
    [SerializeField]
    private GameObject _unknownRankIcon;
    [SerializeField]
    private TMP_Text _rankLetter;
    
    [Header("Class Icon Assignments")]
    [SerializeField]
    private Sprite _warriorIcon;
    [SerializeField]
    private Sprite _mageIcon;
    [SerializeField]
    private Sprite _rogueIcon;
    [SerializeField]
    private Sprite _rangerIcon;
    [SerializeField]
    private Sprite _healerIcon;

    private ReceptionWorkstation _owningReceptionistWorkstation;
    private SAdventurerData      _newAdventurerData;

    public void StartNewAdventurerForm()
    {
        _newAdventurerData = new SAdventurerData(AdventurerManager.GenerateNewAdventurerName());
        SetNewAdventurerData();
    }

    public void StartNewAdventurerForm(SAdventurerData adventurerData)
    {
        _newAdventurerData = adventurerData;
        SetNewAdventurerData();
        gameObject.SetActive(true);
    }

    public void OnSignPressed()
    {
        _owningReceptionistWorkstation.OnSignNewAdventurer(_newAdventurerData);
        gameObject.SetActive(false);
    }

    public void OnRejectedPressed()
    {
        _owningReceptionistWorkstation.OnRejectCurrentAdventurer();
        gameObject.SetActive(false);
    }

    public void OnSetRankPressed()
    {
        RankSelectorUI.ShowRankSelector(OnRankSet);
    }

    private void OnRankSet(string rank)
    {
        _unknownRankIcon.SetActive(false);
        _rankLetter.text = rank;
        _rankLetter.gameObject.SetActive(true);
    }
    

    public void RegisterReceptionWorkstation(ReceptionWorkstation receptionWorkstation)
    {
        _owningReceptionistWorkstation = receptionWorkstation;
    }

    private void SetNewAdventurerData()
    {
        _basicStatsText.text = SetBasicStatsText();
        _statsText.text = SetAttributeStatsText();
    }

    private string SetBasicStatsText()
    {
        StringBuilder sb = StringTools.sharedStringBuilder;
        sb.Clear();
        // NAME
        sb.Append($"{_newAdventurerData.Name}");
        // RACE
        sb.Append($"{_newAdventurerData.Race.ToString()}");
        // LEVEL
        sb.Append($"{_newAdventurerData.Level.ToString()}");
        // CLASS
        sb.Append(_newAdventurerData.Class);
        return sb.ToString();
    }

    private string SetAttributeStatsText()
    {
        StringBuilder sb = StringTools.sharedStringBuilder;
        sb.Clear();
        // HEALTH
        sb.Append($"{_newAdventurerData.Stats.Health.ToString()}");
        // STRENGTH
        sb.Append($"{_newAdventurerData.Stats.Strength.ToString()}");
        // MAGIC
        sb.Append($"{_newAdventurerData.Stats.Magic.ToString()}");
        // SPEED
        sb.Append($"{_newAdventurerData.Stats.Speed.ToString()}");
        return sb.ToString();
    }
}

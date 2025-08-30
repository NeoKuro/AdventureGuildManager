////  PROJECT SUPREMACY    
//// Author: Josh Hughes   
////  Twitch.tv/Neokuro 


//using Newtonsoft.Json;
//using NUnit.Framework;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Management;
//using System.Text.RegularExpressions;
//using Unity.VisualScripting;
//using UnityEditor;
//using UnityEngine;

using UnityEditor;

public class EntityEditorWindow : ICustomWindow
{
    //    private Vector2 sidebarScrollView;
    //    private Vector2 mainScrollView;

    //    private BuildingEntity _defaultEntity = new BuildingEntity(-1);
    //    private BuildingEntity _currentBuilding;
    //    private List<string> tabNames = new List<string>();
    //    private Dictionary<BuildingEntity, bool> _allBuildings = new Dictionary<BuildingEntity, bool>();
    //    private List<BuildingEntity> _pendingBuildingUpdate = new List<BuildingEntity>();

    //    private UnitEntity _currentUnit;
    //    private Dictionary<UnitEntity, bool> _allUnits = new Dictionary<UnitEntity, bool>();
    //    private List<UnitEntity> _pendingUnitUpdates = new List<UnitEntity>();

    //    //private Dictionary<BuildingEntity, bool> _allBuildings = new Dictionary<BuildingEntity, bool>();
    //    //private List<BuildingEntity> _pendingBuildingUpdate = new List<BuildingEntity>();

    //    private UI_Menu _uiMenuLoc = UI_Menu.NONE;
    //    private string _uiMenuOrder = "-1";
    //    private string _descTxt = "";
    //    private string _nameTxt = "";
    //    private string _factionTxt = "";
    //    private string _maxHealthTxt = "0";
    //    private GameObject _entityPrefabRef = null;
    //    private string _constructionCostTxt = "0";

    //    // BUILDINGS ONLY
    //    private string _powerConsumptionTxt = "0";
    //    private bool _ignoreLowPowerMode = false;
    //    private string _resourceProductionTxt = "0";

    //    // UNITS ONLY
    //    private string _unitMoveSpeedTxt = "1";
    //    private string _unitTurnRateRadiansTxt = "1";
    //    private string _unitDamageAmountTxt = "10";
    //    private string _unitAttackCooldownTxt = "2";
    //    private string _unitAttackAoERangeTxt = "0";
    //    private Unit_Armour_Type _unitArmourType = Unit_Armour_Type.NONE;
    //    private List<string> _unitDamageRatios = new List<string>();

    //    private int _nextEntityIndex = 0;
    //    private EntityEditorType _tabType = EntityEditorType.NONE;
    //    private EntityEditorType _prevTab = EntityEditorType.NONE;

    //    private EditorWindow _thisWindow;
    //    private JsonSerializerSettings _jsonSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };


    //    public void InitWindow(EditorWindow window, object[] data)
    //    {
    //        _thisWindow = window;
    //        if (data != null && data.Length == 1)
    //        {
    //            //jsonRootPath = (string)data[0];
    //        }
    //        else
    //        {
    //            Debug.LogErrorFormat("We have been supplied with no data, or more data than expected!");
    //        }

    //        _thisWindow.titleContent = new GUIContent("Entity Editor");
    //        _currentBuilding = new BuildingEntity(-1);
    //        _currentUnit = new UnitEntity(-1);



    //        foreach (Unit_Armour_Type item in Enum.GetValues(typeof(Unit_Armour_Type)))
    //        {
    //            _unitDamageRatios.Add("1");
    //        }

    //        _thisWindow.minSize = new Vector2(825f, 300f);
    //        _thisWindow.maxSize = new Vector2(825f, 300f);
    //    }

    //    public void OnDestroy()
    //    {

    //    }

    //    public void OnEnable()
    //    {
    //        //LoadAssetsOfType("building");
    //        tabNames = Enum.GetNames(typeof(EntityEditorType)).ToList();
    //        tabNames.Remove(EntityEditorType.NONE.ToString());
    //    }

    //    private void LoadAssetsOfType<T>(string entityType, EntityEditorType type) where T : Entity
    //    {
    //        _allBuildings.Clear();
    //        // Reload the JSON files containing our entity data
    //        foreach (string filePath in Directory.EnumerateFiles(GameManager.JSON_ROOT_PATH, "*.json", SearchOption.AllDirectories))
    //        {
    //            // Got a filepath to a JSON
    //            //  Deserialise the JSON int
    //            if (filePath.ToLower().Contains(entityType.ToLower()))
    //            {
    //                try
    //                {
    //                    string factionEntityJson = File.ReadAllText(filePath);
    //                    //var entities = Newtonsoft.Json.JsonConvert.DeserializeObject(factionEntityJson, _jsonSettings);
    //                    Dictionary<string, T> entity = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, T>>(factionEntityJson, _jsonSettings); ;
    //                    //Dictionary<string, T> entity = new Dictionary<string, T>();
    //                    switch (type)
    //                    {
    //                        case EntityEditorType.BUILDINGS:
    //                            _allBuildings = entity.ToDictionary(pair => pair.Value as BuildingEntity, pair => false);
    //                            break;
    //                        case EntityEditorType.UNITS:
    //                            _allUnits = entity.ToDictionary(pair => pair.Value as UnitEntity, pair => false);
    //                            break;
    //                    }

    //                    int tmpMax = 0;
    //                    foreach (T item in entity.Values)
    //                    {
    //                        if (item.id > tmpMax)
    //                        {
    //                            tmpMax = item.id;
    //                        }
    //                    }

    //                    _nextEntityIndex = tmpMax + 1;
    //                }
    //                catch (Exception ex)
    //                {
    //                    DebugManager.Error("Failed to deserialise JSON at filepath {0} to BUildingEntity.\n{1}", filePath, ex.Message);
    //                }
    //            }
    //        }
    //    }

    //    public void OnGUI()
    //    {
    //        // Vertical 1 OPEN
    //        GUILayout.BeginVertical();
    //        // HORIZTONTAL 1 OPEN
    //        GUILayout.BeginHorizontal();

    //        _tabType = (EntityEditorType)GUILayout.Toolbar((int)_tabType, tabNames.ToArray(), GUILayout.Width(250f), GUILayout.Height(15f));

    //        // HORIZONTAL 1 CLOSE
    //        GUILayout.EndHorizontal();
    //        GUILayout.Space(10f);

    //        //Dictionary<BuildingEntity, bool> updatedBuildings = new Dictionary<BuildingEntity, bool>();
    //        //Dictionary<UnitEntity, bool> updatedUnits = new Dictionary<UnitEntity, bool>();
    //        //switch (_tabType)
    //        //{
    //        //    case EntityEditorType.BUILDINGS:

    //        //        if (_tabType != _prevTab)
    //        //        {
    //        //            LoadAssetsOfType<BuildingEntity>("building", EntityEditorType.BUILDINGS);
    //        //        }

    //        //        BuildingEntityEditorView(ref updatedBuildings);
    //        //        break;
    //        //    case EntityEditorType.UNITS:

    //        //        if (_tabType != _prevTab)
    //        //        {
    //        //            LoadAssetsOfType<UnitEntity>("unit", EntityEditorType.UNITS);
    //        //        }

    //        //        UnitEntityEditorView(ref updatedUnits);
    //        //        break;
    //        //}

    //        //_prevTab = _tabType;

    //        //if (_tabType != EntityEditorType.NONE)
    //        //{
    //        //    // HORIZONTAL 3 OPEN
    //        //    GUILayout.BeginHorizontal();

    //        //    if (GUILayout.Button("Save"))
    //        //    {
    //        //        OnSave();
    //        //        GUILayout.EndHorizontal();
    //        //        GUILayout.EndVertical();
    //        //        return;
    //        //    }

    //        //    if (GUILayout.Button("Save All"))
    //        //    {
    //        //        OnSaveAll();
    //        //        GUILayout.EndHorizontal();
    //        //        GUILayout.EndVertical();
    //        //        return;
    //        //    }

    //        //    if (GUILayout.Button("Cancel"))
    //        //    {

    //        //    }

    //        //    if (GUILayout.Button("Delete"))
    //        //    {
    //        //        updatedBuildings.Remove(_currentBuilding);
    //        //        _currentBuilding = _defaultEntity;
    //        //    }
    //        //    // HORIZONTAL 3 CLOSE
    //        //    GUILayout.EndHorizontal();
    //        //}
    //        // VERTICAL 1 CLOSE
    //        GUILayout.EndVertical();

    //        //_allBuildings = updatedBuildings;
    //        //_allUnits = updatedUnits;
    //    }

    //    private void BuildingEntityEditorView(ref Dictionary<BuildingEntity, bool> updated)
    //    {
    //        // HORIZONTAL 2 OPEN
    //        GUILayout.BeginHorizontal();
    //        // SIDEBAR SCROLL VIEW OPEN
    //        sidebarScrollView = EditorGUILayout.BeginScrollView(sidebarScrollView, GUILayout.Width(200f));
    //        GUIStyle sideBarStyle = new GUIStyle();
    //        sideBarStyle.stretchWidth = false;

    //        if (GUILayout.Button("+"))
    //        {
    //            OnAddNewBuilding();
    //        }


    //        IOrderedEnumerable<KeyValuePair<BuildingEntity, bool>> sortedDictionary = from entry in _allBuildings orderby entry.Key.id ascending select entry;
    //        _allBuildings = sortedDictionary.ToDictionary(pair => pair.Key, pair => pair.Value);


    //        updated = _allBuildings.ToDictionary(entry => entry.Key, entry => entry.Value);
    //        foreach (KeyValuePair<BuildingEntity, bool> entity in _allBuildings)
    //        {

    //            Color prevCol = GUI.color;

    //            GUI.color = entity.Value ? Color.red : Color.white;
    //            if (GUILayout.Button(entity.Key.faction + "/" + entity.Key.name))
    //            {
    //                if (entity.Key.id != _currentBuilding.id)
    //                {
    //                    BuildingEntity tmp = OnChangeSelectedBuilding(entity.Key);
    //                    if (tmp.id != _defaultEntity.id)
    //                    {
    //                        BuildingEntity toRemove = updated.Where(pair => { return pair.Key.id == tmp.id; }).FirstOrDefault().Key;

    //                        bool modified = toRemove != tmp;

    //                        for (int i = 0; i < _pendingBuildingUpdate.Count; i++)
    //                        {
    //                            if (_pendingBuildingUpdate[i] == tmp)
    //                            {
    //                                modified = true;
    //                                break;
    //                            }
    //                        }

    //                        updated.Remove(toRemove);
    //                        updated.Add(tmp, modified);

    //                        if (modified && !_pendingBuildingUpdate.Contains(tmp))
    //                        {
    //                            _pendingBuildingUpdate.Add(tmp);
    //                        }
    //                    }
    //                }
    //            }
    //            GUI.color = prevCol;
    //        }

    //        // SIDEBAR SCROLL VIEW CLOSE
    //        EditorGUILayout.EndScrollView();
    //        // MAIN VIEW SCROLL OPEN
    //        mainScrollView = EditorGUILayout.BeginScrollView(mainScrollView, GUILayout.Width(900f));

    //        if (_currentBuilding.id != -1)
    //        {
    //            // VERTICAL 2 OPEN
    //            GUILayout.BeginVertical();

    //            GUILayout.Label("Entity Editor");
    //            GUILayout.Space(25);

    //            // NAME
    //            GUILayout.BeginHorizontal();
    //            GUILayout.Label("Entity Name: ", GUILayout.Width(200f));
    //            _nameTxt = GUILayout.TextField(_nameTxt, GUILayout.Width(400f));
    //            GUILayout.EndHorizontal();

    //            // FACTION
    //            GUILayout.BeginHorizontal();
    //            GUILayout.Label("Entity Faction: ", GUILayout.Width(200f));
    //            _factionTxt = GUILayout.TextField(_factionTxt, GUILayout.Width(400f));
    //            GUILayout.EndHorizontal();

    //            // UI LOCATION
    //            GUILayout.BeginHorizontal();
    //            GUILayout.Label("UI Menu Location: ", GUILayout.Width(200f));
    //            _uiMenuLoc = (UI_Menu)EditorGUILayout.EnumPopup(_uiMenuLoc);
    //            GUILayout.EndHorizontal();

    //            // MENU ORDER INDEX
    //            GUILayout.BeginHorizontal();
    //            GUILayout.Label("UI Menu Order Index: ", GUILayout.Width(200f));
    //            _uiMenuOrder = GUILayout.TextField(_uiMenuOrder, GUILayout.Width(400f));
    //            _uiMenuOrder = Regex.Replace(_uiMenuOrder, "[^-][^0-9]+", "");
    //            GUILayout.EndHorizontal();

    //            // DESCRIPTION
    //            GUILayout.BeginHorizontal();
    //            GUILayout.Label("Entity Description: ", GUILayout.Width(200f));
    //            _descTxt = GUILayout.TextArea(_descTxt, GUILayout.Width(400f), GUILayout.Height(45f));
    //            GUILayout.EndHorizontal();

    //            // MAX HEALTH
    //            GUILayout.BeginHorizontal();
    //            GUILayout.Label("Max Health: ", GUILayout.Width(200f));
    //            _maxHealthTxt = GUILayout.TextField(_maxHealthTxt, GUILayout.Width(400f));
    //            _maxHealthTxt = Regex.Replace(_maxHealthTxt, "[^0-9]", "");
    //            GUILayout.EndHorizontal();

    //            // POWER CONSUMPTION
    //            GUILayout.BeginHorizontal();
    //            GUILayout.Label("Power Consumption: ", GUILayout.Width(200f));
    //            _powerConsumptionTxt = GUILayout.TextField(_powerConsumptionTxt, GUILayout.Width(400f));
    //            _powerConsumptionTxt = Regex.Replace(_powerConsumptionTxt, "[^-][^0-9]+", "");
    //            GUILayout.EndHorizontal();

    //            // LOW POWER SETTING
    //            GUILayout.BeginHorizontal();
    //            GUILayout.Label("Ignore Low Power Mode: ", GUILayout.Width(200f));
    //            _ignoreLowPowerMode = GUILayout.Toggle(_ignoreLowPowerMode, "");
    //            GUILayout.EndHorizontal();

    //            // PREFAB PATH TO FILE
    //            GUILayout.BeginHorizontal();
    //            GUILayout.Label("Path to building Prefab: ", GUILayout.Width(200f));
    //            _entityPrefabRef = (GameObject)EditorGUILayout.ObjectField(_entityPrefabRef, typeof(UnityEngine.Object), false, GUILayout.Width(400f));
    //            GUILayout.EndHorizontal();

    //            // CURRENCY COST
    //            GUILayout.BeginHorizontal();
    //            GUILayout.Label("Currency Cost: ", GUILayout.Width(200f));
    //            _constructionCostTxt = GUILayout.TextField(_constructionCostTxt, GUILayout.Width(400f));
    //            _constructionCostTxt = Regex.Replace(_constructionCostTxt, "[^0-9]", "");
    //            GUILayout.EndHorizontal();

    //            // CURRENCY GENERATION
    //            GUILayout.BeginHorizontal();
    //            GUILayout.Label("Currency Generation: ", GUILayout.Width(200f));
    //            _resourceProductionTxt = GUILayout.TextField(_resourceProductionTxt, GUILayout.Width(400f));
    //            _resourceProductionTxt = Regex.Replace(_resourceProductionTxt, "[^0-9]", "");
    //            GUILayout.EndHorizontal();

    //            // VERTICAL 2 CLOSE
    //            GUILayout.EndVertical();
    //        }

    //        // MAIN SCROLL VIEW CLOSE
    //        EditorGUILayout.EndScrollView();

    //        // HORIZONTAL 2 CLOSE
    //        GUILayout.EndHorizontal();
    //    }

    //    private void UnitEntityEditorView(ref Dictionary<UnitEntity, bool> updated)
    //    {
    //        // HORIZONTAL 2 OPEN
    //        GUILayout.BeginHorizontal();
    //        // SIDEBAR SCROLL VIEW OPEN
    //        sidebarScrollView = EditorGUILayout.BeginScrollView(sidebarScrollView, GUILayout.Width(200f));
    //        GUIStyle sideBarStyle = new GUIStyle();
    //        sideBarStyle.stretchWidth = false;

    //        if (GUILayout.Button("+"))
    //        {
    //            OnAddNewUnit();
    //        }


    //        IOrderedEnumerable<KeyValuePair<UnitEntity, bool>> sortedDictionary = from entry in _allUnits orderby entry.Key.id ascending select entry;
    //        _allUnits = sortedDictionary.ToDictionary(pair => pair.Key, pair => pair.Value);


    //        updated = _allUnits.ToDictionary(entry => entry.Key, entry => entry.Value);
    //        foreach (KeyValuePair<UnitEntity, bool> entity in _allUnits)
    //        {

    //            Color prevCol = GUI.color;

    //            GUI.color = entity.Value ? Color.red : Color.white;
    //            if (GUILayout.Button(entity.Key.faction + "/" + entity.Key.name))
    //            {
    //                if (entity.Key.id != _currentBuilding.id)
    //                {
    //                    UnitEntity tmp = OnChangeSelectedUnit(entity.Key);
    //                    if (tmp.id != _defaultEntity.id)
    //                    {
    //                        UnitEntity toRemove = updated.Where(pair => { return pair.Key.id == tmp.id; }).FirstOrDefault().Key;

    //                        bool modified = toRemove != tmp;

    //                        for (int i = 0; i < _pendingBuildingUpdate.Count; i++)
    //                        {
    //                            if (_pendingBuildingUpdate[i] == tmp)
    //                            {
    //                                modified = true;
    //                                break;
    //                            }
    //                        }

    //                        updated.Remove(toRemove);
    //                        updated.Add(tmp, modified);

    //                        if (modified && !_pendingUnitUpdates.Contains(tmp))
    //                        {
    //                            _pendingUnitUpdates.Add(tmp);
    //                        }
    //                    }
    //                }
    //            }
    //            GUI.color = prevCol;
    //        }

    //        // SIDEBAR SCROLL VIEW CLOSE
    //        EditorGUILayout.EndScrollView();
    //        // MAIN VIEW SCROLL OPEN
    //        mainScrollView = EditorGUILayout.BeginScrollView(mainScrollView, GUILayout.Width(900f));

    //        if (_currentUnit.id != -1)
    //        {
    //            // VERTICAL 2 OPEN
    //            GUILayout.BeginVertical();

    //            GUILayout.Label("Unit Entity Editor");
    //            GUILayout.Space(25);

    //            // NAME
    //            GUILayout.BeginHorizontal();
    //            GUILayout.Label("Entity Name: ", GUILayout.Width(200f));
    //            _nameTxt = GUILayout.TextField(_nameTxt, GUILayout.Width(400f));
    //            GUILayout.EndHorizontal();

    //            // FACTION
    //            GUILayout.BeginHorizontal();
    //            GUILayout.Label("Entity Faction: ", GUILayout.Width(200f));
    //            _factionTxt = GUILayout.TextField(_factionTxt, GUILayout.Width(400f));
    //            GUILayout.EndHorizontal();

    //            // UI LOCATION
    //            GUILayout.BeginHorizontal();
    //            GUILayout.Label("UI Menu Location: ", GUILayout.Width(200f));
    //            _uiMenuLoc = (UI_Menu)EditorGUILayout.EnumPopup(_uiMenuLoc);
    //            GUILayout.EndHorizontal();

    //            // MENU ORDER INDEX
    //            GUILayout.BeginHorizontal();
    //            GUILayout.Label("UI Menu Order Index: ", GUILayout.Width(200f));
    //            _uiMenuOrder = GUILayout.TextField(_uiMenuOrder, GUILayout.Width(400f));
    //            _uiMenuOrder = Regex.Replace(_uiMenuOrder, "[^-][^0-9]+", "");
    //            GUILayout.EndHorizontal();

    //            // DESCRIPTION
    //            GUILayout.BeginHorizontal();
    //            GUILayout.Label("Entity Description: ", GUILayout.Width(200f));
    //            _descTxt = GUILayout.TextArea(_descTxt, GUILayout.Width(400f), GUILayout.Height(45f));
    //            GUILayout.EndHorizontal();

    //            // MAX HEALTH
    //            GUILayout.BeginHorizontal();
    //            GUILayout.Label("Max Health: ", GUILayout.Width(200f));
    //            _maxHealthTxt = GUILayout.TextField(_maxHealthTxt, GUILayout.Width(400f));
    //            _maxHealthTxt = Regex.Replace(_maxHealthTxt, "[^0-9]", "");
    //            GUILayout.EndHorizontal();

    //            // PREFAB PATH TO FILE
    //            GUILayout.BeginHorizontal();
    //            GUILayout.Label("Path to unit Prefab: ", GUILayout.Width(200f));
    //            _entityPrefabRef = (GameObject)EditorGUILayout.ObjectField(_entityPrefabRef, typeof(UnityEngine.Object), false, GUILayout.Width(400f));
    //            GUILayout.EndHorizontal();

    //            // CURRENCY COST
    //            GUILayout.BeginHorizontal();
    //            GUILayout.Label("Currency Cost: ", GUILayout.Width(200f));
    //            _constructionCostTxt = GUILayout.TextField(_constructionCostTxt, GUILayout.Width(400f));
    //            _constructionCostTxt = Regex.Replace(_constructionCostTxt, "[^0-9]", "");
    //            GUILayout.EndHorizontal();

    //            // MOVE SPEED
    //            GUILayout.BeginHorizontal(GUILayout.Width(600f));
    //            GUILayout.BeginVertical();
    //            GUILayout.Label("Move Speed: ", GUILayout.Width(100f));
    //            _unitMoveSpeedTxt = GUILayout.TextField(_unitMoveSpeedTxt, GUILayout.Width(100f));
    //            _unitMoveSpeedTxt = Regex.Replace(_unitMoveSpeedTxt, "[^0-9]", "");
    //            GUILayout.EndVertical();
    //            GUILayout.Space(100f);
    //            GUILayout.BeginVertical();

    //            // TURN RATE
    //            GUILayout.Label("Turn Rate Rads: ", GUILayout.Width(100f));
    //            _unitTurnRateRadiansTxt = GUILayout.TextField(_unitTurnRateRadiansTxt, GUILayout.Width(100f));
    //            _unitTurnRateRadiansTxt = Regex.Replace(_unitTurnRateRadiansTxt, "[^0-9]", "");
    //            GUILayout.EndVertical();
    //            GUILayout.EndHorizontal();

    //            // DAMAGE AMOUNT
    //            GUILayout.BeginHorizontal(GUILayout.Width(600f));
    //            GUILayout.BeginVertical();
    //            GUILayout.Label("Damage Amount: ", GUILayout.Width(100f));
    //            _unitDamageAmountTxt = GUILayout.TextField(_unitDamageAmountTxt, GUILayout.Width(100f));
    //            _unitDamageAmountTxt = Regex.Replace(_unitDamageAmountTxt, "[^0-9]", "");
    //            GUILayout.EndVertical();

    //            // ATTACK COOLDOWN
    //            GUILayout.BeginVertical();
    //            GUILayout.Label("Attack Cooldown: ", GUILayout.Width(100f));
    //            _unitAttackCooldownTxt = GUILayout.TextField(_unitAttackCooldownTxt, GUILayout.Width(100f));
    //            _unitAttackCooldownTxt = Regex.Replace(_unitAttackCooldownTxt, "[^0-9]", "");
    //            GUILayout.EndVertical();

    //            // AOE RANGE
    //            GUILayout.BeginVertical();
    //            GUILayout.Label("AoE Range: ", GUILayout.Width(100f));
    //            _unitAttackAoERangeTxt = GUILayout.TextField(_unitAttackAoERangeTxt, GUILayout.Width(100f));
    //            _unitAttackAoERangeTxt = Regex.Replace(_unitAttackAoERangeTxt, "[^0-9]", "");
    //            GUILayout.EndVertical();
    //            GUILayout.EndHorizontal();

    //            // ARMOUR TYPE
    //            GUILayout.BeginHorizontal();
    //            GUILayout.Label("Armour Type: ", GUILayout.Width(200f));
    //            _unitArmourType = (Unit_Armour_Type)EditorGUILayout.EnumPopup(_unitArmourType);
    //            GUILayout.EndHorizontal();

    //            // LOOP - DAMAGE RATIOS
    //            foreach (Unit_Armour_Type armour_Type in Enum.GetValues(typeof(Unit_Armour_Type)))
    //            {
    //                GUILayout.BeginHorizontal();
    //                GUILayout.Label(string.Format("{0}: ", armour_Type.ToString()), GUILayout.Width(200f));
    //                _unitDamageRatios[(int)armour_Type] = GUILayout.TextField(_unitDamageRatios[(int)armour_Type], GUILayout.Width(400f));
    //                _unitDamageRatios[(int)armour_Type] = Regex.Replace(_unitDamageRatios[(int)armour_Type], "[^.][^0-9]+", "");
    //                GUILayout.EndHorizontal();
    //            }

    //            // VERTICAL 2 CLOSE
    //            GUILayout.EndVertical();
    //        }

    //        // MAIN SCROLL VIEW CLOSE
    //        EditorGUILayout.EndScrollView();

    //        // HORIZONTAL 2 CLOSE
    //        GUILayout.EndHorizontal();
    //    }


    //    private void OnAddNewBuilding()
    //    {
    //        BuildingEntity newBuilding = new BuildingEntity(_nextEntityIndex);
    //        _allBuildings.Add(newBuilding, true);
    //        _nextEntityIndex++;

    //        _currentBuilding = newBuilding;
    //        _entityPrefabRef = null;
    //        _uiMenuLoc = UI_Menu.NONE;
    //        _uiMenuOrder = "-1";
    //        _nameTxt = _factionTxt = _descTxt = "";
    //        _maxHealthTxt = _powerConsumptionTxt = _constructionCostTxt = _resourceProductionTxt = "0";
    //        _ignoreLowPowerMode = false;
    //    }

    //    private void OnAddNewUnit()
    //    {
    //        UnitEntity newUnit = new UnitEntity(_nextEntityIndex);
    //        _allUnits.Add(newUnit, true);
    //        _nextEntityIndex++;

    //        _currentUnit = newUnit; 
    //        _entityPrefabRef = null;
    //        _nameTxt = _factionTxt = _descTxt = "";
    //        _maxHealthTxt = _constructionCostTxt = "0";
    //        _unitMoveSpeedTxt = _unitTurnRateRadiansTxt = _unitDamageAmountTxt = _unitAttackCooldownTxt = _unitAttackAoERangeTxt = "0";
    //        _unitArmourType = Unit_Armour_Type.NONE;
    //        for (int i = 0; i < _unitDamageRatios.Count; i++)
    //        {
    //            _unitDamageRatios[i] = "1";
    //        }
    //    }

    //    private void OnSave()
    //    {
    //        switch (_tabType)
    //        {
    //            case EntityEditorType.BUILDINGS:
    //                BuildingEntity oldBuilding = _allBuildings.Where(x => { return x.Key.id == _currentBuilding.id; }).FirstOrDefault().Key;
    //                _allBuildings.Remove(oldBuilding);
    //                UpdateEntity<BuildingEntity>(ref _currentBuilding);
    //                _allBuildings.Add(_currentBuilding, false);
    //                break;
    //            case EntityEditorType.UNITS:
    //                UnitEntity oldUnit = _allUnits.Where(x => { return x.Key.id == _currentUnit.id; }).FirstOrDefault().Key;
    //                _allUnits.Remove(oldUnit);
    //                UpdateEntity<UnitEntity>(ref _currentUnit);
    //                _allUnits.Add(_currentUnit, false);
    //                break;
    //        }
    //    }

    //    private void OnSaveAll()
    //    {
    //        if (_currentBuilding.id != -1)
    //        {
    //            OnSave();
    //        }

    //        switch (_tabType)
    //        {
    //            case EntityEditorType.BUILDINGS:
    //                Dictionary<string, List<BuildingEntity>> factionSortedBuildingList = new Dictionary<string, List<BuildingEntity>>();
    //                foreach (BuildingEntity entity in _allBuildings.Keys)
    //                {
    //                    if (!factionSortedBuildingList.ContainsKey(entity.faction))
    //                    {
    //                        factionSortedBuildingList.Add(entity.faction, new List<BuildingEntity>());
    //                    }

    //                    factionSortedBuildingList[entity.faction].Add(entity);
    //                }
    //                WriteToJson<BuildingEntity>(GameManager.STOCK_BUILDINGS_FILE_NAME, factionSortedBuildingList);
    //                break;
    //            case EntityEditorType.UNITS:
    //                Dictionary<string, List<UnitEntity>> factionSortedUnitList = new Dictionary<string, List<UnitEntity>>();
    //                foreach (UnitEntity entity in _allUnits.Keys)
    //                {
    //                    if (!factionSortedUnitList.ContainsKey(entity.faction))
    //                    {
    //                        factionSortedUnitList.Add(entity.faction, new List<UnitEntity>());
    //                    }

    //                    factionSortedUnitList[entity.faction].Add(entity);
    //                }
    //                WriteToJson<UnitEntity>(GameManager.STOCK_UNITS_FILE_NAME, factionSortedUnitList);
    //                break;
    //        }

    //        _allBuildings = _allBuildings.ToDictionary(pair => pair.Key, pair => false);
    //        _allUnits = _allUnits.ToDictionary(pair => pair.Key, pair => false);
    //    }

    //    private void WriteToJson<T>(string fileName, Dictionary<string, List<T>> factionSortedList) where T : Entity
    //    {
    //        foreach (KeyValuePair<string, List<T>> kvpEntities in factionSortedList)
    //        {
    //            Dictionary<string, T> entities = kvpEntities.Value.ToDictionary(pair => pair.name, pair => pair);
    //            string jsonEntities = Newtonsoft.Json.JsonConvert.SerializeObject(entities, _jsonSettings);
    //            string path = string.Format(@"{0}\{1}\", GameManager.JSON_ROOT_PATH, kvpEntities.Key);
    //            Directory.CreateDirectory(path);
    //            File.WriteAllText(path + fileName, jsonEntities);
    //        }
    //    }

    //    private void UpdateEntity<T>(ref T toUpdate) where T : Entity
    //    {
    //        string path = AssetDatabase.GetAssetPath(_entityPrefabRef);
    //        List<GameResourcePair> list = new List<GameResourcePair>() { new GameResourcePair(Game_Resources.Currency, int.Parse(_constructionCostTxt)) };
    //        switch (_tabType)
    //        {
    //            case EntityEditorType.BUILDINGS:

    //                List<GameResourcePair> prod = new List<GameResourcePair>() { new GameResourcePair(Game_Resources.Currency, int.Parse(_resourceProductionTxt)) };

    //                BuildingEntity tmpBuilding = new BuildingEntity(_currentBuilding.id,
    //                    _factionTxt, _nameTxt, _descTxt, (int)_uiMenuLoc, int.Parse(_uiMenuOrder), path,
    //                    int.Parse(_maxHealthTxt), int.Parse(_powerConsumptionTxt), _ignoreLowPowerMode, list, prod);

    //                toUpdate = tmpBuilding as T;
    //                break;
    //            case EntityEditorType.UNITS:
    //                List<ArmourDamageRatio> damageRatios = new List<ArmourDamageRatio>();

    //                foreach (Unit_Armour_Type armour_Type in Enum.GetValues(typeof(Unit_Armour_Type)))
    //                {
    //                    damageRatios.Add(new ArmourDamageRatio() { armour = armour_Type, damageRatio = float.Parse(_unitDamageRatios[(int)armour_Type]) });
    //                }

    //                UnitEntity tmpUnit = new UnitEntity(_currentUnit.id,
    //                    _factionTxt, _nameTxt, _descTxt, (int)_uiMenuLoc, int.Parse(_uiMenuOrder),
    //                    path, int.Parse(_maxHealthTxt), list, int.Parse(_unitMoveSpeedTxt), int.Parse(_unitTurnRateRadiansTxt), _unitArmourType, int.Parse(_unitDamageAmountTxt),
    //                    float.Parse(_unitAttackCooldownTxt), float.Parse(_unitAttackAoERangeTxt), damageRatios);
    //                toUpdate = tmpUnit as T;
    //                break;
    //        }
    //    }

    //    private BuildingEntity OnChangeSelectedBuilding(BuildingEntity entity)
    //    {
    //        if (_currentBuilding.id == -1)
    //        {
    //            _currentBuilding = entity;

    //            _entityPrefabRef = AssetDatabase.LoadAssetAtPath<GameObject>(_currentBuilding.entityPrefabReference);
    //            _nameTxt = _currentBuilding.name;
    //            _factionTxt = _currentBuilding.faction;
    //            _descTxt = _currentBuilding.description;
    //            _uiMenuLoc = _currentBuilding.menuLocation;
    //            _uiMenuOrder = _currentBuilding.menuOrderIndex.ToString();
    //            _maxHealthTxt = _currentBuilding.maxHealth.ToString();
    //            _powerConsumptionTxt = _currentBuilding.powerConsumption.ToString();
    //            _constructionCostTxt = _currentBuilding.constructionCost.FirstOrDefault()._resourceCount.ToString();
    //            _resourceProductionTxt = _currentBuilding.resourceProduction.FirstOrDefault()._resourceCount.ToString();
    //            return _currentBuilding;
    //        }

    //        if (entity.id == -1)
    //        {
    //            _entityPrefabRef = null;
    //            _nameTxt = _factionTxt = _descTxt = "";
    //            _maxHealthTxt = _powerConsumptionTxt = _constructionCostTxt = _resourceProductionTxt = "0";
    //            _currentBuilding = entity;
    //            return _currentBuilding;
    //        }

    //        List<GameResourcePair> list = new List<GameResourcePair>() { new GameResourcePair(Game_Resources.Currency, int.Parse(_constructionCostTxt)) };
    //        List<GameResourcePair> prod = new List<GameResourcePair>() { new GameResourcePair(Game_Resources.Currency, int.Parse(_resourceProductionTxt)) };

    //        string path = AssetDatabase.GetAssetPath(_entityPrefabRef);

    //        BuildingEntity tmp = new BuildingEntity(_currentBuilding.id,
    //            _factionTxt, _nameTxt, _descTxt, (int)_uiMenuLoc, int.Parse(_uiMenuOrder),
    //            path, int.Parse(_maxHealthTxt), int.Parse(_powerConsumptionTxt), _ignoreLowPowerMode, list, prod);

    //        _currentBuilding = entity;
    //        _entityPrefabRef = AssetDatabase.LoadAssetAtPath<GameObject>(_currentBuilding.entityPrefabReference);
    //        _nameTxt = _currentBuilding.name;
    //        _factionTxt = _currentBuilding.faction;
    //        _descTxt = _currentBuilding.description;
    //        _uiMenuLoc = _currentBuilding.menuLocation;
    //        _uiMenuOrder = _currentBuilding.menuOrderIndex.ToString();
    //        _maxHealthTxt = _currentBuilding.maxHealth.ToString();
    //        _powerConsumptionTxt = _currentBuilding.powerConsumption.ToString();
    //        _constructionCostTxt = _currentBuilding.constructionCost.FirstOrDefault()._resourceCount.ToString();
    //        _resourceProductionTxt = _currentBuilding.resourceProduction.FirstOrDefault()._resourceCount.ToString();

    //        return tmp;
    //    }

    //    private UnitEntity OnChangeSelectedUnit(UnitEntity entity)
    //    {
    //        if (_currentBuilding.id == -1)
    //        {
    //            _currentUnit = entity;

    //            _entityPrefabRef = AssetDatabase.LoadAssetAtPath<GameObject>(_currentBuilding.entityPrefabReference);
    //            _nameTxt = _currentUnit.name;
    //            _factionTxt = _currentUnit.faction;
    //            _descTxt = _currentUnit.description;
    //            _uiMenuLoc = _currentUnit.menuLocation;
    //            _uiMenuOrder = _currentUnit.menuOrderIndex.ToString();
    //            _maxHealthTxt = _currentUnit.maxHealth.ToString();
    //            _constructionCostTxt = _currentUnit.constructionCost.FirstOrDefault()._resourceCount.ToString();

    //            _unitMoveSpeedTxt = _currentUnit.moveSpeed.ToString();
    //            _unitTurnRateRadiansTxt = _currentUnit.turnRateRadians.ToString();
    //            _unitDamageAmountTxt = _currentUnit.unitDamage.ToString();
    //            _unitAttackCooldownTxt = _currentUnit.unitAttackCooldownSeconds.ToString();
    //            _unitAttackAoERangeTxt = _currentUnit.unitAttackAoERange.ToString();
    //            _unitArmourType = _currentUnit.unitArmourType;
    //            _unitDamageRatios = _currentUnit.damageToArmourTypes.Select(x => x.damageRatio.ToString()).ToList();

    //            return _currentUnit;
    //        }

    //        if (entity.id == -1)
    //        {
    //            _entityPrefabRef = null;
    //            _nameTxt = _factionTxt = _descTxt = "";
    //            _maxHealthTxt = _constructionCostTxt = "0";
    //            _unitMoveSpeedTxt = _unitTurnRateRadiansTxt = _unitDamageAmountTxt = _unitAttackCooldownTxt = _unitAttackAoERangeTxt = "0";
    //            _unitArmourType = Unit_Armour_Type.NONE;
    //            for (int i = 0; i < _unitDamageRatios.Count; i++)
    //            {
    //                _unitDamageRatios[i] = "1";
    //            }
    //            _currentUnit = entity;
    //            return _currentUnit;
    //        }

    //        List<GameResourcePair> list = new List<GameResourcePair>() { new GameResourcePair(Game_Resources.Currency, int.Parse(_constructionCostTxt)) };
    //        List<ArmourDamageRatio> damageRatios = new List<ArmourDamageRatio>();

    //        foreach (Unit_Armour_Type armour_Type in Enum.GetValues(typeof(Unit_Armour_Type)))
    //        {
    //            damageRatios.Add(new ArmourDamageRatio() { armour = armour_Type, damageRatio = float.Parse(_unitDamageRatios[(int)armour_Type]) });
    //        }

    //        string path = AssetDatabase.GetAssetPath(_entityPrefabRef);

    //        UnitEntity tmp = new UnitEntity(_currentUnit.id,
    //            _factionTxt, _nameTxt, _descTxt, (int)_uiMenuLoc, int.Parse(_uiMenuOrder),
    //            path, int.Parse(_maxHealthTxt), list, int.Parse(_unitMoveSpeedTxt), int.Parse(_unitTurnRateRadiansTxt), _unitArmourType, int.Parse(_unitDamageAmountTxt),
    //            float.Parse(_unitAttackCooldownTxt), float.Parse(_unitAttackAoERangeTxt), damageRatios);

    //        _currentUnit = entity;
    //        _entityPrefabRef = AssetDatabase.LoadAssetAtPath<GameObject>(_currentBuilding.entityPrefabReference);
    //        _nameTxt = _currentUnit.name;
    //        _factionTxt = _currentUnit.faction;
    //        _descTxt = _currentUnit.description;
    //        _uiMenuLoc = _currentUnit.menuLocation;
    //        _uiMenuOrder = _currentUnit.menuOrderIndex.ToString();
    //        _maxHealthTxt = _currentUnit.maxHealth.ToString();
    //        _constructionCostTxt = _currentUnit.constructionCost.FirstOrDefault()._resourceCount.ToString();


    //        _unitMoveSpeedTxt = _currentUnit.moveSpeed.ToString();
    //        _unitTurnRateRadiansTxt = _currentUnit.turnRateRadians.ToString();
    //        _unitDamageAmountTxt = _currentUnit.unitDamage.ToString();
    //        _unitAttackCooldownTxt = _currentUnit.unitAttackCooldownSeconds.ToString();
    //        _unitAttackAoERangeTxt = _currentUnit.unitAttackAoERange.ToString();
    //        _unitArmourType = _currentUnit.unitArmourType;
    //        _unitDamageRatios = _currentUnit.damageToArmourTypes.Select(x => x.damageRatio.ToString()).ToList();

    //        return tmp;
    //    }
    public void InitWindow(EditorWindow window, object[] data)
    {
        throw new System.NotImplementedException();
    }

    public void OnDestroy()
    {
        throw new System.NotImplementedException();
    }

    public void OnEnable()
    {
        throw new System.NotImplementedException();
    }

    public void OnGUI()
    {
        throw new System.NotImplementedException();
    }
}
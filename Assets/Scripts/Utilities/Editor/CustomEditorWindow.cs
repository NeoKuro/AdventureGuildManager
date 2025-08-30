//-----------------------------\\
//      Project Breedables
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//      Twitch.tv/neokuro
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class CustomEditorWindow : EditorWindow
{
    private System.Type _type = null;

    private object myImplementation = null;

    public System.Type GenericType
    {
        get
        {
            return _type.GetGenericArguments()[0];
        }
        set
        {
            _type = typeof(DynamicEnumEditorWindow<>).MakeGenericType(value);
            myImplementation = System.Activator.CreateInstance(_type);
        }
    }

    public System.Type NonGenericType
    {
        get
        {
            return _type;
        }
        set
        {
            _type = value;
            myImplementation = System.Activator.CreateInstance(_type);
        }
    }

    private static Dictionary<string, CustomEditorWindow> _editorWindowInstances = new Dictionary<string, CustomEditorWindow>();

    [MenuItem("Par-Tee/Entity Editor")]
    public static void ShowEntityEditor()
    {
        ShowWindowForNonGenericWindow<EntityEditorWindow>("EntityEditor", new object[1] { @"Assets\Resources\Entities\" });
    }

    //[MenuItem("Phoenix/Dynamic Enums/Control Actions")]
    //public static void ShowTraitTypeWindow()
    //{
    //    ShowWindow<ControlActions>("ControlActions", new object[1] { @"Assets\Scripts\DynamicEnums\ControlActions.cs" });
    //}
    //[MenuItem("Phoenix/Dynamic Enums/Control Defaults")]
    //public static void ShowDefaultControlsWindow()
    //{
    //    ShowWindow<ControlDefaults>("ControlDefaults", new object[1] { @"Assets\Scripts\DynamicEnums\ControlDefaults.cs" });
    //}
    //[MenuItem("Phoenix/Dynamic Enums/Ship Data Requests")]
    //public static void ShowShipDataRequestsWindow()
    //{
    //    ShowWindow<ShipDataRequests>("ShipDataRequests", new object[1] { @"Assets\Scripts\DynamicEnums\ShipDataRequests.cs" });
    //}
    //[MenuItem("Phoenix/Dynamic Enums/Factions (Abbvs)")]
    //public static void ShowFactionsAbbvWindow()
    //{
    //    ShowWindow<FactionsAbbv>("FactionsAbbv", new object[1] { @"Assets\Scripts\DynamicEnums\FactionsAbbv.cs" });
    //}
    //[MenuItem("Phoenix/Dynamic Enums/Ship Types")]
    //public static void ShowShipTypesWindow()
    //{
    //    ShowWindow<ShipTypes>("ShipTypes", new object[1] { @"Assets\Scripts\DynamicEnums\ShipTypes.cs" });
    //}
    //[MenuItem("Phoenix/Dynamic Enums/AI Behaviour Types")]
    //public static void ShowAIBehaviourTypesWindow()
    //{
    //    ShowWindow<AIBehaviourTypes>("AIBehaviourTypes", new object[1] { @"Assets\Scripts\DynamicEnums\AIBehaviourTypes.cs" });
    //}

    private static void ShowWindowForGenericWindow<T>(string key, object[] data)
    {
        if (_editorWindowInstances.ContainsKey(key))
        {
            if (_editorWindowInstances[key] == null)
            {
                _editorWindowInstances.Remove(key);
            }
            else
            {
                Debug.LogErrorFormat("ERROR: Trying to open a duplicate window of TraitTypes! This is not allowed!");
                return;
            }
        }
        CustomEditorWindow window = EditorWindow.CreateInstance<CustomEditorWindow>();
        window.SetTypeForGenericWindow<T>(data);
        window.Show();
        _editorWindowInstances.Add(key, window);
    }

    private static void ShowWindowForNonGenericWindow<T>(string key, object[] data)
    {
        if (_editorWindowInstances.ContainsKey(key))
        {
            if (_editorWindowInstances[key] == null)
            {
                _editorWindowInstances.Remove(key);
            }
            else
            {
                Debug.LogErrorFormat("ERROR: Trying to open a duplicate window of TraitTypes! This is not allowed!");
                return;
            }
        }
        CustomEditorWindow window = EditorWindow.CreateInstance<CustomEditorWindow>();
        window.SetTypeForNonGenericWindow<T>(data);
        window.Show();
        _editorWindowInstances.Add(key, window);
    }

    public void SetTypeForGenericWindow<T>(object[] data)
    {
        GenericType = typeof(T);
        Init(data);
    }

    public void SetTypeForNonGenericWindow<T>(object[] data)
    {
        NonGenericType = typeof(T);
        Init(data);
    }

    private void Init(object[] data)
    {
        if (myImplementation != null)
        {
            (myImplementation as ICustomWindow).InitWindow(this, data);
            (myImplementation as ICustomWindow).OnEnable();
        }
    }

    private void OnGUI()
    {
        if (myImplementation != null)
        {
            (myImplementation as ICustomWindow).OnGUI();
        }
    }

    private void OnDestroy()
    {
        if (myImplementation != null)
        {
            (myImplementation as ICustomWindow).OnDestroy();
        }

    }
}
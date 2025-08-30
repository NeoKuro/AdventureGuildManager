//-----------------------------\\
//      Project HITH
//    Author: Joshua Hughes
//      Twitch.tv/neokuro
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;

public class WorldEditorWindow_AddWorld : ICustomWindow
{

    private Dictionary<string, int> _enums = new Dictionary<string, int>();
    private Dictionary<string, bool> _enumSelections = new Dictionary<string, bool>();

    //private List<World_SO> _worlds = new List<World_SO>();
    //private Dictionary<World_SO, bool> _worldSelections = new Dictionary<World_SO, bool>();

    private List<string> _newEnumsToAdd = new List<string>();
    private List<string> _initialEnums = new List<string>();

    //private List<World_SO> _newWorldsToAdd = new List<World_SO>();
    //private List<World_SO> _initialWorlds = new List<World_SO>();

    private Vector2 scroll;
    private string _newWorldName;
    private string _newWorldId;
    private string _newWorldPfName;
    private bool _isDefault;

    private bool updated = false;

    private static string path = @"Assets\ScriptableObjects\Worlds\";

    private EditorWindow _thisWindow;
    private WorldEditorWindow _parentWindow;



    public void InitWindow(EditorWindow window, object[] data)
    {
        _thisWindow = window;
        if (data != null && data.Length == 2)
        {
            path = (string)data[0];
            _parentWindow = (WorldEditorWindow)data[1];
            _thisWindow.titleContent.text = "Add New World";
        }
        else
        {
            Debug.LogErrorFormat("We have been supplied with no data, or more data than expected!");
        }
    }

    public void OnEnable()
    {

    }

    public void OnDestroy()
    {

    }

    public void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.stretchWidth = true;
        style.fixedHeight = 15f;

        // This changes the parent window also for some reason?
        //_thisWindow.maxSize = new Vector2(300f, 175f);
        //_thisWindow.minSize = _thisWindow.maxSize;
        EditorGUILayout.BeginVertical();

        _newWorldId = EditorGUILayout.TextField("New World ID", _newWorldId, GUILayout.ExpandWidth(true), GUILayout.Height(25f));
        _newWorldName = EditorGUILayout.TextField("New World Name", _newWorldName, GUILayout.ExpandWidth(true), GUILayout.Height(25f));
        _newWorldPfName = EditorGUILayout.TextField("New World PF Name", _newWorldPfName, GUILayout.ExpandWidth(true), GUILayout.Height(25f));
        _isDefault= EditorGUILayout.Toggle("Is Default", _isDefault, GUILayout.ExpandWidth(true), GUILayout.Height(25f));

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Cancel"))
        {
            if (_thisWindow != null)
            {
                _thisWindow.Close();
            }
            if (_parentWindow != null)
            {
                _parentWindow.OnEnable();
            }
        }
        //if (GUILayout.Button("Delete"))
        //{
        //    //EditorPrefs.SetString("ScriptSignature", signature);
        //    UpdateEnums(false);

        //}
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Save"))
        {
            //World_SO newSO = ScriptableObject.CreateInstance<World_SO>();
            //newSO.UpdateWorld(Convert.ToInt32(_newWorldId), _newWorldName, _newWorldPfName, _isDefault);
            //newSO.SaveChanges();

            //AssetDatabase.CreateAsset(newSO, path + _newWorldPfName + ".asset");
            //AssetDatabase.SaveAssets();

            //if (_thisWindow != null)
            //{
            //    _thisWindow.Close();
            //}
            //if(_parentWindow != null)
            //{
            //    _parentWindow.OnEnable();
            //}
            //Close();
        }
        EditorGUILayout.EndVertical();
    }
}
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

public class WorldEditorWindow : ICustomWindow
{
    private Dictionary<string, int> _enums = new Dictionary<string, int>();
    private Dictionary<string, bool> _enumSelections = new Dictionary<string, bool>();

    //private List<World_SO> _worlds = new List<World_SO>();
    //private Dictionary<World_SO, bool> _worldSelections = new Dictionary<World_SO, bool>();
    //private World_SO _selectedWorld;
    //private List<World_SO> _toDelete = new List<World_SO>();

    private List<string> _newEnumsToAdd = new List<string>();
    private List<string> _initialEnums = new List<string>();

    //private List<World_SO> _newWorldsToAdd = new List<World_SO>();
    //private List<World_SO> _initialWorlds = new List<World_SO>();

    private Vector2 scroll;
    private string _newEnum;

    private bool updated = false;

    private static string path = @"Assets\ScriptableObjects\Worlds";

    private EditorWindow _thisWindow;


    private string _newWorldName;
    private string _newWorldId;
    private string _newWorldPfName;
    private bool _isDefault;

    public void InitWindow(EditorWindow window, object[] data)
    {
        _thisWindow = window;
        if (data != null && data.Length == 1)
        {
            path = (string)data[0];
        }
        else
        {
            Debug.LogErrorFormat("We have been supplied with no data, or more data than expected!");
        }
    }

    public void OnEnable()
    {
        //string[] filePaths = System.IO.Directory.GetFiles(path);
        //_selectedWorld = null;
        //_worlds.Clear();
        //if (filePaths != null && filePaths.Length > 0)
        //{
        //    for (int i = 0; i < filePaths.Length; i++)
        //    {
        //        World_SO obj = AssetDatabase.LoadAssetAtPath(filePaths[i], typeof(World_SO)) as World_SO;

        //        if (obj != null)
        //        {
        //            _worlds.Add(obj);
        //        }
        //    }
        //}

        //_thisWindow.minSize = new Vector2(300f, 350f);
    }

    public void OnDestroy()
    {
        //World_SO[] defaultsCountCheck = _worlds.Where(x => x._tmpDefault).ToArray();
        //if (defaultsCountCheck == null || defaultsCountCheck.Length != 1)
        //{
        //    Debug.LogError("Something has gone wrong when trying to save changes to existing Worlds. Any deletions have been confirmed, but updates have not been saved.");
        //    if (defaultsCountCheck == null)
        //    {
        //        Debug.LogErrorFormat("Changes have NOT been saved and will be reverted.\nTrying to check how many default worlds there are but null List was returned. ");
        //    }
        //    else if (defaultsCountCheck.Length > 1)
        //    {
        //        Debug.LogErrorFormat("Changes have NOT been saved and will be reverted.\nCannot have more than 1 default world. Deselect the other worlds");
        //    }
        //    else if (defaultsCountCheck.Length < 1)
        //    {
        //        Debug.LogErrorFormat("Changes have NOT been saved and will be reverted.\nCurrently there are 0 default worlds. Forcing First one to be default World");
        //        World_SO w = _worlds.FirstOrDefault();
        //        if(w != null)
        //        {
        //            w.UpdateWorld(w._tmpId, w._tmpWorldName, w._tmpPfWOrldName, true);
        //            w.SaveChanges();
        //        }
        //    }

        //    for (int i = 0; i < _worlds.Count; i++)
        //    {
        //        _worlds[i].UndoChanges();
        //    }
        //    return;
        //}

        //for (int i = 0; i < _worlds.Count; i++)
        //{
        //    _worlds[i].SaveChanges();
        //}
    }

    public void OnGUI()
    {
        scroll = EditorGUILayout.BeginScrollView(scroll);
        GUIStyle style = new GUIStyle();
        style.stretchWidth = true;
        style.fixedHeight = 25f;



        //Dictionary<World_SO, bool> newSelections = new Dictionary<World_SO, bool>();

        //for (int i = 0; i < _worlds.Count; i++)
        //{
        //    if (_selectedWorld == _worlds[i])
        //    {
        //        _newWorldId = EditorGUILayout.TextField("World ID", _newWorldId, GUILayout.ExpandWidth(true), GUILayout.Height(25f));
        //        _newWorldName = EditorGUILayout.TextField("World Name", _newWorldName, GUILayout.ExpandWidth(true), GUILayout.Height(25f));
        //        _newWorldPfName = EditorGUILayout.TextField("World PF Name", _newWorldPfName, GUILayout.ExpandWidth(true), GUILayout.Height(25f));
        //        _isDefault = EditorGUILayout.Toggle("Is Default", _isDefault, GUILayout.ExpandWidth(true), GUILayout.Height(25f));

        //        EditorGUILayout.BeginHorizontal();
        //        // If we select delete, refresh right away (we remove it and are going in alphabetical order, to do a Forr loop it would be backwards)
        //        if (GUILayout.Button("Save", "button", GUILayout.Height(15f), GUILayout.ExpandWidth(true)))
        //        {
        //            int i32;
        //            if (!int.TryParse(_newWorldId, out i32))
        //            {
        //                Debug.LogError("New World Id is not a valid number. It must be an int 32");
        //                continue;
        //            }

        //            _worlds[i].UpdateWorld(Convert.ToInt32(_newWorldId), _newWorldName, _newWorldPfName, _isDefault);

        //            List<World_SO> defaultsCountCheck = _worlds.Where(x => x._tmpDefault).ToList();
        //            if (defaultsCountCheck == null || defaultsCountCheck.Count != 1)
        //            {
        //                if (defaultsCountCheck == null)
        //                {
        //                    Debug.LogErrorFormat("Changes have NOT been saved.\nTrying to check how many default worlds there are but null List was returned");
        //                }
        //                else if (defaultsCountCheck.Count > 1)
        //                {
        //                    Debug.LogErrorFormat("Changes have NOT been saved.\nCannot have more than 1 default world. Deselect the other worlds first");
        //                }
        //                else if (defaultsCountCheck.Count < 1)
        //                {
        //                    Debug.LogErrorFormat("Changes have NOT been saved.\nCurrently there are 0 default worlds. Select one");
        //                }
        //                return;
        //            }

        //            _worlds[i].SaveChanges();
        //            _selectedWorld = null;
        //            return;
        //        }
        //        // If we select delete, refresh right away (we remove it and are going in alphabetical order, to do a Forr loop it would be backwards)
        //        if (GUILayout.Button("Undo Changes", "button", GUILayout.Height(15f), GUILayout.ExpandWidth(true)))
        //        {
        //            _worlds[i].UndoChanges();
        //            _newWorldId = _worlds[i]._tmpId.ToString();
        //            _newWorldName = _worlds[i]._tmpWorldName;
        //            _newWorldPfName = _worlds[i]._tmpPfWOrldName;
        //            _isDefault = _worlds[i]._tmpDefault;
        //            GUI.FocusControl(null);
        //            return;
        //        }
        //        // If we select delete, refresh right away (we remove it and are going in alphabetical order, to do a Forr loop it would be backwards)
        //        if (GUILayout.Button("Delete", "button", GUILayout.Height(15f), GUILayout.ExpandWidth(true)))
        //        {
        //            string deletePath = AssetDatabase.GetAssetPath(_worlds[i]);
        //            if (string.IsNullOrEmpty(deletePath))
        //            {
        //                continue;
        //            }
        //            AssetDatabase.DeleteAsset(deletePath);

        //            _selectedWorld = null;
        //            _worlds.RemoveAt(i);
        //            return;
        //        }
        //        EditorGUILayout.EndHorizontal();
        //    }
        //    else
        //    {
        //        if (GUILayout.Button(_worlds[i].worldName, "button", GUILayout.Height(25f), GUILayout.ExpandWidth(true)))
        //        {
        //            _selectedWorld = _worlds[i];
        //            _newWorldId = _worlds[i]._tmpId.ToString();
        //            _newWorldName = _worlds[i]._tmpWorldName;
        //            _newWorldPfName = _worlds[i]._tmpPfWOrldName;
        //            _isDefault = _worlds[i]._tmpDefault;
        //            GUI.FocusControl(null);
        //            return;
        //        }
        //    }
        //}
        EditorGUILayout.EndScrollView();


        //EditorGUILayout.BeginVertical();
        //EditorGUILayout.BeginHorizontal();

        //if (GUILayout.Button("Add"))
        //{
        //    GenericEditorWindow.ShowWorldEditor_AddWorld(this);
        //}

        //EditorGUILayout.EndHorizontal();

        //if (GUILayout.Button("Close"))
        //{
        //    _thisWindow.Close();
        //}
        ////if (GUILayout.Button("Save"))
        ////{
        ////    List<World_SO> defaultsCountCheck = _worlds.Where(x => x._tmpDefault) as List<World_SO>;
        ////    if (defaultsCountCheck == null || defaultsCountCheck.Count != 1)
        ////    {
        ////        if (defaultsCountCheck == null)
        ////        {
        ////            Debug.LogErrorFormat("Changes have NOT been saved.\nTrying to check how many default worlds there are but null List was returned");
        ////        }
        ////        else if (defaultsCountCheck.Count > 1)
        ////        {
        ////            Debug.LogErrorFormat("Changes have NOT been saved.\nCannot have more than 1 default world. Deselect the other worlds first");
        ////        }
        ////        else if (defaultsCountCheck.Count < 1)
        ////        {
        ////            Debug.LogErrorFormat("Changes have NOT been saved.\nCurrently there are 0 default worlds. Select one");
        ////        }
        ////        return;
        ////    }

        ////    for (int i = 0; i < _worlds.Count; i++)
        ////    {
        ////        _worlds[i].SaveChanges();
        ////    }

        ////    List<string> deletePaths = new List<string>();
        ////    for (int i = 0; i < _toDelete.Count; i++)
        ////    {
        ////        string deletePath = AssetDatabase.GetAssetPath(_toDelete[i]);
        ////        if (string.IsNullOrEmpty(deletePath))
        ////        {
        ////            continue;
        ////        }

        ////        deletePaths.Add(deletePath);
        ////    }

        ////    if (deletePaths.Count > 0)
        ////    {
        ////        List<string> failedPaths = new List<string>();
        ////        AssetDatabase.DeleteAssets(deletePaths.ToArray(), failedPaths);
        ////    }

        ////    if (_thisWindow != null)
        ////    {

        ////        _thisWindow.Close();
        ////    }
        ////}
        //EditorGUILayout.EndVertical();
    }



}
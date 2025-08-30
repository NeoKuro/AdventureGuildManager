using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class DynamicEnumEditorWindow<T> : ICustomWindow
{
    private Dictionary<string, int> _enums = new Dictionary<string, int>();
    private Dictionary<string, bool> _enumSelections = new Dictionary<string, bool>();
    private List<string> _newEnumsToAdd = new List<string>();
    private List<string> _initialEnums = new List<string>();
    private Vector2 scroll;
    private static DynamicEnumEditorWindow<T> editorWindow;
    private string _newEnum;
    private int _highestValue = -1;

    private bool updated = false;


    private static string _traitLocatorPath = @"Assets\Scripts\Genetics\Phenotype\Traits\DynamicTraitLocatorEnums.cs";
    private static string _traitTypePath = @"Assets\Scripts\Genetics\Phenotype\Traits\DynamicTraitTypeEnums.cs";
    private static string path = @"Assets\Scripts\Genetics\Phenotype\Traits\DynamicTraitTypeEnums.cs";

    private EditorWindow _thisWindow;


    //[MenuItem("Phoenix/Trait Types")]
    //public static void ShowTraitTypeWindow()
    //{
    //    path = _traitTypePath;
    //    //editorWindow = GetWindowWithRect<DynamicEnumEditorWindow<TraitType>>(new Rect(250f, 250f, 250f, 250f), true, "Trait Locator Editor") as DynamicEnumEditorWindow<T>;
    //    ShowWindow<TraitType>();
    //}

    //[MenuItem("Phoenix/Trait Locators")]
    //public static void ShowTraitLocatorWindow()
    //{
    //    path = _traitLocatorPath;
    //    //editorWindow = GetWindowWithRect<DynamicEnumEditorWindow<TraitLocatorType>>(new Rect(250f, 250f, 250f, 250f), true, "Trait Locator Editor") as DynamicEnumEditorWindow<T>;
    //    ShowWindow<TraitLocatorType>();
    //}

    //private static void ShowWindow<Q>()
    //{
    //    GetWindowWithRect<DynamicEnumEditorWindow<Q>>(new Rect(250f, 250f, 250f, 250f), true, "Trait Locator Editor") as DynamicEnumEditorWindow<T>;
    //}

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
        List<string> tmp = Enum.GetNames(typeof(T)).ToList();
        int[] tmpV = (int[])Enum.GetValues(typeof(T));
        for (int i = 0; i < tmp.Count; i++)
        {
            _initialEnums.Add(tmp[i]);
            _enums.Add(tmp[i], tmpV[i]);
            _enumSelections.Add(tmp[i], false);
            if (tmpV[i] > _highestValue)
            {
                _highestValue = tmpV[i];
            }
        }
    }

    public void OnDestroy()
    {

    }

    public void OnGUI()
    {
        //if (updated)
        //{
        //    _enums.Clear();
        //    _enumSelections.Clear();
        //    if (!EditorApplication.isCompiling)
        //    {
        //        updated = false;
        //    }

        //    List<string> tmp = Enum.GetNames(typeof(TraitType)).ToList();
        //    int[] tmpV = (int[])Enum.GetValues(typeof(TraitType));
        //    for (int i = 0; i < tmp.Count; i++)
        //    {
        //        _enums.Add(tmp[i], tmpV[i]);
        //        _enumSelections.Add(tmp[i], false);
        //    }
        //}

        scroll = EditorGUILayout.BeginScrollView(scroll);
        //signature = EditorGUILayout.TextArea(signature, GUILayout.Height(250f));
        GUIStyle style = new GUIStyle();
        style.stretchWidth = true;
        style.fixedHeight = 25f;

        Dictionary<string, bool> newSelections = new Dictionary<string, bool>();
        foreach (KeyValuePair<string, bool> item in _enumSelections)
        {
            Color prevCol = GUI.color;
            if (item.Value == true)
            {
                if (_newEnumsToAdd.Contains(item.Key))
                {
                    _enums.Remove(item.Key);
                    //_enumSelections.Remove(item.Key);
                    _newEnumsToAdd.Remove(item.Key);
                    continue;
                }

                GUI.color = Color.red;
                newSelections.Add(item.Key, GUILayout.Toggle(item.Value, item.Key.Replace('_', ' '), "button", GUILayout.Height(25f), GUILayout.ExpandWidth(true)));
                GUI.color = prevCol;
                continue;
            }

            if (_newEnumsToAdd.Contains(item.Key))
            {
                GUI.color = Color.green;
            }
            newSelections.Add(item.Key, GUILayout.Toggle(item.Value, item.Key.Replace('_', ' '), "button", GUILayout.Height(25f), GUILayout.ExpandWidth(true)));
            GUI.color = prevCol;
        }

        _enumSelections = newSelections;
        EditorGUILayout.EndScrollView();



        EditorGUILayout.BeginVertical();

        _newEnum = EditorGUILayout.TextField("", _newEnum, GUILayout.ExpandWidth(true), GUILayout.Height(25f));

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add"))
        {
            AddEnumLocal();
            _newEnum = "";
        }
        //if (GUILayout.Button("Delete"))
        //{
        //    //EditorPrefs.SetString("ScriptSignature", signature);
        //    UpdateEnums(false);

        //}
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Save"))
        {
            if (_thisWindow != null)
            {
                SaveEnums();
                _thisWindow.Close();
            }
            //Close();
        }
        EditorGUILayout.EndVertical();
    }

    private void AddEnumLocal()
    {
        if (_enums.ContainsKey(_newEnum))
        {
            Debug.LogErrorFormat("{0} already contains enum {1}", typeof(T).ToString(), _newEnum);
            return;
        }

        if (_enumSelections.ContainsKey(_newEnum))
        {
            Debug.LogErrorFormat("Somehow, the _enumSelections dictionary already contains {0} but the main '_enums' dictionary doesn't? Will continue but overwrite _enumsSelection!", _newEnum);
            _enumSelections.Remove(_newEnum);
        }
        _newEnum = _newEnum.Replace(' ', '_').ToUpper();
        _enums.Add(_newEnum, (_highestValue + 1));
        _enumSelections.Add(_newEnum, false);
        _newEnumsToAdd.Add(_newEnum);
        updated = true;
        _highestValue += 1;
    }


    /// <summary>
    /// This will be used if we want to don't want to delete enums right away, and instead want to rely on a save system for that too
    /// However, currently the delete system works by selecting and deselecting what you do and don't want deleted anyway!
    /// </summary>
    private void DeleteEnumLocal()
    {
        Dictionary<string, bool> tmp = new Dictionary<string, bool>(_enumSelections);
        foreach (KeyValuePair<string, bool> selectedEnums in _enumSelections)
        {
            if (selectedEnums.Value == false)
            {
                continue;
            }

            if (!_enums.ContainsKey(selectedEnums.Key))
            {
                Debug.LogErrorFormat("{0} does not contain enum {1}", typeof(T).ToString(), selectedEnums.Key);
                _enumSelections.Remove(selectedEnums.Key);
                continue;
            }

            _enums.Remove(selectedEnums.Key);

        }
    }

    private void SaveEnums()
    {
        using (StreamWriter file = File.CreateText(path))
        {
            file.Write(EditorPrefs.GetString("ScriptSignature"));
            file.WriteLine("");
            file.WriteLine("public enum "+ typeof(T).ToString() +"\n{");

            int i = -1;
            foreach (KeyValuePair<string, int> line in _enums)
            {
                // If We have selected this enum for deletion, don't insert it into the file and continue
                if (_enumSelections[line.Key])
                {
                    continue;
                }

                string lineRep = line.Key.ToString().Replace(" ", "_");
                if (!string.IsNullOrEmpty(lineRep))
                {
                    file.WriteLine(string.Format("\t{0} = {1},",
                        lineRep, line.Value));

                    if (line.Value > i)
                    {
                        i = line.Value;
                    }
                }
            }

            //if (addNewEnum)
            //{
            //    file.WriteLine(string.Format("\t{0}= {1}",
            //                _newEnum, i + 1));
            //}

            file.WriteLine("}");
        }

        AssetDatabase.ImportAsset(path);
    }

    private void UpdateEnums(bool addNewEnum)
    {
        bool shouldUpdate = false;
        foreach (string enumKey in _enums.Keys)
        {
            if (_initialEnums.Contains(enumKey))
            {
                shouldUpdate = true;
            }
        }

        shouldUpdate = _enumSelections.Where(x => { return x.Value == true; }).Count() > 0;
        if (!shouldUpdate)
        {
            return;
        }

        using (StreamWriter file = File.CreateText(path))
        {
            file.Write(EditorPrefs.GetString("ScriptSignature"));
            file.WriteLine("");
            file.WriteLine("public enum TraitType \n{");

            int i = -1;
            foreach (KeyValuePair<string, int> line in _enums)
            {
                if (!addNewEnum)
                {
                    if (_enumSelections[line.Key])
                    {
                        continue;
                    }
                }

                string lineRep = line.Key.ToString().Replace(" ", "_");
                if (!string.IsNullOrEmpty(lineRep))
                {
                    file.WriteLine(string.Format("\t{0} = {1},",
                        lineRep, line.Value));

                    if (line.Value > i)
                    {
                        i = line.Value;
                    }
                }
            }

            if (addNewEnum)
            {
                file.WriteLine(string.Format("\t{0}= {1}",
                            _newEnum, i + 1));
            }

            file.WriteLine("}");
        }

        AssetDatabase.ImportAsset(path);
        updated = true;
    }
}

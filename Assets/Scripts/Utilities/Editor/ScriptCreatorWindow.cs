using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


public class ScriptCreatorWindow : EditorWindow
{
    private enum ScriptType
    {
        MONO = 0,
        STRUCT = 1,
        INTERFACE = 2,
        ENUM = 3,
        SCRIPT_OBJ = 4,
        NETWORK = 5,
        COMPONENT_DATA = 1000,
        SYSTEM = 1001,
        AUTHORING = 1002,
        ASPECT = 1003,
        BUFFER_DATA = 1004
    }

    private const int DOTS_TYPE_OFFSET= 1000;

    private bool _focused = false;
    private bool _ecsScript = false;

    private int _scriptType = 0;
    private int _scriptTypeIndex = 0;
    private float _windowWidthPerItem = 125f;
    private string _scriptName = "NewMonoBehaviour";
    private static ScriptCreatorWindow _window;
    private EditScriptSignatureWindow _editSigWindow;

    private readonly ScriptType[] _monoTypes = new ScriptType[] { ScriptType.MONO, ScriptType.STRUCT, ScriptType.INTERFACE, ScriptType.ENUM, ScriptType.SCRIPT_OBJ, ScriptType.NETWORK };
    private readonly ScriptType[] _dotsTypes = new ScriptType[] { ScriptType.COMPONENT_DATA, ScriptType.SYSTEM, ScriptType.AUTHORING, ScriptType.ASPECT, ScriptType.BUFFER_DATA };

    [MenuItem("Assets/Create New C# Script")]
    public static void ShowWindow()
    {
        //GetWindow<ScriptCreatorWindow>(true, "Create C# Script", true);
        _window = GetWindowWithRect<ScriptCreatorWindow>(new Rect(Vector2.zero, new Vector2(600f, 100f)), true, "Create C# Script", true);
    }

    private void OnGUI()
    {
        if (Event.current.keyCode == KeyCode.Return)
        {
            CreateScript();
            OpenScript();
            Close();
            return;
        }

        EditorGUILayout.BeginVertical();
        GUI.SetNextControlName("scriptNameTxt");
        _scriptName = EditorGUILayout.TextField("Script Name", _scriptName);

        EditorGUILayout.BeginVertical();
        {
            _ecsScript = GUILayout.Toggle(_ecsScript, "Is DOTS Script");
            // Script Options Row
            EditorGUILayout.BeginHorizontal();

            if (!_ecsScript)
            {
                _window.minSize = _window.maxSize = new Vector2(_windowWidthPerItem * _monoTypes.Length, 100f);
                _scriptTypeIndex = GUILayout.SelectionGrid(_scriptTypeIndex, _monoTypes.Select(x => " " + x.ToString()).ToArray(), _monoTypes.Length, EditorStyles.radioButton, GUILayout.ExpandWidth(true));
                _scriptType = _scriptTypeIndex;
            }
            else
            {
                _window.minSize = _window.maxSize = new Vector2(_windowWidthPerItem * _monoTypes.Length, 100f);
                _scriptTypeIndex = GUILayout.SelectionGrid(_scriptTypeIndex, _dotsTypes.Select(x => x.ToString()).ToArray(), _dotsTypes.Length, EditorStyles.radioButton, GUILayout.ExpandWidth(true));
                // Increment the type by the DOTS offset
                _scriptType = DOTS_TYPE_OFFSET + _scriptTypeIndex;
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();


        EditorGUILayout.BeginHorizontal();

        if (!_focused)
        {
            EditorGUI.FocusTextInControl("scriptNameTxt");
            _focused = true;
        }

        if (GUILayout.Button("Edit Signature"))
        {
            _editSigWindow = GetWindowWithRect<EditScriptSignatureWindow>(new Rect(Vector2.zero, new Vector2(400f, 300f)), true, "Edit Signature", true);
        }
        if (GUILayout.Button("Create Script"))
        {
            CreateScript();
            OpenScript();
            Close();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }

    private void CreateScript()
    {
        string defaultScriptSignature =
            "//    PROJECT NAME    \n" +
            "//  Author: NeoKuro   \n" +
            "// Twitch.tv/Neokuro \n";
        string scriptSignature = EditorPrefs.GetString("ScriptSignature_" + UsefulMethods.GetProjectName(), defaultScriptSignature);

        string scriptTxt =
        scriptSignature +

            "\n" +
            "\n" +
            "using System;\n" +
            "using System.Collections;\n" +
            "using System.Collections.Generic;\n" +
            "using System.Linq;\n" +
            "using UnityEngine;\n";

        string dotsDependencies =
            "using Unity.Burst;\n" +
            "using Unity.Entities;\n" +
            "using Unity.Mathematics;\n" +
            "using Unity.Transforms;\n";

        ScriptType selectedType = (ScriptType)_scriptType;

        switch (selectedType)
        {
            default:
            case ScriptType.MONO:
                scriptTxt += "\n" +
                    "public class " + _scriptName + " : MonoBehaviour\n" +
                    "{\n" +
                    "   private void Start()\n" +
                    "   {\n" +
                    "       // Stuff In here ran on object creation\n" +
                    "   }\n" +
                    "\n" +
                    "   private void Update()\n" +
                    "   {\n" +
                    "       // Stuff in here ran each frame\n" +
                    "   }\n" +
                    "}";
                break;
            case ScriptType.STRUCT:
                scriptTxt += "\n" +
                    "public struct " + _scriptName + "\n" +
                    "{\n" +
                    "\n" +
                    "}";
                break;
            case ScriptType.INTERFACE:
                scriptTxt += "\n" +
                    "public interface " + _scriptName + "\n" +
                    "{\n" +
                    "\n" +
                    "}";
                break;
            case ScriptType.ENUM:
                scriptTxt += "\n" +
                    "public enum " + _scriptName + "\n" +
                    "{\n" +
                    "\n" +
                    "}";
                break;
            case ScriptType.SCRIPT_OBJ:
                scriptTxt += "\n" +
                    "public class " + _scriptName + " : ScriptableObject\n" +
                    "{\n" +
                    "\n" +
                    "}";
                break;
            case ScriptType.NETWORK:
                // Not yet implemented. 
                // Depends on which Networking system is used
                throw new NotImplementedException();

            // DOTS
            case ScriptType.COMPONENT_DATA:
                scriptTxt += dotsDependencies;
                scriptTxt += "\n" +
                    "[BurstCompile]\n" +
                    "public struct " + _scriptName + " : IComponentData\n" +
                    "{\n" +
                    "\n" +
                    "}";
                break;
            case ScriptType.SYSTEM:
                scriptTxt += dotsDependencies;
                scriptTxt += "\n" +
                    "[BurstCompile]\n" +
                    "[UpdateInGroup(typeof(INTENTIONAL_ERROR__DEFINE_SYSTEM))]\n" +
                    "public partial struct " + _scriptName + " : ISystem\n" +
                    "{\n" +
                    "\t[BurstCompile]\n" +
                    "\tpublic void OnCreate(ref SystemState state)\n" +
                    "\t{\n" +
                    "\t\n" +
                    "\t}\n\n" +
                    "\t[BurstCompile]\n" +
                    "\tpublic void OnDestroy(ref SystemState state)\n" +
                    "\t{\n" +
                    "\t\n" +
                    "\t}\n\n" +
                    "\t[BurstCompile]\n" +
                    "\tpublic void OnUpdate(ref SystemState state)\n" +
                    "\t{\n" +
                    "\t\n" +
                    "\t}\n" +
                    "}";
                break;
            case ScriptType.AUTHORING:
                scriptTxt += dotsDependencies;
                string tmp = _scriptName;
                tmp.Replace("AUTHORING", "");
                if (tmp.ToUpper().Contains("AUTHORING"))
                {
                    int index = tmp.IndexOf("AUTHORING");
                    _scriptName.Remove(index, "AUTHORING".Length);
                    Debug.Log(tmp + "    " + _scriptName);
                }
                string scriptNameAuth = _scriptName + "Authoring";
                string scriptNameBaker = _scriptName + "Baker";
                scriptTxt += "\n" +
                    "public class " + scriptNameAuth + " : MonoBehaviour\n" +
                    "{\n" +
                    "\n" +
                    "}\n\n" +
                    "\n" +
                    "public class " + scriptNameBaker + " : Baker <" + scriptNameAuth + "> " +
                    "{\n" +
                    "\tpublic override void Bake(" + scriptNameAuth + " authoring)\n" +
                    "\t{\n" +
                    "\t\n" +
                    "\t}\n" +
                    "}";
                _scriptName = scriptNameAuth;
                break;
            case ScriptType.ASPECT:
                scriptTxt += dotsDependencies;
                scriptTxt += "\n" +
                    "[BurstCompile]\n" +
                    "public readonly partial struct " + _scriptName + " : IAspect\n" +
                    "{\n" +
                    "\tpublic readonly Entity self;\n" +
                    "\n" +
                    "}\n";
                break;
            case ScriptType.BUFFER_DATA:
                scriptTxt += dotsDependencies;
                scriptTxt += "\n" +
                    "[BurstCompile]\n" +
                    "public partial struct " + _scriptName + " : IBufferElementData\n" +
                    "{\n" +
                    "\n" +
                    "}\n";
                break;
        }

        File.WriteAllText(AssetDatabase.GetAssetPath(Selection.activeObject) + string.Format(@"\{0}.cs", _scriptName), scriptTxt);
        AssetDatabase.Refresh();
    }

    private void OpenScript()
    {
        UnityEngine.Object script = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(AssetDatabase.GetAssetPath(Selection.activeObject) + string.Format(@"\{0}.cs", _scriptName));
        if (script == null)
            return;
        AssetDatabase.OpenAsset(script);
        Debug.Log("Successfully created '" + _scriptName + "'.");
    }
}

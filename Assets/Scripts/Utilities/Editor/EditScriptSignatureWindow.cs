using UnityEditor;
using UnityEngine;

public class EditScriptSignatureWindow : EditorWindow
{
    private string signature = "";
    private Vector2 scroll;
    string defaultScriptSignature =
        "//    PROJECT NAME    \n" +
        "//  Author: NeoKuro   \n" +
        "// Twitch.tv/Neokuro \n";

    private void OnEnable()
    {
        signature = EditorPrefs.GetString("ScriptSignature_" + UsefulMethods.GetProjectName(),defaultScriptSignature);
    }

    private void OnGUI()
    {
        scroll = EditorGUILayout.BeginScrollView(scroll);
        signature = EditorGUILayout.TextArea(signature, GUILayout.Height(250f));
        EditorGUILayout.EndScrollView();

        if(GUILayout.Button("Save"))
        {
            EditorPrefs.SetString("ScriptSignature_" + UsefulMethods.GetProjectName(), signature);
            Close();
        }
    }
}

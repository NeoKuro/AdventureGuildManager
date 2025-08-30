using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ScriptableObjectCreator
{
    private const string DATA_DIRECTORY = @"Assets/ScriptableObjects/ShipClasses";


    [MenuItem("Assets/Scriptable Objects/Ships/Ship Class Data")]
    public static void CreateShipClass_SO()
    {
        // Game.Ships.PlayerClassData data = CreateGenericAsset<Game.Ships.PlayerClassData>(DATA_DIRECTORY, "NewShipClassData.asset");
    }

    private static T CreateGenericAsset<T>(string savePath, string defaultName = "NewItem") where T : ScriptableObject
    {
        if (!defaultName.ToLower().Contains(".asset"))
        {
            defaultName += ".asset";
        }

        if (savePath[savePath.Length - 1] != '/')
        {
            savePath += '/';
        }

        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        T newAsset = ScriptableObject.CreateInstance<T>();
        AssetDatabase.CreateAsset(newAsset, savePath + defaultName);
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = newAsset;
        return newAsset;
    }
}

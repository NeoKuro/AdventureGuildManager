//-----------------------------\\
//      Project CyberTex
//    Author: Joshua Hughes
//      Twitch.tv/neokuro
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class ResourceObjectCreator
{
    private static string enemy_asset_directory = @"Assets/Resources/AI/Enemies/";
    private static string weapon_asset_directory = @"Assets/Resources/Weapons/MobileWeapons/";

    [MenuItem("Assets/Twitch/Resources/AI/Enemy Prefab (To Be Coded!)")]
    public static void CreateCommonEnemyPrefab()
    {
        //GameObject commonEnemyObj = CreateGenericAsset(enemy_asset_directory, "NewEnemy.prefab");
        //commonEnemyObj.AddComponent<Enemy_AIEntity>();
        //commonEnemyObj.AddComponent<CharacterStatsController>();
        //commonEnemyObj.AddComponent<EquipmentController>();
        //commonEnemyObj.AddComponent<EntityAnimatorController>();
    }

    //[MenuItem("Assets/Twitch/Resources/Weapons/Mobile Bullet Weapon")]
    //public static void CreateBulletMobileWeaponPrefab()
    //{
    //    GameObject commonEnemyObj = CreateGenericAsset(weapon_asset_directory, "NewBulletWeapon.prefab");
    //    commonEnemyObj.AddComponent<Bullet_MobileWeapon>();
    //}



    private static GameObject CreateGenericAsset(string savePath, string defaultName = "NewItem")
    {
        if (!defaultName.ToLower().Contains(".prefab"))
        {
            defaultName += ".prefab";
        }

        if (savePath[savePath.Length - 1] != '/')
        {
            savePath += '/';
        }

        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }


        GameObject instanceObj = new GameObject(defaultName);
        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(instanceObj, savePath + defaultName);
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = prefab;
        GameObject.DestroyImmediate(instanceObj);
        return prefab;
    }
}
//     Project:  RWI MMO    
//       Author: NeoKuro   
//     Twitch.tv/Neokuro 


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

//[CustomEditor(typeof(ShipClass_SO))]
public class ShipClassEditor : Editor
{
    SerializedProperty mName;
    SerializedProperty mBaseMass;
    SerializedProperty mClassType;
    SerializedProperty mSize;

    SerializedProperty mComponents;

    // Ship Movement Properties
    SerializedProperty mMaxSpeed;
    SerializedProperty mMaxRevSpeed;
    SerializedProperty mMaxTurnRate;

    bool showMovementProperties, showComponentsList = false;

    //private Dictionary<ShipComponentType, int> _shipComponentTypeCounters = new Dictionary<ShipComponentType, int>();

    private void OnEnable()
    {
        mName = serializedObject.FindProperty("m_ShipName");
        mBaseMass = serializedObject.FindProperty("m_BaseMass");
        mClassType = serializedObject.FindProperty("m_ClassType");
        mSize = serializedObject.FindProperty("m_Size");
        mComponents = serializedObject.FindProperty("m_ShipComponents");
        mMaxSpeed = serializedObject.FindProperty("m_MaxSpeed");
        mMaxRevSpeed = serializedObject.FindProperty("m_MaxRevSpeed");
        mMaxTurnRate = serializedObject.FindProperty("m_MaxTurnRate");
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        serializedObject.Update();

        EditorGUILayout.PropertyField(mName);
        EditorGUILayout.PropertyField(mBaseMass);
        EditorGUILayout.PropertyField(mClassType);
        EditorGUILayout.PropertyField(mSize);

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        showComponentsList = EditorGUILayout.Foldout(showComponentsList, "Ship Component Config");
        if (showComponentsList)
        {
            //_shipComponentTypeCounters.Clear();
            //for (int i = 0; i < mComponents.arraySize; i++)
            //{
            //    SerializedProperty prop = mComponents.GetArrayElementAtIndex(i);
            //    ShipComponent compProp = prop.objectReferenceValue as ShipComponent;
            //    string tName = compProp.ComponentType.ToString();
            //    if (!_shipComponentTypeCounters.ContainsKey(compProp.ComponentType))
            //    {
            //        _shipComponentTypeCounters.Add(compProp.ComponentType, 0);
            //    }
            //    _shipComponentTypeCounters[compProp.ComponentType]++;
            //}

            //EditorGUILayout.BeginHorizontal();
            //foreach (KeyValuePair<ShipComponentType, int> item in _shipComponentTypeCounters)
            //{
            //    EditorGUILayout.LabelField(string.Format("{0}: {1}", item.Key.ToString(), item.Value));
            //}
            EditorGUILayout.EndHorizontal();

            mComponents = serializedObject.FindProperty("m_ShipComponents");

            //ShowArrayProperty<ShipComponentType>(mComponents, "m_ComponentType");

            EditorGUILayout.PropertyField(mComponents, new GUIContent("Ship Components"), true);
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        showMovementProperties = EditorGUILayout.Foldout(showMovementProperties, "Ship Movement Config");
        if (showMovementProperties)
        {
            EditorGUILayout.PropertyField(mMaxSpeed);
            EditorGUILayout.PropertyField(mMaxRevSpeed);
            EditorGUILayout.PropertyField(mMaxTurnRate);
        }

        serializedObject.ApplyModifiedProperties();
    }

    string AddSpacesToSentence(string text, bool preserveAcronyms)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;
        StringBuilder newText = new StringBuilder(text.Length * 2);
        newText.Append(text[0]);
        for (int i = 1; i < text.Length; i++)
        {
            if (char.IsUpper(text[i]))
                if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) ||
                    (preserveAcronyms && char.IsUpper(text[i - 1]) &&
                     i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                    newText.Append(' ');
            newText.Append(text[i]);
        }
        return newText.ToString();
    }
    public void ShowArrayProperty<T>(SerializedProperty list, string relativeField) where T : struct
    {
        EditorGUILayout.PropertyField(list, new GUIContent("Ship Components"), true);

        Dictionary<T, int> tCount = new Dictionary<T, int>();

        EditorGUI.indentLevel += 1;
        for (int i = 0; i < list.arraySize; i++)
        {
            SerializedProperty prop = list.GetArrayElementAtIndex(i);
            T p;
            Enum.TryParse("", out p);

            if(!tCount.ContainsKey(p))
            {
                tCount.Add(p, 0);
            }
            tCount[p]++;

            string name = p.ToString();
            EditorGUILayout.PropertyField(prop,
                new GUIContent(name + " " + (tCount[p]).ToString()));
        }
        EditorGUI.indentLevel -= 1;
    }
}
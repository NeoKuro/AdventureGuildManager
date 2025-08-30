//-----------------------------\\
//      Project Breedables
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//-----------------------------\\

#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public interface IGenericWindow
{
    void OnDestroy();
    void OnGUI();
    void OnEnable();

    void InitWindow(EditorWindow window, object[] data);
}
#endif
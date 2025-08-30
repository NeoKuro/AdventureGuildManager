//-----------------------------\\
//      Project Breedables
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//      Twitch.tv/neokuro
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public interface ICustomWindow
{
    void OnDestroy();
    void OnGUI();
    void OnEnable();

    void InitWindow(EditorWindow window, object[] data);
}
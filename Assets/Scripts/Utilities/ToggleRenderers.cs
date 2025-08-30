//-----------------------------\\
//              Project HITHC
//    Author: Joshua Hughes
//        Twitch.tv/neokuro
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ToggleRenderers : MonoBehaviour
{
    public bool _toggleRenderers;
    public bool _toggleColliders;

    private void Update()
    {
        ToggleColliders();

        if(!_toggleRenderers)
        {
            return;
        }

        _toggleRenderers = false;
        Renderer[] rends = GetComponentsInChildren<Renderer>();

        for (int i = 0; i < rends.Length; i++)
        {
            rends[i].enabled = !rends[i].enabled;
        }
    }

    private void ToggleColliders()
    {
        if(!_toggleColliders)
        {
            return;
        }

        _toggleColliders = false;

        Collider[] cols = GetComponentsInChildren<Collider>();
        for (int i = 0; i < cols.Length; i++)
        {
            cols[i].enabled = !cols[i].enabled;
        }
    }
}
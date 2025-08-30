//-----------------------------\\
//              Project HITHC
//    Author: Joshua Hughes
//        Twitch.tv/neokuro
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;

using Hzn.Framework;

using UnityEngine;

public class GenericTriggerHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject triggerEventHandler;

    private void OnTriggerEnter(Collider other)
    {
        if (HandleTriggerEvent(other))
        {
            return;
        }
    }

    private void OnTriggerStay(Collider other)
    {

    }

    private void OnTriggerExit(Collider other)
    {

    }

    private bool HandleTriggerEvent(Collider other)
    {
        ITriggerEvent ite = triggerEventHandler.GetComponentInChildren<ITriggerEvent>();

        if (ite == null)
        {
            Dbg.Error(Log.Tools,"ITriggerEvent handler is not assigned. Attempting to get reference on colliding object");
            ite = other.GetComponentInChildren<ITriggerEvent>();
        }

        if (ite == null)
        {
            return false;
        }

        ite.OnTriggerEntered(other);
        return true;
    }
}
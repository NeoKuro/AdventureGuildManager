//-----------------------------\\
//              Project HITHC
//    Author: Joshua Hughes
//        Twitch.tv/neokuro
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITriggerEvent
{
    void OnTriggerEntered(Collider other);
}
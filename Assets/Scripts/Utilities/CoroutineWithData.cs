//-----------------------------\\
//        Project HITH
//    Author: Joshua Hughes
//      Twitch.tv/neokuro
//      Twitch.tv/neokuro
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineWithData
{
    public Coroutine coroutine { get; private set; }
    public object result;
    public bool isRunning { get; private set; }
    private IEnumerator target;

    public CoroutineWithData(MonoBehaviour owner, IEnumerator target)
    {
        this.target = target;
        this.coroutine = owner.StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        isRunning = true;
        while(target.MoveNext())
        {
            result = target.Current;
            yield return result;
        }
        isRunning = false;
    }
}
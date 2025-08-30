//-----------------------------\\
//      Project Idle AI
//    Author: Joshua Hughes
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using UnityEngine;

public class DebugManager : Singleton<DebugManager>
{
    public enum DebugLevels
    {
        DEBUG = 1,
        INFO = 2,
        WARN = 3,
        ERROR = 4,
        NONE = 99
    }

    [SerializeField]
    private DebugLevels _debugLevel = DebugLevels.ERROR;
    public static DebugLevels DebugLevel
    {
        get
        {
            return Instance._debugLevel;
        }
    }

    [SerializeField]
    private bool _disableAllDebugs = false;

    public static bool DisableAllDebugs
    {
        get
        {
            return Instance._disableAllDebugs;
        }
    }

    [SerializeField]
    private bool _disablePointDebugs = false;
    public static bool DisablePointDebugs
    {
        get
        {
            return Instance._disablePointDebugs;
        }
    }

    [SerializeField]
    private bool _disableCircleDebugs = false;
    public static bool DisableCircleDebugs
    {
        get
        {
            return Instance._disableCircleDebugs;
        }
    }

    [SerializeField]
    private bool _printAsErrorLog = true;
    private static bool PrintAsErrorLog
    {
        get
        {
            return Instance._printAsErrorLog;
        }
    }


    public override void Initialise()
    {
        //#if UNITY_EDITOR == false
        //    Destroy(this);
        //#endif
    }

    public override void OnRetryExecuted()
    {

    }
}
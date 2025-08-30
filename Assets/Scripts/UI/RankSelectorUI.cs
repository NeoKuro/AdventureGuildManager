//    ADVENTURE GUILD MANAGER    
//          Author: Josh Hughes


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Hzn.Framework;

using UnityEngine;

public class RankSelectorUI : Singleton<RankSelectorUI>
{
    private Action<string> _onRankSelectedCallback;
    private EInputContext _previousContext;

    private void OnEnable()
    {
        _previousContext = InputContextManager.PeekContext();
        InputContextManager.Get().SetContextPriority(EInputContext.UI);
    }

    private void OnDisable()
    {
        InputContextManager.Get().SetContextPriority(_previousContext);
    }

    public static void ShowRankSelector(Action<string> callback)
    {
        Instance._onRankSelectedCallback = callback;
        Instance.gameObject.SetActive(true);
    }
    
    public void OnRankPressed(string rank)
    {
        if (_onRankSelectedCallback == null)
        {
            Dbg.Error(Log.UI, "RankSelectorUI: OnRankPressed called but no callback was set!" );
            return;
        }
        
        
        _onRankSelectedCallback(rank);
        gameObject.SetActive(false);
    }
}
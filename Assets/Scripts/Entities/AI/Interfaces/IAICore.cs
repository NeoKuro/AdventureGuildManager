using System;
using System.Collections.Generic;

using UnityEngine;

public interface IAICore
{
    void InitialiseBehaviour();
    
    void FixedUpdateBehaviour();

    void SetDestination(Vector3 destination);

    Dictionary<EEntityPriorities, float> EvaluateStats();
}
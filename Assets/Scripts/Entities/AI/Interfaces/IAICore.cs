using System;
using System.Collections.Generic;

public interface IAICore
{
    void InitialiseBehaviour();
    
    void FixedUpdateBehaviour();

    Dictionary<EEntityPriorities, float> EvaluateStats();
}
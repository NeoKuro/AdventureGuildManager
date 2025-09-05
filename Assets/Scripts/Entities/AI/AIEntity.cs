using System.Collections.Generic;

using Hzn.Framework;

using UnityEngine;

public abstract class AIEntity : Entity, IAICore, IQueueable
{
    protected StatsHandler _statsHandler;

    public virtual void InitialiseBehaviour()
    {
        AIBehaviourManager.RegisterNewAI(this);
        _statsHandler     = new StatsHandler(this);
    }

    public abstract void FixedUpdateBehaviour();

    public void SetDestination(Vector3 destination)
    {
        // TODO: Create a navigation controller which handles setting destinations with NavMesh etc
        throw new System.NotImplementedException();
    }

    public Dictionary<EEntityPriorities, float> EvaluateStats()
    {
        Dbg.LogVerbose(Log.AI, "Evaluating needs - determining what to do next");
        Dictionary<EEntityPriorities, float> priorities = _statsHandler.EvaluateStats();

        // These two aren't affected by the priorities, they are purely for information that can feed into further decisions
        //   IE if Health is at 1, then even if Confidence priority is at 1 as well, they will go to an infirmiry
        //  Confidence / Money is used to decide "Ok so I'm bored, do I have money to go shopping? What can I buy?" etc
        //  Additionally, Confidence can be used 
        
        // Calculate Money / Confidence stats next
        //  These are used to feed into other decisions after core priorities are evaluated (IE HP is ok etc)
        //  If the AI has low confidence but high money, they may go buy some gear to improve confidence before going on a quest
        //  If they have low confidnce + low money they may choose easier quests which they are more comfortable with
        //      To do this one though, the AI will need to evaluate their base confidence against the quest reported difficulty
        //      A rank C may choose for a Rank D quest if low confidence
        //  Otherwise, if high confidence but low money, they may prioritise better payouts regardless of the risk
        
        // These will likely go through the 'INVENTORY_HANDLER' class when created as it almost entirely depends on their
        //  inventory (how prepared are they - confidence may also tie into their skills/level as well)
        //  The AI should have a 'minimum Wealth Threshold' or expectation. IE a Rank F may have low expectations, so average
        //      weekly balance of say 50 silver. But a Rank A will have much higher, with weekly balance of say 50 gold, but also
        //      average net worth too (gear value etc)
        //  If they spend too long at low values, they may grow frustrated and leave (indicates they aren't able to earn enough
        //      from quests for their levels

        // TODO: REDO THIS? The Priorities account for the Core stats. But other things influence decisions as well;
        //      - Such as how much money they have
        //      - What is in their inventory (lots to sell? Enough to buy things? Or just filling inventory?)
        //      - Do they need to do more prep like buying potions and equipment?
        //      - Can they afford a hospital? Will they need to rest at home instead cuz they can't afford it?
        //  NOTE: It may be best to split this up into multiple functions instead. The core priorities may influence what
        //      they generally want to do (need to heal, rest, eat etc). But beyond that we start looking into what they
        //      have or don't have, want etc.
        return priorities;
    }

    public void NotifyRemovedFromQueue()
    {
        throw new System.NotImplementedException();
    }

    public void NotifyQueueCleared()
    {
        throw new System.NotImplementedException();
    }

    public void MoveToNextQueuePosition(Vector3 position)
    {
        throw new System.NotImplementedException();
    }

    public void OnReachedFrontOfQueue()
    {
        // 
        throw new System.NotImplementedException();
    }
}
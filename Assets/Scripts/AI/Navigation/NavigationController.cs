using Hzn.Framework;

public class NavigationController
{
    private AIEntity _aiEntity;
    
    public NavigationController(AIEntity aiRef)
    {
        Dbg.LogVerbose(Log.AI, "Creating Navigation Controller");
        _aiEntity = aiRef;
    }
}
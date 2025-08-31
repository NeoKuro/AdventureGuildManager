public interface IObjectiveTarget
{
    /// <summary>
    /// Get the name of this target. Pass in the <see cref="amount"/> to determine if is plural form or not
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    string GetName(int amount = 1);
    int GetId();
}
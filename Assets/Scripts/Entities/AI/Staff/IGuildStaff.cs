using UnityEngine;

public interface IGuildStaff : IGuildCore
{
    public bool CreateNewEntity(Transform root, GameObject prefab);
}
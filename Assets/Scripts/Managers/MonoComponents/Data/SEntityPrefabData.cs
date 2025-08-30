using System;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public struct SEntityPrefabData
{
    public EEntityPrefabCategories category;
    public List<GameObject>        prefabs;
}
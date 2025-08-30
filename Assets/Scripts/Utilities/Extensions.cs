using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Hzn.Framework;

using UnityEngine;
using Random = UnityEngine.Random;

public static class Extensions
{
    public static T ToEnum<T>(this string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }

    public static string Filter(this string str, List<char> charsToRemove)
    {
        foreach (char c in charsToRemove)
        {
            str = str.Replace(c.ToString(), String.Empty);
        }

        return str;
    }

    public static bool TryGetComponentInParent<T>(this Transform obj, out T result)
    {
        Transform tmp = obj;
        if (!tmp.TryGetComponent<T>(out result))
        {
            while (tmp != null && result == null)
            {
                tmp = tmp.parent;
                if (tmp.TryGetComponent<T>(out result))
                {
                    break;
                }
            }
        }

        return result != null;
    }

    public static bool TryGetComponentInParent<T>(this GameObject obj, out T result)
    {
        Transform tmp = obj.transform;
        if (!tmp.TryGetComponent<T>(out result))
        {
            while (tmp != null && result == null)
            {
                tmp = tmp.parent;
                if (tmp.TryGetComponent<T>(out result))
                {
                    break;
                }
            }
        }

        return result != null;
    }

    public static bool TryGetComponentInChildren<T>(this Transform obj, out T result)
    {
        Transform tmp = obj;
        if (tmp.TryGetComponent<T>(out result))
        {
            return true;
        }

        if (tmp.childCount == 0)
        {
            return false;
        }

        Transform childCounter = tmp;
        Stack<Transform> childrenWithChildren = new Stack<Transform>();

        Dbg.Warn(Log.Tools,"This method is less efficient than Unity's by a significant margin (~20000ms)");

        do
        {
            for (int i = 0; i < childCounter.childCount; i++)
            {
                Transform child = childCounter.GetChild(i);
                if (child.childCount > 0)
                {
                    childrenWithChildren.Push(child);
                }

                if (child.TryGetComponent<T>(out result))
                {
                    childrenWithChildren.Clear();
                    return true;
                }
            }
            if (!childrenWithChildren.TryPop(out childCounter))
            {
                break;
            }
        } while (result == null && childrenWithChildren.Count > 0);

        return result != null;
    }

    public static bool TryGetComponentInChildren<T>(this GameObject obj, out T result)
    {
        Transform tmp = obj.transform;
        if (tmp.TryGetComponent<T>(out result))
        {
            return true;
        }

        if (tmp.childCount == 0)
        {
            return false;
        }

        Transform childCounter = tmp;
        Stack<Transform> childrenWithChildren = new Stack<Transform>();
        bool checkChildren = true;

        Dbg.Warn(Log.Tools,"This method is less efficient than Unity's by a significant margin (~20000ms)");

        do
        {
            for (int i = 0; i < childCounter.childCount; i++)
            {
                Transform child = childCounter.GetChild(i);
                Dbg.Log(Log.Tools,$"TESTING: {child.name}");
                if (child.childCount > 0)
                {
                    Dbg.Log(Log.Tools,$"ADDING: {child.name}");
                    childrenWithChildren.Push(child);
                }

                if (child.TryGetComponent<T>(out result))
                {
                    childrenWithChildren.Clear();
                    return true;
                }
            }

            checkChildren = childrenWithChildren.Count > 0;

            if (!childrenWithChildren.TryPop(out childCounter))
            {
                break;
            }

            Dbg.Log(Log.Tools,$"POPPING: {childCounter.name}");
        } while (result == null && checkChildren);

        return result != null;
    }

    public static Vector3 NicePosition(this MonoBehaviour obj)
    {
        return obj.transform.position;
    }

    public static Vector3 LocalPosition(this MonoBehaviour obj)
    {
        return obj.transform.localPosition;
    }


    /// <summary>
    /// Returns whether the two vector3s are 'close enough' to be considered the same. Whether each three elements are less than theta.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <param name="theta">The maximum allowed difference</param>
    /// <returns></returns>
    public static bool Approximately(this Vector3 lhs, Vector3 rhs, float theta)
    {
        float dX = Mathf.Abs(lhs.x - rhs.x);
        float dY = Mathf.Abs(lhs.y - rhs.y);
        float dZ = Mathf.Abs(lhs.z - rhs.z);

        if (dX > theta)
        {
            return false;
        }

        if (dY > theta)
        {
            return false;
        }

        if (dZ > theta)
        {
            return false;
        }
        return true;
    }

    public static float Square(this float val)
    {
        return val * val;

    }
    public static int LayerToLayerMask(this GameObject obj)
    {
        return 1 << obj.layer;
    }

    public static Vector3 Flatten(this Vector3 pos)
    {
        return new Vector3(pos.x, 0f, pos.z);
    }

    public static Vector3 SetX(this Vector3 val, float newVal)
    {
        return new Vector3(newVal, val.y, val.z);
    }

    public static Vector3 SetY(this Vector3 val, float newVal)
    {
        return new Vector3(val.x, newVal, val.z);
    }

    public static Vector3 SetZ(this Vector3 val, float newVal)
    {
        return new Vector3(val.x, val.y, newVal);
    }

    public static Vector3 AddX(this Vector3 val, float newVal)
    {
        return new Vector3(val.x + newVal, val.y, val.z);
    }

    public static Vector3 AddY(this Vector3 val, float newVal)
    {
        return new Vector3(val.x, val.y + newVal, val.z);
    }

    public static Vector3 AddZ(this Vector3 val, float newVal)
    {
        return new Vector3(val.x, val.y, val.z + newVal);
    }

    //private static System.Random rng = new System.Random();
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            //int k = rng.Next(n + 1);
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static T GetRandom<T>(this IList<T> list)
    {
        if (list == null || list.Count == 0)
        {
            Dbg.Error(Log.Tools, $"Trying to Get random from list, but the list is null or empty");
            return default(T);
        }

        if (list.Count == 1)
        {
            return list[0];
        }
        
        int k = Random.Range(0, list.Count);
        return list[k];
    }

    public static Transform FindObjectWithTagRecursive(this Transform parent, string tag)
    {
        if (parent.CompareTag(tag))
        {
            return parent;
        }

        List<Transform> childrenWithChildren = new List<Transform>();
        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).CompareTag(tag))
            {
                return parent.GetChild(i);
            }

            if (parent.GetChild(i).childCount == 0)
            {
                continue;
            }
            
            childrenWithChildren.Add(parent.GetChild(i));
        }
        
        foreach (Transform child in childrenWithChildren)
        {
            Transform result = FindObjectWithTagRecursive(child, tag);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }

    #region - Get Largest Element In Vector -
    public static float GetLargestElementInVector(this Vector2 vector)
    {
        float maxVal = 0f;
        if (vector.x > maxVal)
            maxVal = vector.x;

        if (vector.y > maxVal)
            maxVal = vector.y;

        return maxVal;
    }

    public static float GetLargestElementInVector(this Vector3 vector)
    {
        float maxVal = 0f;
        if (vector.x > maxVal)
            maxVal = vector.x;

        if (vector.y > maxVal)
            maxVal = vector.y;

        if (vector.z > maxVal)
            maxVal = vector.z;

        return maxVal;
    }

    public static float GetLargestElementInVector(this Vector4 vector)
    {
        float maxVal = 0f;
        if (vector.x > maxVal)
            maxVal = vector.x;

        if (vector.y > maxVal)
            maxVal = vector.y;

        if (vector.z > maxVal)
            maxVal = vector.z;

        if (vector.w > maxVal)
            maxVal = vector.w;

        return maxVal;
    }
    #endregion

}

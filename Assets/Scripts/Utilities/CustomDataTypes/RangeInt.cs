//-----------------------------\\
//    Project TowerDefence
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//      Twitch.tv/neokuro
//-----------------------------\\

using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class RangeInt
{
    public int min = 0;
    public int max = 1;

    public RangeInt(int _min, int _max)
    {
        min = _min;
        max = _max;
    }

    public int Random
    {
        get
        {
            return UnityEngine.Random.Range(min, max);
        }
    }

    public static RangeInt operator +(RangeInt lhs, int rhs)
    {
        return new RangeInt(lhs.min + rhs, lhs.max + rhs);
    }
}

[Serializable]
public class RangeFloat
{
    public float min = 0;
    public float max = 1;

    public RangeFloat(float _min, float _max)
    {
        min = _min;
        max = _max;
    }

    public float Random
    {
        get
        {
            return UnityEngine.Random.Range(min, max);
        }
    }

    public bool Contains(float val)
    {
        return val >= min && val <= max;
    }

    public float GetClosestVal(float val)
    {
        float diff1 = Mathf.Abs(val - min);
        float diff2 = Mathf.Abs(val - max);
        return diff1 < diff2 ? min : max;
    }

    public static RangeFloat operator +(RangeFloat lhs, float rhs)
    {
        return new RangeFloat(lhs.min + rhs, lhs.max + rhs);
    }
}
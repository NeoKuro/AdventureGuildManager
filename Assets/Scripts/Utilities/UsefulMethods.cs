using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Hzn.Framework;

using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class UsefulMethods : MonoBehaviour
{
    // Gravity Functions
    public static float AccelerationFromSubterraneanGravity(float avgDensity, float distToCentre)
    {
        return 1f;
        //float a = (4f / 3f) * Mathf.PI * Constants.GRAVITATIONAL_CONSTANT * avgDensity * distToCentre;
        //return a;
    }

    /// <summary>
    /// Calculate the acceleration due to gravity (above the surface)
    ///     Values passed in should NOT be scaled
    /// </summary>
    /// <param name="m1">Mass of centre of gravity (IE larger body / planet)</param>
    /// <param name="m2">Mass of satellite</param>
    /// <param name="radius">Unscaled radius</param>
    /// <returns>Acceleration in 1000Km/s - IE 1km/s would be 0.001,  6000km/s would be 6.0</returns>
    public static float AccelerationFromGravityBetweenTwoBodies(float m1, float m2, float radius)
    {
        return 1f;
        //float r2 = (radius * 1000000f) * (radius * 1000000f);
        //float fGrav = (Constants.GRAVITATIONAL_CONSTANT * (m1 * m2) / (r2));
        //float acc = fGrav / m2;
        //return acc / 1000000f;
    }

    /// <summary>
    /// Calculate the acceleration due to gravity (above the surface)
    ///     Values passed in should NOT be scaled
    /// </summary>
    /// <param name="m1">Mass of centre of gravity (IE larger body / planet)</param>
    /// <param name="m2">Mass of satellite</param>
    /// <param name="radius">Unscaled radius</param>
    /// <returns>Acceleration in 1000Km/s - IE 1km/s would be 0.001,  6000km/s would be 6.0</returns>
    public static double AccelerationFromGravity(double m1, double radius)
    {
        double r2 = (radius * 1000000d) * (radius * 1000000d);
        double accGrav = Constants.GRAVITATIONAL_CONSTANT * m1 / r2;
        return accGrav / 1000000d;
    }

    public static double Deg2RadD
    {
        get
        {
            return Math.PI / 180d;
        }
    }

    public static float CheapDistance(Vector3 v1, Vector3 v2)
    {
        return Mathf.Sqrt(CheapDistanceSquared(v1, v2));
    }

    public static float CheapDistanceSquared(Vector3 v1, Vector3 v2)
    {
        Vector3 deltaPos = v2 - v1;
        float dist = deltaPos.x.Square() + deltaPos.y.Square() + deltaPos.z.Square();
        return dist;
    }

    public static float Remap(float value, float from1, float to1, float from2, float to2)
    {
        //return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        return from2 + (value - from1) * (to2 - from2) / (to1 - from1);
    }

    public static int IntToBit(int layer)
    {
        return 1 << layer;
    }

    public static float TicksToHours(decimal ticks)
    {
        return (float)(ticks / 10000000) / 3600f;
    }

    #region - Get Largest Element In Vector -
    public static float GetLargestElementInVector(Vector2 vector)
    {
        float maxVal = 0f;
        if (vector.x > maxVal)
            maxVal = vector.x;

        if (vector.y > maxVal)
            maxVal = vector.y;

        return maxVal;
    }

    public static float GetLargestElementInVector(Vector3 vector)
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

    public static float GetLargestElementInVector(Vector4 vector)
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

    public static float Map(float value, float inputMin, float inputMax, float outputMin, float outputMax)
    {
        if (value < inputMin)
        {
            value = inputMin;
        }

        if (value > inputMax)
        {
            value = inputMax;
        }

        return (value - inputMin) / (inputMax - inputMin) * (outputMax - outputMin) + outputMin;
    }

    public static Vector3 GetPointAroundCentre(Vector3 centre, float minRadius, float maxRadius, bool flatten)
    {
        Vector3 point = centre;

        float x = Random.Range(minRadius, maxRadius);
        x *= Random.Range(0f, 1f) > 0.5f ? 1f : -1f;

        float y = Random.Range(minRadius, maxRadius);
        y *= Random.Range(0f, 1f) > 0.5f ? 1f : -1f;

        float z = Random.Range(minRadius, maxRadius);
        z *= Random.Range(0f, 1f) > 0.5f ? 1f : -1f;

        point += new Vector3(x, y, z);

        if (flatten)
        {
            point = point.SetY(0f);
        }

        return point;
    }

    public static Vector3 GetPointAroundCentre(Vector3 centre, float minRadius, float maxRadius, float maxYDelta)
    {
        Vector3 point = centre;

        float x = Random.Range(minRadius, maxRadius);
        x *= Random.Range(0f, 1f) > 0.5f ? 1f : -1f;

        float y = Random.Range(-maxYDelta, maxYDelta);
        y *= Random.Range(0f, 1f) > 0.5f ? 1f : -1f;

        float z = Random.Range(minRadius, maxRadius);
        z *= Random.Range(0f, 1f) > 0.5f ? 1f : -1f;

        point += new Vector3(x, y, z);

        return point;
    }

    public static Vector3 GetRandomDestinationInArc(Vector3 centre, Vector3 forward, float staticDistance, float angle)
    {
        return RandomPointInCircle(centre, forward, staticDistance, Random.Range(-angle, angle));
    }

    public static Vector3 GetRandomDestinationInArc(Vector3 centre, Vector3 forward, float minDist, float maxDist, float angle)
    {
        return RandomPointInCircle(centre, forward, Random.Range(minDist, maxDist), Random.Range(-angle, angle));
    }

    public static Vector3 GetRandomPositionAroundCentre(Vector3 centre, float minDist, float maxDist)
    {
        return RandomPointInCircle(centre, Vector3.forward, Random.Range(minDist, maxDist), Random.Range(-360f, 360f));
    }

    public static Vector3 GetRandomPositionAroundCentreWithNavMeshRaycast(Vector3 centre, float minDist, float maxDist, int maxTries = 5, float yOffset = 0f)
    {
        Vector3 targetLocation;
        bool success = false;
        int tries = 0;
        do
        {
            targetLocation = RandomPointInCircle(centre, Vector3.forward, Random.Range(minDist, maxDist), 360f);

            RaycastHit hit;
            Ray ray = new Ray(targetLocation, Vector3.down);
            if(Physics.Raycast(ray, out hit, 5f))
            {
                targetLocation = hit.point;
                success = true;
            }

            tries++;
        } while (tries < maxTries && !success);

        return targetLocation + Vector3.up * yOffset;
    }

    public static Vector3 RandomPointInCircle(Vector3 center, Vector3 forward, float radius, float angle)
    {    //Draws circle of radius, with center center, and locates a point on that circle within angle angle     
        Vector3 position;
        //position.x = center.x + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
        //position.z = center.z + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
        //position.y = center.y;
        position = center + Quaternion.AngleAxis(angle / 2, Vector3.up) * (forward * radius);
        return position;
    }


    public static string GetFormattedCurrency(double _currentCurrency)
    {
        if (_currentCurrency < 1d)
        {
            return "0";
        }

        int n = (int)Math.Log(_currentCurrency, 1000);
        int unitCount = Enum.GetValues(typeof(UnitCurrencyScale)).Length;
        double m = _currentCurrency / Math.Pow(1000, n);
        string unit;

        if (n < unitCount)
        {
            if (Enum.IsDefined(typeof(UnitCurrencyScale), n))
            {
                unit = ((UnitCurrencyScale)n).ToString();
                if (unit == (UnitCurrencyScale.NONE).ToString() || unit == (UnitCurrencyScale.K).ToString())
                {
                    return (Math.Floor(_currentCurrency).ToString("#,###"));
                }
            }
            else
            {
                Dbg.Error(Hzn.Framework.Log.Tools,$"Attempted to cast from int {n} to UnitCurrencyScale enum but failed. The index value {n} is not defined in the enum. Defaulting to AA format");
                unit = GetUnitInAAFormat(n, unitCount);
            }
        }
        else
        {
            unit = GetUnitInAAFormat(n, unitCount);
        }

        return (Math.Floor(m * 100) / 100).ToString("0.###") + unit;
    }

    public static string GetProjectName()
    {
        string[] s = Application.dataPath.Split('/');
        string projectName = s[s.Length - 2];
        return projectName;
    }

    private static readonly int charAValue = Convert.ToInt32('a');
    public static string GetUnitInAAFormat(int n, int unitCount)
    {
        int primaryUnitValue = n - unitCount;
        int secondUnit = primaryUnitValue % 26;
        int firstUnit = primaryUnitValue / 26;
        char first = Convert.ToChar((firstUnit + charAValue));
        char second = Convert.ToChar(secondUnit + charAValue);
        return first.ToString() + second.ToString();
    }
}

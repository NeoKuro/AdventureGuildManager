using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class DebugExtensions
{

    public static void DrawCircle(Vector3 centre, float radius, Color debugColour)
    {
        DrawCircle(centre, radius, 1f, debugColour);
    }

    public static void DrawCircle(Vector3 centre, float radius, float duration, Color debugColour)
    {
        if (DebugManager.DisableAllDebugs || DebugManager.DisableCircleDebugs)
        {
            return;
        }
#if UNITY_EDITOR
        float theta = 0;
        float x = radius * Mathf.Cos(theta);
        float y = radius * Mathf.Sin(theta);
        Vector3 currentPos = centre + new Vector3(x, 0, y);
        Vector3 nextPos = currentPos;
        Vector3 finalPos = currentPos;
        for (theta = 0.1f; theta < Mathf.PI * 2; theta += 0.1f)
        {
            x = radius * Mathf.Cos(theta);
            y = radius * Mathf.Sin(theta);
            nextPos = centre + new Vector3(x, 0f, y);
            Debug.DrawLine(currentPos, nextPos, debugColour, duration);
            currentPos = nextPos;
        }
        Debug.DrawLine(currentPos, finalPos, debugColour, duration);
#endif
    }

    public static void DrawWedge(Vector3 centre, Vector3 forward, float minRadius, float maxRadius, float angle, float duration, Color col)
    {
        if (DebugManager.DisableAllDebugs || DebugManager.DisablePointDebugs)
        {
            return;
        }
#if UNITY_EDITOR
        float finalInner = DrawWireArc(centre, forward, minRadius, angle, duration, col);
        float finalOuter = DrawWireArc(centre, forward, maxRadius, angle, duration, col);
        float thisAngle = Vector3.SignedAngle(Vector3.forward, forward, Vector3.up) - (angle / 2f);
        float x = Mathf.Sin(Mathf.Deg2Rad * thisAngle);
        float y = Mathf.Cos(Mathf.Deg2Rad * thisAngle);

        Debug.DrawLine(new Vector3(x * minRadius, 0f, y * minRadius) + centre, new Vector3(x * maxRadius, 0f, y * maxRadius) + centre, col, duration);

        x = Mathf.Sin(Mathf.Deg2Rad * finalInner);
        y = Mathf.Cos(Mathf.Deg2Rad * finalInner);

        Debug.DrawLine(new Vector3(x * minRadius, 0f, y * minRadius) + centre, new Vector3(x * maxRadius, 0f, y * maxRadius) + centre, col, duration);
#endif
    }

    public static float DrawWireArc(Vector3 centre, Vector3 forward, float radius, float angle, float duration, Color col)
    {
        if (DebugManager.DisableAllDebugs || DebugManager.DisablePointDebugs)
        {
            return 0f;
        }
#if UNITY_EDITOR
        List<Vector3> arcPoints = new List<Vector3>();
        float thisAngle = Vector3.SignedAngle(Vector3.forward, forward, Vector3.up) - (angle / 2f);
        float arcLength = angle;
        int segments = 010;
        for (int i = 0; i <= segments; i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * thisAngle) * radius;
            float y = Mathf.Cos(Mathf.Deg2Rad * thisAngle) * radius;
            arcPoints.Add(new Vector3(x, 0f, y) + centre);
            if (i < segments - 1)
            {
                thisAngle += (arcLength / segments);
            }
        }

        for (int i = 1; i < arcPoints.Count; i++)
        {
            Debug.DrawLine(arcPoints[i - 1], arcPoints[i], col, duration);
        }

        return thisAngle;
#else
        return 0f;
#endif
    }

    public static void DrawPoint(Vector3 centre, float size, Color col)
    {
        DrawPoint(centre, size, 0f, col);
    }

    public static void DrawPoint(Vector3 centre, float size, float duration, Color col)
    {
        if (DebugManager.DisableAllDebugs || DebugManager.DisablePointDebugs)
        {
            return;
        }

#if UNITY_EDITOR
        Debug.DrawLine(centre + Vector3.down * size, centre + (Vector3.up * size), col, duration);
        Debug.DrawLine(centre + Vector3.right * size, centre + (Vector3.left * size), col, duration);
        Debug.DrawLine(centre + Vector3.back * size, centre + (Vector3.forward * size), col, duration);
#endif
    }

}
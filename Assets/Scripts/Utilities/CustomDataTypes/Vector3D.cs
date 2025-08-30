using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public struct Vector3D
{
    public double x { get; private set; }
    public double y { get; private set; }
    public double z { get; private set; }

    public Vector3D(double x, double y, double z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public static Vector3D zero { get { return new Vector3D(0, 0, 0); } }

    public static explicit operator Vector3(Vector3D val)
    {
        return new Vector3((float)val.x, (float)val.y, (float)val.z);
    }

    public static explicit operator Vector3D(Vector3 val)
    {
        return new Vector3D(val.x, val.y, val.z);
    }


    public static Vector3D operator *(Vector3D lhs, double rhs)
    {
        return new Vector3D(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs);
    }

    public static Vector3D operator /(Vector3D lhs, double rhs)
    {
        return new Vector3D(lhs.x / rhs, lhs.y / rhs, lhs.z / rhs);
    }

    public static Vector3D operator +(Vector3D lhs, Vector3D rhs)
    {
        return new Vector3D(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
    }

    public static Vector3D operator -(Vector3D lhs, Vector3D rhs)
    {
        return new Vector3D(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
    }

    public static Vector3D operator -(Vector3D lhs, Vector3 rhs)
    {
        return new Vector3D(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
    }
}

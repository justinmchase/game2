using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Extensions
{

    public static Vector3 XY(this Vector3 v)
    {
        return new Vector3(v.x, v.y, 0f);
    }

    public static Direction ToDirection(this Vector3 v)
    {
        float epsilon = 0.2f;
        if (v.y > epsilon) return Direction.N;
        if (v.y < -epsilon) return Direction.S;
        if (v.x > epsilon) return Direction.E;
        if (v.x < -epsilon) return Direction.W;
        return default(Direction);
    }
    
    public static Direction ToDirection(this Vector3Int v)
    {
        float epsilon = 0.2f;
        if (v.y > epsilon) return Direction.N;
        if (v.y < -epsilon) return Direction.S;
        if (v.x > epsilon) return Direction.E;
        if (v.x < -epsilon) return Direction.W;
        return default(Direction);
    }

    public static Direction DirTo(this Vector3 src, Vector3 dst)
    {
        return (src - dst).ToDirection();
    }

    public static Vector3Int ToInt(this Vector3 src)
    {
        return Vector3Int.FloorToInt(src);
    }

    public static TValue SafeGetValue<TKey, TValue>(this Dictionary<TKey, TValue> d, TKey k)
    {
        var v = default(TValue);
        if(d.TryGetValue(k, out v))
        {
            return v;
        }
        return v;
    }
}
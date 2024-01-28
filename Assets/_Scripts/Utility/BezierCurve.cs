using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve
{
    public Vector2 X { get; set; }
    public Vector2 Y { get; set; }
    public Vector2 Z { get; set; }

    public Vector2 GetPoint(float t)
    {
        Vector2 P1 = Vector2.Lerp(X, Z, t);
        Vector2 P2 = Vector2.Lerp(Z, Y, t);
        return Vector2.Lerp(P1, P2, t);
    }
}

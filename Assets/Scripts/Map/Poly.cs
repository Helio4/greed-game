using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Poly {

    public static bool ContainsPoint(Vector2[] polyPoints, Vector2 point) {
        bool inside = false;
        int j = polyPoints.Length - 1;
        for (int i = 0; i < polyPoints.Length; j = i++) {
            if( ((polyPoints[i].y < point.y && point.y < polyPoints[j].y) || (polyPoints[j].y < point.y && point.y < polyPoints[i].y)) &&
                (point.x < (polyPoints[j].x - polyPoints[i].x) * (point.y - polyPoints[i].y) / (polyPoints[j].y - polyPoints[i].y) + polyPoints[i].x) ) 
            {
                inside = !inside;
            }
        }
        return inside;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LineRendererExtensions
{
    public static float Length(this LineRenderer renderer)
    {
        int total = renderer.positionCount;
        if (total < 2) return 0;
        float length = 0;
        for (int i = 0; i < total - 1; i++)
        {
            length += Vector3.Distance(renderer.GetPosition(i), renderer.GetPosition(i + 1));
        }
        return length;
    }
}

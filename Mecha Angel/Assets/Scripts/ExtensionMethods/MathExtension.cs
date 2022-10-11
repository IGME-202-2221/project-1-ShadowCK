using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathExtension
{
    public static float Map(this float oldValue, float oldMin, float oldMax, float newMin, float newMax)
    {
        return (oldValue - oldMin) / (oldMax - oldMin) * (newMax - newMin) + newMin;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Reference: https://answers.unity.com/questions/888257/access-left-right-top-and-bottom-of-recttransform.html
/// </summary>
public static class RectTransformExtensions
{
    public static void SetLeft(this RectTransform rt, float left)
    {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    }

    public static void SetRight(this RectTransform rt, float right)
    {
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    }

    public static void SetTop(this RectTransform rt, float top)
    {
        rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
    }

    public static void SetBottom(this RectTransform rt, float bottom)
    {
        rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    }

    public static void ResetOffsets(this RectTransform rt)
    {
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }
}

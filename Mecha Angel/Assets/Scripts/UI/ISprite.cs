using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISprite
{
    [HideInInspector]
    public SpriteRenderer Sprite
    {
        get;
    }
    [HideInInspector]
    public Bounds Bounds
    {
        get;
    }

    // bounds.center it's not always equivalent to transform.position! (change of bounds / anchor of object not at center)
    public Vector3 Center => Bounds.center;
    public float X => Bounds.center.x;
    public float Y => Bounds.center.y;
    public Vector3 Min => Bounds.min;
    public float MinX => Min.x;
    public float MinY => Min.y;
    public Vector3 Max => Bounds.max;
    public float MaxX => Max.x;
    public float MaxY => Max.y;
    public Vector3 Size => Bounds.size;
    public float Width => Size.x;
    public float Height => Size.y;
    public Vector3 Extents => Extents;
}
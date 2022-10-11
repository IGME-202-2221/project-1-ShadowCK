using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete("Some scrolling I tried... Not pleasant.", true)]
public class ScrollingBackground : MonoBehaviour, ISprite
{
    private Vector3 originalPosition;
    [SerializeField]
    private float scrollSpeed;
    [SerializeField]
    private Vector3 scrollDirection;

    private SpriteRenderer sprite;
    [SerializeField]
    private Bounds bounds;

    public SpriteRenderer Sprite => sprite;
    public Bounds Bounds => bounds;

    // bounds.center it's not always equivalent to transform.position! (change of bounds / anchor of object not at center)
    public Vector3 Center => bounds.center;
    public float X => bounds.center.x;
    public float Y => bounds.center.y;
    public Vector3 Min => bounds.min;
    public float MinX => Min.x;
    public float MinY => Min.y;
    public Vector3 Max => bounds.max;
    public float MaxX => Max.x;
    public float MaxY => Max.y;
    public Vector3 Size => bounds.size;
    public float Width => Size.x;
    public float Height => Size.y;
    public Vector3 Extents => Extents;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        bounds = sprite.bounds;

        originalPosition = transform.position;
        // Normalizes direction if not normalized
        scrollDirection = scrollDirection.normalized;
    }

    void Update()
    {
        // Scrolls the background
        Camera camera = Camera.main;
        Bounds cameraBounds = Game.CameraSettings.OrthographicBounds(camera);
        transform.position += scrollSpeed * Time.deltaTime * scrollDirection;
        // Updates bounds after movement
        bounds = sprite.bounds;

        // Reverts direction when camera is not fully contained in background
        bool isContainsCamera = ContainsAll(bounds, cameraBounds, true);
        if (!isContainsCamera)
        {
            scrollDirection *= -1;
        }
    }

    /// <summary>
    /// If the Bounds contains all vertices of another Bounds
    /// Reference: https://answers.unity.com/questions/29797/how-to-get-8-vertices-from-bounds-properties.html
    /// </summary>
    /// <param name="container"> The container Bounds </param>
    /// <param name="other"> The other Bounds </param>
    /// <returns> If all vertices of the other Bounds are contained </returns>
    public static bool ContainsAll(Bounds container, Bounds other, bool is2D)
    {
        List<Vector3> vertices = new();
        Vector3 boundPoint1 = other.min;
        Vector3 boundPoint2 = other.max;

        if (is2D)
        {
            vertices.Add(new Vector3(boundPoint1.x, boundPoint1.y, container.max.z));
            vertices.Add(new Vector3(boundPoint1.x, boundPoint2.y, container.max.z));
            vertices.Add(new Vector3(boundPoint2.x, boundPoint1.y, container.max.z));
            vertices.Add(new Vector3(boundPoint2.x, boundPoint2.y, container.max.z));
        }
        else
        {
            vertices.Add(boundPoint1);
            vertices.Add(boundPoint2);
            vertices.Add(new Vector3(boundPoint1.x, boundPoint1.y, boundPoint2.z));
            vertices.Add(new Vector3(boundPoint1.x, boundPoint2.y, boundPoint1.z));
            vertices.Add(new Vector3(boundPoint2.x, boundPoint1.y, boundPoint1.z));
            vertices.Add(new Vector3(boundPoint1.x, boundPoint2.y, boundPoint2.z));
            vertices.Add(new Vector3(boundPoint2.x, boundPoint1.y, boundPoint2.z));
            vertices.Add(new Vector3(boundPoint2.x, boundPoint2.y, boundPoint1.z));
        }
        foreach (Vector3 vertice in vertices)
        {
            if (!container.Contains(vertice))
            {
                return false;
            }
        }
        return true;
    }
}
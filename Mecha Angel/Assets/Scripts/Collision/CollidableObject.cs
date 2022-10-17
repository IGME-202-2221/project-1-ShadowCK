using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidableObject : MonoBehaviour, ISprite
{
    public bool isCurrentlyColliding = false;

    public List<CollidableObject> collisions = new List<CollidableObject>();

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

    // Radius for Circle Collision
    // This initialization is temporary! Could have a better way of customizing radii.
    public float Radius => Mathf.Max(Width / 2, Height / 2);

    void Awake()
    {
        // Initializes fields
        sprite = GetComponent<SpriteRenderer>();
        bounds = sprite.bounds;

        // Adds the object to CollisionManager, if the manager already exists
        CollisionManager manager = CollisionManager.instance;
        if (manager != null)
        {
            // Does not need to try if the program is well-examined (impossible) - i.e. collision manager 
            // registers all existing objects on instantiation and all objects after get registered here.
            manager.TryRegister(this);
        }
    }

    void Update()
    {
        // Update the bounds every frame because it's a struct (value type)
        bounds = sprite.bounds;

        // TODO: This is not necessary
        //// If I'm currently colliding, turn me red.
        //if (isCurrentlyColliding)
        //{
        //    sprite.color = Color.red;
        //}
        //else
        //{
        //    sprite.color = Color.white;
        //}
    }

    private void OnDestroy()
    {
        CollisionManager.instance.UnRegister(this);
    }

    public void RegisterCollision(CollidableObject other)
    {
        isCurrentlyColliding = true;
        collisions.Add(other);
    }

    public bool AABBCollision(CollidableObject other)
    {
        // Return true if the two objects are colliding; false if not
        return (MinX < other.MaxX &&
            other.MinX < MaxX &&
            MinY < other.MaxY &&
            other.MinY < MaxY);
    }

    public bool CircleCollision(CollidableObject other)
    {
        // Return true if the two objects are colliding; false if not
        float squaredDistance = Mathf.Pow(X - other.X, 2) + Mathf.Pow(Y - other.Y, 2);
        float squaredMinDistance = Mathf.Pow(Radius + other.Radius, 2);

        if (squaredDistance < squaredMinDistance)
        {
            // Outputs colliding objects
            Debug.Log($"{name} collides with {other.name} dist: {squaredDistance} < {squaredMinDistance}");
        }
        return squaredDistance < squaredMinDistance;
    }

    public bool PointToShape_Box(CollidableObject other)
    {
        return (X > other.MinX &&
            X < other.MaxX &&
            Y > other.MinY &&
            Y < other.MaxY);
    }

    public bool PointToShape_Circle(CollidableObject other)
    {
        float squaredDistance = Vector2.SqrMagnitude(Center - other.Center);
        return squaredDistance < Mathf.Pow(Radius + other.Radius, 2);
    }

    public void ResetCollision()
    {
        isCurrentlyColliding = false;
        collisions.Clear();
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying || CollisionManager.instance == null) return;
        if (isCurrentlyColliding)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.green;
        }

        if (CollisionManager.instance.isUsingCircleCollision)
        {
            Gizmos.DrawWireSphere(Center, Radius);
        }
        else
        {
            Gizmos.DrawWireCube(Center, Size);
        }
    }
}
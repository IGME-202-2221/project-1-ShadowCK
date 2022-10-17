using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    // Store all of the collidable objects in my scene;
    public List<CollidableObject> collidableObjects = new List<CollidableObject>();

    // Keeps track of whether circle collision or AABB is being used.
    [HideInInspector]
    public bool isUsingCircleCollision = false;

    [SerializeField]
    private TextMesh modeInfo;

    public static CollisionManager instance = null;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        UpdateModeInfo();
        // Add all objects in the current scene into the list
        // Doesn't take care of objects instantiated later - use Register() to do so
        foreach (var obj in FindObjectsOfType<GameObject>())
        {
            CollidableObject collidable = obj.GetComponent<CollidableObject>();
            if (collidable != null)
            {
                collidableObjects.Add(collidable);
            }
        }
    }

    void Update()
    {
        // Reset collision;
        foreach (var collidable in collidableObjects)
        {
            collidable.ResetCollision();
        }

        // Checks collision;
        for (int i = 0; i < collidableObjects.Count; i++)
        {
            for (int j = i + 1; j < collidableObjects.Count; j++)
            {
                // Circle collision check
                bool colliding = false;
                if (isUsingCircleCollision && CircleCollision(collidableObjects[i], collidableObjects[j]))
                {
                    colliding = true;
                }
                // AABB collision check
                else if (AABBCollision(collidableObjects[i], collidableObjects[j]))
                {
                    colliding = true;
                }
                if (colliding)
                {
                    collidableObjects[i].RegisterCollision(collidableObjects[j]);
                    collidableObjects[j].RegisterCollision(collidableObjects[i]);
                }
            }
        }

        // Process all collisions
        // TODO: ToArray() is a bad way to deal with Concurrent modification!
        foreach (CollidableObject collidable in collidableObjects.ToArray())
        {
            ProcessCollisions(collidable);
        }
    }
    private bool AABBCollision(CollidableObject objectA, CollidableObject objectB)
    {
        return objectA.AABBCollision(objectB);
    }

    private bool CircleCollision(CollidableObject objectA, CollidableObject objectB)
    {
        return objectA.CircleCollision(objectB);
    }

    public void ChangeCollisionMode()
    {
        isUsingCircleCollision = !isUsingCircleCollision;
        UpdateModeInfo();
    }

    private void UpdateModeInfo()
    {
        string mode = isUsingCircleCollision ? "Circle" : "AABB";
        modeInfo.text = $"Collision Mode: {mode}";
    }

    public void Register(CollidableObject collidable)
    {
        collidableObjects.Add(collidable);
    }

    /// <summary>
    /// Removes the collidable from the list
    /// </summary>
    /// <param name="collidable"> object to remove </param>
    public void UnRegister(CollidableObject collidable)
    {
        collidableObjects.Remove(collidable);
    }

    public bool HasRegistered(CollidableObject collidable)
    {
        return collidableObjects.Contains(collidable);
    }

    /// <summary>
    /// Tries to register the collidable into the list
    /// </summary>
    /// <param name="collidable"> collidable object </param>
    /// <returns> Returns true if the object is registered by this function; false if already exists </returns>
    public bool TryRegister(CollidableObject collidable)
    {
        if (!HasRegistered(collidable))
        {
            Register(collidable);
            return true;
        }
        return false;
    }

    public void ProcessCollisions(CollidableObject collidable)
    {
        foreach (CollidableObject other in collidable.collisions)
        {
            // A technique for shortening a bunch of if-remove-continues. The bool here has no other use.
            bool worked =
                Bullet_LivingEntity(collidable, other)
                // Swaps sides and checks again for every A-B collision type
                || Bullet_LivingEntity(other, collidable)
                || LivingEntity_LivingEntity(collidable, other);
            // Whether a collision was performed or not, remove the collision so it won't get double processed
            other.collisions.Remove(collidable);
            // collidable's collisions get reset every Update(), no need to repeat
        }
    }

    private bool Bullet_LivingEntity(CollidableObject collider, CollidableObject other)
    {
        Bullet bullet = collider.GetComponent<Bullet>();
        LivingEntity entity = other.GetComponent<LivingEntity>();

        if (bullet != null && entity != null)
        {
            bool hasParent = bullet.HasParent;
            LivingEntity parent = hasParent ? bullet.Parent.GetComponent<LivingEntity>() : null;
            if (!bullet.IsDead
                && (!hasParent || entity.gameObject != bullet.Parent.gameObject)
                && LivingEntity.IsEnemy(parent != null ? parent : bullet, entity))
            {
                if (entity is Bullet)
                {
                    Bullet entityBullet = entity as Bullet;
                    if (bullet.Parent == null || entityBullet.Parent == null || LivingEntity.IsEnemy(bullet.Parent.GetComponent<LivingEntity>(), entityBullet.Parent.GetComponent<LivingEntity>()))
                    {
                        float bulletDmg = bullet.Health;
                        float entityDmg = entity.Health;
                        entity.TakeDamage(bulletDmg);
                        bullet.TakeDamage(entityDmg);
                    }
                }
                else
                {
                    entity.TakeDamage(bullet.bulletDamage);
                    bullet.Die();
                }

                // Player's bullet hits meteor, awards score of a small fraction
                if (hasParent && bullet.Parent.GetComponent<Player>() != null && entity is Meteor)
                {
                    Game.instance.AddScore(bullet.bulletDamage / 5f);
                }
            }
            return true;
        }
        return false;
    }

    private bool LivingEntity_LivingEntity(CollidableObject collider, CollidableObject other)
    {
        LivingEntity colliderEntity = collider.GetComponent<LivingEntity>();
        LivingEntity otherEntity = other.GetComponent<LivingEntity>();
        if (colliderEntity != null && otherEntity != null)
        {
            // TODO: Pushes other by applying a force
            return true;
        }
        return false;
    }
}

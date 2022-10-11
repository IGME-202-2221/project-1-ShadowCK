using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
}

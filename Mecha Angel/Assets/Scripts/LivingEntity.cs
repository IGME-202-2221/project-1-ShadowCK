using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// An entity that can move, collide and has health
/// </summary>
public class LivingEntity : MonoBehaviour
{
    [Header("LivingEntity Stats")]
    [SerializeField]
    protected float speed = 1f;
    [SerializeField]
    protected float health = 100f;
    [SerializeField]
    protected float maxHealth = 100f;

    // The "position" field prevents modifications to the transform
    protected Vector3 position;
    protected Vector3 direction;
    protected Vector3 velocity;

    public float Health
    {
        get => health;
        set => health = value;
    }

    public float MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }

    public float HealthPercent
    {
        get => health / maxHealth;
    }

    /// <summary>
    /// The property helps normalize direction on both get and set
    /// </summary>
    public Vector3 Direction
    {
        get
        {
            if (direction.sqrMagnitude != 1.0f)
            {
                direction = direction.normalized;
            }
            return direction;
        }
        set
        {
            direction = value.normalized;
        }
    }

    [SerializeField]
    protected Camera cameraObject;

    protected virtual void Awake()
    {
        position = transform.position;
        direction = Vector3.zero;
        cameraObject = Camera.main;
    }

    protected virtual void Update()
    {
        // Updates position based on velocity and deltaTime
        velocity = direction * speed;
        Vector3 displacement = velocity * Time.deltaTime;
        position += displacement;
        transform.position = position;
    }

    /// <summary>
    /// Saves time from calling Health and MaxHealth separately
    /// </summary>
    /// <param name="health"></param>
    /// <param name="maxHealth"></param>
    public void SetHealth(float health, float maxHealth)
    {
        this.health = health;
        this.maxHealth = maxHealth;
    }
}

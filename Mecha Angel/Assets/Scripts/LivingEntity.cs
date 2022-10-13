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

    public bool IsDead
    {
        get => health <= 0;
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
        position = transform.position;

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
        this.maxHealth = maxHealth;
        this.health = health;
    }

    public void TakeDamage(float damage)
    {
        // Dead entities needn't take more damage
        if (IsDead)
        {
            return;
        }

        health -= damage;
        if (health < 0) health = 0;
        if (health == 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        health = 0;
        foreach (MonoBehaviour component in GetComponents<MonoBehaviour>())
        {
            component.enabled = false;
        }
        Destroy(gameObject, 0.1f);
    }

    protected virtual void WrapPosition()
    {
        // Does nothing
    }
}

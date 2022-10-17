using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    protected bool destroyFlag = false;

    protected Vector3 position;
    [SerializeField]
    protected Vector3 direction;
    protected Vector3 velocity;

    private SpriteRenderer sprite;
    public SpriteRenderer Sprite => sprite;

    private Coroutine hurtRoutine;
    private Coroutine deathRoutine;

    public float Health
    {
        get => Mathf.Clamp(health, 0, maxHealth);
        set => health = Mathf.Clamp(value, 0, maxHealth);
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
        sprite = GetComponent<SpriteRenderer>();

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
        WrapPosition();
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
        Debug.Log($"{name} took {damage} damage");

        health -= damage;
        if (health < 0) health = 0;
        if (health == 0)
        {
            Die();
        }
        // Health > 0, Hurts
        else
        {
            hurtRoutine ??= StartCoroutine(HurtRadient(0.2f));
        }
    }

    public virtual void Die()
    {
        health = 0;
        foreach (MonoBehaviour component in GetComponents<MonoBehaviour>())
        {
            component.enabled = false;
        }
        deathRoutine ??= StartCoroutine(DeathGradient(0.5f));
        Destroy(gameObject, 0.5f);
    }

    protected virtual void WrapPosition()
    {
        // Does nothing by default
    }

    private IEnumerator HurtRadient(float duration)
    {
        Color color = sprite.material.color;

        float red = color.r;
        float green = color.g;
        float blue = color.b;
        color.r = 1f;
        color.g = 0f;
        color.b = 0f;
        float step = 1f / duration;
        // Uses one extra frame to force an update where progress = 1
        // Another solution is to simply set back the rgb colors after the loop
        bool isSmoothEnding = false;
        for (float progress = 0; progress <= 1f;)
        {
            color.r = 1f - (1f - red) * progress;
            color.g = progress * green;
            color.b = progress * blue;
            sprite.material.color = color;
            yield return null;

            progress += Time.deltaTime * step;
            if (isSmoothEnding) break;
            else if (progress > 1)
            {
                isSmoothEnding = true;
                progress = 1;
            }
        }
        hurtRoutine = null;
    }

    private IEnumerator DeathGradient(float duration)
    {
        Color color = sprite.material.color;
        float step = 1 / duration;
        for (float alpha = 1f; alpha >= 0; alpha -= Time.deltaTime * step)
        {
            color.a = alpha;
            sprite.material.color = color;
            yield return null;
        }
        deathRoutine = null;
    }

    public static bool IsEnemy(LivingEntity attacker, LivingEntity attacked)
    {
        if (attacker == null || attacked == null)
        {
            return false;
        }
        if (attacker == attacked)
        {
            return false;
        }
        if (attacker.GetType() == attacked.GetType())
        {
            return false;
        }
        if (attacker is Enemy && attacked is Enemy)
        {
            return false;
        }
        return true;
    }
}

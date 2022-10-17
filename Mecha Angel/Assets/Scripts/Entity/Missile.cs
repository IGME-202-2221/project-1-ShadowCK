using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Missile : Bullet
{
    public LivingEntity target = null;
    protected float trackInertia = 1f;

    protected override void Update()
    {
        // Forgets destroyed target
        if (target != null && target.IsDestroyed())
        {
            target = null;
        }
        // Updates direction to track target
        // If target is not valid, functions as a normal bullet.
        if (target != null && target.enabled)
        {
            // Updates direction
            Vector2 towards = target.transform.position - position;
            towards.Normalize();
            Vector2 diff = towards - (Vector2)direction;
            direction += (Vector3)diff * (Time.deltaTime / trackInertia);
            // Updates rotation
            transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        }
        base.Update();
    }

    public static Missile Instantiate(Vector3 position, Vector3 direction, float bulletDamage, float bulletSpeed, Color bulletColor, float trackInertia, LivingEntity target = null)
    {
        Missile missile = Instantiate(Game.instance.missilePrefab, Vector3.zero, Quaternion.identity).GetComponent<Missile>();
        missile.Initialize(position, direction, bulletDamage, bulletSpeed, bulletColor);
        missile.SetTarget(target, trackInertia);
        return missile;
    }

    public static Missile Instantiate(Vector3 position, Vector3 direction, Shooter shooter, float trackInertia, LivingEntity target = null)
    {
        Missile missile = Instantiate(position, direction, shooter.bulletDamage, shooter.bulletSpeed, shooter.bulletColor, trackInertia, target);
        missile.parent = shooter;
        return missile;
    }

    /// <summary>
    /// Sets the missile's tracking target
    /// </summary>
    /// <param name="target"> Target to track </param>
    protected void SetTarget(LivingEntity target, float trackInertia)
    {
        this.target = target;
        this.trackInertia = trackInertia;
    }
}

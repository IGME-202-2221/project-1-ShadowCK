using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bullet : LivingEntity
{
    [Header("Bullet Stats")]
    public float bulletDamage = 10f;
    public Color bulletColor = Color.white;
    protected Shooter parent = null;

    public bool HasParent
    {
        get
        {
            if (parent.IsDestroyed())
            {
                parent = null;
            }
            return parent != null;
        }
    }

    public Shooter Parent
    {
        get
        {
            if (parent.IsDestroyed())
            {
                parent = null;
            }
            return parent;
        }
    }

    protected override void Update()
    {
        base.Update();
        // If out of bounds, destroy after 0.5s
        if (!destroyFlag && Game.IsOutOfBounds(this, Game.instance.mainCamera))
        {
            destroyFlag = true;
            Destroy(gameObject, 0.1f);
        }
    }

    protected virtual Bullet Initialize(Vector3 position, Vector3 direction, float bulletDamage, float bulletSpeed, Color bulletColor)
    {
        transform.position = position;
        Quaternion rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, direction));
        transform.rotation = rotation;
        this.bulletDamage = bulletDamage;
        speed = bulletSpeed;
        Direction = direction;
        this.bulletColor = bulletColor;
        return this;
    }

    /// <summary>
    /// Instantiates a bullet iwth no parent
    /// </summary>
    /// <param name="position"> Initial position </param>
    /// <param name="direction"> Movement direction </param>
    /// <param name="bulletDamage"> Damage dealt on hit </param>
    /// <param name="bulletSpeed"> Movement speed </param>
    /// <param name="bulletColor"> Color </param>
    /// <returns></returns>
    public static Bullet Instantiate(Vector3 position, Vector3 direction, float bulletDamage, float bulletSpeed, Color bulletColor)
    {
        // Note that Vector2.up is the sprite orientation (where it faces) which Unity doesn't have an AI to tell.
        Quaternion rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, direction));
        Bullet bullet = Instantiate(Game.instance.bulletPrefab, position, rotation).GetComponent<Bullet>();
        bullet.bulletDamage = bulletDamage;
        bullet.speed = bulletSpeed;
        bullet.Direction = direction;
        bullet.bulletColor = bulletColor;
        return bullet;
    }

    /// <summary>
    /// Instantiates a bullet with a shooter parent
    /// </summary>
    /// <param name="position"> Initial position </param>
    /// <param name="direction"> Movement direction </param>
    /// <param name="shooter"> Shooter parent. A bullet won't hurt its parent. </param>
    /// <returns></returns>
    public static Bullet Instantiate(Vector3 position, Vector3 direction, Shooter shooter)
    {
        Bullet bullet = Instantiate(position, direction, shooter.bulletDamage, shooter.bulletSpeed, shooter.bulletColor);
        bullet.parent = shooter;
        return bullet;
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : LivingEntity
{
    [Header("Bullet Stats")]
    public float bulletDamage = 10f;
    public Color bulletColor = Color.white;
    public Shooter parent = null;

    public bool HasParent
    {
        get => parent != null;
    }

    protected override void Update()
    {
        base.Update();
        // If out of bounds, destroy after 0.5s
        if (Game.IsOutOfBounds(this, Game.Instance.mainCamera))
        {
            Destroy(gameObject, 0.5f);
        }
    }


    public static Bullet Instantiate(Vector3 position, Vector3 direction, float bulletDamage, float bulletSpeed, Color bulletColor)
    {
        Bullet bullet = Instantiate(Game.Instance.bulletPrefab, position, Quaternion.identity).GetComponent<Bullet>();
        bullet.bulletDamage = bulletDamage;
        bullet.speed = bulletSpeed;
        bullet.direction = direction;
        bullet.bulletColor = bulletColor;
        bullet.SetHealth(1f, 1f);
        return bullet;
    }

    public static Bullet Instantiate(Vector3 position, Vector3 direction, Shooter shooter)
    {
        Bullet bullet = Instantiate(position, direction, shooter.bulletDamage, shooter.bulletSpeed, shooter.bulletColor);
        bullet.parent = shooter;
        return bullet;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// An entity that shoots bullets.
/// </summary>
public class Shooter : MonoBehaviour
{
    // Player Fields
    [HideInInspector]
    public bool isPlayer = false;
    [HideInInspector]
    public bool p_canShoot = false;
    [HideInInspector]
    public bool p_isShooting = false;

    // Enemy Fields
    [SerializeField]
    protected ShootMode shootMode = ShootMode.AlwaysTargetPlayer;
    protected enum ShootMode
    {
        AlwaysTargetPlayer,
        UseShootDirection,
        UseFaceDirection
    }
    [SerializeField]
    protected Vector3 shootDirection = Vector3.zero;

    // Others
    public float shootTimer = 1f;
    public float shootInterval = 1f;

    public int bulletsPerShot = 1;
    public float bulletsSpread = 30f;
    public float bulletDamage = 10f;
    public float bulletSpeed = 10f;
    public Color bulletColor = Color.white;

    [Header("Tracking")]
    [Tooltip("Fires a missile that tracks the target instead of a bullet.")]
    public bool trackingBullets = false;
    [Tooltip("Sets the \"turning-rate\" of the missile. Lower value makes faster turns.")]
    public float trackInertia = 1f;


    private void Start()
    {
        isPlayer = GetComponent<Player>() != null;
        shootTimer = shootInterval;
    }

    private void Update()
    {
        // Player upgrades
        if (isPlayer)
        {
            float score = Game.Instance.score;
            if (score > 100)
            {
                bulletsPerShot = 1;
                bulletDamage = 12;
            }
            if (score > 200)
            {
                shootInterval = 0.18f;
            }
            if (score > 400)
            {
                bulletsPerShot = 2;
            }
            if (score > 600)
            {
                shootInterval = 0.15f;
                bulletSpeed = 30;
            }
            if (score > 800)
            {
                bulletsPerShot = 3;
                bulletsSpread = 45f;
            }
            if (score > 1000)
            {
                bulletDamage = 15;
            }
        }
        if (shootTimer > 0)
        {
            shootTimer -= Time.deltaTime;
        }
        if (shootTimer <= 0)
        {
            // Allows the player to fire a bullet
            if (isPlayer)
            {
                p_canShoot = true;
            }
            // NPC shoots a bullet to the player or a direction
            else
            {
                if (shootMode == ShootMode.AlwaysTargetPlayer)
                {
                    Shoot(Game.Instance.Player);
                }
                else if (shootMode == ShootMode.UseShootDirection)
                {
                    Shoot(transform.position + shootDirection);
                }
                else if (shootMode == ShootMode.UseFaceDirection)
                {
                    Quaternion rotation = transform.rotation;
                    Shoot(transform.position + rotation * Vector2.up);
                }
            }
        }
    }

    /// <summary>
    /// Shoots a bullet towards a target using its position
    /// </summary>
    /// <param name="target"> target to fire the bullet to </param>
    public void Shoot(LivingEntity target)
    {
        Shoot(target.transform.position, target);
    }

    /// <summary>
    /// Shoots a bullet towards a position
    /// </summary>
    /// <param name="targetPosition"> where to fire the bullet to </param>
    public void Shoot(Vector3 targetPosition, LivingEntity target = null)
    {
        // Resets the shoot Timer
        shootTimer = shootInterval;
        // Generates the bullet
        Vector3 position = transform.position;
        // Do NOT count z in direction!
        Vector2 direction = targetPosition - position;
        // Shoots one bullet
        if (bulletsPerShot == 1)
        {
            if (trackingBullets)
            {
                Missile.Instantiate(position, direction, this, trackInertia, target);
            }
            else
            {
                Bullet.Instantiate(position, direction, this);
            }
        }
        // Shoots multiple bullets
        else
        {
            float startAngle = bulletsSpread / 2;
            float stepAngle = -bulletsSpread / bulletsPerShot;
            direction = Quaternion.Euler(0, 0, startAngle) * direction;
            for (int i = 0; i < bulletsPerShot; i++)
            {
                if (trackingBullets)
                {
                    Missile.Instantiate(position, direction, this, trackInertia, target);
                }
                else
                {
                    Bullet.Instantiate(position, direction, this);
                }
                // Changes direction of next bullet
                direction = Quaternion.Euler(0, 0, stepAngle) * direction;
            }
        }
    }
}

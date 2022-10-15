using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// An entity that shoots bullets
/// </summary>
public class Shooter : MonoBehaviour
{
    public bool isPlayer = false;
    // Fields for Player
    [HideInInspector]
    public bool p_canShoot = false;
    [HideInInspector]
    public bool p_isShooting = false;

    public float shootTimer = 1f;
    public float shootInterval = 1f;

    public int bulletsPerShot = 1;
    public float bulletsSpread = 30f;
    public float bulletDamage = 10f;
    public float bulletSpeed = 10f;
    public Color bulletColor = Color.white;

    private void Start()
    {
        isPlayer = GetComponent<Player>() != null;
        shootTimer = shootInterval;
    }

    private void Update()
    {
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
            // NPC shoots a bullet to the player
            else
            {
                Shoot(Game.Instance.Player);
            }
        }
    }

    /// <summary>
    /// Shoots a bullet towards a target using its position
    /// </summary>
    /// <param name="target"> target to fire the bullet to </param>
    public void Shoot(LivingEntity target)
    {
        Shoot(target.transform.position);
    }

    /// <summary>
    /// Shoots a bullet towards a position
    /// </summary>
    /// <param name="targetPosition"> where to fire the bullet to </param>
    public void Shoot(Vector3 targetPosition)
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
            Bullet.Instantiate(position, direction, this);
        }
        // Shoots multiple bullets
        else
        {
            float startAngle = bulletsSpread / 2;
            float stepAngle = -bulletsSpread / bulletsPerShot;
            direction = Quaternion.Euler(0, 0, startAngle) * direction;
            for (int i = 0; i < bulletsPerShot; i++)
            {
                Bullet.Instantiate(position, direction, this);
                direction = Quaternion.Euler(0, 0, stepAngle) * direction;
            }
        }
    }
}

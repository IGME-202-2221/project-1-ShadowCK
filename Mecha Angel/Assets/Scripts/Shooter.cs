using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An entity that shoots bullets
/// </summary>
public class Shooter : MonoBehaviour
{
    public bool isShooting = false;
    public Color bulletColor = Color.white;

    public float shootTimer;
    public float shootInterval;

    void Start()
    {
        shootTimer = shootInterval;
    }

    void Update()
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0)
        {
            // Reset the timer and shoots bullets
            shootTimer = shootInterval;
            Shoot();
        }
    }

    /// <summary>
    /// Shoots bullet
    /// </summary>
    void Shoot()
    {
        // TODO
    }
}

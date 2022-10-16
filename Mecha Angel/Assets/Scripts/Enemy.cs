using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Enemy : LivingEntity
{
    [SerializeField]
    [Tooltip("If the enemy follows the player")]
    protected bool followsPlayer = false;

    private void Start()
    {
        // Initialization
        if (followsPlayer)
        {
            direction = (Vector2)(Game.Instance.Player.transform.position - transform.position);
            direction.Normalize();
            transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        }
        else
        {
            // Sets direction to the same as starting rotation
            direction = transform.rotation * Vector3.up;
        }

    }
    protected override void Update()
    {
        if (followsPlayer)
        {
            // Updates direction, following player
            direction = (Vector2)(Game.Instance.Player.transform.position - transform.position);
            direction.Normalize();
            // Keeps facing player
            transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        }

        // Updates position based on velocity and deltaTime
        base.Update();

        if (!destroyFlag && Game.IsOutOfBounds(this, Game.Instance.mainCamera))
        {
            destroyFlag = true;
            // Subtracts score by health left when it escapes
            Game.Instance.score -= Health;
            Destroy(gameObject, 0.1f);
        }
    }

    public override void Die()
    {
        // Enemies give score to the player on death
        Game.Instance.score += maxHealth;
        base.Die();
    }
}

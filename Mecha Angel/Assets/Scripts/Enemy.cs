using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Enemy : LivingEntity
{
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    protected override void Update()
    {
        // Updates direction, following player
        direction = (Game.Instance.Player.transform.position - position).normalized;

        // Updates position based on velocity and deltaTime
        base.Update();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : LivingEntity
{
    [Header("Meteor")]
    [Tooltip("How fast the Meteor randomly changes its direction")]
    public float disorientCoefficient = 0;
    public float disorientInterval = 0.1f;
    protected float disorientTimer;

    private void Start()
    {
        // Initial direction is towards the player
        Vector2 dir = (Game.Instance.Player.transform.position - position).normalized;
        // Adds a random rotation less than +-90 degrees so it's not moving away
        dir = Quaternion.Euler(0, 0, Random.Range(-90f, 90f)) * dir;
        direction = dir;
    }

    protected override void Update()
    {
        // Randomly adjusts the meteor's direcion
        float disorientationPower = disorientCoefficient * Time.deltaTime;
        Debug.Log(disorientationPower);
        direction = Quaternion.Euler(0, 0, Random.Range(-disorientationPower, disorientationPower)) * direction;

        base.Update();
    }
}

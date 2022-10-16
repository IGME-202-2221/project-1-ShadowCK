using System.Collections;
using UnityEngine;

public class Meteor : LivingEntity
{
    [Header("Meteor")]
    [Tooltip("Degrees every elapsedTime the meteor rotates")]
    public float disorientDegrees = 60f;
    public float disorientInterval = 1f;
    public float disorientDuration = 0.2f;
    private CountdownTimer disorientTimer;
    private Coroutine disorientRoutine;

    public float damage = 2f;
    private CountdownTimer damageTimer;

    private void Start()
    {
        // Initial direction is towards the player
        Vector2 dir = (Vector2)(Game.Instance.Player.transform.position - position);
        dir.Normalize();
        // Adds a random rotation less than +-90 degrees so it's not moving away
        dir = Quaternion.Euler(0, 0, Random.Range(-90f, 90f)) * dir;
        direction = dir;
        // Starts Timer
        disorientTimer = new CountdownTimer(System.TimeSpan.FromSeconds(disorientInterval));
        disorientTimer.OnEnd += Disorient;
    }

    protected override void Update()
    {
        disorientTimer.Update(true);

        base.Update();

        if (!destroyFlag && Game.IsOutOfBounds(this, Game.Instance.mainCamera))
        {
            destroyFlag = true;
            Destroy(gameObject, 0.1f);
        }
    }

    /// <summary>
    /// Every once in a while, disorient the meteor in a short duration
    /// </summary>
    private void Disorient()
    {
        // Randomly adjusts the meteor's direcion
        float disorientationPower = Random.Range(-disorientDegrees, disorientDegrees);
        float degreesPerSecond = disorientationPower / disorientDuration;
        disorientRoutine ??= StartCoroutine(Rotate(degreesPerSecond));
    }

    /// <summary>
    /// Rotates the meteor (changes its direction) linearly
    /// </summary>
    /// <param name="degreesPerSecond"></param>
    /// <returns></returns>
    private IEnumerator Rotate(float degreesPerSecond)
    {
        for (float elapsedTime = 0; elapsedTime < disorientDuration; elapsedTime += Time.deltaTime)
        {
            direction = Quaternion.Euler(0, 0, Time.deltaTime * degreesPerSecond) * direction;
            yield return null;
        }
    }

    public void Damage(LivingEntity other)
    {
        other.TakeDamage(damage);
    }
}

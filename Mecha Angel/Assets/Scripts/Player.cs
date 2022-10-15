using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : LivingEntity
{
    private Shooter shooter;

    protected float mana = 100f;
    protected float maxMana = 100f;

    public enum CombatMode
    {
        Air,
        Ground
    }

    public CombatMode mode = CombatMode.Ground;

    public float Mana
    {
        get => mana;
        set => mana = value;
    }

    public float MaxMana
    {
        get => maxMana;
        set => maxMana = value;
    }

    protected override void Awake()
    {
        base.Awake();
        shooter = GetComponent<Shooter>();
        cameraObject = GetComponent<PlayerInput>().camera;
    }

    protected override void Update()
    {
        // Updates position based on velocity and deltaTime
        base.Update();

        // Shoots bullet
        // fire to the mouse position
        if (shooter.p_isShooting && shooter.p_canShoot)
        {
            shooter.p_canShoot = false;
            shooter.Shoot(Game.GetMousePos());
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();

        // If no rotation is applied, don't rotate the vehicle
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.back, direction);
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        // Source: https://stackoverflow.com/questions/59837392/how-to-repeat-function-while-button-is-held-down-new-unity-input-system
        // Essentially, the function you assign to the button will be triggered twice per button press, once when it is pressed
        // (performed), and once when it is released (canceled). you can toggle a bool on and off stating whether or not the
        // button is pressed, then perform actions during Update dependent on the state of the bool. - Oliver Philbrick
        if (context.started)
        {
            shooter.p_isShooting = true;
        }
        else if (context.canceled)
        {
            shooter.p_isShooting = false;
        }
    }

    /// <summary>
    /// Wraps the player's position if he's out of bounds
    /// </summary>
    protected override void WrapPosition()
    {
        // Get the camera's height
        float totalCamHeight = cameraObject.orthographicSize * 2f;
        // Calculate the camera's width (must be calculated)
        float totalCamWidth = totalCamHeight * cameraObject.aspect;
        // Get all the bounds' coordinates.
        float left = cameraObject.transform.position.x - totalCamWidth / 2;
        float bottom = cameraObject.transform.position.y - totalCamHeight / 2;
        // Horizontal wrap
        position.x = left + Modulus(position.x - left, totalCamWidth);
        // Vertical wrap
        position.y = bottom + Modulus(position.y - bottom, totalCamHeight);
    }

    /// <summary>
    /// Mathematical modulo operation
    /// Warning: Does not work for two negative values
    /// </summary>
    /// <returns> remainder of dividend / divisor in math </returns>
    private static float Modulus(float dividend, float divisor)
    {
        float remainder = dividend % divisor;
        return remainder < 0 ? remainder + divisor : remainder;

    }
}
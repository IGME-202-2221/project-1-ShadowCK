using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the player's inputs and corresponding actions
/// </summary>
public class InputManager : MonoBehaviour
{
    public float baseSpeed = 10f;
    public float currentSpeed = 10f;
    public CombatMode mode = CombatMode.Ground;

    public enum CombatMode
    {
        Air,
        Ground
    }

    [SerializeField]
    private Shooter shooter;

    void Awake()
    {
        shooter = GetComponent<Shooter>();
    }

    void Update()
    {
        // Handles movement in different modes
        Vector2 dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetMouseButton(0))
        {
            shooter.isShooting = true;
            if (Input.GetMouseButtonDown(0))
            {

            }
        }
        else
        {
            shooter.isShooting = false;
        }
       
    }
}

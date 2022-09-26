using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Vehicle : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;

    private Vector3 vehiclePosition;
    private Vector3 direction;
    private Vector3 velocity;

    [SerializeField]
    private Camera cameraObject;

    void Start()
    {
        vehiclePosition = transform.position;
        direction = Vector3.zero;
        cameraObject = GetComponent<PlayerInput>().camera;
    }

    void Update()
    {
        // velocity is direction * speed * deltaTime
        velocity = direction * speed * Time.deltaTime;

        // Add velocity to position
        vehiclePosition += velocity;

        // Wrap the vehicle if it's out of camera
        WrapPosition();

        // "Draw" this vehicle at that position
        transform.position = vehiclePosition;
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

    private void WrapPosition()
    {
        // Get the camera's height
        float totalCamHeight = cameraObject.orthographicSize * 2f;
        // Calculate the camera's width (must be calculated)
        float totalCamWidth = totalCamHeight * cameraObject.aspect;
        // Get all the bounds' coordinates.
        float left = cameraObject.transform.position.x - totalCamWidth / 2;
        float bottom = cameraObject.transform.position.y - totalCamHeight / 2;
        // Horizontal wrap
        vehiclePosition.x = left + Modulus(vehiclePosition.x - left, totalCamWidth);
        // Vertical wrap
        vehiclePosition.y = bottom + Modulus(vehiclePosition.y - bottom, totalCamHeight);
    }

    private float Modulus(float dividend, float divisor)
    {
        float remainder = dividend % divisor;
        return remainder < 0 ? remainder + divisor : remainder;
    }
}
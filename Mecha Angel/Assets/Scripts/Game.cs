using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Stores accesible game assets
/// </summary>
public class Game : MonoBehaviour
{
    public GameObject bulletPrefab;

    public static Game Instance
    {
        get; private set;
    }

    public Player Player
    {
        get; private set;
    }

    public class CameraSettings
    {
        public Camera instance;
        public float left, right, top, bottom;

        public CameraSettings()
        {
            Update();
        }

        public void Update()
        {
            if (instance == null || instance != Camera.main)
            {
                instance = Camera.main;
                float totalCamHeight = instance.orthographicSize * 2f;
                // Calculate the camera's width (must be calculated)
                float totalCamWidth = totalCamHeight * instance.aspect;
                // Get all the bounds' coordinates.
                Vector3 cameraPos = instance.transform.position;
                left = cameraPos.x - totalCamWidth / 2;
                right = cameraPos.x + totalCamWidth / 2;
                bottom = cameraPos.y - totalCamHeight / 2;
                top = cameraPos.y + totalCamHeight / 2;
            }
        }
    }

    public CameraSettings mainCamera;

    private void Awake()
    {
        // Game is singleton
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            // Initializes basic game data
            Player = GetComponent<Player>("Player");
            mainCamera = new CameraSettings();
        }
    }

    void Update()
    {
        // Updates camera
        mainCamera.Update();
    }

    /// <summary>
    /// Returns the specified component of a GameObject of given name
    /// </summary>
    /// <typeparam name="T"> Unity Component </typeparam>
    /// <param name="name"> name of GameObject </param>
    /// <returns> the component or null if not found </returns>
    public static T GetComponent<T>(string name) where T : Component
    {
        GameObject obj = GameObject.Find(name);
        if (obj != null)
        {
            T component = obj.GetComponent<T>();
            // Returns component found or null if not found
            return component != null ? component : null;
        }
        // Object not found, returns null
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Gets the world position of the mouse
    /// </summary>
    /// <returns> Mouse position in the game world </returns>
    public static Vector3 GetMousePos()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        // Converts mouse position to world Position (both 2D and 3D)
        mousePos.z = Camera.main.nearClipPlane;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        return worldPos;
    }

    public static bool IsOutOfBounds(Component obj, CameraSettings camera)
    {
        Vector3 objPos = obj.transform.position;
        return objPos.x < camera.left || objPos.x > camera.right || objPos.y < camera.bottom || objPos.y > camera.top;
    }
}

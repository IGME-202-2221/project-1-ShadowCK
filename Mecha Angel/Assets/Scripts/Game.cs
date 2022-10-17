using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

/// <summary>
/// Stores accesible game assets
/// </summary>
public class Game : MonoBehaviour
{
    private float score = 0;

    // Prefabs and presets
    public GameObject bulletPrefab;
    public GameObject missilePrefab;

    public TextMesh info;

    public static Game instance;

    public float Score
    {
        get => score;
    }

    public Player Player
    {
        get; private set;
    }

    public class CameraSettings
    {
        public Camera instance;
        public float width, height, left, right, top, bottom;

        public Vector3 Center => instance.transform.position;

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
                width = totalCamWidth;
                height = totalCamHeight;
                // Get all the bounds' coordinates.
                Vector3 cameraPos = instance.transform.position;
                left = cameraPos.x - totalCamWidth / 2;
                right = cameraPos.x + totalCamWidth / 2;
                bottom = cameraPos.y - totalCamHeight / 2;
                top = cameraPos.y + totalCamHeight / 2;
            }
        }

        /// <summary>
        /// Returns a Vector2 for the orthographic box of the camera
        /// </summary>
        /// <param name="camera"></param>
        /// <returns></returns>
        public static Vector2 OrthographicBox(Camera camera)
        {
            float totalCamHeight = camera.orthographicSize * 2f;
            float totalCamWidth = totalCamHeight * camera.aspect;
            return new Vector2(totalCamWidth, totalCamHeight);
        }

        public static Bounds OrthographicBounds(Camera camera)
        {
            return new Bounds(camera.transform.position, OrthographicBox(camera));
        }
    }

    public CameraSettings mainCamera;

    private void Awake()
    {
        // Game is singleton
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            // Initializes basic game data
            Application.targetFrameRate = 144;
            Player = GetComponent<Player>("Player");
            mainCamera = new CameraSettings();
        }
    }

    private void Update()
    {
        // Updates camera
        mainCamera.Update();
    }

    public void AddScore(float points)
    {
        score += points;
        if (score < 0) score = 0;
    }

    public void GameOver()
    {
        Time.timeScale = 0.2f;
        info.text = "Game Over!";
        HUDManager.instance.replayCanvas.SetActive(value: true);
    }

    public void Restart(float newTimeScale = 1f)
    {
        Time.timeScale = newTimeScale;
        HUDManager.instance.replayCanvas.SetActive(value: false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

    public static bool IsValid(MonoBehaviour component)
    {
        return component != null
            && !component.gameObject.IsDestroyed()
            && !component.IsDestroyed();
    }
}

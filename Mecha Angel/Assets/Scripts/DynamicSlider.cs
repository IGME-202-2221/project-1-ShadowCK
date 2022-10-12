using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicSlider : MonoBehaviour
{
    [SerializeField]
    private float current; // Can be health or some other value
    [SerializeField]
    private float maximum; // Can be max health or some other value

    private float lerpPoint; // Where to lerp to; Where the real current value is
    private enum LerpMode
    {
        NoLerp,
        Decreasing,
        Increasing
    }

    [SerializeField]
    private LerpMode lerpMode = LerpMode.NoLerp;

    [Tooltip("How fast the delay bar catches up to the immediate one. Lerp percent = deltaTime * lerpSpeed")]
    public float lerpSpeed = 1f;

    public Slider frontBar; // Current when health is decreasing; Delay when health is increasing
    public Slider backBar;  // Delay when health is decreasing; Current when health is increasing
    private Image frontBarFillImage; // Slider->FillArea->Fill.Image
    [SerializeField]
    private Image backBarFillImage;
    private Image backBarBackground;

    public Color increaseColor = Color.green;
    public Color decreaseColor = Color.red;
    [Header("Optional")]
    [Tooltip("If turned on, you may not remove, add or reorder the sliders' children. The script will do some justifications and overwrite following colors in your sliders on Awake. Turn this off if you want more customization like a prefab.")]
    public bool preprocess = true;
    public Color frontBarColor = Color.white;
    public Color backgroundColor = Color.gray;

    private void Awake()
    {
        HUDManager manager = HUDManager.instance;
        if (manager != null)
        {
            HUDManager.instance.RegisterSlider(this);
        }
        if (preprocess)
        {
            frontBar.interactable = false;
            backBar.interactable = false;
            try
            {
                // Stores images
                frontBarFillImage = frontBar.transform.GetChild(1).GetChild(0).GetComponent<Image>();
                backBarFillImage = backBar.transform.GetChild(1).GetChild(0).GetComponent<Image>();
                backBarBackground = backBar.transform.GetChild(0).GetComponent<Image>();
                // Overwrites front slider's color and back slider's background
                frontBarFillImage.color = frontBarColor;
                backBarBackground.color = backgroundColor;
                // Resets left/right/top/bottom of Fill Area's and Fill's rect transforms
                frontBar.fillRect.ResetOffsets();
                backBar.fillRect.ResetOffsets();
                frontBar.fillRect.parent.GetComponent<RectTransform>().ResetOffsets();
                backBar.fillRect.parent.GetComponent<RectTransform>().ResetOffsets();
                // Removes frontBar Background - Destroy is not instant, will execute after these codes
                Destroy(frontBar.transform.GetChild(0).gameObject);
                // Removes Handle Slide Area
                Destroy(frontBar.transform.GetChild(2).gameObject);
                Destroy(backBar.transform.GetChild(2).gameObject);
            }
            catch (Exception ex) { Debug.Log($"{ex.Message}\n{ex.StackTrace}"); }
        }
        // If preprocess is off and no image has been manually assigned (typically for a prefab,) tries to find the Fill Image.
        else if (backBarFillImage == null)
        {
            try
            {
                backBarFillImage = backBar.transform.Find("Fill Area").Find("Fill").GetComponent<Image>();
            }
            catch (Exception ex) { Debug.Log($"{ex.Message}\n{ex.StackTrace}"); }
        }
    }

    /// <summary>
    /// Call this in some UI manager script. Make sure the UI manager has higher Script Execution Order.
    /// </summary>
    /// <param name="current"> current value </param>
    /// <param name="maximum"> maximum value </param>
    public void UpdateValue(float current, float maximum)
    {
        this.current = current;
        this.maximum = maximum;
    }

    private void Update()
    {
        if (maximum <= 0)
        {
            Debug.Log($"Unset maximum value of Dynamic Slider for UI element: {gameObject.name}");
            return;
        }

        current = Mathf.Clamp(current, 0, maximum);

        float frontFill = frontBar.value;
        float backFill = backBar.value;
        float ratio = current / maximum; // LerpPoint from last update is "previous ratio"

        if (ratio > lerpPoint && ratio > backFill)
        {
            lerpMode = LerpMode.Increasing;
        }
        else if (ratio < lerpPoint && ratio < frontFill)
        {
            lerpMode = LerpMode.Decreasing;
        }

        // Updates lerp point to match current health
        lerpPoint = ratio;

        // Health has decreased
        if (lerpMode == LerpMode.Decreasing)
        {
            frontBar.value = ratio;

            backBarFillImage.color = decreaseColor;
            backBar.value = Mathf.Lerp(backFill, ratio, Time.deltaTime * lerpSpeed);
            // Stops lerping when very close to the lerpPoint
            if (Mathf.Abs(backBar.value - ratio) < 0.001f)
            {
                lerpMode = LerpMode.NoLerp;
                backBar.value = ratio;
            }
        }
        // Health has increased/restored
        else if (lerpMode == LerpMode.Increasing)
        {
            backBar.value = ratio;

            backBarFillImage.color = increaseColor;
            frontBar.value = Mathf.Lerp(frontFill, ratio, Time.deltaTime * lerpSpeed);
            // Stops lerping when very close to the lerpPoint
            if (Mathf.Abs(frontBar.value - ratio) < 0.001f)
            {
                lerpMode = LerpMode.NoLerp;
                frontBar.value = ratio;
            }
        }
    }
}

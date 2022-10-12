using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Reference: https://www.youtube.com/watch?v=CFASjEuhyf4&ab_channel=NattyCreations
/// </summary>
public class DynamicBar : MonoBehaviour
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

    public Image frontBar; // Current when health is decreasing; Delay when health is increasing
    public Image backBar;  // Delay when health is decreasing; Current when health is increasing

    public Color increaseColor = Color.green;
    public Color decreaseColor = Color.red;

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
            Debug.Log($"Unset maximum value of Dynamic Bar for UI element: {gameObject.name}");
            return;
        }

        current = Mathf.Clamp(current, 0, maximum);

        float frontFill = frontBar.fillAmount;
        float backFill = backBar.fillAmount;
        float ratio = current / maximum; // LerpPoint from last update is "previous ratio"

        // Stores the status of the presentation of health change: no change, increase or decrease
        // The ratio > backFill and ratio < frontFill conditionals prevent cases where lerpMode
        // switches directly to the other when it shouldn't.
        // P - current health ¡ö - health ¡õ - empty ¡ø - increase ¡ô - decrease 
        // Correct: ¡ö¡ö¡ö¡ö¡ö¡öP¡ô¡ô¡ô¡ô¡ô¡ô¡õ¡õ -> ¡ö¡ö¡ö¡ö¡ö¡ö¡ö¡öP¡ô¡ô¡ô¡ô¡õ¡õ The visual is still "decreasing", just the amount has reduced!
        // Wrong:   ¡ö¡ö¡ö¡ö¡ö¡öP¡ô¡ô¡ô¡ô¡ô¡ô¡õ¡õ -> ¡ö¡ö¡ö¡ö¡ö¡ö¡ø¡øP¡õ¡õ¡õ¡õ¡õ¡õ Abandons the decay instantly to display increase
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
            frontBar.fillAmount = ratio;

            backBar.color = decreaseColor;
            backBar.fillAmount = Mathf.Lerp(backFill, ratio, Time.deltaTime * lerpSpeed);
            // Stops lerping when very close to the lerpPoint
            if (Mathf.Abs(backBar.fillAmount - ratio) < 0.001f)
            {
                lerpMode = LerpMode.NoLerp;
                backBar.fillAmount = ratio;
            }
        }
        // Health has increased/restored
        else if (lerpMode == LerpMode.Increasing)
        {
            backBar.fillAmount = ratio;

            backBar.color = increaseColor;
            frontBar.fillAmount = Mathf.Lerp(frontFill, ratio, Time.deltaTime * lerpSpeed);
            // Stops lerping when very close to the lerpPoint
            if (Mathf.Abs(frontBar.fillAmount - ratio) < 0.001f)
            {
                lerpMode = LerpMode.NoLerp;
                frontBar.fillAmount = ratio;
            }
        }
    }
}

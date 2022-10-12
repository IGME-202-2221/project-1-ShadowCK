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
    private float maximum = 100f; // Can be max health or some other value

    private float lerpPoint; // Where to lerp to; Where the real current value is 

    [Tooltip("How fast the delay bar catches up to the immediate one. Lerp percent = deltaTime * lerpSpeed")]
    public float lerpSpeed = 1f;
    public Image frontBar; // Current when health is decreasing; Delay when health is increasing
    public Image backBar;  // Delay when health is decreasing; Current when health is increasing
    public Color increaseColor = Color.green;
    public Color decreaseColor = Color.red;

    private void Start()
    {
        current = maximum;
    }

    private void Update()
    {
        current = Mathf.Clamp(current, 0, maximum);
        UpdateUI();
    }

    public void UpdateUI()
    {
        float frontFill = frontBar.fillAmount;
        float backFill = backBar.fillAmount;
        float ratio = current / maximum;

        // Resets lerp point when health has changed
        if (lerpPoint != ratio)
        {
            lerpPoint = ratio;
        }

        // Health has decreased
        if (backFill > ratio)
        {
            frontBar.fillAmount = ratio;

            backBar.color = decreaseColor;
            backBar.fillAmount = Mathf.Lerp(backFill, ratio, Time.deltaTime * lerpSpeed);
        }
        // Health has increased/restored
        else if (frontFill < ratio)
        {
            backBar.fillAmount = ratio;

            backBar.color = increaseColor;
            frontBar.fillAmount = Mathf.Lerp(frontFill, ratio, Time.deltaTime * lerpSpeed);
        }
    }
}

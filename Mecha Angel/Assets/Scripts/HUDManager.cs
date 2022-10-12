using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField]
    Text scoreLabel;

    [SerializeField]
    Slider healthBar;

    // Update is called once per frame
    void Update()
    {
        scoreLabel.text = "Score: " + Game.Instance.score;
        healthBar.value = Game.Instance.Player.HealthPercent;
    }
}

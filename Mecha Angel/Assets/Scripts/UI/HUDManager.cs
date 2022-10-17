using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField]
    private Text scoreLabel;
    [SerializeField]
    private Text waveCountdownLabel;

    public static HUDManager instance;

    public List<DynamicSlider> sliders;

    public GameObject HUDCanvas;
    public GameObject replayCanvas;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        foreach (var obj in FindObjectsOfType<GameObject>())
        {
            DynamicSlider slider = obj.GetComponent<DynamicSlider>();
            if (slider != null)
            {
                sliders.Add(slider);
            }
        }
    }

    private void Update()
    {
        scoreLabel.text = "Score: " + Game.instance.Score.ToString("0.##");
        waveCountdownLabel.text = "Next Wave: " + EnemySpawner.instance.NextWaveCountdown().ToString("0");

        Player player = Game.instance.Player;
        foreach (DynamicSlider slider in sliders)
        {
            switch (slider.name)
            {
                case "HealthBar":
                    slider.UpdateValue(player.Health, player.MaxHealth);
                    break;
                // TODO
                case "ManaBar":
                    slider.UpdateValue(player.Mana, player.MaxMana);
                    break;
            }
        }
    }

    public bool RegisterSlider(DynamicSlider slider)
    {
        if (!sliders.Contains(slider))
        {
            sliders.Add(slider);
            return true;
        }
        return false;
    }
}

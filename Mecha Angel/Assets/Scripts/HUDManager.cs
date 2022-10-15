using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField]
    private Text scoreLabel;

    public static HUDManager instance;

    public List<DynamicSlider> sliders;

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
        scoreLabel.text = "Score: " + Game.Instance.score;

        Player player = Game.Instance.Player;
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

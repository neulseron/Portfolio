using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInGameUI : MonoBehaviour
{
    public StatsObject playerStats;

    public Text levelTxt;
    public Slider healthSlider;
    public Slider manaSlider;


    void Start()
    {
        levelTxt.text = playerStats.level.ToString();

        healthSlider.value = playerStats.HealthPercentage;
        manaSlider.value = playerStats.ManaPercentage;
    }

    void OnEnable() {
        playerStats.OnChangedStats += OnChangedStats;
    }

    void OnDisable() {
        playerStats.OnChangedStats -= OnChangedStats;
    }

    void OnChangedStats(StatsObject statsObject)
    {
        healthSlider.value = statsObject.HealthPercentage;
        manaSlider.value = statsObject.ManaPercentage;
    }
}

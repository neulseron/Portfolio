using UnityEngine;
using UnityEngine.UI;

public class PlayerInGameUI : MonoBehaviour
{
#region Variables
    public StatsObject playerStats;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite dmgHeart;
#endregion Variables


#region Unity Methods
    void Start()
    {
        FillHeart(playerStats.Health);
    }

    void OnEnable() {
        playerStats.OnChangedStats += OnChangedStats;
    }

    void OnDisable() {
        playerStats.OnChangedStats -= OnChangedStats;
    }
#endregion Unity Methods


#region Methods
    void OnChangedStats(StatsObject statsObject)
    {
        FillHeart(statsObject.Health);
    }

    public void FillHeart(int currHP)
    {   
        int currMaxHeart = playerStats.GetModifiedValue(AttributeType.MaxHealth) / 2;
        int halfHP = currHP % 2;
        currHP = currHP / 2;

        for (int i = 0; i < hearts.Length; i++) {
            if (i < currMaxHeart) {
                if (i < currHP) {
                    hearts[i].sprite = fullHeart;
                    hearts[i].color = new Color(1, 1, 1, 1);
                } else if (halfHP == 1 && i == currHP) {
                    hearts[i].sprite = halfHeart;
                    hearts[i].color = new Color(1, 1, 1, 1);
                } else {
                    hearts[i].sprite = dmgHeart;
                    hearts[i].color = new Color(1, 1, 1, 1);
                }
            } else {    
                hearts[i].color = new Color(1, 1, 1, 0);
            }
        }
    }
#endregion Methods
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCBattleUI : MonoBehaviour
{
    Slider hpSlider;

    [SerializeField]
    GameObject dmgTxtPrefab;

    public float MinValue
    {
        get => hpSlider.minValue;
        set {
            hpSlider.minValue = value;
        }
    }

    public float MaxValue
    {
        get => hpSlider.maxValue;
        set {
            hpSlider.maxValue = value;
        }
    }

    public float Value
    {
        get => hpSlider.value;
        set {
            hpSlider.value = value;
        }
    }

    void Awake() {
        hpSlider = gameObject.GetComponentInChildren<Slider>();
    }

    void OnEnable() {
        GetComponent<Canvas>().enabled = true;
    }

    void OnDisable() {
        GetComponent<Canvas>().enabled = false;
    }

    public void CreateDmgTxt(int damage)
    {
        if (dmgTxtPrefab != null) {
            GameObject dmgTxtGO = Instantiate(dmgTxtPrefab, transform);
            
            DamageTxt dmgTxt = dmgTxtGO.GetComponent<DamageTxt>();
            if (dmgTxt == null) {
                Destroy(dmgTxtGO);
            } else {
                dmgTxt.Damage = damage;
            }
        }
    }
}

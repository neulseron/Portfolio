using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageTxt : MonoBehaviour
{
    public float delayTimeToDestroy = 0.5f;
    public int Damage
    {
        get {
            return int.Parse(dmgTxt.text);
        }
        set {
            dmgTxt.text = value.ToString();
        }
    }
    TextMeshProUGUI dmgTxt;

    void Awake() {
        dmgTxt = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        Destroy(gameObject, delayTimeToDestroy);
    }
}

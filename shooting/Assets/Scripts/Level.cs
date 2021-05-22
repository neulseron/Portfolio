using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    public int myLevel;
    public Image[] stars;

    GameObject levelManager;
    LevelManager lv;

    private void Start() {
        levelManager = GameObject.Find("LevelManager");
        lv = levelManager.GetComponent<LevelManager>();

        for (int i = 0; i < lv.starCntArr[myLevel]; i++) {
            if (lv.allClearArr[myLevel] == true)
                stars[i].sprite = lv.clearStar;
            else
                stars[i].sprite = lv.starOn;
        }
    }
}

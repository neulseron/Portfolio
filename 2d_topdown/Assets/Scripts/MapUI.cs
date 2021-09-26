using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUI : MonoBehaviour
{
    public GameObject[] maps;
    int showIndex;

    void OnEnable() {
        maps[showIndex].SetActive(false);
        
        switch (GameManager.Instance.currMapIndex) {
            case 13:
            case 15:
                showIndex = 0;
                break;
            case 0:
            case 1:
            case 11:
            case 12:
                showIndex = 1;
                break;
            case 2:
            case 3:
            case 14:
                showIndex = 2;
                break;
            case 4:
                showIndex = 3;
                break;
            case 5:
            case 6:
            case 7:
            case 8:
            case 9:
                showIndex = 4;
                break;
        }

        
        maps[showIndex].SetActive(true);
    }

    public void Btn1F()
    {
        maps[showIndex].SetActive(false);
        maps[0].SetActive(true);
        showIndex = 0;
    }

    public void Btn2F()
    {
        maps[showIndex].SetActive(false);
        maps[1].SetActive(true);
        showIndex = 1;
    }

    public void Btn3F()
    {
        maps[showIndex].SetActive(false);
        maps[2].SetActive(true);
        showIndex = 2;
    }
    
    public void BtnDistrict()
    {
        maps[showIndex].SetActive(false);
        maps[3].SetActive(true);
        showIndex = 3;
    }
}

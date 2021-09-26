using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public GameObject[] miniMaps;

    int currMinimap;


    void Update()
    {
        if (currMinimap != GameManager.Instance.currShowMinimapIndex) {
            miniMaps[currMinimap].SetActive(false);

            currMinimap = GameManager.Instance.currShowMinimapIndex;
            miniMaps[currMinimap].SetActive(true);
        }
    }
}

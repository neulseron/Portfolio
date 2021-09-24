using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject[] miniMaps;

    int currMinimap;


    void Update()
    {
        if (currMinimap != gameManager.currShowMinimapIndex) {
            miniMaps[currMinimap].SetActive(false);

            currMinimap = gameManager.currShowMinimapIndex;
            miniMaps[currMinimap].SetActive(true);
        }
    }
}

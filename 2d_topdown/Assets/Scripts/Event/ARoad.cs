using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARoad : MonoBehaviour
{
    public GameManager gameManager;
    public SceneManager sceneManager;
    public SwitchManager switchManager;
    GameObject player;
    PlayerAction playerLogic;
    //--------------------------------------
    public GameObject screenUI;
    //======================================

    void Start()
    {
        player = gameManager.player;
        playerLogic = player.GetComponent<PlayerAction>();
    }

    void Update()
    {
        if (switchManager.switchdata["A_Screen"].on) {
            switchManager.switchdata["A_Screen"].on = false;
            Screen();
        }
    }

    void Screen()
    {
        switchManager.ing = true;
        //=====================================================
        screenUI.SetActive(true);
        //=====================================================
        switchManager.ing = false;
    }

    public void BtnExit()
    {
        screenUI.SetActive(false);
    }
}

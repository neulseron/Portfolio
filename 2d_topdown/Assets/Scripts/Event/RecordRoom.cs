using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordRoom : MonoBehaviour
{
    public GameManager gameManager;
    public SceneManager sceneManager;
    public SwitchManager switchManager;
    GameObject player;
    PlayerAction playerLogic;
    //--------------------------------------
    public GameObject listUI;
    //--------------------------------------
    public Transform[] spawnPoints;
    //======================================
    void Start() {
        player = gameManager.player;
        playerLogic = player.GetComponent<PlayerAction>();
    }

    void Update() {
        if (switchManager.switchdata["FR_BookShelf"].on || switchManager.switchdata["FR_BookShelf2"].on) {
            switchManager.switchdata["FR_BookShelf"].on = false;
            switchManager.switchdata["FR_BookShelf2"].on = false;
            StartCoroutine(BookShelf());
        }

        if (switchManager.switchdata["FR_BookShelf3"].on) {
            switchManager.switchdata["FR_BookShelf3"].on = false;
            listOn();
        }

        if (switchManager.switchdata["Record_Page1"].on) {
            switchManager.switchdata["Record_Page1"].on = false;
            StartCoroutine(Page1());
        }

        if (switchManager.switchdata["Record_Page2"].on) {
            switchManager.switchdata["Record_Page2"].on = false;
            StartCoroutine(Page2());
        }
    }

    IEnumerator BookShelf()
    {
        switchManager.ing = true;
        //=====================================================
        sceneManager.PlayTalk(30, 100);
        yield return new WaitUntil(() => gameManager.isTalking == false);
        //=====================================================
        switchManager.ing = false;
    }

    void listOn()
    {
        gameManager.dontMove = true;
        //=====================================================
        listUI.SetActive(true);
    }

    IEnumerator Page1() {
        //=====================================================
        switchManager.ing = true;
        yield return new WaitForSeconds(0.5f);
        //-----------------------------------------------------
        sceneManager.PlayTalk(0, 800);
        yield return new WaitUntil(() => gameManager.isTalking == false);
        //=====================================================
        switchManager.ing = false;
        switchManager.switchdata["Record_Page1"].off = true;
    }

    IEnumerator Page2() {
        switchManager.switchdata["Record_Page2"].off = true;
        //=====================================================
        switchManager.ing = true;
        yield return new WaitForSeconds(0.5f);
        //-----------------------------------------------------
        sceneManager.PlayTalk(10, 800);
        yield return new WaitUntil(() => gameManager.isTalking == false);
        //=====================================================
        switchManager.ing = false;
    }

}

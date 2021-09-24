using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReferenceRoom : MonoBehaviour
{
    public GameManager gameManager;
    public SceneManager sceneManager;
    public SwitchManager switchManager;
    GameObject player;
    PlayerAction playerLogic;
    //--------------------------------------
    public GameObject screenUI;
    public GameObject rightPerson;
    //--------------------------------------
    public Transform[] spawnPoints;
    //======================================
    void Start() {
        player = gameManager.player;
        playerLogic = player.GetComponent<PlayerAction>();
    }

    void Update() {
        if (switchManager.switchdata["RR_Bookshelf"].on || switchManager.switchdata["RR_Bookshelf2"].on) {
            switchManager.switchdata["RR_Bookshelf"].on = false;
            switchManager.switchdata["RR_Bookshelf2"].on = false;
            StartCoroutine(BookShelf());
        }

        if (switchManager.switchdata["RR_rightScreen"].on) {
            switchManager.switchdata["RR_rightScreen"].on = false;
            screenOn();
        }

        if (!switchManager.switchdata["A1_End"].off && switchManager.switchdata["RR_Bookshelf3"].on) {
            switchManager.switchdata["RR_Bookshelf3"].on = false;
            StartCoroutine(BookShelf3());
        } else if (switchManager.switchdata["A1_End"].off && switchManager.switchdata["RR_Bookshelf3"].on) {
            switchManager.switchdata["RR_Bookshelf3"].on = false;
            StartCoroutine(BookShelf3_2());
        }

        if (switchManager.switchdata["RR_leftUsing"].on) {
            switchManager.switchdata["RR_leftUsing"].on = false;
            StartCoroutine(LeftCantGo());
        }

        if (switchManager.switchdata["A1_End"].off) {
            rightPerson.SetActive(false);
        }

        if (!switchManager.switchdata["A1_End"].off && switchManager.switchdata["RR_rightUsing"].on) {
            switchManager.switchdata["RR_rightUsing"].on = false;
            StartCoroutine(RightCantGo());
        }
    }

    IEnumerator BookShelf()
    {
        switchManager.ing = true;
        //=====================================================
        gameManager.isSelecting = true;
        sceneManager.SetSelectTxt();
        sceneManager.SetSelectBtnTxt("수색대 기초 서적", "정신질환의 진단 및 통계", "구역 매뉴얼", "");
        yield return new WaitUntil(() => gameManager.selectIndex != -1);

        switch (gameManager.selectIndex) {
            case 1:
                gameManager.selectBox.SetActive(false);
                sceneManager.PlayObj(0, 600);
                yield return new WaitUntil(() => gameManager.isInterAction == false);
                break;
            case 2:
                gameManager.selectBox.SetActive(false);
                sceneManager.PlayObj(10, 600);
                yield return new WaitUntil(() => gameManager.isInterAction == false);
                break;
            case 3:
                gameManager.selectBox.SetActive(false);
                sceneManager.PlayObj(20, 600);
                yield return new WaitUntil(() => gameManager.isInterAction == false);
                break;
        }

        gameManager.selectIndex = -1;
        gameManager.isSelecting = false;
        //=====================================================
        switchManager.ing = false;
    }

    IEnumerator BookShelf3()
    {
        switchManager.ing = true;
        //=====================================================
        sceneManager.PlayTalk(40, 100);
        yield return new WaitUntil(() => gameManager.isTalking == false);
        //=====================================================
        switchManager.ing = false;
    }

    IEnumerator BookShelf3_2()
    {
        switchManager.ing = true;
        //=====================================================
        sceneManager.PlayTalk(60, 100);
        yield return new WaitUntil(() => gameManager.isTalking == false);
        //=====================================================
        switchManager.ing = false;
    }

    IEnumerator LeftCantGo()
    {
        switchManager.ing = true;
        //=====================================================
        sceneManager.PlayTalk(50, 100);
        yield return new WaitUntil(() => gameManager.isTalking == false);
        //-----------------------------------------------------
        playerLogic.SetWaypoint(2, player.transform.position, player.transform.position + new Vector3(1.5f, 0, 0));
        playerLogic.isOn = true;
        yield return new WaitUntil(() => playerLogic.isOff == true);
        playerLogic.isOff = false;
        //=====================================================
        switchManager.ing = false;
    }

    IEnumerator RightCantGo()
    {
        switchManager.ing = true;
        //=====================================================
        sceneManager.PlayTalk(50, 100);
        yield return new WaitUntil(() => gameManager.isTalking == false);
        //-----------------------------------------------------
        playerLogic.SetWaypoint(2, player.transform.position, player.transform.position + new Vector3(-1.5f, 0, 0));
        playerLogic.isOn = true;
        yield return new WaitUntil(() => playerLogic.isOff == true);
        playerLogic.isOff = false;
        //=====================================================
        switchManager.ing = false;
    }

    void screenOn()
    {
        switchManager.ing = true;
        //=====================================================
        screenUI.SetActive(true);
    }
}

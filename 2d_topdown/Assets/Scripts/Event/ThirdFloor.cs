using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdFloor : MonoBehaviour
{
    public GameManager gameManager;
    public SceneManager sceneManager;
    public SwitchManager switchManager;
    GameObject player;
    PlayerAction playerLogic;
    //--------------------------------------
    public GameObject tutoMark;
    public GameObject SearchingSW;
    public GameObject SearchingJU;
    //--------------------------------------
    public Transform[] spawnPoints;
    //======================================
    void Start() {
        player = gameManager.player;
        playerLogic = player.GetComponent<PlayerAction>();
    }


    void Update() {
        if (switchManager.switchdata["Tutorial"].off && switchManager.switchdata["Tutorial_SearchCom"].on && !switchManager.switchdata["Tutorial_OffMarkSearchCom"].off) {
            tutoMark.SetActive(true);
        }
        if (switchManager.switchdata["Tutorial_OffMarkSearchCom"].on) {
            tutoMark.SetActive(false);
        }
        //--------------------------------------
        if (!switchManager.switchdata["SecondF_movingSM"].off && switchManager.switchdata["JR_callDr"].off) {
            SearchingSW.SetActive(true);
        } else {
            SearchingSW.SetActive(false);
            SearchingJU.SetActive(false);
        }

        if (switchManager.switchdata["ThirdF_CabinetHyeonSeok"].on) {
            switchManager.switchdata["ThirdF_CabinetHyeonSeok"].on = false;
            StartCoroutine(CabinetHS());
        }
        if (switchManager.switchdata["ThirdF_CabinetJiun"].on) {
            switchManager.switchdata["ThirdF_CabinetJiun"].on = false;
            StartCoroutine(CabinetJU());
        }
        if (switchManager.switchdata["ThirdF_CabinetJiEun"].on) {
            switchManager.switchdata["ThirdF_CabinetJiEun"].on = false;
            StartCoroutine(CabinetJE());
        }
        if (switchManager.switchdata["ThirdF_CabinetJaeHa"].on) {
            switchManager.switchdata["ThirdF_CabinetJaeHa"].on = false;
            StartCoroutine(CabinetJH());
        }
        if (switchManager.switchdata["ThirdF_CabinetSuHyeon"].on) {
            switchManager.switchdata["ThirdF_CabinetSuHyeon"].on = false;
            StartCoroutine(CabinetSH());
        }
        if (switchManager.switchdata["ThirdF_CabinetArum"].on) {
            switchManager.switchdata["ThirdF_CabinetArum"].on = false;
            StartCoroutine(CabinetAR());
        }
        if (switchManager.switchdata["ThirdF_CabinetYeonu"].on) {
            switchManager.switchdata["ThirdF_CabinetYeonu"].on = false;
            StartCoroutine(CabinetYU());
        }
        if (switchManager.switchdata["ThirdF_CabinetJihyeong"].on) {
            switchManager.switchdata["ThirdF_CabinetJihyeong"].on = false;
            StartCoroutine(CabinetJiHyeong());
        }


        if (switchManager.switchdata["ThirdF_SearchingJiun"].on) {
            switchManager.switchdata["ThirdF_SearchingJiun"].on = false;
            StartCoroutine(SearchingJiUn());
        }
        if (switchManager.switchdata["ThirdF_SearchingSunwoo"].on) {
            switchManager.switchdata["ThirdF_SearchingSunwoo"].on = false;
            StartCoroutine(SearchingSunWoo());
        }
        //--------------------------------------
        if (switchManager.switchdata["ThirdF_DirectRoomDoor"].on) {
            switchManager.switchdata["ThirdF_DirectRoomDoor"].on = false;
            if (switchManager.switchdata["SecondF_appearHS"].off && !switchManager.switchdata["DR_callHS"].off) {
                gameManager.OffPlayer(gameManager.player);
                gameManager.ChangeMap(3, "P_HyeonSeok", new Vector3(20, 14.5f, 0), "right", 2);
            } else 
                StartCoroutine(CannotEnter(20));
        }
        if (switchManager.switchdata["ThirdF_MeetingRoomDoor"].on) {
            switchManager.switchdata["ThirdF_MeetingRoomDoor"].on = false;
            StartCoroutine(CannotEnter(30));
        }
        if (switchManager.switchdata["ThirdF_EmptyRoomDoor"].on) {
            switchManager.switchdata["ThirdF_EmptyRoomDoor"].on = false;
            StartCoroutine(CannotEnter(40));
        }
    }

    IEnumerator CabinetHS()
    {
        switchManager.ing = true;
        //=====================================================x
        sceneManager.PlayObj(0, 300, "UC");
        yield return new WaitUntil(() => gameManager.isInterAction == false);
        //=====================================================x
        switchManager.ing = false;
    }

    IEnumerator CabinetJU()
    {
        switchManager.ing = true;

        sceneManager.PlayObj(10, 300, "UC");
        yield return new WaitUntil(() => gameManager.isInterAction == false);

        switchManager.ing = false;
    }

    IEnumerator CabinetJE()
    {
        switchManager.ing = true;

        sceneManager.PlayObj(20, 300, "UC");
        yield return new WaitUntil(() => gameManager.isInterAction == false);

        switchManager.ing = false;
    }

    IEnumerator CabinetJH()
    {
        switchManager.ing = true;
        //=====================================================x
        sceneManager.PlayObj(30, 300, "UC");
        yield return new WaitUntil(() => gameManager.isInterAction == false);
        //-----------------------------------------------------
        if (!gameManager.inventory.GetComponent<Inventory>().havingItem(2)) {
            gameManager.inventory.GetComponent<Inventory>().AddItem(2);
            gameManager.currSceneIndex = 300;
            gameManager.currCutIndex = 20;
            gameManager.SystemAction();
        }
        //=====================================================x
        switchManager.ing = false;
    }

    IEnumerator CabinetSH()
    {
        switchManager.ing = true;

        sceneManager.PlayObj(40, 300, "UC");
        yield return new WaitUntil(() => gameManager.isInterAction == false);

        switchManager.ing = false;
    }

    IEnumerator CabinetAR()
    {
        switchManager.ing = true;

        sceneManager.PlayObj(50, 300, "UC");
        yield return new WaitUntil(() => gameManager.isInterAction == false);

        switchManager.ing = false;
    }

    IEnumerator CabinetYU()
    {
        switchManager.ing = true;

        sceneManager.PlayObj(60, 300, "UC");
        yield return new WaitUntil(() => gameManager.isInterAction == false);

        switchManager.ing = false;
    }

    IEnumerator CabinetJiHyeong()
    {
        switchManager.ing = true;

        sceneManager.PlayObj(70, 300, "UC");
        yield return new WaitUntil(() => gameManager.isInterAction == false);

        switchManager.ing = false;
    }

    IEnumerator SearchingJiUn()
    {
        switchManager.ing = true;

        sceneManager.PlayTalk(10, 100);
        yield return new WaitUntil(() => gameManager.isTalking == false);

        switchManager.ing = false;
    }

    IEnumerator SearchingSunWoo()
    {
        switchManager.ing = true;

        sceneManager.PlayTalk(20, 100);
        yield return new WaitUntil(() => gameManager.isTalking == false);

        switchManager.ing = false;
    }

    IEnumerator CannotEnter(int _obj)
    {
        sceneManager.PlayObj(_obj, 500, "UC");
        yield return new WaitUntil(() => gameManager.isInterAction == false);
    }
}

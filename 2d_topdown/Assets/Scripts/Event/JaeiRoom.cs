using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JaeiRoom : MonoBehaviour
{
    public GameManager gameManager;
    public SceneManager sceneManager;
    public SwitchManager switchManager;
    GameObject player;
    PlayerAction playerLogic;
    //--------------------------------------
    public GameObject tutorialSet;
    public GameObject JaeiID;
    public GameObject monitorUI;
    public GameObject monitorObj;
    Animator monitorAnimator;
    //--------------------------------------
    public Transform[] spawnPoints;
    //======================================

    void Start() {
        player = gameManager.player;
        playerLogic = player.GetComponent<PlayerAction>();
        monitorAnimator = monitorObj.GetComponent<Animator>();

        if (gameManager.inventory.GetComponent<Inventory>().havingItem(1)) {
            JaeiID.SetActive(false);
        }
    }

    void Update() {
        if (switchManager.switchdata["SecondF_mail9"].on && !switchManager.switchdata["SecondF_readMail9"].on) {
            sceneManager.JR_newMail = true;
        } else if (switchManager.switchdata["SecondF_readMail9"].on) {
            sceneManager.JR_newMail = false;
        }
        if (switchManager.switchdata["SecondF_mail10"].on && !switchManager.switchdata["SecondF_readMail10"].on) {
            sceneManager.JR_newMail = true;
        } else if (switchManager.switchdata["SecondF_readMail10"].on) {
            sceneManager.JR_newMail = false;
        }

        monitorAnimator.SetBool("getMessage", sceneManager.JR_newMail);

        if (switchManager.switchdata["JR_callDr"].on) {
            switchManager.switchdata["JR_callDr"].on = false;
            StartCoroutine(callDr());
        }

        if (switchManager.switchdata["Tutorial_Space"].on) {
            switchManager.switchdata["Tutorial"].on = true;
            switchManager.switchdata["Tutorial_Space"].on = false;
            StartCoroutine(TutorialEx());
        }

        if (switchManager.switchdata["Tutorial"].off) {
            tutorialSet.transform.Find("guideLine").gameObject.SetActive(false);
        }

        if (switchManager.switchdata["JR_answerMail"].on && !monitorUI.activeSelf) {
            switchManager.switchdata["JR_answerMail"].on = false;
            StartCoroutine(noticeChange());
        }

        if (gameManager.canSleep && switchManager.switchdata["JR_Bed"].on) {   
            switchManager.switchdata["JR_Bed"].on = false;
            StartCoroutine(Change());
        }

        // ** 오브젝트 상호작용 **
        if (!switchManager.switchdata["Tutorial"].on && switchManager.switchdata["JR_Monitor"].on) {  // 모니터
            switchManager.switchdata["JR_Monitor"].on = false;

            gameManager.dontMove = true;
            sceneManager.JR_newMail = false;

            if (switchManager.switchdata["A1_End"].off && !switchManager.switchdata["JR_answerMail"].on && !switchManager.switchdata["JR_answerMail"].off) {
                switchManager.switchdata["JR_answerMail"].on = true;
            }

            Monitor();
        }

        if (switchManager.switchdata["JR_BookShelf"].on) {
            switchManager.switchdata["JR_BookShelf"].on = false;
            StartCoroutine(BookShelf());
        }

        if (switchManager.switchdata["Item_JaeiId"].on) {
            switchManager.switchdata["Item_JaeiId"].on = false;
            gameManager.AddItem(1);
        }
    }

    void Monitor()
    {
        switchManager.ing = true;
        //=====================================================
        monitorAnimator.SetBool("getMessage", false);
        //-----------------------------------------------------
        monitorUI.SetActive(true);
        //=====================================================
    }

    IEnumerator BookShelf()
    {
        switchManager.ing = true;
        //=====================================================
        gameManager.isSelecting = true;
        sceneManager.SetSelectBtnTxt("신문 스크랩", "", "", "");
        yield return new WaitUntil(() => gameManager.selectIndex != -1);

        switch (gameManager.selectIndex) {
            case 1:
                sceneManager.PlayObj(0, 200);
                yield return new WaitUntil(() => gameManager.isInterAction == false);
                break;
            case 2:
                sceneManager.PlayObj(10, 200);
                yield return new WaitUntil(() => gameManager.isInterAction == false);
                break;
            case 3:
                sceneManager.PlayObj(20, 200);
                yield return new WaitUntil(() => gameManager.isInterAction == false);
                break;
            case 4:
                sceneManager.PlayObj(30, 200);
                yield return new WaitUntil(() => gameManager.isInterAction == false);
                break;
        }

        gameManager.selectIndex = -1;
        gameManager.isSelecting = false;
        //=====================================================
        switchManager.ing = false;
    }

    IEnumerator callDr() {
        switchManager.ing = true;
        gameManager.dontMove = true;
        //=====================================================
        sceneManager.PlayTalk(0, 0);
        yield return new WaitUntil(() => gameManager.isTalking == false);
        //-----------------------------------------------------
        GameObject Sunwoo = sceneManager.PlayNPC("N_Sunwoo", spawnPoints[0].position);
        NPCMove SunwooLogic = Sunwoo.GetComponent<NPCMove>();

        SunwooLogic.SetWaypoint(2, spawnPoints[0].position, spawnPoints[1].position);
        SunwooLogic.isOn = true;
        yield return new WaitUntil(() => SunwooLogic.isOff == true);
        SunwooLogic.isOff = false;
        //-----------------------------------------------------
        playerLogic.Turn("left");

        sceneManager.PlayTalk(10, 0);
        yield return new WaitUntil(() => gameManager.isTalking == false);

        SunwooLogic.SetWaypoint(2, spawnPoints[1].position, spawnPoints[0].position);
        SunwooLogic.isOn = true;
        yield return new WaitUntil(() => SunwooLogic.isOff == true);
        SunwooLogic.isOff = false;
        
        Sunwoo.SetActive(false);
        //-----------------------------------------------------
        yield return new WaitForSeconds(0.5f);
        sceneManager.PlayTalk(20, 0);
        yield return new WaitUntil(() => gameManager.isTalking == false);
        //=====================================================
        gameManager.dontMove = false;
        switchManager.ing = false;
        switchManager.switchdata["JR_callDr"].off = true;

        switchManager.switchdata["Tutorial_Space"].on = true;
    }


    IEnumerator TutorialEx()
    {
        switchManager.ing = true;
        //=====================================================
        yield return new WaitForSeconds(1f);
        //-----------------------------------------------------
        tutorialSet.transform.Find("keyDirection").gameObject.SetActive(true);
        tutorialSet.transform.Find("background").gameObject.SetActive(true);
        sceneManager.PlaySystemTalk(0, 500);
        yield return new WaitUntil(() => gameManager.isSystem == false);
        tutorialSet.transform.Find("background").gameObject.SetActive(false);
        //-----------------------------------------------------
        yield return new WaitForSeconds(2f);
        tutorialSet.transform.Find("keySpace").gameObject.SetActive(true);
        tutorialSet.transform.Find("background").gameObject.SetActive(true);
        sceneManager.PlaySystemTalk(10, 500);
        yield return new WaitUntil(() => gameManager.isSystem == false);
        tutorialSet.transform.Find("background").gameObject.SetActive(false);
        //-----------------------------------------------------
        switchManager.ing = false;
        tutorialSet.transform.Find("mark").gameObject.SetActive(true);
        yield return new WaitUntil(() => gameManager.inventory.GetComponent<Inventory>().havingItem(1));
        switchManager.ing = true;
        tutorialSet.transform.Find("keyDirection").gameObject.SetActive(false);
        tutorialSet.transform.Find("keySpace").gameObject.SetActive(false);
        tutorialSet.transform.Find("mark").gameObject.SetActive(false);
        
        gameManager.dontMove = true;
        yield return new WaitUntil(() => gameManager.isSystem == false);
        tutorialSet.transform.Find("keyI").gameObject.SetActive(true);
        tutorialSet.transform.Find("background").gameObject.SetActive(true);
        sceneManager.PlaySystemTalk(20, 500);
        yield return new WaitUntil(() => gameManager.isSystem == false);
        tutorialSet.transform.Find("background").gameObject.SetActive(false);
        gameManager.dontMove = false;
        yield return new WaitUntil(() => gameManager.inventory.activeSelf);
        tutorialSet.transform.Find("keyI").gameObject.SetActive(false);
        yield return new WaitUntil(() => gameManager.inventory.activeSelf == false);
        //-----------------------------------------------------
        yield return new WaitForSeconds(1f);
        tutorialSet.transform.Find("keyM").gameObject.SetActive(true);
        tutorialSet.transform.Find("background").gameObject.SetActive(true);
        sceneManager.PlaySystemTalk(30, 500);
        yield return new WaitUntil(() => gameManager.isSystem == false);
        tutorialSet.transform.Find("background").gameObject.SetActive(false);
        yield return new WaitUntil(() => gameManager.mapUI.activeSelf);
        tutorialSet.transform.Find("keyM").gameObject.SetActive(false);
        yield return new WaitUntil(() => gameManager.mapUI.activeSelf == false);
        //-----------------------------------------------------
        sceneManager.PlaySystemTalk(40, 500);
        yield return new WaitUntil(() => gameManager.isSystem == false);
        //=====================================================
        switchManager.ing = false;

        tutorialSet.transform.Find("guideLine").gameObject.SetActive(false);
        switchManager.switchdata["Tutorial"].on = false;
        switchManager.switchdata["Tutorial"].off = true;
        switchManager.switchdata["Tutorial_DataCom"].on = true;
        switchManager.switchdata["Tutorial_SearchCom"].on = true;

        gameManager.fadeEffect.FadeImg.color = new Color(gameManager.fadeEffect.FadeImg.color.r, gameManager.fadeEffect.FadeImg.color.g, gameManager.fadeEffect.FadeImg.color.b, 1);

        StartCoroutine(gameManager.IntroFade());
    }


    IEnumerator noticeChange()
    {
        switchManager.ing = true;
        //=====================================================
        playerLogic.Turn("down");
        yield return new WaitForSeconds(0.4f);
        //-----------------------------------------------------
        sceneManager.PlaySystemTalk(0, 0);
        yield return new WaitUntil(() => gameManager.isSystem == false);

        gameManager.canSleep = true;
        //=====================================================
        switchManager.ing = false;
        switchManager.switchdata["JR_answerMail"].off = true;

    }

/*
    IEnumerator Change()
    {
        switchManager.ing = true;
        //=====================================================x
        gameManager.isSelecting = true;
        sceneManager.SetSelectTxt();
        //sceneManager.Btn2Txt.gameObject.GetComponent<Button>().interactable = false;
        sceneManager.SetSelectBtnTxt("인격 변경", "", "", "취소하기");
        yield return new WaitUntil(() => gameManager.selectIndex != -1);

        switch (gameManager.selectIndex) {
            case 1:
                gameManager.selectIndex = -1;
                gameManager.isSelecting = false;
                //-----------------------------------------------------
                StartCoroutine(FadeAndChangeMap());

                gameManager.canSleep = false;
                //=====================================================x
                switchManager.ing = false;
                break;
                /*
            case 2:
                gameManager.selectIndex = -1;
                gameManager.isSelecting = false;

                gameManager.selectBox.SetActive(false);
                switchManager.ing = false;

                StopAllCoroutines();

                StartCoroutine(reallySleep());
                break;
            case 2:
            case 3:
                StartCoroutine(Change());
                break;
            case 4:
                gameManager.selectIndex = -1;
                gameManager.isSelecting = false;

                switchManager.ing = false;
                break;
        }
        //sceneManager.Btn2Txt.gameObject.GetComponent<Button>().interactable = true;
    }

    IEnumerator FadeAndChangeMap()
    {
        gameManager.currTime = 3;
        gameManager.fadeEffect.nameStr = "서재하";
        gameManager.fadeEffect.timeStr = "2030-03-06(수)\n오후  9:20:00";
        gameManager.fadeEffect.OnFade(FadeState.FadeOut);
        yield return new WaitUntil(() => gameManager.fadeEffect.endFade == true);

        gameManager.OffPlayer(player);
        gameManager.ChangeMap(4, "P_Jaeha", new Vector3(27f, 11, 0), "down", 3);
    }
*/
     IEnumerator Change()
    {
        switchManager.ing = true;
        //=====================================================x
        gameManager.isSelecting = true;
        sceneManager.SetSelectTxt();
        //sceneManager.Btn1Txt.gameObject.GetComponent<Button>().interactable = false;
        sceneManager.SetSelectBtnTxt("", "잠들기", "", "취소하기");
        yield return new WaitUntil(() => gameManager.selectIndex != -1);

        switch (gameManager.selectIndex) {
            /*
            case 1:
                gameManager.selectIndex = -1;
                gameManager.isSelecting = false;
                gameManager.selectBox.SetActive(false);
                //-----------------------------------------------------

                gameManager.canSleep = false;
                //=====================================================x
                switchManager.ing = false;
                break;
                */
            case 1:
            case 3:
                StartCoroutine(Change());
                break;
            case 2:
                gameManager.selectIndex = -1;
                gameManager.isSelecting = false;

                gameManager.selectBox.SetActive(false);
                switchManager.ing = false;

                StopAllCoroutines();
                
                StartCoroutine(FadeAndChangeMap());

                //StartCoroutine(reallySleep());
                break;
            case 4:
                gameManager.selectIndex = -1;
                gameManager.isSelecting = false;
                gameManager.selectBox.SetActive(false);

                switchManager.ing = false;
                break;
        }
        //sceneManager.Btn1Txt.gameObject.GetComponent<Button>().interactable = true;

    }

    IEnumerator FadeAndChangeMap()
    {
        gameManager.currTime = 4;
        gameManager.fadeEffect.nameStr = "서재이";
        gameManager.fadeEffect.timeStr = "2030-03-07(목)\n오전  9:30:00";
        gameManager.fadeEffect.OnFade(FadeState.FadeOut);
        yield return new WaitUntil(() => gameManager.fadeEffect.endFade == true);

        gameManager.OffPlayer(player);
        gameManager.ChangeMap(0, "P_Jaei", new Vector3(29f, 6, 0), "left", 1);
        
        switchManager.switchdata["SecondF_movingSM"].on = true;
    }

    

    IEnumerator reallySleep()
    {
        gameManager.isSelecting = true;
        sceneManager.SetSelectTxt("잠에 들면 다음날로 넘어가게 됩니다. 정말 잠에 드시겠습니까?");
        sceneManager.SetSelectBtnTxt("", "", "예", "아니오");
        yield return new WaitUntil(() => gameManager.selectIndex != -1);

        switch (gameManager.selectIndex) {
            case 3:
                gameManager.selectIndex = -1;
                gameManager.isSelecting = false;

                yield return new WaitForSeconds(0.5f);
                gameManager.OffPlayer(player);
                gameManager.ChangeMap(0, "P_Jaei", new Vector3(29f, 4, 0), "right", 1);

                gameManager.canSleep = false;
                

                StopAllCoroutines();
                break;
            case 4:
                gameManager.selectIndex = -1;
                gameManager.isSelecting = false;
                

                StopAllCoroutines();
                break;
        }
    }
}

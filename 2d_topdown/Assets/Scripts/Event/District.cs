using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class District : MonoBehaviour
{
    public GameManager gameManager;
    public SceneManager sceneManager;
    public SwitchManager switchManager;
    GameObject player;
    PlayerAction playerLogic;
    //--------------------------------------
    public GameObject Sunwoo_FirstSearch;
    NPCMove SunwooLogic;
    public GameObject monitorUI;
    public GameObject monitorObj;
    Animator monitorAnimator;
    //--------------------------------------
    public Transform[] spawnPoints;
    //======================================
    void Start() {
        player = gameManager.player;
        playerLogic = player.GetComponent<PlayerAction>();
    }

    void OnEnable() {
        monitorAnimator = monitorObj.GetComponent<Animator>();
        monitorAnimator.SetBool("getMessage", sceneManager.D_newMail);
    }

    void Update() {
        if (switchManager.switchdata["Searchcom_signupJihyeon"].on) {
            SunwooLogic = Sunwoo_FirstSearch.GetComponent<NPCMove>();
            SunwooLogic.Turn("up");
        }
        if (switchManager.switchdata["JR_answerMail"].off) {
            Sunwoo_FirstSearch.SetActive(false);
        }

        if (switchManager.switchdata["Searchcom_signupJihyeon"].on && switchManager.switchdata["D_FirstSearch"].on) {
            switchManager.switchdata["Searchcom_signupJihyeon"].on = false;
            switchManager.switchdata["D_FirstSearch"].on = false;
            StartCoroutine(FirstSearch());
        }

        if (switchManager.switchdata["JR_answerMail"].off && switchManager.switchdata["D_Bot"].on) {   // bot
            switchManager.switchdata["D_Bot"].on = false;
            StartCoroutine(FirstChange());
        }

        if (switchManager.switchdata["D_Monitor"].on) {  // 모니터
            switchManager.switchdata["D_Monitor"].on = false;
            sceneManager.D_newMail = false;
            Monitor();
        }

        if (gameManager.canSleep && switchManager.switchdata["D_Bed"].on) {
            switchManager.switchdata["D_Bed"].on = false;
            StartCoroutine(Change());
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

    IEnumerator FirstSearch()
    {
        switchManager.ing = true;
        //=====================================================
        sceneManager.PlayTalk(0, 300);
        yield return new WaitUntil(() => gameManager.isTalking == false);
        //=====================================================
        switchManager.ing = false;
        switchManager.switchdata["D_FirstSearch"].off = true;
    }


    IEnumerator FirstChange()
    {
        switchManager.ing = true;
        //=====================================================x
        sceneManager.PlayTalk(0, 400);
        yield return new WaitUntil(() => gameManager.isTalking == false);
        //=====================================================x
        switchManager.ing = false;
        switchManager.switchdata["D_Bot"].off = true;

        //switchManager.switchdata["SecondF_movingSM"].on = true;

        gameManager.canSleep = true;
        StopAllCoroutines();
    }

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

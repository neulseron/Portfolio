using System.Collections;
using UnityEngine;

public class JaeiRoom : MapEvent
{
#region Variables
    public GameObject tutorialSet;
    public GameObject JaeiID;
    public GameObject monitorUI;
    public GameObject monitorObj;
    Animator monitorAnimator;

    public Transform[] spawnPoints;
#endregion Variables


#region Unity Methods
    protected override void Start() {
        base.Start();

        monitorAnimator = monitorObj.GetComponent<Animator>();

        if (GameManager.Instance.inventory.GetComponent<Inventory>().havingItem(1)) {
            JaeiID.SetActive(false);
        }
    }

    void Update() {
        if (SwitchManager.Instance.switchdata["SecondF_mail9"].on && !SwitchManager.Instance.switchdata["SecondF_readMail9"].on) {
            SceneManager.Instance.JR_newMail = true;
        } else if (SwitchManager.Instance.switchdata["SecondF_readMail9"].on) {
            SceneManager.Instance.JR_newMail = false;
        }
        if (SwitchManager.Instance.switchdata["SecondF_mail10"].on && !SwitchManager.Instance.switchdata["SecondF_readMail10"].on) {
            SceneManager.Instance.JR_newMail = true;
        } else if (SwitchManager.Instance.switchdata["SecondF_readMail10"].on) {
            SceneManager.Instance.JR_newMail = false;
        }

        monitorAnimator.SetBool("getMessage", SceneManager.Instance.JR_newMail);

        if (SwitchManager.Instance.switchdata["JR_callDr"].on) {
            SwitchManager.Instance.switchdata["JR_callDr"].on = false;
            StartCoroutine(callDr());
        }

        if (SwitchManager.Instance.switchdata["Tutorial_Space"].on) {
            SwitchManager.Instance.switchdata["Tutorial"].on = true;
            SwitchManager.Instance.switchdata["Tutorial_Space"].on = false;
            StartCoroutine(TutorialEx());
        }

        if (SwitchManager.Instance.switchdata["Tutorial"].off) {
            tutorialSet.transform.Find("guideLine").gameObject.SetActive(false);
        }

        if (SwitchManager.Instance.switchdata["JR_answerMail"].on && !monitorUI.activeSelf) {
            SwitchManager.Instance.switchdata["JR_answerMail"].on = false;
            StartCoroutine(noticeChange());
        }

        if (GameManager.Instance.canSleep && SwitchManager.Instance.switchdata["JR_Bed"].on) {   
            SwitchManager.Instance.switchdata["JR_Bed"].on = false;
            StartCoroutine(Change());
        }

        // ** 오브젝트 상호작용 **
        if (!SwitchManager.Instance.switchdata["Tutorial"].on && SwitchManager.Instance.switchdata["JR_Monitor"].on) {  // 모니터
            SwitchManager.Instance.switchdata["JR_Monitor"].on = false;

            GameManager.Instance.dontMove = true;
            SceneManager.Instance.JR_newMail = false;

            if (SwitchManager.Instance.switchdata["A1_End"].off && !SwitchManager.Instance.switchdata["JR_answerMail"].on && !SwitchManager.Instance.switchdata["JR_answerMail"].off) {
                SwitchManager.Instance.switchdata["JR_answerMail"].on = true;
            }

            Monitor();
        }

        if (SwitchManager.Instance.switchdata["JR_BookShelf"].on) {
            SwitchManager.Instance.switchdata["JR_BookShelf"].on = false;
            StartCoroutine(BookShelf());
        }

        if (SwitchManager.Instance.switchdata["Item_JaeiId"].on) {
            SwitchManager.Instance.switchdata["Item_JaeiId"].on = false;
            GameManager.Instance.AddItem(1);
        }
    }
#endregion Unity Methods


#region Event
    void Monitor()
    {
        SwitchManager.Instance.ing = true;
        //=====================================================
        monitorAnimator.SetBool("getMessage", false);
        //-----------------------------------------------------
        monitorUI.SetActive(true);
        //=====================================================
    }

    IEnumerator BookShelf()
    {
        SwitchManager.Instance.ing = true;
        //=====================================================
        GameManager.Instance.isSelecting = true;
        SceneManager.Instance.SetSelectBtnTxt("신문 스크랩", "", "", "");
        yield return new WaitUntil(() => GameManager.Instance.selectIndex != -1);

        switch (GameManager.Instance.selectIndex) {
            case 1:
                SceneManager.Instance.PlayObj(0, 200);
                yield return new WaitUntil(() => GameManager.Instance.isInterAction == false);
                break;
            case 2:
                SceneManager.Instance.PlayObj(10, 200);
                yield return new WaitUntil(() => GameManager.Instance.isInterAction == false);
                break;
            case 3:
                SceneManager.Instance.PlayObj(20, 200);
                yield return new WaitUntil(() => GameManager.Instance.isInterAction == false);
                break;
            case 4:
                SceneManager.Instance.PlayObj(30, 200);
                yield return new WaitUntil(() => GameManager.Instance.isInterAction == false);
                break;
        }

        GameManager.Instance.selectIndex = -1;
        GameManager.Instance.isSelecting = false;
        //=====================================================
        SwitchManager.Instance.ing = false;
    }

    IEnumerator callDr() {
        SwitchManager.Instance.ing = true;
        GameManager.Instance.dontMove = true;
        //=====================================================
        SceneManager.Instance.PlayTalk(0, 0);
        yield return new WaitUntil(() => GameManager.Instance.isTalking == false);
        //-----------------------------------------------------
        GameObject Sunwoo = SceneManager.Instance.PlayNPC("N_Sunwoo", spawnPoints[0].position);
        NPCMove SunwooLogic = Sunwoo.GetComponent<NPCMove>();

        SunwooLogic.SetWaypoint(2, spawnPoints[0].position, spawnPoints[1].position);
        SunwooLogic.isOn = true;
        yield return new WaitUntil(() => SunwooLogic.isOff == true);
        SunwooLogic.isOff = false;
        //-----------------------------------------------------
        playerLogic.Turn("left");

        SceneManager.Instance.PlayTalk(10, 0);
        yield return new WaitUntil(() => GameManager.Instance.isTalking == false);

        SunwooLogic.SetWaypoint(2, spawnPoints[1].position, spawnPoints[0].position);
        SunwooLogic.isOn = true;
        yield return new WaitUntil(() => SunwooLogic.isOff == true);
        SunwooLogic.isOff = false;
        
        Sunwoo.SetActive(false);
        //-----------------------------------------------------
        yield return new WaitForSeconds(0.5f);
        SceneManager.Instance.PlayTalk(20, 0);
        yield return new WaitUntil(() => GameManager.Instance.isTalking == false);
        //=====================================================
        GameManager.Instance.dontMove = false;
        SwitchManager.Instance.ing = false;
        SwitchManager.Instance.switchdata["JR_callDr"].off = true;

        SwitchManager.Instance.switchdata["Tutorial_Space"].on = true;
    }


    IEnumerator TutorialEx()
    {
        SwitchManager.Instance.ing = true;
        //=====================================================
        yield return new WaitForSeconds(1f);
        //-----------------------------------------------------
        tutorialSet.transform.Find("keyDirection").gameObject.SetActive(true);
        tutorialSet.transform.Find("background").gameObject.SetActive(true);
        SceneManager.Instance.PlaySystemTalk(0, 500);
        yield return new WaitUntil(() => GameManager.Instance.isSystem == false);
        tutorialSet.transform.Find("background").gameObject.SetActive(false);
        //-----------------------------------------------------
        yield return new WaitForSeconds(2f);
        tutorialSet.transform.Find("keySpace").gameObject.SetActive(true);
        tutorialSet.transform.Find("background").gameObject.SetActive(true);
        SceneManager.Instance.PlaySystemTalk(10, 500);
        yield return new WaitUntil(() => GameManager.Instance.isSystem == false);
        tutorialSet.transform.Find("background").gameObject.SetActive(false);
        //-----------------------------------------------------
        SwitchManager.Instance.ing = false;
        tutorialSet.transform.Find("mark").gameObject.SetActive(true);
        yield return new WaitUntil(() => GameManager.Instance.inventory.GetComponent<Inventory>().havingItem(1));
        SwitchManager.Instance.ing = true;
        tutorialSet.transform.Find("keyDirection").gameObject.SetActive(false);
        tutorialSet.transform.Find("keySpace").gameObject.SetActive(false);
        tutorialSet.transform.Find("mark").gameObject.SetActive(false);
        
        GameManager.Instance.dontMove = true;
        yield return new WaitUntil(() => GameManager.Instance.isSystem == false);
        tutorialSet.transform.Find("keyI").gameObject.SetActive(true);
        tutorialSet.transform.Find("background").gameObject.SetActive(true);
        SceneManager.Instance.PlaySystemTalk(20, 500);
        yield return new WaitUntil(() => GameManager.Instance.isSystem == false);
        tutorialSet.transform.Find("background").gameObject.SetActive(false);
        GameManager.Instance.dontMove = false;
        yield return new WaitUntil(() => GameManager.Instance.inventory.activeSelf);
        tutorialSet.transform.Find("keyI").gameObject.SetActive(false);
        yield return new WaitUntil(() => GameManager.Instance.inventory.activeSelf == false);
        //-----------------------------------------------------
        yield return new WaitForSeconds(1f);
        tutorialSet.transform.Find("keyM").gameObject.SetActive(true);
        tutorialSet.transform.Find("background").gameObject.SetActive(true);
        SceneManager.Instance.PlaySystemTalk(30, 500);
        yield return new WaitUntil(() => GameManager.Instance.isSystem == false);
        tutorialSet.transform.Find("background").gameObject.SetActive(false);
        yield return new WaitUntil(() => GameManager.Instance.mapUI.activeSelf);
        tutorialSet.transform.Find("keyM").gameObject.SetActive(false);
        yield return new WaitUntil(() => GameManager.Instance.mapUI.activeSelf == false);
        //-----------------------------------------------------
        SceneManager.Instance.PlaySystemTalk(40, 500);
        yield return new WaitUntil(() => GameManager.Instance.isSystem == false);
        //=====================================================
        SwitchManager.Instance.ing = false;

        tutorialSet.transform.Find("guideLine").gameObject.SetActive(false);
        SwitchManager.Instance.switchdata["Tutorial"].on = false;
        SwitchManager.Instance.switchdata["Tutorial"].off = true;
        SwitchManager.Instance.switchdata["Tutorial_DataCom"].on = true;
        SwitchManager.Instance.switchdata["Tutorial_SearchCom"].on = true;

        GameManager.Instance.fadeEffect.FadeImg.color = new Color(GameManager.Instance.fadeEffect.FadeImg.color.r, GameManager.Instance.fadeEffect.FadeImg.color.g, GameManager.Instance.fadeEffect.FadeImg.color.b, 1);

        StartCoroutine(GameManager.Instance.IntroFade());
    }


    IEnumerator noticeChange()
    {
        SwitchManager.Instance.ing = true;
        //=====================================================
        playerLogic.Turn("down");
        yield return new WaitForSeconds(0.4f);
        //-----------------------------------------------------
        SceneManager.Instance.PlaySystemTalk(0, 0);
        yield return new WaitUntil(() => GameManager.Instance.isSystem == false);

        GameManager.Instance.canSleep = true;
        //=====================================================
        SwitchManager.Instance.ing = false;
        SwitchManager.Instance.switchdata["JR_answerMail"].off = true;

    }

     IEnumerator Change()
    {
        SwitchManager.Instance.ing = true;
        //=====================================================x
        GameManager.Instance.isSelecting = true;
        SceneManager.Instance.SetSelectTxt();
        SceneManager.Instance.SetSelectBtnTxt("", "잠들기", "", "취소하기");
        yield return new WaitUntil(() => GameManager.Instance.selectIndex != -1);

        switch (GameManager.Instance.selectIndex) {
            case 1:
            case 3:
                StartCoroutine(Change());
                break;
            case 2:
                GameManager.Instance.selectIndex = -1;
                GameManager.Instance.isSelecting = false;

                GameManager.Instance.selectBox.SetActive(false);
                SwitchManager.Instance.ing = false;

                StopAllCoroutines();
                
                StartCoroutine(FadeAndChangeMap());

                break;
            case 4:
                GameManager.Instance.selectIndex = -1;
                GameManager.Instance.isSelecting = false;
                GameManager.Instance.selectBox.SetActive(false);

                SwitchManager.Instance.ing = false;
                break;
        }
    }

    IEnumerator FadeAndChangeMap()
    {
        GameManager.Instance.currTime = 4;
        GameManager.Instance.fadeEffect.nameStr = "서재이";
        GameManager.Instance.fadeEffect.timeStr = "2030-03-07(목)\n오전  9:30:00";
        GameManager.Instance.fadeEffect.OnFade(FadeState.FadeOut);
        yield return new WaitUntil(() => GameManager.Instance.fadeEffect.endFade == true);

        GameManager.Instance.OffPlayer(player);
        GameManager.Instance.ChangeMap(0, "P_Jaei", new Vector3(29f, 6, 0), "left", 1);
        
        SwitchManager.Instance.switchdata["SecondF_movingSM"].on = true;
    }

#endregion Event
}

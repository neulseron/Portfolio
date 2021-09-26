using System.Collections;
using UnityEngine;

public class District : MapEvent
{
#region Variables
    public GameObject Sunwoo_FirstSearch;
    NPCMove SunwooLogic;

    public GameObject monitorUI;
    public GameObject monitorObj;
    Animator monitorAnimator;

    public Transform[] spawnPoints;
#endregion Variables


#region Unity Methods
    void OnEnable() {
        monitorAnimator = monitorObj.GetComponent<Animator>();
        monitorAnimator.SetBool("getMessage", SceneManager.Instance.D_newMail);
    }

    void Update() {
        if (SwitchManager.Instance.switchdata["Searchcom_signupJihyeon"].on) {
            SunwooLogic = Sunwoo_FirstSearch.GetComponent<NPCMove>();
            SunwooLogic.Turn("up");
        }
        if (SwitchManager.Instance.switchdata["JR_answerMail"].off) {
            Sunwoo_FirstSearch.SetActive(false);
        }

        if (SwitchManager.Instance.switchdata["Searchcom_signupJihyeon"].on && SwitchManager.Instance.switchdata["D_FirstSearch"].on) {
            SwitchManager.Instance.switchdata["Searchcom_signupJihyeon"].on = false;
            SwitchManager.Instance.switchdata["D_FirstSearch"].on = false;
            StartCoroutine(FirstSearch());
        }

        if (SwitchManager.Instance.switchdata["JR_answerMail"].off && SwitchManager.Instance.switchdata["D_Bot"].on) {   // bot
            SwitchManager.Instance.switchdata["D_Bot"].on = false;
            StartCoroutine(FirstChange());
        }

        if (SwitchManager.Instance.switchdata["D_Monitor"].on) {  // 모니터
            SwitchManager.Instance.switchdata["D_Monitor"].on = false;
            SceneManager.Instance.D_newMail = false;
            Monitor();
        }

        if (GameManager.Instance.canSleep && SwitchManager.Instance.switchdata["D_Bed"].on) {
            SwitchManager.Instance.switchdata["D_Bed"].on = false;
            StartCoroutine(Change());
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

    IEnumerator FirstSearch()
    {
        SwitchManager.Instance.ing = true;
        //=====================================================
        SceneManager.Instance.PlayTalk(0, 300);
        yield return new WaitUntil(() => GameManager.Instance.isTalking == false);
        //=====================================================
        SwitchManager.Instance.ing = false;
        SwitchManager.Instance.switchdata["D_FirstSearch"].off = true;
    }


    IEnumerator FirstChange()
    {
        SwitchManager.Instance.ing = true;
        //=====================================================x
        SceneManager.Instance.PlayTalk(0, 400);
        yield return new WaitUntil(() => GameManager.Instance.isTalking == false);
        //=====================================================x
        SwitchManager.Instance.ing = false;
        SwitchManager.Instance.switchdata["D_Bot"].off = true;

        GameManager.Instance.canSleep = true;
        StopAllCoroutines();
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
    }

#endregion Event
}

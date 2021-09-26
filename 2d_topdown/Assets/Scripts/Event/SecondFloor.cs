using System.Collections;
using UnityEngine;

public class SecondFloor : MapEvent
{
#region Variables
    public ComaData comaData;
    public GameObject DataComUI;

    public GameObject Black;

    public GameObject cameraMiddleTarget;

    public GameObject tutoMark;

    public Animator JiHyeonAnimator;
    public GameObject Jihyeon;
    public GameObject JihyeonList;

    public GameObject Hyeongseo;
    public GameObject HyeongseoList;

    public GameObject Sunwoo;
    NPCMove SunwooLogic;

    public Transform[] spawnPoints;
#endregion Variables


#region Unity Methods
    protected override void Start() {
        base.Start();

        SunwooLogic = Sunwoo.GetComponent<NPCMove>();
    }

    void Update() {
        if (SwitchManager.Instance.switchdata["Tutorial"].off && SwitchManager.Instance.switchdata["Tutorial_DataCom"].on && !SwitchManager.Instance.switchdata["Tutorial_OffMarkDataCom"].on) {
            tutoMark.SetActive(true);
        }
        if (SwitchManager.Instance.switchdata["Tutorial_OffMarkDataCom"].on) {
            tutoMark.SetActive(false);
        }

        if (SwitchManager.Instance.switchdata["A1_End"].on) {   
            SwitchManager.Instance.switchdata["A1_End"].on = false;
            StartCoroutine(wakeupJihyeon());
        }

        if (SwitchManager.Instance.switchdata["SecondF_movingSM"].on) {
            SwitchManager.Instance.switchdata["SecondF_movingSM"].on = false;
            StartCoroutine(walkingSumin());
        }

        if (SwitchManager.Instance.switchdata["SecondF_movingSM"].off && SwitchManager.Instance.switchdata["SecondF_strangeSM"].on) {
            SwitchManager.Instance.switchdata["SecondF_strangeSM"].on = false;
            StartCoroutine(strangeSumin());
        }

        if (SwitchManager.Instance.switchdata["SecondF_appearHS"].on) {
            SwitchManager.Instance.switchdata["SecondF_appearHS"].on = false;
            StartCoroutine(AppearHyeonSeok());
        }

        if (SwitchManager.Instance.switchdata["SecondF_dischargedJH"].on) {
            SwitchManager.Instance.switchdata["SecondF_dischargedJH"].on = false;
            StartCoroutine(dischargeJihyeon());
        }

        if (SwitchManager.Instance.switchdata["DataCom_Chk103015"].on && !DataComUI.activeSelf) {
            SwitchManager.Instance.switchdata["DataCom_Chk103015"].on = false;
            StartCoroutine(ChkYunJaei());
        }

        //--------------------------------------
        // ** 오브젝트 **
        if (!SwitchManager.Instance.switchdata["DR_callHS"].off && SwitchManager.Instance.switchdata["SecondF_movingSM"].off) {
            Sunwoo.SetActive(true);
            SunwooLogic.Turn("left");
        }
        
        if (SwitchManager.Instance.switchdata["DR_callHS"].off) {  // 지현 퇴원
            Jihyeon.SetActive(false);
            JihyeonList.SetActive(false);
            Hyeongseo.SetActive(false);
            HyeongseoList.SetActive(false);
            Sunwoo.SetActive(false);

            medicalDataForm mdf = comaData.medicalID("456871"); // 지현
            mdf.date.Add("2030.03.07");
            mdf.content.Add("-퇴원");
            mdf = comaData.medicalID("521791"); // 형서
            mdf.date.Add("2030.03.07");
            mdf.content.Add("-퇴원");
        }
        if (SwitchManager.Instance.switchdata["SecondF_SuMin"].on) {
            SwitchManager.Instance.switchdata["SecondF_SuMin"].on = false;
            StartCoroutine(SuMinInfo());
        }
        if (SwitchManager.Instance.switchdata["SecondF_ChaeEun"].on) {
            SwitchManager.Instance.switchdata["SecondF_ChaeEun"].on = false;
            StartCoroutine(ChaeEunInfo());
        }
        if (SwitchManager.Instance.switchdata["SecondF_HyeongSeo"].on) {
            SwitchManager.Instance.switchdata["SecondF_HyeongSeo"].on = false;
            StartCoroutine(HyeongSeoInfo());
        }
        if (SwitchManager.Instance.switchdata["SecondF_JiHyeon"].on) {
            SwitchManager.Instance.switchdata["SecondF_JiHyeon"].on = false;
            StartCoroutine(JiHyeonInfo());
        }
        //--------------------------------------
        // ** 문 **
        if (SwitchManager.Instance.switchdata["SecondF_SunwooRoomDoor"].on) {
            SwitchManager.Instance.switchdata["SecondF_SunwooRoomDoor"].on = false;
            StartCoroutine(CannotEnter(0));
        }
        if (SwitchManager.Instance.switchdata["SecondF_HyeonseokRoomDoor"].on) {
            SwitchManager.Instance.switchdata["SecondF_HyeonseokRoomDoor"].on = false;
            if (SwitchManager.Instance.switchdata["JR_ChkMail"].on && !SwitchManager.Instance.switchdata["HR_ChkCalender"].off) {
                GameManager.Instance.OffPlayer(GameManager.Instance.player);
                GameManager.Instance.ChangeMap(12, "P_Jaei", new Vector3(-14, 4.5f, 0), "left", 1);
            } else 
                StartCoroutine(CannotEnter(10));
        }
    }
#endregion Unity Methods


#region Event
    IEnumerator wakeupJihyeon()
    {
        SwitchManager.Instance.ing = true;
        //=====================================================
        yield return new WaitForSeconds(0.3f);
        JiHyeonAnimator.SetBool("isWake", true);
        //-----------------------------------------------------
        SceneManager.Instance.PlayTalk(20, 200);
        yield return new WaitUntil(() => GameManager.Instance.isTalking == false);
        //-----------------------------------------------------
        JiHyeonAnimator.SetBool("isWake", false);
        //=====================================================
        SwitchManager.Instance.ing = false;
        SwitchManager.Instance.switchdata["A1_End"].off = true;

        SwitchManager.Instance.switchdata["SecondF_mail9"].on = true;
        //-----------------------------------------------------
        StopAllCoroutines();
    }

    IEnumerator walkingSumin()
    {
        SwitchManager.Instance.ing = true;
        GameManager.Instance.dontMove = true;
        //=====================================================
        GameObject Sumin = SceneManager.Instance.PlayNPC("N_Sumin2Coma", spawnPoints[0].position);
        NPCMove SuminLogic = Sumin.GetComponent<NPCMove>();

        SuminLogic.SetWaypoint(2, spawnPoints[0].position, spawnPoints[1].position);
        SuminLogic.isOn = true;
        yield return new WaitUntil(() => SuminLogic.isOff == true);
        SuminLogic.isOff = false;

        playerLogic.Turn("up");
        //-----------------------------------------------------
        SceneManager.Instance.PlayTalk(0, 500);
        yield return new WaitUntil(() => GameManager.Instance.isTalking == false);
        //-----------------------------------------------------
        SuminLogic.SetWaypoint(3, Sumin.transform.position, Sumin.transform.position + new Vector3(0, -3, 0), Sumin.transform.position + new Vector3(-5, -3, 0));
        SuminLogic.isOn = true;
        yield return new WaitUntil(() => SuminLogic.isOff == true);
        SuminLogic.isOff = false;

        Sumin.SetActive(false);
        //-----------------------------------------------------
        playerLogic.Turn("left");

        SceneManager.Instance.PlayTalk(10, 500);
        yield return new WaitUntil(() => GameManager.Instance.isTalking == false);
        //=====================================================
        GameManager.Instance.dontMove = false;
        SwitchManager.Instance.ing = false;
        SwitchManager.Instance.switchdata["SecondF_movingSM"].off = true;
    }

    IEnumerator strangeSumin()
    {
        SwitchManager.Instance.ing = true;
        //=====================================================
        SunwooLogic.Turn("down");
        //-----------------------------------------------------
        SceneManager.Instance.PlayTalk(20, 500);
        yield return new WaitUntil(() => GameManager.Instance.isTalking == false);
        //=====================================================
        SwitchManager.Instance.ing = false;
        SwitchManager.Instance.switchdata["SecondF_strangeSM"].off = true;
        //-----------------------------------------------------
        SwitchManager.Instance.switchdata["SecondF_appearHS"].on = true;
    }

    IEnumerator AppearHyeonSeok()
    {
        SwitchManager.Instance.ing = true;
        //=====================================================
        SunwooLogic.Turn("right");
        yield return new WaitForSeconds(0.1f);
        playerLogic.Turn("right");
        //-----------------------------------------------------
        GameObject HS = SceneManager.Instance.PlayNPC("N_HS_R", spawnPoints[0].position);
        NPCMove HSLogic = HS.GetComponent<NPCMove>();

        GameManager.Instance.main_camera.CameraMoving(HS.transform);
        GameManager.Instance.main_camera.target = HS.transform;

        HSLogic.SetWaypoint(2, HS.transform.position, HS.transform.position + new Vector3(-5, 0, 0));
        HSLogic.isOn = true;
        yield return new WaitUntil(() => HSLogic.isOff == true);
        HSLogic.isOff = false;
        //-----------------------------------------------------
        SceneManager.Instance.PlayTalk(30, 500);
        yield return new WaitUntil(() => GameManager.Instance.isTalking == false);

        yield return new WaitForSeconds(0.3f);
        HSLogic.Turn("up");
        yield return new WaitForSeconds(0.4f);
        HSLogic.Turn("down");
        yield return new WaitForSeconds(0.4f);
        HSLogic.Turn("left");
        //-----------------------------------------------------
        GameManager.Instance.main_camera.CameraMoving(cameraMiddleTarget.transform);
        GameManager.Instance.main_camera.target = cameraMiddleTarget.transform;

        SceneManager.Instance.PlayTalk(40, 500);
        yield return new WaitUntil(() => GameManager.Instance.isTalking == false);
        //-----------------------------------------------------
        GameObject Jiun = SceneManager.Instance.PlayNPC("N_Jiun", spawnPoints[0].position + new Vector3(0, 3, 0));
        NPCMove JiunLogic = Jiun.GetComponent<NPCMove>();
        
        GameManager.Instance.main_camera.target = HS.transform;
        HSLogic.SetWaypoint(2, HS.transform.position, HS.transform.position + new Vector3(2, 0, 0));
        HSLogic.isOn = true;

        JiunLogic.SetWaypoint(2, Jiun.transform.position, spawnPoints[0].position, Jiun.transform.position + new Vector3(-3, 0, 0));
        JiunLogic.isOn = true;
        yield return new WaitUntil(() => JiunLogic.isOff == true);
        JiunLogic.isOff = false;
        HSLogic.isOff = false;

        JiunLogic.Turn("left");
        //-----------------------------------------------------
        SceneManager.Instance.PlayTalk(50, 500);
        yield return new WaitUntil(() => GameManager.Instance.isTalking == false);
        //-----------------------------------------------------
        StartCoroutine(FadeAndChangeMap());

        SwitchManager.Instance.switchdata["DR_callHS"].on = true;
        //=====================================================
        SwitchManager.Instance.ing = false;
        SwitchManager.Instance.switchdata["SecondF_appearHS"].off = true;
        
        HS.SetActive(false);
        Jiun.SetActive(false);
    }

    IEnumerator FadeAndChangeMap()
    {
        GameManager.Instance.currTime = 5;
        GameManager.Instance.fadeEffect.nameStr = "차현석";
        GameManager.Instance.fadeEffect.timeStr = "2030-03-07(목)\n오전  9:50:00";
        GameManager.Instance.fadeEffect.OnFade(FadeState.FadeOut);
        yield return new WaitUntil(() => GameManager.Instance.fadeEffect.endFade == true);
        GameManager.Instance.main_camera.target = player.transform;

        GameManager.Instance.OffPlayer(GameManager.Instance.player);
        GameManager.Instance.ChangeMap(1, "P_HyeonSeok", spawnPoints[0].position, "up", 1);

        GameManager.Instance.inventory.GetComponent<Inventory>().TempClearInven();
        GameManager.Instance.inventory.GetComponent<Inventory>().AddItem(3);
    }

    IEnumerator dischargeJihyeon()
    {
        SwitchManager.Instance.ing = true;
        //=====================================================
        GameObject JH = SceneManager.Instance.PlayNPC("N_Jihyeon", player.transform.position + new Vector3(-1.5f, 0, 0));
        NPCMove JHLogic = JH.GetComponent<NPCMove>();
        JHLogic.Turn("right");
        yield return new WaitForSeconds(0.1f);
        //-----------------------------------------------------
        SceneManager.Instance.PlayTalk(0, 700);
        yield return new WaitUntil(() => GameManager.Instance.isTalking == false);
        //-----------------------------------------------------
        playerLogic.Turn("right");

        JHLogic.SetWaypoint(2, JH.transform.position, JH.transform.position + new Vector3(4, 0, 0));
        JHLogic.isOn = true;
        yield return new WaitUntil(() => JHLogic.isOff == true);
        JHLogic.isOff = false;

        JH.SetActive(false);
        //=====================================================
        SwitchManager.Instance.switchdata["SecondF_mail10"].on = true;

        SwitchManager.Instance.ing = false;
        SwitchManager.Instance.switchdata["SecondF_dischargedJH"].off = true;
        StopAllCoroutines();
    }

    IEnumerator ChkYunJaei()
    {
        SwitchManager.Instance.ing = true;
        //=====================================================
        SceneManager.Instance.PlayTalk(20, 800);
        yield return new WaitUntil(() => GameManager.Instance.isTalking == false);
        //=====================================================
        SwitchManager.Instance.ing = false;
        SwitchManager.Instance.switchdata["DataCom_Chk103015"].off = false;

        Black.SetActive(true);
        yield return new WaitForSeconds(1f);

        SceneManager.Instance.PlayTalk(0, 1000);
        yield return new WaitUntil(() => GameManager.Instance.isTalking == false);

        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
    }

    //--------------------------------------------------------------

    IEnumerator SuMinInfo()
    {
        SwitchManager.Instance.ing = true;

        SceneManager.Instance.PlayObj(0, 100, "UC");
        yield return new WaitUntil(() => GameManager.Instance.isInterAction == false);

        SwitchManager.Instance.ing = false;
    }

    IEnumerator ChaeEunInfo()
    {
        SwitchManager.Instance.ing = true;

        SceneManager.Instance.PlayObj(10, 100, "UC");
        yield return new WaitUntil(() => GameManager.Instance.isInterAction == false);

        SwitchManager.Instance.ing = false;
    }

    IEnumerator HyeongSeoInfo()
    {
        SwitchManager.Instance.ing = true;

        SceneManager.Instance.PlayObj(20, 100, "UC");
        yield return new WaitUntil(() => GameManager.Instance.isInterAction == false);

        SwitchManager.Instance.ing = false;
    }

    IEnumerator JiHyeonInfo()
    {
        SwitchManager.Instance.ing = true;

        SceneManager.Instance.PlayObj(30, 100, "UC");
        yield return new WaitUntil(() => GameManager.Instance.isInterAction == false);

        SwitchManager.Instance.ing = false;
    }

    IEnumerator CannotEnter(int _obj)
    {
        SceneManager.Instance.PlayObj(_obj, 500, "UC");
        yield return new WaitUntil(() => GameManager.Instance.isInterAction == false);
    }
#endregion Event
}

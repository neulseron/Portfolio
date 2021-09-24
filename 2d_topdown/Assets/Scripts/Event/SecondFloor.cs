using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondFloor : MonoBehaviour
{
    public GameManager gameManager;
    public SceneManager sceneManager;
    public SwitchManager switchManager;
    public ComaData comaData;
    GameObject player;
    PlayerAction playerLogic;
    //--------------------------------------
    public GameObject Black;
    public GameObject DataComUI;
    public GameObject tutoMark;
    public Animator JiHyeonAnimator;
    public GameObject mail10;
    public GameObject Jihyeon;
    public GameObject JihyeonList;
    public GameObject Hyeongseo;
    public GameObject HyeongseoList;
    public GameObject cameraMiddleTarget;
    public GameObject Sunwoo;
    NPCMove SunwooLogic;
    //--------------------------------------
    public Transform[] spawnPoints;
    //======================================
    void Start() {
        player = gameManager.player;
        playerLogic = player.GetComponent<PlayerAction>();
        
        SunwooLogic = Sunwoo.GetComponent<NPCMove>();
    }

    void Update() {
        if (switchManager.switchdata["Tutorial"].off && switchManager.switchdata["Tutorial_DataCom"].on && !switchManager.switchdata["Tutorial_OffMarkDataCom"].on) {
            tutoMark.SetActive(true);
        }
        if (switchManager.switchdata["Tutorial_OffMarkDataCom"].on) {
            tutoMark.SetActive(false);
        }

        if (switchManager.switchdata["A1_End"].on) {   
            switchManager.switchdata["A1_End"].on = false;
            StartCoroutine(wakeupJihyeon());
        }

        if (switchManager.switchdata["SecondF_movingSM"].on) {
            switchManager.switchdata["SecondF_movingSM"].on = false;
            StartCoroutine(walkingSumin());
        }

        if (switchManager.switchdata["SecondF_movingSM"].off && switchManager.switchdata["SecondF_strangeSM"].on) {
            switchManager.switchdata["SecondF_strangeSM"].on = false;
            StartCoroutine(strangeSumin());
        }

        if (switchManager.switchdata["SecondF_appearHS"].on) {
            switchManager.switchdata["SecondF_appearHS"].on = false;
            StartCoroutine(AppearHyeonSeok());
        }

        if (switchManager.switchdata["SecondF_dischargedJH"].on) {
            switchManager.switchdata["SecondF_dischargedJH"].on = false;
            StartCoroutine(dischargeJihyeon());
        }

        if (switchManager.switchdata["DataCom_Chk103015"].on && !DataComUI.activeSelf) {
            switchManager.switchdata["DataCom_Chk103015"].on = false;
            StartCoroutine(ChkYunJaei());
        }

        //--------------------------------------
        // ** 오브젝트 **
        if (!switchManager.switchdata["DR_callHS"].off && switchManager.switchdata["SecondF_movingSM"].off) {
            Sunwoo.SetActive(true);
            SunwooLogic.Turn("left");
        }
        
        if (switchManager.switchdata["DR_callHS"].off) {  // 지현 퇴원
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
        if (switchManager.switchdata["SecondF_SuMin"].on) {
            switchManager.switchdata["SecondF_SuMin"].on = false;
            StartCoroutine(SuMinInfo());
        }
        if (switchManager.switchdata["SecondF_ChaeEun"].on) {
            switchManager.switchdata["SecondF_ChaeEun"].on = false;
            StartCoroutine(ChaeEunInfo());
        }
        if (switchManager.switchdata["SecondF_HyeongSeo"].on) {
            switchManager.switchdata["SecondF_HyeongSeo"].on = false;
            StartCoroutine(HyeongSeoInfo());
        }
        if (switchManager.switchdata["SecondF_JiHyeon"].on) {
            switchManager.switchdata["SecondF_JiHyeon"].on = false;
            StartCoroutine(JiHyeonInfo());
        }
        //--------------------------------------
        // ** 문 **
        if (switchManager.switchdata["SecondF_SunwooRoomDoor"].on) {
            switchManager.switchdata["SecondF_SunwooRoomDoor"].on = false;
            StartCoroutine(CannotEnter(0));
        }
        if (switchManager.switchdata["SecondF_HyeonseokRoomDoor"].on) {
            switchManager.switchdata["SecondF_HyeonseokRoomDoor"].on = false;
            if (switchManager.switchdata["JR_ChkMail"].on && !switchManager.switchdata["HR_ChkCalender"].off) {
                gameManager.OffPlayer(gameManager.player);
                gameManager.ChangeMap(12, "P_Jaei", new Vector3(-14, 4.5f, 0), "left", 1);
            } else 
                StartCoroutine(CannotEnter(10));
        }
    }


    IEnumerator wakeupJihyeon()
    {
        switchManager.ing = true;
        //=====================================================
        yield return new WaitForSeconds(0.3f);
        JiHyeonAnimator.SetBool("isWake", true);
        //-----------------------------------------------------
        sceneManager.PlayTalk(20, 200);
        yield return new WaitUntil(() => gameManager.isTalking == false);
        //-----------------------------------------------------
        JiHyeonAnimator.SetBool("isWake", false);
        //=====================================================
        switchManager.ing = false;
        switchManager.switchdata["A1_End"].off = true;

        switchManager.switchdata["SecondF_mail9"].on = true;
        //-----------------------------------------------------
        StopAllCoroutines();
    }

    IEnumerator walkingSumin()
    {
        switchManager.ing = true;
        gameManager.dontMove = true;
        //=====================================================
        GameObject Sumin = sceneManager.PlayNPC("N_Sumin2Coma", spawnPoints[0].position);
        NPCMove SuminLogic = Sumin.GetComponent<NPCMove>();

        SuminLogic.SetWaypoint(2, spawnPoints[0].position, spawnPoints[1].position);
        SuminLogic.isOn = true;
        yield return new WaitUntil(() => SuminLogic.isOff == true);
        SuminLogic.isOff = false;

        playerLogic.Turn("up");
        //-----------------------------------------------------
        sceneManager.PlayTalk(0, 500);
        yield return new WaitUntil(() => gameManager.isTalking == false);
        //-----------------------------------------------------
        SuminLogic.SetWaypoint(3, Sumin.transform.position, Sumin.transform.position + new Vector3(0, -3, 0), Sumin.transform.position + new Vector3(-5, -3, 0));
        SuminLogic.isOn = true;
        yield return new WaitUntil(() => SuminLogic.isOff == true);
        SuminLogic.isOff = false;

        Sumin.SetActive(false);
        //-----------------------------------------------------
        playerLogic.Turn("left");

        sceneManager.PlayTalk(10, 500);
        yield return new WaitUntil(() => gameManager.isTalking == false);
        //=====================================================
        gameManager.dontMove = false;
        switchManager.ing = false;
        switchManager.switchdata["SecondF_movingSM"].off = true;
    }

    IEnumerator strangeSumin()
    {
        switchManager.ing = true;
        //=====================================================
        SunwooLogic.Turn("down");
        //-----------------------------------------------------
        sceneManager.PlayTalk(20, 500);
        yield return new WaitUntil(() => gameManager.isTalking == false);
        //=====================================================
        switchManager.ing = false;
        switchManager.switchdata["SecondF_strangeSM"].off = true;
        //-----------------------------------------------------
        switchManager.switchdata["SecondF_appearHS"].on = true;
    }

    IEnumerator AppearHyeonSeok()
    {
        switchManager.ing = true;
        //=====================================================
        SunwooLogic.Turn("right");
        yield return new WaitForSeconds(0.1f);
        playerLogic.Turn("right");
        //-----------------------------------------------------
        GameObject HS = sceneManager.PlayNPC("N_HS_R", spawnPoints[0].position);
        NPCMove HSLogic = HS.GetComponent<NPCMove>();

        gameManager.main_camera.CameraMoving(HS.transform);
        gameManager.main_camera.target = HS.transform;

        HSLogic.SetWaypoint(2, HS.transform.position, HS.transform.position + new Vector3(-5, 0, 0));
        HSLogic.isOn = true;
        yield return new WaitUntil(() => HSLogic.isOff == true);
        HSLogic.isOff = false;
        //-----------------------------------------------------
        sceneManager.PlayTalk(30, 500);
        yield return new WaitUntil(() => gameManager.isTalking == false);

        yield return new WaitForSeconds(0.3f);
        HSLogic.Turn("up");
        yield return new WaitForSeconds(0.4f);
        HSLogic.Turn("down");
        yield return new WaitForSeconds(0.4f);
        HSLogic.Turn("left");
        //-----------------------------------------------------
        gameManager.main_camera.CameraMoving(cameraMiddleTarget.transform);
        gameManager.main_camera.target = cameraMiddleTarget.transform;

        sceneManager.PlayTalk(40, 500);
        yield return new WaitUntil(() => gameManager.isTalking == false);
        //-----------------------------------------------------
        GameObject Jiun = sceneManager.PlayNPC("N_Jiun", spawnPoints[0].position + new Vector3(0, 3, 0));
        NPCMove JiunLogic = Jiun.GetComponent<NPCMove>();
        
        gameManager.main_camera.target = HS.transform;
        HSLogic.SetWaypoint(2, HS.transform.position, HS.transform.position + new Vector3(2, 0, 0));
        HSLogic.isOn = true;

        JiunLogic.SetWaypoint(2, Jiun.transform.position, spawnPoints[0].position, Jiun.transform.position + new Vector3(-3, 0, 0));
        JiunLogic.isOn = true;
        yield return new WaitUntil(() => JiunLogic.isOff == true);
        JiunLogic.isOff = false;
        HSLogic.isOff = false;

        JiunLogic.Turn("left");
        //-----------------------------------------------------
        sceneManager.PlayTalk(50, 500);
        yield return new WaitUntil(() => gameManager.isTalking == false);
        //-----------------------------------------------------
        StartCoroutine(FadeAndChangeMap());

        switchManager.switchdata["DR_callHS"].on = true;
        //=====================================================
        switchManager.ing = false;
        switchManager.switchdata["SecondF_appearHS"].off = true;
        
        HS.SetActive(false);
        Jiun.SetActive(false);
    }

    IEnumerator FadeAndChangeMap()
    {
        gameManager.currTime = 5;
        gameManager.fadeEffect.nameStr = "차현석";
        gameManager.fadeEffect.timeStr = "2030-03-07(목)\n오전  9:50:00";
        gameManager.fadeEffect.OnFade(FadeState.FadeOut);
        yield return new WaitUntil(() => gameManager.fadeEffect.endFade == true);
        gameManager.main_camera.target = player.transform;

        gameManager.OffPlayer(gameManager.player);
        gameManager.ChangeMap(1, "P_HyeonSeok", spawnPoints[0].position, "up", 1);

        gameManager.inventory.GetComponent<Inventory>().TempClearInven();
        gameManager.inventory.GetComponent<Inventory>().AddItem(3);
    }

    IEnumerator dischargeJihyeon()
    {
        switchManager.ing = true;
        //=====================================================
        GameObject JH = sceneManager.PlayNPC("N_Jihyeon", player.transform.position + new Vector3(-1.5f, 0, 0));
        NPCMove JHLogic = JH.GetComponent<NPCMove>();
        JHLogic.Turn("right");
        yield return new WaitForSeconds(0.1f);
        //-----------------------------------------------------
        sceneManager.PlayTalk(0, 700);
        yield return new WaitUntil(() => gameManager.isTalking == false);
        //-----------------------------------------------------
        playerLogic.Turn("right");

        JHLogic.SetWaypoint(2, JH.transform.position, JH.transform.position + new Vector3(4, 0, 0));
        JHLogic.isOn = true;
        yield return new WaitUntil(() => JHLogic.isOff == true);
        JHLogic.isOff = false;

        JH.SetActive(false);
        //=====================================================
        switchManager.switchdata["SecondF_mail10"].on = true;

        switchManager.ing = false;
        switchManager.switchdata["SecondF_dischargedJH"].off = true;
        StopAllCoroutines();
    }

    IEnumerator ChkYunJaei()
    {
        switchManager.ing = true;
        //=====================================================
        sceneManager.PlayTalk(20, 800);
        yield return new WaitUntil(() => gameManager.isTalking == false);
        //=====================================================
        switchManager.ing = false;
        switchManager.switchdata["DataCom_Chk103015"].off = false;

        Black.SetActive(true);
        yield return new WaitForSeconds(1f);

        sceneManager.PlayTalk(0, 1000);
        yield return new WaitUntil(() => gameManager.isTalking == false);

        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");

    }

    //--------------------------------------------------------------

    IEnumerator SuMinInfo()
    {
        switchManager.ing = true;

        sceneManager.PlayObj(0, 100, "UC");
        yield return new WaitUntil(() => gameManager.isInterAction == false);

        switchManager.ing = false;
    }

    IEnumerator ChaeEunInfo()
    {
        switchManager.ing = true;

        sceneManager.PlayObj(10, 100, "UC");
        yield return new WaitUntil(() => gameManager.isInterAction == false);

        switchManager.ing = false;
    }

    IEnumerator HyeongSeoInfo()
    {
        switchManager.ing = true;

        sceneManager.PlayObj(20, 100, "UC");
        yield return new WaitUntil(() => gameManager.isInterAction == false);

        switchManager.ing = false;
    }

    IEnumerator JiHyeonInfo()
    {
        switchManager.ing = true;

        sceneManager.PlayObj(30, 100, "UC");
        yield return new WaitUntil(() => gameManager.isInterAction == false);

        switchManager.ing = false;
    }

    IEnumerator CannotEnter(int _obj)
    {
        sceneManager.PlayObj(_obj, 500, "UC");
        yield return new WaitUntil(() => gameManager.isInterAction == false);
    }
}

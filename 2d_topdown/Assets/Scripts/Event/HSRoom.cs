using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HSRoom : MonoBehaviour
{
    public GameManager gameManager;
    public SceneManager sceneManager;
    public SwitchManager switchManager;
    GameObject player;
    PlayerAction playerLogic;
    //--------------------------------------
    //--------------------------------------
    public Transform[] spawnPoints;
    //======================================

    void Start() {
        player = gameManager.player;
        playerLogic = player.GetComponent<PlayerAction>();
    }

    void Update() {
        if (switchManager.switchdata["JR_ChkMail"].on) {
            switchManager.switchdata["JR_ChkMail"].on = false;
            StartCoroutine(ChkMail());
        }

        if (switchManager.switchdata["JR_ChkMail"].off && switchManager.switchdata["HR_ChkCalender"].on) {
            switchManager.switchdata["HR_ChkCalender"].on = false;
            StartCoroutine(ChkCalender());
        }
        //-----------------------------------------------------
        if (switchManager.switchdata["HR_Calender"].on) {
            switchManager.switchdata["HR_Calender"].on = false;
            StartCoroutine(Calender());
        }
        if (switchManager.switchdata["HR_Album"].on) {
            switchManager.switchdata["HR_Album"].on = false;
            StartCoroutine(Album());
        }
    }

    IEnumerator ChkMail()
    {
        switchManager.ing = true;
        //=====================================================
        sceneManager.PlayTalk(0, 900);
        yield return new WaitUntil(() => gameManager.isTalking == false);
        //=====================================================
        switchManager.ing = false;
        switchManager.switchdata["JR_ChkMail"].off = true;
    }

    IEnumerator ChkCalender()
    {
        switchManager.ing = true;
        //=====================================================
        sceneManager.PlayTalk(10, 900);
        yield return new WaitUntil(() => gameManager.isTalking == false);
        //-----------------------------------------------------
        GameObject HS = sceneManager.PlayNPC("N_HS_R", spawnPoints[0].position);
        NPCMove HSLogic = HS.GetComponent<NPCMove>();

        playerLogic.Turn("right");

        HSLogic.SetWaypoint(2, HS.transform.position, HS.transform.position + new Vector3(-3, 0, 0));
        HSLogic.isOn = true;
        yield return new WaitUntil(() => HSLogic.isOff == true);
        HSLogic.isOff = false;
        //-----------------------------------------------------
        sceneManager.PlayTalk(20, 900);
        yield return new WaitUntil(() => gameManager.isTalking == false);
        //-----------------------------------------------------
        StartCoroutine(FadeAndChangeMap());
        //=====================================================
        switchManager.ing = false;
        switchManager.switchdata["HR_ChkCalender"].off = true;
        HS.SetActive(false);
    }
    
    IEnumerator FadeAndChangeMap()
    {
        gameManager.currTime = 7;
        gameManager.fadeEffect.nameStr = "차현석";
        gameManager.fadeEffect.timeStr = "2030-03-07(목)\n오후  4:52:00";
        gameManager.fadeEffect.OnFade(FadeState.FadeOut);
        yield return new WaitUntil(() => gameManager.fadeEffect.endFade == true);

        gameManager.OffPlayer(gameManager.player);
        gameManager.ChangeMap(1, "P_HyeonSeok", new Vector3(-10, 4.5f, 0), "right", 1);

        gameManager.inventory.GetComponent<Inventory>().TempClearInven();
        gameManager.inventory.GetComponent<Inventory>().AddItem(3);
    }




    IEnumerator Calender()
    {
        switchManager.ing = true;
        //=====================================================
        sceneManager.PlayObj(0, 400);
        yield return new WaitUntil(() => gameManager.isInterAction == false);

        if (!switchManager.switchdata["HR_ChkCalender"].off) {
            switchManager.switchdata["HR_ChkCalender"].on = true;
        }
        //=====================================================
        switchManager.ing = false;
    }

    IEnumerator Album()
    {
        switchManager.ing = true;
        //=====================================================
        sceneManager.PlayObj(10, 400);
        yield return new WaitUntil(() => gameManager.isInterAction == false);
        //=====================================================
        switchManager.ing = false;
    }

}

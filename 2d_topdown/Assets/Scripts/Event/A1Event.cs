using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A1Event : MonoBehaviour
{
    public GameManager gameManager;
    public SceneManager sceneManager;
    public SwitchManager switchManager;
    public ComaData comaData;
    GameObject player;
    PlayerAction playerLogic;
    //--------------------------------------
    public GameObject IdleDoor;
    public GameObject Door;
    //--------------------------------------
    public Transform[] spawnPoints;
    //======================================
    void Start() {
        player = gameManager.player;
        playerLogic = player.GetComponent<PlayerAction>();
    }

    void Update() {
        if (switchManager.switchdata["A1_Closet"].on) {
            switchManager.switchdata["A1_Closet"].on = false;
            StartCoroutine(SM2Appear());
        }

        if (switchManager.switchdata["A1Maze_InitTalk"].off) {
            switchManager.switchdata["DataCom_JHsearchEnd"].on = true;
        }
    }

    IEnumerator SM2Appear()
    {
        switchManager.ing = true;
        //=====================================================x
        playerLogic.SetWaypoint(2, player.transform.position, player.transform.position + new Vector3(0, -2, 0));
        playerLogic.isOn = true;
        yield return new WaitUntil(() => playerLogic.isOff == true);
        playerLogic.isOff = false;
        playerLogic.Turn("up");
        //-----------------------------------------------------
        GameObject Jihyeon = sceneManager.PlayNPC("N_Jihyeon", spawnPoints[0].position);
        NPCMove JihyeonLogic = Jihyeon.GetComponent<NPCMove>();
        JihyeonLogic.Turn("down");
        //-----------------------------------------------------
        IdleDoor.SetActive(false);
        Door.SetActive(true);
        //-----------------------------------------------------
        sceneManager.PlayTalk(10, 200);
        yield return new WaitUntil(() => gameManager.isTalking == false);
        //-----------------------------------------------------
        while(gameManager.scanObj.name != "Door") {
            JihyeonLogic.speed = 3.5f;
            JihyeonLogic.SetWaypoint(2, Jihyeon.transform.position, player.transform.position + new Vector3(0, 1, 0));
            JihyeonLogic.isOn = true;
            yield return new WaitUntil(() => JihyeonLogic.isOff == true);
            JihyeonLogic.isOff = false;
        }
        JihyeonLogic.speed = 2.3f;
        //-----------------------------------------------------
        switchManager.ing = false;
        StartCoroutine(FadeAndChangeMap());
        //=====================================================x
        switchManager.switchdata["A1_End"].on = true;
        Jihyeon.SetActive(false);
        //-----------------------------------------------------
        switchManager.switchdata["A1_Closet"].off = true;
    }

    IEnumerator FadeAndChangeMap()
    {
        gameManager.currTime = 2;
        gameManager.fadeEffect.nameStr = "서재이";
        gameManager.fadeEffect.timeStr = "2030-03-06(수)\n오후  5:13:00";
        gameManager.fadeEffect.OnFade(FadeState.FadeOut);
        yield return new WaitUntil(() => gameManager.fadeEffect.endFade == true);

        gameManager.OffPlayer(gameManager.player);
        gameManager.ChangeMap(1, "P_Jaei", new Vector3(3.6f, 16.93f, 0), "left", 1);
    }

}

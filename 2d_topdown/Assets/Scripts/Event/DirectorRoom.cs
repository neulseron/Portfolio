using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectorRoom : MonoBehaviour
{
    public GameManager gameManager;
    public SceneManager sceneManager;
    public SwitchManager switchManager;
    GameObject player;
    PlayerAction playerLogic;
    //--------------------------------------
    public GameObject Director;
    NPCMove DirectorLogic;
    //--------------------------------------
    public Transform[] spawnPoints;
    //======================================s

    void Start() {
        player = gameManager.player;
        playerLogic = player.GetComponent<PlayerAction>();

        DirectorLogic = Director.GetComponent<NPCMove>();
        DirectorLogic.Turn("left");
    }

    void Update() {
        if (switchManager.switchdata["DR_callHS"].on) {
            switchManager.switchdata["DR_callHS"].on = false;
            StartCoroutine(CallHyeonSeok());
        }
    }

    IEnumerator CallHyeonSeok()
    {
        switchManager.ing = true;
        //=====================================================
        playerLogic.SetWaypoint(2, player.transform.position, player.transform.position + new Vector3(1, 0, 0));
        playerLogic.isOn = true;
        yield return new WaitUntil(() => playerLogic.isOff == true);
        playerLogic.isOff = false;
        //-----------------------------------------------------
        sceneManager.PlayTalk(0, 600);
        yield return new WaitUntil(() => gameManager.isTalking == false);
        //-----------------------------------------------------
        player.GetComponent<CapsuleCollider2D>().isTrigger = true;
        playerLogic.SetWaypoint(2, player.transform.position, player.transform.position + new Vector3(1.5f, 0, 0));
        playerLogic.isOn = true;
        yield return new WaitUntil(() => playerLogic.isOff == true);
        playerLogic.isOff = false;

        sceneManager.PlayTalk(10, 600);
        yield return new WaitUntil(() => gameManager.isTalking == false);
        //-----------------------------------------------------
        /*
        player.GetComponent<CapsuleCollider2D>().isTrigger = true;
        playerLogic.SetWaypoint(2, player.transform.position, player.transform.position + new Vector3(-1, 0, 0));
        playerLogic.isOn = true;
        yield return new WaitUntil(() => playerLogic.isOff == true);
        playerLogic.isOff = false;

        sceneManager.PlayTalk(20, 600);
        yield return new WaitUntil(() => gameManager.isTalking == false);
        //-----------------------------------------------------
*/
        player.GetComponent<CapsuleCollider2D>().isTrigger = false;
        StartCoroutine(FadeAndChangeMap());
        //=====================================================
        switchManager.switchdata["SecondF_dischargedJH"].on = true;

        switchManager.ing = false;
        switchManager.switchdata["DR_callHS"].off = true;
    }

    IEnumerator FadeAndChangeMap()
    {
        gameManager.currTime = 6;
        gameManager.fadeEffect.nameStr = "서재이";
        gameManager.fadeEffect.timeStr = "2030-03-07(목)\n오후  1:40:00";
        gameManager.fadeEffect.OnFade(FadeState.FadeOut);
        yield return new WaitUntil(() => gameManager.fadeEffect.endFade == true);

        gameManager.OffPlayer(gameManager.player);
        gameManager.ChangeMap(1, "P_Jaei", new Vector3(3.6f, 15f, 0), "left", 1);

        gameManager.inventory.GetComponent<Inventory>().TempClearInven();
        gameManager.inventory.GetComponent<Inventory>().InitInven(DataManager.Instance.gameData.itemList);
    }
}

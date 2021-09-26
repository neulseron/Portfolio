using System.Collections;
using UnityEngine;

public class A1Event : MapEvent
{
#region Variables
    public GameObject IdleDoor;
    public GameObject Door;

    public Transform[] spawnPoints;
#endregion Variables


#region Unity Methods
    void Update() {
        if (SwitchManager.Instance.switchdata["A1_Closet"].on) {
            SwitchManager.Instance.switchdata["A1_Closet"].on = false;
            StartCoroutine(SM2Appear());
        }

        if (SwitchManager.Instance.switchdata["A1Maze_InitTalk"].off) {
            SwitchManager.Instance.switchdata["DataCom_JHsearchEnd"].on = true;
        }
    }
#endregion Unity Methods


#region Event
    IEnumerator SM2Appear()
    {
        SwitchManager.Instance.ing = true;
        //=====================================================x
        playerLogic.SetWaypoint(2, player.transform.position, player.transform.position + new Vector3(0, -2, 0));
        playerLogic.isOn = true;
        yield return new WaitUntil(() => playerLogic.isOff == true);
        playerLogic.isOff = false;
        playerLogic.Turn("up");
        //-----------------------------------------------------
        GameObject Jihyeon = SceneManager.Instance.PlayNPC("N_Jihyeon", spawnPoints[0].position);
        NPCMove JihyeonLogic = Jihyeon.GetComponent<NPCMove>();
        JihyeonLogic.Turn("down");
        //-----------------------------------------------------
        IdleDoor.SetActive(false);
        Door.SetActive(true);
        //-----------------------------------------------------
        SceneManager.Instance.PlayTalk(10, 200);
        yield return new WaitUntil(() => GameManager.Instance.isTalking == false);
        //-----------------------------------------------------
        while(GameManager.Instance.scanObj.name != "Door") {
            JihyeonLogic.speed = 3.5f;
            JihyeonLogic.SetWaypoint(2, Jihyeon.transform.position, player.transform.position + new Vector3(0, 1, 0));
            JihyeonLogic.isOn = true;
            yield return new WaitUntil(() => JihyeonLogic.isOff == true);
            JihyeonLogic.isOff = false;
        }
        JihyeonLogic.speed = 2.3f;
        //-----------------------------------------------------
        SwitchManager.Instance.ing = false;
        StartCoroutine(FadeAndChangeMap());
        //=====================================================x
        SwitchManager.Instance.switchdata["A1_End"].on = true;
        Jihyeon.SetActive(false);
        //-----------------------------------------------------
        SwitchManager.Instance.switchdata["A1_Closet"].off = true;
    }

    IEnumerator FadeAndChangeMap()
    {
        GameManager.Instance.currTime = 2;
        GameManager.Instance.fadeEffect.nameStr = "서재이";
        GameManager.Instance.fadeEffect.timeStr = "2030-03-06(수)\n오후  5:13:00";
        GameManager.Instance.fadeEffect.OnFade(FadeState.FadeOut);
        yield return new WaitUntil(() => GameManager.Instance.fadeEffect.endFade == true);

        GameManager.Instance.OffPlayer(GameManager.Instance.player);
        GameManager.Instance.ChangeMap(1, "P_Jaei", new Vector3(3.6f, 16.93f, 0), "left", 1);
    }
#endregion Event
}

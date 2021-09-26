using System.Collections;
using UnityEngine;

public class DirectorRoom : MapEvent
{
#region Variables
    public GameObject Director;
    NPCMove DirectorLogic;
#endregion Variables


#region Unity Methods
    protected override void Start() {
        base.Start();

        DirectorLogic = Director.GetComponent<NPCMove>();
        DirectorLogic.Turn("left");
    }

    void Update() {
        if (SwitchManager.Instance.switchdata["DR_callHS"].on) {
            SwitchManager.Instance.switchdata["DR_callHS"].on = false;
            StartCoroutine(CallHyeonSeok());
        }
    }
#endregion Unity Methods


#region Event
    IEnumerator CallHyeonSeok()
    {
        SwitchManager.Instance.ing = true;
        //=====================================================
        playerLogic.SetWaypoint(2, player.transform.position, player.transform.position + new Vector3(1, 0, 0));
        playerLogic.isOn = true;
        yield return new WaitUntil(() => playerLogic.isOff == true);
        playerLogic.isOff = false;
        //-----------------------------------------------------
        SceneManager.Instance.PlayTalk(0, 600);
        yield return new WaitUntil(() => GameManager.Instance.isTalking == false);
        //-----------------------------------------------------
        player.GetComponent<CapsuleCollider2D>().isTrigger = true;
        playerLogic.SetWaypoint(2, player.transform.position, player.transform.position + new Vector3(1.5f, 0, 0));
        playerLogic.isOn = true;
        yield return new WaitUntil(() => playerLogic.isOff == true);
        playerLogic.isOff = false;

        SceneManager.Instance.PlayTalk(10, 600);
        yield return new WaitUntil(() => GameManager.Instance.isTalking == false);
        //-----------------------------------------------------
        player.GetComponent<CapsuleCollider2D>().isTrigger = false;
        StartCoroutine(FadeAndChangeMap());
        //=====================================================
        SwitchManager.Instance.switchdata["SecondF_dischargedJH"].on = true;

        SwitchManager.Instance.ing = false;
        SwitchManager.Instance.switchdata["DR_callHS"].off = true;
    }

    IEnumerator FadeAndChangeMap()
    {
        GameManager.Instance.currTime = 6;
        GameManager.Instance.fadeEffect.nameStr = "서재이";
        GameManager.Instance.fadeEffect.timeStr = "2030-03-07(목)\n오후  1:40:00";
        GameManager.Instance.fadeEffect.OnFade(FadeState.FadeOut);
        yield return new WaitUntil(() => GameManager.Instance.fadeEffect.endFade == true);

        GameManager.Instance.OffPlayer(GameManager.Instance.player);
        GameManager.Instance.ChangeMap(1, "P_Jaei", new Vector3(3.6f, 15f, 0), "left", 1);

        GameManager.Instance.inventory.GetComponent<Inventory>().TempClearInven();
        GameManager.Instance.inventory.GetComponent<Inventory>().InitInven(DataManager.Instance.gameData.itemList);
    }
#endregion Event
}

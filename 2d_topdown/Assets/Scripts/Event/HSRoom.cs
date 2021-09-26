using System.Collections;
using UnityEngine;

public class HSRoom : MapEvent
{
#region Variables
    public Transform[] spawnPoints;
#endregion Variables


#region Unity Methods
    void Update() {
        if (SwitchManager.Instance.switchdata["JR_ChkMail"].on) {
            SwitchManager.Instance.switchdata["JR_ChkMail"].on = false;
            StartCoroutine(ChkMail());
        }

        if (SwitchManager.Instance.switchdata["JR_ChkMail"].off && SwitchManager.Instance.switchdata["HR_ChkCalender"].on) {
            SwitchManager.Instance.switchdata["HR_ChkCalender"].on = false;
            StartCoroutine(ChkCalender());
        }
        //-----------------------------------------------------
        if (SwitchManager.Instance.switchdata["HR_Calender"].on) {
            SwitchManager.Instance.switchdata["HR_Calender"].on = false;
            StartCoroutine(Calender());
        }
        if (SwitchManager.Instance.switchdata["HR_Album"].on) {
            SwitchManager.Instance.switchdata["HR_Album"].on = false;
            StartCoroutine(Album());
        }
    }
#endregion Unity Methods


#region Event
    IEnumerator ChkMail()
    {
        SwitchManager.Instance.ing = true;
        //=====================================================
        SceneManager.Instance.PlayTalk(0, 900);
        yield return new WaitUntil(() => GameManager.Instance.isTalking == false);
        //=====================================================
        SwitchManager.Instance.ing = false;
        SwitchManager.Instance.switchdata["JR_ChkMail"].off = true;
    }

    IEnumerator ChkCalender()
    {
        SwitchManager.Instance.ing = true;
        //=====================================================
        SceneManager.Instance.PlayTalk(10, 900);
        yield return new WaitUntil(() => GameManager.Instance.isTalking == false);
        //-----------------------------------------------------
        GameObject HS = SceneManager.Instance.PlayNPC("N_HS_R", spawnPoints[0].position);
        NPCMove HSLogic = HS.GetComponent<NPCMove>();

        playerLogic.Turn("right");

        HSLogic.SetWaypoint(2, HS.transform.position, HS.transform.position + new Vector3(-3, 0, 0));
        HSLogic.isOn = true;
        yield return new WaitUntil(() => HSLogic.isOff == true);
        HSLogic.isOff = false;
        //-----------------------------------------------------
        SceneManager.Instance.PlayTalk(20, 900);
        yield return new WaitUntil(() => GameManager.Instance.isTalking == false);
        //-----------------------------------------------------
        StartCoroutine(FadeAndChangeMap());
        //=====================================================
        SwitchManager.Instance.ing = false;
        SwitchManager.Instance.switchdata["HR_ChkCalender"].off = true;
        HS.SetActive(false);
    }
    
    IEnumerator FadeAndChangeMap()
    {
        GameManager.Instance.currTime = 7;
        GameManager.Instance.fadeEffect.nameStr = "차현석";
        GameManager.Instance.fadeEffect.timeStr = "2030-03-07(목)\n오후  4:52:00";
        GameManager.Instance.fadeEffect.OnFade(FadeState.FadeOut);
        yield return new WaitUntil(() => GameManager.Instance.fadeEffect.endFade == true);

        GameManager.Instance.OffPlayer(GameManager.Instance.player);
        GameManager.Instance.ChangeMap(1, "P_HyeonSeok", new Vector3(-10, 4.5f, 0), "right", 1);

        GameManager.Instance.inventory.GetComponent<Inventory>().TempClearInven();
        GameManager.Instance.inventory.GetComponent<Inventory>().AddItem(3);
    }

    IEnumerator Calender()
    {
        SwitchManager.Instance.ing = true;
        //=====================================================
        SceneManager.Instance.PlayObj(0, 400);
        yield return new WaitUntil(() => GameManager.Instance.isInterAction == false);

        if (!SwitchManager.Instance.switchdata["HR_ChkCalender"].off) {
            SwitchManager.Instance.switchdata["HR_ChkCalender"].on = true;
        }
        //=====================================================
        SwitchManager.Instance.ing = false;
    }

    IEnumerator Album()
    {
        SwitchManager.Instance.ing = true;
        //=====================================================
        SceneManager.Instance.PlayObj(10, 400);
        yield return new WaitUntil(() => GameManager.Instance.isInterAction == false);
        //=====================================================
        SwitchManager.Instance.ing = false;
    }
#endregion Event
}

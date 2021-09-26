using System.Collections;
using UnityEngine;

public class ThirdFloor : MapEvent
{
#region Variables
    public GameObject tutoMark;
    public GameObject SearchingSW;
    public GameObject SearchingJU;
#endregion Variables


#region Unity Methods
    void Update() {
        if (SwitchManager.Instance.switchdata["Tutorial"].off && SwitchManager.Instance.switchdata["Tutorial_SearchCom"].on && !SwitchManager.Instance.switchdata["Tutorial_OffMarkSearchCom"].off) {
            tutoMark.SetActive(true);
        }
        if (SwitchManager.Instance.switchdata["Tutorial_OffMarkSearchCom"].on) {
            tutoMark.SetActive(false);
        }
        //--------------------------------------
        if (!SwitchManager.Instance.switchdata["SecondF_movingSM"].off && SwitchManager.Instance.switchdata["JR_callDr"].off) {
            SearchingSW.SetActive(true);
        } else {
            SearchingSW.SetActive(false);
            SearchingJU.SetActive(false);
        }

        if (SwitchManager.Instance.switchdata["ThirdF_CabinetHyeonSeok"].on) {
            SwitchManager.Instance.switchdata["ThirdF_CabinetHyeonSeok"].on = false;
            StartCoroutine(CabinetHS());
        }
        if (SwitchManager.Instance.switchdata["ThirdF_CabinetJiun"].on) {
            SwitchManager.Instance.switchdata["ThirdF_CabinetJiun"].on = false;
            StartCoroutine(CabinetJU());
        }
        if (SwitchManager.Instance.switchdata["ThirdF_CabinetJiEun"].on) {
            SwitchManager.Instance.switchdata["ThirdF_CabinetJiEun"].on = false;
            StartCoroutine(CabinetJE());
        }
        if (SwitchManager.Instance.switchdata["ThirdF_CabinetJaeHa"].on) {
            SwitchManager.Instance.switchdata["ThirdF_CabinetJaeHa"].on = false;
            StartCoroutine(CabinetJH());
        }
        if (SwitchManager.Instance.switchdata["ThirdF_CabinetSuHyeon"].on) {
            SwitchManager.Instance.switchdata["ThirdF_CabinetSuHyeon"].on = false;
            StartCoroutine(CabinetSH());
        }
        if (SwitchManager.Instance.switchdata["ThirdF_CabinetArum"].on) {
            SwitchManager.Instance.switchdata["ThirdF_CabinetArum"].on = false;
            StartCoroutine(CabinetAR());
        }
        if (SwitchManager.Instance.switchdata["ThirdF_CabinetYeonu"].on) {
            SwitchManager.Instance.switchdata["ThirdF_CabinetYeonu"].on = false;
            StartCoroutine(CabinetYU());
        }
        if (SwitchManager.Instance.switchdata["ThirdF_CabinetJihyeong"].on) {
            SwitchManager.Instance.switchdata["ThirdF_CabinetJihyeong"].on = false;
            StartCoroutine(CabinetJiHyeong());
        }


        if (SwitchManager.Instance.switchdata["ThirdF_SearchingJiun"].on) {
            SwitchManager.Instance.switchdata["ThirdF_SearchingJiun"].on = false;
            StartCoroutine(SearchingJiUn());
        }
        if (SwitchManager.Instance.switchdata["ThirdF_SearchingSunwoo"].on) {
            SwitchManager.Instance.switchdata["ThirdF_SearchingSunwoo"].on = false;
            StartCoroutine(SearchingSunWoo());
        }
        //--------------------------------------
        if (SwitchManager.Instance.switchdata["ThirdF_DirectRoomDoor"].on) {
            SwitchManager.Instance.switchdata["ThirdF_DirectRoomDoor"].on = false;
            if (SwitchManager.Instance.switchdata["SecondF_appearHS"].off && !SwitchManager.Instance.switchdata["DR_callHS"].off) {
                GameManager.Instance.OffPlayer(GameManager.Instance.player);
                GameManager.Instance.ChangeMap(3, "P_HyeonSeok", new Vector3(20, 14.5f, 0), "right", 2);
            } else 
                StartCoroutine(CannotEnter(20));
        }
        if (SwitchManager.Instance.switchdata["ThirdF_MeetingRoomDoor"].on) {
            SwitchManager.Instance.switchdata["ThirdF_MeetingRoomDoor"].on = false;
            StartCoroutine(CannotEnter(30));
        }
        if (SwitchManager.Instance.switchdata["ThirdF_EmptyRoomDoor"].on) {
            SwitchManager.Instance.switchdata["ThirdF_EmptyRoomDoor"].on = false;
            StartCoroutine(CannotEnter(40));
        }
    }
#endregion Unity Methods


#region Event
    IEnumerator CabinetHS()
    {
        SwitchManager.Instance.ing = true;
        //=====================================================x
        SceneManager.Instance.PlayObj(0, 300, "UC");
        yield return new WaitUntil(() => GameManager.Instance.isInterAction == false);
        //=====================================================x
        SwitchManager.Instance.ing = false;
    }

    IEnumerator CabinetJU()
    {
        SwitchManager.Instance.ing = true;

        SceneManager.Instance.PlayObj(10, 300, "UC");
        yield return new WaitUntil(() => GameManager.Instance.isInterAction == false);

        SwitchManager.Instance.ing = false;
    }

    IEnumerator CabinetJE()
    {
        SwitchManager.Instance.ing = true;

        SceneManager.Instance.PlayObj(20, 300, "UC");
        yield return new WaitUntil(() => GameManager.Instance.isInterAction == false);

        SwitchManager.Instance.ing = false;
    }

    IEnumerator CabinetJH()
    {
        SwitchManager.Instance.ing = true;
        //=====================================================x
        SceneManager.Instance.PlayObj(30, 300, "UC");
        yield return new WaitUntil(() => GameManager.Instance.isInterAction == false);
        //-----------------------------------------------------
        if (!GameManager.Instance.inventory.GetComponent<Inventory>().havingItem(2)) {
            GameManager.Instance.inventory.GetComponent<Inventory>().AddItem(2);
            GameManager.Instance.currSceneIndex = 300;
            GameManager.Instance.currCutIndex = 20;
            GameManager.Instance.SystemAction();
        }
        //=====================================================x
        SwitchManager.Instance.ing = false;
    }

    IEnumerator CabinetSH()
    {
        SwitchManager.Instance.ing = true;

        SceneManager.Instance.PlayObj(40, 300, "UC");
        yield return new WaitUntil(() => GameManager.Instance.isInterAction == false);

        SwitchManager.Instance.ing = false;
    }

    IEnumerator CabinetAR()
    {
        SwitchManager.Instance.ing = true;

        SceneManager.Instance.PlayObj(50, 300, "UC");
        yield return new WaitUntil(() => GameManager.Instance.isInterAction == false);

        SwitchManager.Instance.ing = false;
    }

    IEnumerator CabinetYU()
    {
        SwitchManager.Instance.ing = true;

        SceneManager.Instance.PlayObj(60, 300, "UC");
        yield return new WaitUntil(() => GameManager.Instance.isInterAction == false);

        SwitchManager.Instance.ing = false;
    }

    IEnumerator CabinetJiHyeong()
    {
        SwitchManager.Instance.ing = true;

        SceneManager.Instance.PlayObj(70, 300, "UC");
        yield return new WaitUntil(() => GameManager.Instance.isInterAction == false);

        SwitchManager.Instance.ing = false;
    }

    IEnumerator SearchingJiUn()
    {
        SwitchManager.Instance.ing = true;

        SceneManager.Instance.PlayTalk(10, 100);
        yield return new WaitUntil(() => GameManager.Instance.isTalking == false);

        SwitchManager.Instance.ing = false;
    }

    IEnumerator SearchingSunWoo()
    {
        SwitchManager.Instance.ing = true;

        SceneManager.Instance.PlayTalk(20, 100);
        yield return new WaitUntil(() => GameManager.Instance.isTalking == false);

        SwitchManager.Instance.ing = false;
    }

    IEnumerator CannotEnter(int _obj)
    {
        SceneManager.Instance.PlayObj(_obj, 500, "UC");
        yield return new WaitUntil(() => GameManager.Instance.isInterAction == false);
    }
#endregion Event
}

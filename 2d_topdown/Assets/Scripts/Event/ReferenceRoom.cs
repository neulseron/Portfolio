using System.Collections;
using UnityEngine;

public class ReferenceRoom : MapEvent
{
#region Variables
    public GameObject screenUI;
    public GameObject rightPerson;
#endregion Variables


#region Unity Methods
    void Update() {
        if (SwitchManager.Instance.switchdata["RR_Bookshelf"].on || SwitchManager.Instance.switchdata["RR_Bookshelf2"].on) {
            SwitchManager.Instance.switchdata["RR_Bookshelf"].on = false;
            SwitchManager.Instance.switchdata["RR_Bookshelf2"].on = false;
            StartCoroutine(BookShelf());
        }

        if (SwitchManager.Instance.switchdata["RR_rightScreen"].on) {
            SwitchManager.Instance.switchdata["RR_rightScreen"].on = false;
            screenOn();
        }

        if (!SwitchManager.Instance.switchdata["A1_End"].off && SwitchManager.Instance.switchdata["RR_Bookshelf3"].on) {
            SwitchManager.Instance.switchdata["RR_Bookshelf3"].on = false;
            StartCoroutine(BookShelf3());
        } else if (SwitchManager.Instance.switchdata["A1_End"].off && SwitchManager.Instance.switchdata["RR_Bookshelf3"].on) {
            SwitchManager.Instance.switchdata["RR_Bookshelf3"].on = false;
            StartCoroutine(BookShelf3_2());
        }

        if (SwitchManager.Instance.switchdata["RR_leftUsing"].on) {
            SwitchManager.Instance.switchdata["RR_leftUsing"].on = false;
            StartCoroutine(LeftCantGo());
        }

        if (SwitchManager.Instance.switchdata["A1_End"].off) {
            rightPerson.SetActive(false);
        }

        if (!SwitchManager.Instance.switchdata["A1_End"].off && SwitchManager.Instance.switchdata["RR_rightUsing"].on) {
            SwitchManager.Instance.switchdata["RR_rightUsing"].on = false;
            StartCoroutine(RightCantGo());
        }
    }
#endregion Unity Methods


#region Event
    IEnumerator BookShelf()
    {
        SwitchManager.Instance.ing = true;
        //=====================================================
        GameManager.Instance.isSelecting = true;
        SceneManager.Instance.SetSelectTxt();
        SceneManager.Instance.SetSelectBtnTxt("수색대 기초 서적", "정신질환의 진단 및 통계", "구역 매뉴얼", "");
        yield return new WaitUntil(() => GameManager.Instance.selectIndex != -1);

        switch (GameManager.Instance.selectIndex) {
            case 1:
                GameManager.Instance.selectBox.SetActive(false);
                SceneManager.Instance.PlayObj(0, 600);
                yield return new WaitUntil(() => GameManager.Instance.isInterAction == false);
                break;
            case 2:
                GameManager.Instance.selectBox.SetActive(false);
                SceneManager.Instance.PlayObj(10, 600);
                yield return new WaitUntil(() => GameManager.Instance.isInterAction == false);
                break;
            case 3:
                GameManager.Instance.selectBox.SetActive(false);
                SceneManager.Instance.PlayObj(20, 600);
                yield return new WaitUntil(() => GameManager.Instance.isInterAction == false);
                break;
        }

        GameManager.Instance.selectIndex = -1;
        GameManager.Instance.isSelecting = false;
        //=====================================================
        SwitchManager.Instance.ing = false;
    }

    IEnumerator BookShelf3()
    {
        SwitchManager.Instance.ing = true;
        //=====================================================
        SceneManager.Instance.PlayTalk(40, 100);
        yield return new WaitUntil(() => GameManager.Instance.isTalking == false);
        //=====================================================
        SwitchManager.Instance.ing = false;
    }

    IEnumerator BookShelf3_2()
    {
        SwitchManager.Instance.ing = true;
        //=====================================================
        SceneManager.Instance.PlayTalk(60, 100);
        yield return new WaitUntil(() => GameManager.Instance.isTalking == false);
        //=====================================================
        SwitchManager.Instance.ing = false;
    }

    IEnumerator LeftCantGo()
    {
        SwitchManager.Instance.ing = true;
        //=====================================================
        SceneManager.Instance.PlayTalk(50, 100);
        yield return new WaitUntil(() => GameManager.Instance.isTalking == false);
        //-----------------------------------------------------
        playerLogic.SetWaypoint(2, player.transform.position, player.transform.position + new Vector3(1.5f, 0, 0));
        playerLogic.isOn = true;
        yield return new WaitUntil(() => playerLogic.isOff == true);
        playerLogic.isOff = false;
        //=====================================================
        SwitchManager.Instance.ing = false;
    }

    IEnumerator RightCantGo()
    {
        SwitchManager.Instance.ing = true;
        //=====================================================
        SceneManager.Instance.PlayTalk(50, 100);
        yield return new WaitUntil(() => GameManager.Instance.isTalking == false);
        //-----------------------------------------------------
        playerLogic.SetWaypoint(2, player.transform.position, player.transform.position + new Vector3(-1.5f, 0, 0));
        playerLogic.isOn = true;
        yield return new WaitUntil(() => playerLogic.isOff == true);
        playerLogic.isOff = false;
        //=====================================================
        SwitchManager.Instance.ing = false;
    }

    void screenOn()
    {
        SwitchManager.Instance.ing = true;
        //=====================================================
        screenUI.SetActive(true);
    }
#endregion Event
}

using System.Collections;
using UnityEngine;

public class RecordRoom : MapEvent
{
#region Variables
    public GameObject listUI;
#endregion Variables


#region Unity Methods
    void Update() {
        if (SwitchManager.Instance.switchdata["FR_BookShelf"].on || SwitchManager.Instance.switchdata["FR_BookShelf2"].on) {
            SwitchManager.Instance.switchdata["FR_BookShelf"].on = false;
            SwitchManager.Instance.switchdata["FR_BookShelf2"].on = false;
            StartCoroutine(BookShelf());
        }

        if (SwitchManager.Instance.switchdata["FR_BookShelf3"].on) {
            SwitchManager.Instance.switchdata["FR_BookShelf3"].on = false;
            listOn();
        }

        if (SwitchManager.Instance.switchdata["Record_Page1"].on) {
            SwitchManager.Instance.switchdata["Record_Page1"].on = false;
            StartCoroutine(Page1());
        }

        if (SwitchManager.Instance.switchdata["Record_Page2"].on) {
            SwitchManager.Instance.switchdata["Record_Page2"].on = false;
            StartCoroutine(Page2());
        }
    }
#endregion Unity Methods


#region Event
    IEnumerator BookShelf()
    {
        SwitchManager.Instance.ing = true;
        //=====================================================
        SceneManager.Instance.PlayTalk(30, 100);
        yield return new WaitUntil(() => GameManager.Instance.isTalking == false);
        //=====================================================
        SwitchManager.Instance.ing = false;
    }

    void listOn()
    {
        GameManager.Instance.dontMove = true;
        //=====================================================
        listUI.SetActive(true);
    }

    IEnumerator Page1() {
        //=====================================================
        SwitchManager.Instance.ing = true;
        yield return new WaitForSeconds(0.5f);
        //-----------------------------------------------------
        SceneManager.Instance.PlayTalk(0, 800);
        yield return new WaitUntil(() => GameManager.Instance.isTalking == false);
        //=====================================================
        SwitchManager.Instance.ing = false;
        SwitchManager.Instance.switchdata["Record_Page1"].off = true;
    }

    IEnumerator Page2() {
        SwitchManager.Instance.switchdata["Record_Page2"].off = true;
        //=====================================================
        SwitchManager.Instance.ing = true;
        yield return new WaitForSeconds(0.5f);
        //-----------------------------------------------------
        SceneManager.Instance.PlayTalk(10, 800);
        yield return new WaitUntil(() => GameManager.Instance.isTalking == false);
        //=====================================================
        SwitchManager.Instance.ing = false;
    }
#endregion Event
}

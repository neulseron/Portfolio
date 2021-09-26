using System.Collections;
using UnityEngine;

public class D1Event : MapEvent
{
#region Variables
    [SerializeField]
    GameObject sumin;
    NPCMove suminLogic;
#endregion Variables


#region Unity Methods
    protected override void Start() {
        base.Start();

        if (!SwitchManager.Instance.D1_TalkSumin.off) {
            suminLogic = sumin.GetComponent<NPCMove>();
            suminLogic.Turn("up");
        }
    }

    void Update() {
        if (SwitchManager.Instance.D1_TalkSumin.on) {
            SwitchManager.Instance.D1_TalkSumin.on = false;
            StartCoroutine(talkSumin());
        }
    }
#endregion Unity Methods


#region Event
    IEnumerator talkSumin()
    {
        SwitchManager.Instance.D1_TalkSumin.ing = true;
        //=====================================================x
        suminLogic.Turn("down");
        //-----------------------------------------------------
        SceneManager.Instance.PlayTalk(0, 500);
        yield return new WaitUntil(() => GameManager.Instance.isTalking == false);
        //-----------------------------------------------------
        //=====================================================x
        SwitchManager.Instance.D1_TalkSumin.ing = false;
        SwitchManager.Instance.D1_TalkSumin.off = true;
    }
#endregion Event
}

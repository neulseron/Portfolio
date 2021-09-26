using System.Collections;
using UnityEngine;

public class FirstFloor : MapEvent
{
#region Unity Methods
    void Update() {

        if (SwitchManager.Instance.switchdata["FirstF_RecordRoom"].on) {
            SwitchManager.Instance.switchdata["FirstF_RecordRoom"].on = false;
            if (SwitchManager.Instance.switchdata["HR_ChkCalender"].off) {
                GameManager.Instance.ChangeMap(15, GameManager.Instance.player.GetComponent<PlayerAction>().myName, new Vector3(-14, 21.5f, 0), "left", 0);
            } else 
                StartCoroutine(CannotEnter(50));
        }
    }
#endregion Unity Methods


#region Event
    IEnumerator CannotEnter(int _obj)
    {
        SceneManager.Instance.PlayObj(_obj, 500, "UC");
        yield return new WaitUntil(() => GameManager.Instance.isInterAction == false);
    }
#endregion Event
}

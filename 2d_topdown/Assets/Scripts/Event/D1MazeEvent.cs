using System.Collections;
using UnityEngine;

public class D1MazeEvent : MapEvent
{
#region Variables
    public GameObject Light;
#endregion Variables


#region Unity Methods
    protected override void Start() {
        base.Start();

        Light.SetActive(true);
    }

    void Update() {
        if (SwitchManager.Instance.switchdata["HyeongSeoMaze"].on) {
            SwitchManager.Instance.switchdata["HyeongSeoMaze"].on = false;
            StartCoroutine(FadeAndChangeMap());
        }
    }
#endregion Unity Methods


#region Event
    IEnumerator FadeAndChangeMap()
    {
        GameManager.Instance.currTime = 0;
        GameManager.Instance.fadeEffect.nameStr = "서재이";
        GameManager.Instance.fadeEffect.timeStr = "2030-03-06(수)\n오후  2:10:00";
        GameManager.Instance.fadeEffect.OnFade(FadeState.FadeOut);
        yield return new WaitUntil(() => GameManager.Instance.fadeEffect.endFade == true);
        //=====================================================
        GameManager.Instance.OffPlayer(player);
        GameManager.Instance.ChangeMap(2, "P_Jaei", new Vector3(-2f, 16f, 0), "up", 1);
        //=====================================================
        SwitchManager.Instance.switchdata["HyeongSeoMaze"].off = true;
        GameManager.Instance.dontSave = false;
    }
#endregion Event
}

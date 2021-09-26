using UnityEngine;

public class ARoad : MapEvent
{
#region Variables
    [SerializeField]
    GameObject screenUI;
#endregion Variables


#region Unity Methods
    void Update()
    {
        if (SwitchManager.Instance.switchdata["A_Screen"].on) {
            SwitchManager.Instance.switchdata["A_Screen"].on = false;
            Screen();
        }
    }
#endregion Unity Methods


#region Event
    void Screen()
    {
        SwitchManager.Instance.ing = true;
        //=====================================================
        screenUI.SetActive(true);
        //=====================================================
        SwitchManager.Instance.ing = false;
    }

    public void BtnExit()
    {
        screenUI.SetActive(false);
    }
#endregion Event
}

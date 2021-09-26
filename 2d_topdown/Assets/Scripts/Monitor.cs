using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Monitor : MonoBehaviour
{
    //-----------------------------------------------------
    public Inventory inventory;
    public Animator monitorAnimator;
    public GameObject mail6;
    public GameObject mail7;
    public GameObject mail8;
    public GameObject mail9;
    public GameObject mail10;
    //-----------------------------------------------------
    public GameObject ReceiveMailSet;
    public Sprite readIcon;

    public GameObject ReceiveDetailSet;
    public Text mailName;
    public Text mailTitle;
    public Text mailDate;
    public Text mailContent;
    public GameObject BtnAttach;
    //-----------------------------------------------------
    int roomNum;         // 0 : JR, 1 : D
    public int currWindow;     // 0 : List, 1 : Detail

    void OnEnable()
    {
        currWindow = 0;
        ReceiveMailSet.SetActive(true);

        roomNum = GameManager.Instance.currMapIndex == 0 ? 0 : 1;

        if (roomNum == 0 && SwitchManager.Instance.switchdata["SecondF_mail9"].on) {
            mail9.SetActive(true);
        }

        if (roomNum == 0 && SwitchManager.Instance.switchdata["SecondF_mail10"].on) {
            mail10.SetActive(true);
        }

        if (roomNum == 0 && SwitchManager.Instance.switchdata["SecondF_mailAttached"].on) {
            mail6.SetActive(false);
            mail8.SetActive(true);
            mail7.SetActive(true);
        }

        if (roomNum == 0 && SwitchManager.Instance.switchdata["SecondF_readMail7"].on) {
            mail7.transform.Find("icon").gameObject.GetComponent<Image>().sprite = readIcon;
        }
        if (roomNum == 0 && SwitchManager.Instance.switchdata["SecondF_readMail9"].on) {
            mail9.transform.Find("icon").gameObject.GetComponent<Image>().sprite = readIcon;
        }
        if (roomNum == 0 && SwitchManager.Instance.switchdata["SecondF_readMail10"].on) {
            mail10.transform.Find("icon").gameObject.GetComponent<Image>().sprite = readIcon;
        }
    }

    private void Update() {

    }

    public void ExitReceiveMail()
    {
        switch (currWindow) {
            case 0:
                ReceiveMailSet.SetActive(false);
                gameObject.SetActive(false);
                monitorAnimator.SetTrigger("isOff");
                GameManager.Instance.dontMove = false;
                SwitchManager.Instance.ing = false;
                break;
            case 1:
                currWindow = 0;
                ReceiveDetailSet.SetActive(false);
                ReceiveMailSet.SetActive(true);
                break;
        }
    }

    public void ClickMail()
    {
        currWindow = 1;

        // ** 메일 읽음 표시로 변경 **
        GameObject btn = EventSystem.current.currentSelectedGameObject;
        Image icon = btn.transform.Find("icon").gameObject.GetComponent<Image>();
        icon.sprite = readIcon;
        
        Mail mail = CSVManager.Instance.GetMailInfo(roomNum * 100, int.Parse(btn.name));
        mailName.text = "보낸 사람 : " + mail.Name;
        mailTitle.text = mail.Title;
        mailDate.text = mail.Date;
        mailContent.text = mail.Content.Replace("\\n", "\n");

        ReceiveMailSet.SetActive(false);

        if (roomNum == 0 && btn.name == "7" && !SwitchManager.Instance.switchdata["SecondF_readMail7"].on) {
            SwitchManager.Instance.switchdata["SecondF_readMail7"].on = true;
        }
        if (roomNum == 0 && btn.name == "9" && !SwitchManager.Instance.switchdata["SecondF_readMail9"].on) {
            SwitchManager.Instance.switchdata["SecondF_readMail9"].on = true;
        }
        if (roomNum == 0 && btn.name == "10" && !SwitchManager.Instance.switchdata["SecondF_readMail10"].on) {
            SwitchManager.Instance.switchdata["SecondF_readMail10"].on = true;
        }

        if (roomNum == 0 && btn.name == "10" && !SwitchManager.Instance.switchdata["JR_ChkMail"].off) {
            SwitchManager.Instance.switchdata["JR_ChkMail"].on = true;
        }

        if (roomNum == 0 && btn.name == "6") {
            BtnAttach.SetActive(true);
        } else {
            BtnAttach.SetActive(false);
        }
        

        ReceiveDetailSet.SetActive(true);
    }

    public void ClickBtnAttach() 
    {
        inventory.nowUsing = true;
        inventory.gameObject.SetActive(true);
    }
}

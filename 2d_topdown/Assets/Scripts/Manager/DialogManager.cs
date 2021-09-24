using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public CSVManager csvManager;
    public GameManager gameManager;
    //---------------------------------------------
    public Image nameBox;
    public TypingEffect talkText;
    public Text npcName;
    public Sprite[] portraitArr;
    public Image portraitImg;
    //---------------------------------------------
    public TypingEffect SystemTxt;
    public string itemName;
    //---------------------------------------------
    int dialogIndex = 1;


    void Awake()
    {
        GenerateData();
    }

    // Update is called once per frame
    void GenerateData()
    {
        // Talk Data
        // line : + 1 ('\n')
        // cut : + 10 (&)
        // scene : + 100 (/)
        csvManager.SetDialog();
        csvManager.SetObjDialog();
        csvManager.SetSystemDialog();
        csvManager.SetItemText();
        csvManager.SetMailText();
    }

    public Sprite GetPortrait(int index)
    {
        // Portrait Data
        // 0 : Normal, 1 : Smile, 2 : Angry, 3 : ?
        return portraitArr[index];
    }

    public void Talk(int _sceneIndex, int _cutIndex)
    {
        //int questDialogIndex = 0;
        RectTransform textRect = talkText.gameObject.GetComponent<RectTransform>();
        textRect.anchoredPosition = new Vector2(40, -16);
        var data = csvManager.GetDialog(_sceneIndex, _cutIndex, dialogIndex);


        if (talkText.isAnim) {
            talkText.SetMsg("");
            return;
        } else {
            //questDialogIndex = q_manager.GetQuestDialogIndex(id);
        }

        // ** End Dialog **
        if (data == null) {
            gameManager.isTalking = false;
            dialogIndex = 1;
            //questTxt.text = q_manager.CheckQuest(id);
            return;
        }

        // ** Set Data **
        nameBox.enabled = true;
        npcName.text = data.Speaker;
        data.Txt = data.Txt.Replace("\\n", "\n");

        string[] txts = data.Txt.Split(':');

        if (txts.Length > 1) { // * 한번 끊기 존재하면
            var dataTxt = txts[0] + txts[1];

            if (!talkText.isPortraitDiff) {
                talkText.SetMsg(txts[0]);

                if (data.Portrait1 != -1) { 
                    textRect.sizeDelta = new Vector2(560, 120);
                    portraitImg.sprite = GetPortrait(data.Portrait1);
                    portraitImg.color = new Color(1, 1, 1, 1);
                } else {    // * 초상화 없는 사람
                    textRect.sizeDelta = new Vector2(700, 120);
                    portraitImg.color = new Color(1, 1, 1, 0);
                }

                talkText.isPortraitDiff = true;
            } else {
                talkText.SetMsg(dataTxt, txts[0].Length);

                if (data.Portrait1 != -1) {
                    textRect.sizeDelta = new Vector2(560, 120);
                    portraitImg.sprite = GetPortrait(data.Portrait2);
                    portraitImg.color = new Color(1, 1, 1, 1);
                } else {
                    textRect.sizeDelta = new Vector2(700, 120);
                    portraitImg.color = new Color(1, 1, 1, 0);
                }

                talkText.isPortraitDiff = false;
            }
        } else { // * 그냥 보통 출력
            talkText.SetMsg(data.Txt);

            if (data.Portrait1 != -1) { 
                textRect.sizeDelta = new Vector2(560, 120);
                portraitImg.sprite = GetPortrait(data.Portrait1);
                portraitImg.color = new Color(1, 1, 1, 1);
            } else {    // * 초상화 없는 사람
                textRect.sizeDelta = new Vector2(700, 120);
                portraitImg.color = new Color(1, 1, 1, 0);
            }
        }

        gameManager.isTalking = true;
        if (!talkText.isPortraitDiff)
            dialogIndex++;
    }
    //---------------------------------------------

    public void ObjTalk(int _objIndex, int _cutIndex)
    {
        RectTransform textRect = talkText.gameObject.GetComponent<RectTransform>();
        textRect.sizeDelta = new Vector2(700, 120);
        textRect.anchoredPosition = new Vector2(40, -8);

        var data = csvManager.GetObj(_objIndex, _cutIndex, dialogIndex);

        if (talkText.isAnim) {
            talkText.SetMsg("");
            return;
        }

        // ** End Dialog **
        if (data == null) {
            gameManager.isInterAction = false;
            dialogIndex = 1;
            return;
        }

        // ** Set Data **
        data.Txt = data.Txt.Replace("\\n", "\n");

        nameBox.enabled = false;
        npcName.text = "";
        talkText.SetMsg(data.Txt);
        portraitImg.color = new Color(1, 1, 1, 0);

        dialogIndex++;
    }
    //---------------------------------------------
   
    public void SystemTalk(int _sceneIndex, int _cutIndex)
    {
        //int questDialogIndex = 0;
        var data = csvManager.GetSystem(_sceneIndex, _cutIndex, dialogIndex);

        if (SystemTxt.isAnim) {
            SystemTxt.SetMsg("");
            return;
        }

        // ** End Dialog **
        if (data == null) {
            dialogIndex = 1;
            gameManager.isSystem = false;
            return;
        }

        // ** Set Data **
        data.Txt = data.Txt.Replace("\\n", "\n");
        //string[] txts = data.Txt.Split(':');

        if (_sceneIndex == 100) { // ** 아이템 추가 메세지
            string str = itemName + data.Txt;
            SystemTxt.SetMsg(str);
        } else {
            SystemTxt.SetMsg(data.Txt);
        }

        dialogIndex++;
    }
    
    //---------------------------------------------
    public void SelectBoxClick()
    {
        GameObject btn = EventSystem.current.currentSelectedGameObject;
        switch (btn.name) {
            case "Btn1":
                gameManager.selectIndex = 1;
                break;
            case "Btn2":
                gameManager.selectIndex = 2;
                break;
            case "Btn3":
                gameManager.selectIndex = 3;
                break;
            case "Btn4":
                gameManager.selectIndex = 4;
                break;
        }

        btn.transform.parent.gameObject.SetActive(false);
    }
}

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
#region Singletone
    static DialogManager instance;
    public static DialogManager Instance => instance;
#endregion Singletone


#region Variables
    public Image nameBox;
    public TypingEffect talkText;
    public Text npcName;
    public Sprite[] portraitArr;
    public Image portraitImg;

    public TypingEffect SystemTxt;
    public string itemName;

    int dialogIndex = 1;
#endregion Variables


#region Methods
    void Awake()
    {
        instance = this;
        
        GenerateData();
    }

    #region Generate Dialogue Data
    void GenerateData()
    {
        // Talk Data
        // line : + 1 ('\n')
        // cut : + 10 (&)
        // scene : + 100 (/)
        CSVManager.Instance.SetDialog();
        CSVManager.Instance.SetObjDialog();
        CSVManager.Instance.SetSystemDialog();
        CSVManager.Instance.SetItemText();
        CSVManager.Instance.SetMailText();
    }

    public Sprite GetPortrait(int index)
    {
        // Portrait Data
        // 0 : Normal, 1 : Smile, 2 : Angry, 3 : ?
        return portraitArr[index];
    }
    #endregion Generate Dialogue Data

    public void Talk(int _sceneIndex, int _cutIndex)
    {
        RectTransform textRect = talkText.gameObject.GetComponent<RectTransform>();
        textRect.anchoredPosition = new Vector2(40, -16);
        var data = CSVManager.Instance.GetDialog(_sceneIndex, _cutIndex, dialogIndex);


        if (talkText.isAnim) {
            talkText.SetMsg("");
            return;
        }

        // ** End Dialog **
        if (data == null) {
            GameManager.Instance.isTalking = false;
            dialogIndex = 1;
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

        GameManager.Instance.isTalking = true;
        if (!talkText.isPortraitDiff)
            dialogIndex++;
    }

    public void ObjTalk(int _objIndex, int _cutIndex)
    {
        RectTransform textRect = talkText.gameObject.GetComponent<RectTransform>();
        textRect.sizeDelta = new Vector2(700, 120);
        textRect.anchoredPosition = new Vector2(40, -8);

        var data = CSVManager.Instance.GetObj(_objIndex, _cutIndex, dialogIndex);

        if (talkText.isAnim) {
            talkText.SetMsg("");
            return;
        }

        // ** End Dialog **
        if (data == null) {
            GameManager.Instance.isInterAction = false;
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
   
    public void SystemTalk(int _sceneIndex, int _cutIndex)
    {
        var data = CSVManager.Instance.GetSystem(_sceneIndex, _cutIndex, dialogIndex);

        if (SystemTxt.isAnim) {
            SystemTxt.SetMsg("");
            return;
        }

        // ** End Dialog **
        if (data == null) {
            dialogIndex = 1;
            GameManager.Instance.isSystem = false;
            return;
        }

        // ** Set Data **
        data.Txt = data.Txt.Replace("\\n", "\n");

        if (_sceneIndex == 100) { // ** 아이템 추가 메세지
            string str = itemName + data.Txt;
            SystemTxt.SetMsg(str);
        } else {
            SystemTxt.SetMsg(data.Txt);
        }

        dialogIndex++;
    }
    
    public void SelectBoxClick()
    {
        GameObject btn = EventSystem.current.currentSelectedGameObject;
        switch (btn.name) {
            case "Btn1":
                GameManager.Instance.selectIndex = 1;
                break;
            case "Btn2":
                GameManager.Instance.selectIndex = 2;
                break;
            case "Btn3":
                GameManager.Instance.selectIndex = 3;
                break;
            case "Btn4":
                GameManager.Instance.selectIndex = 4;
                break;
        }

        btn.transform.parent.gameObject.SetActive(false);
    }
#endregion Methods
}

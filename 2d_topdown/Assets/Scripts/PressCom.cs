using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PressCom : MonoBehaviour
{
    public SwitchManager switchManager;
    public CSVManager csvManager;

    public GameObject mainWindow;
    public GameObject pressWindow;
    public GameObject articleWindow;
    public Text headline;
    public Text date;
    public Text content;
    public GameObject chooseBox;

    GameObject backBtn;
    int currentScreen;      // 0 : main, 1 : press, 2 : article

    void OnEnable() {
        backBtn = chooseBox.transform.Find("Btn3").gameObject;
        backBtn.GetComponent<Button>().interactable = false;
        mainWindow.SetActive(true);
    }

    public void BtnPress()
    {
        currentScreen = 1;
        pressWindow.SetActive(true);
        mainWindow.SetActive(false);
        backBtn.GetComponent<Button>().interactable = true;
    }

    public void ClickArticle()
    {
        currentScreen = 2;

        GameObject btn = EventSystem.current.currentSelectedGameObject;
        headline.text = csvManager.GetObj(0, int.Parse(btn.name), 1).Txt;
        date.text = csvManager.GetObj(0, int.Parse(btn.name), 2).Txt;
        content.text = csvManager.GetObj(0, int.Parse(btn.name), 3).Txt.Replace("\\n", "\n");

        pressWindow.SetActive(false);
        articleWindow.SetActive(true);
    }

    public void BtnBack()
    {
        switch (currentScreen) {
            case 1:
                backBtn.GetComponent<Button>().interactable = false;
                pressWindow.SetActive(false);
                mainWindow.SetActive(true);
                currentScreen = 0;
                break;
            case 2:
                articleWindow.SetActive(false);
                pressWindow.SetActive(true);
                currentScreen = 1;
                break;
        }
    }

    public void BtnExit()
    {
        switch (currentScreen) {
            case 0:
                mainWindow.SetActive(false);
                break;
            case 1:
                pressWindow.SetActive(false);
                break;
            case 2:
                articleWindow.SetActive(false);
                break;
        }

        currentScreen = 0;
        backBtn.GetComponent<Button>().interactable = false;

        gameObject.SetActive(false);
        switchManager.ing = false;
    }
}

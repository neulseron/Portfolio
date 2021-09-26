using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DataCom : MonoBehaviour
{
    public GameObject DataComUI;
    public GameObject inventory;
    public GameObject tutorialSet;
    //------------------------------------------------------
    public int currentScreen;      // 0 : login, 1 : main, 2 : currComaList, 3 : totalComa, 4 : comaDetail, 5 : cannotFound, 6 : comaResult, 7 : miaList
    //=====================================================
    // ** 로그인 **
    public GameObject Login;
    public bool loginEnd = false;
    //=====================================================
    // ** 메인 화면 **
    public GameObject Main;
    public GameObject MiaList;
    public GameObject ListHS;
    //------------------------------------------------------
    public void BtnCurrComa()
    {
        if (!SwitchManager.Instance.switchdata["Tutorial_DataCom"].off)
            return;

        currentScreen = 2;
        Main.SetActive(false);
        CurrComaList.SetActive(true);
        backBtn.GetComponent<Button>().interactable = true;
    }

    public void BtnTotalComa()
    {
        if (!SwitchManager.Instance.switchdata["Tutorial_DataCom"].off)
            return;

        currentScreen = 3;
        Main.SetActive(false);
        FindComa.SetActive(true);
        backBtn.GetComponent<Button>().interactable = true;
    }

    public void BtnMiaList()
    {
        if (!SwitchManager.Instance.switchdata["Tutorial_DataCom"].off)
            return;

        currentScreen = 7;
        Main.SetActive(false);
        MiaList.SetActive(true);
        backBtn.GetComponent<Button>().interactable = true;
    }

    //=====================================================
    public GameObject CurrComaList;
    public ComaData comaData;
    public GameObject DetailComa;
    public Text nameTxt;
    public Text birthTxt;
    public Text districtTxt;
    public Text responseTxt;
    public Text searcherTxt;
    public Text noteTxt;
    //------------------------------------------------------
    public void ClickComa()
    {
        GameObject btn = EventSystem.current.currentSelectedGameObject;

        comaDataForm cdf = comaData.comaID(btn.name);
        nameTxt.text = "이름 : " + cdf.name;
        birthTxt.text = "생년월일 : " + cdf.birth;
        districtTxt.text = "구역 : " + cdf.dis;
        responseTxt.text = "담당자 : " + cdf.response;
        searcherTxt.text = "수색자 : " + cdf.search;
        noteTxt.text = "상태 : " + cdf.state;

        currentScreen = 4;
        CurrComaList.SetActive(false);
        DetailComa.SetActive(true);
    }
    //=====================================================
    public GameObject FindComa;
    public GameObject ComaResult;
    //------------------------------------------------------
    //=====================================================
    public GameObject CannotFound;
    //=====================================================
    public GameObject ChooseBox;
    GameObject getDataBtn;
    GameObject backBtn;
    //------------------------------------------------------
    public void BtnInventory()
    {
        inventory.SetActive(true);
    }

    public void BtnEnd()
    {
        DataComUI.SetActive(false);
        SwitchManager.Instance.ing = false;

        switch(currentScreen) {
            case 1:
                Main.SetActive(false);
                break;
            case 2:
                CurrComaList.SetActive(false);
                break;
            case 3:
                FindComa.SetActive(false);
                break;
            case 4:
                DetailComa.SetActive(false);
                break;
            case 5:
                CannotFound.SetActive(false);
                break;
            case 6:
                ComaResult.SetActive(false);
                break;
            case 7:
                MiaList.SetActive(false);
                break;
        }
        
        currentScreen = 0;
        backBtn.GetComponent<Button>().interactable = false;
    }

    public void BtnBack()
    {
        switch(currentScreen) {
            case 2:
                backBtn.GetComponent<Button>().interactable = false;
                CurrComaList.SetActive(false);
                Main.SetActive(true);
                currentScreen = 1;
                break;
            case 3:
                backBtn.GetComponent<Button>().interactable = false;
                FindComa.SetActive(false);
                Main.SetActive(true);
                currentScreen = 1;
                break;
            case 4:
                DetailComa.SetActive(false);
                CurrComaList.SetActive(true);
                currentScreen = 2;
                break;
            case 5:
                CannotFound.SetActive(false);
                FindComa.SetActive(true);
                currentScreen = 3;
                break;
            case 6:
                ComaResult.SetActive(false);
                FindComa.SetActive(true);
                getDataBtn.GetComponent<Button>().interactable = false;
                currentScreen = 3;
                break;
            case 7:
                backBtn.GetComponent<Button>().interactable = false;
                MiaList.SetActive(false);
                Main.SetActive(true);
                currentScreen = 1;
                break;
        }
    }

    public void BtnRequest()
    {
        if (comaData.inputComaId == "378951") {
            // 가지고 있으면
            if (inventory.GetComponent<Inventory>().havingItem(4)) {
                GameManager.Instance.currSceneIndex = 300;
                GameManager.Instance.currCutIndex = 10;
                GameManager.Instance.SystemAction();
                return;
            }
            // 가지고 있지 않으면
            inventory.GetComponent<Inventory>().AddItem(4);
            GameManager.Instance.currSceneIndex = 300;
            GameManager.Instance.currCutIndex = 0;
            GameManager.Instance.SystemAction();
            
        } else {
            GameManager.Instance.currSceneIndex = 100;
            GameManager.Instance.currCutIndex = 0;
            GameManager.Instance.Action();
        }
    }

    //=====================================================
    void OnEnable() {
        getDataBtn = ChooseBox.transform.Find("Btn1").gameObject;
        getDataBtn.GetComponent<Button>().interactable = false;
        backBtn = ChooseBox.transform.Find("Btn3").gameObject;
        backBtn.GetComponent<Button>().interactable = false;
    }

    void Update() {
        if (SwitchManager.Instance.switchdata["SecondF_movingSM"].off) {
            ListHS.transform.Find("state").gameObject.GetComponent<Text>().text = "-";
        }

        if (SwitchManager.Instance.switchdata["DataCom_JHsearchEnd"].on) {
            comaDataForm cdf = comaData.comaID("456871");
            cdf.search = "서재하(수색)";
            cdf.state = "입원(수색종료)";
        }
        
        if (SwitchManager.Instance.switchdata["SecondF_DataCom"].on) {
            SwitchManager.Instance.switchdata["SecondF_DataCom"].on = false;

            SwitchManager.Instance.switchdata["Tutorial_OffMarkDataCom"].on = true;

            DataComUI.SetActive(true);
            Login.SetActive(true);
            SwitchManager.Instance.ing = true;
        }

        if (SwitchManager.Instance.switchdata["Tutorial_DataCom"].on && !Login.activeSelf) {
            SwitchManager.Instance.switchdata["Tutorial_DataCom"].on = false;
            StartCoroutine(Tutorial());
        }
        
        if (SwitchManager.Instance.ing) {
            if (loginEnd) {
                currentScreen = 1;
                Main.SetActive(true);
                loginEnd = false;
            } else if (comaData.searchComaEnd) {
                if (comaData.ChkID()) {
                    
                    if (comaData.inputComaId == "103015" && !SwitchManager.Instance.switchdata["DataCom_Chk103015"].off) {
                        SwitchManager.Instance.switchdata["DataCom_Chk103015"].on = true;
                    }
                    
                    currentScreen = 6;
                    ComaResult.SetActive(true);
                    getDataBtn.GetComponent<Button>().interactable = true;
                } else {
                    currentScreen = 5;
                    CannotFound.SetActive(true);
                }

                comaData.searchComaEnd = false;
            }

        }

        IEnumerator Tutorial()
        {
            //=====================================================
            yield return new WaitForSeconds(0.5f);
            //-----------------------------------------------------
            tutorialSet.transform.Find("mark").gameObject.SetActive(true);
            SceneManager.Instance.PlaySystemTalk(0, 600);
            yield return new WaitUntil(() => GameManager.Instance.isSystem == false);
            //-----------------------------------------------------
            tutorialSet.transform.Find("mark").gameObject.transform.position = new Vector3(Main.transform.Find("Btn_TotalComaList").gameObject.transform.position.x, tutorialSet.transform.Find("mark").gameObject.transform.position.y, 0);
            SceneManager.Instance.PlaySystemTalk(10, 600);
            yield return new WaitUntil(() => GameManager.Instance.isSystem == false);
            //-----------------------------------------------------
            tutorialSet.transform.Find("mark").gameObject.transform.position = new Vector3(Main.transform.Find("Btn_MiaList").gameObject.transform.position.x, Main.transform.Find("Btn_MiaList").gameObject.transform.position.y, 0) + new Vector3(0, 1, 0);
            SceneManager.Instance.PlaySystemTalk(20, 600);
            yield return new WaitUntil(() => GameManager.Instance.isSystem == false);
            //=====================================================
            SwitchManager.Instance.switchdata["Tutorial_DataCom"].off = true;
            tutorialSet.transform.Find("mark").gameObject.SetActive(false);
        }
    }
}

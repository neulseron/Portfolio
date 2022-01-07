using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using BackEnd;
using LitJson;

public class MainManager : MonoBehaviour
{
    //enum Heros { Guardians, DrStrange, BlackWidow, BlackPanther, SpiderMan, IronMan, AntMan, Avengers, CaptinMarvel, CaptinAmerica, Thor, Hulk, Hawkeye };

    public GameObject sideMenuPanel;
    public GameObject[] HeroBtns;

    void Start() {
        LoadInterestData();
    }

    void Update() {
        if (sideMenuPanel.activeSelf && Input.GetKey(KeyCode.Escape)) {
            sideMenuPanel.SetActive(false);
        }
    }

    public void LoadInterestData()
    {
        var bro = Backend.GameData.GetMyData("Interest", new Where());

        if (!bro.IsSuccess()) {
            Debug.Log("불러오기 실패");
            return;
        } else if (bro.GetReturnValuetoJSON()["rows"].Count <= 0) {
            Debug.Log("조회할 데이터 없음");
            return;
        }

        foreach (var btn in HeroBtns) {
            var data = bro.Rows()[0][btn.name][0];
            btn.SetActive((bool)data);
        }
    }

    public void OpenSideMenuPanel()
    {
        sideMenuPanel.SetActive(true);
    }

    public void LoadWorldViewScene()
    {
        SceneManager.LoadScene(3);
    }

    public void LoadSeriesScene()
    {
        SceneManager.LoadScene(4);
    }

    public void LoadCommunityScene()
    {
        SceneManager.LoadScene(5);
    }

    public void LoadMypageScene()
    {
        SceneManager.LoadScene(6);
    }
}

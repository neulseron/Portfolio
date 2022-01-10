using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using BackEnd;
using LitJson;

public class InterestManager : MonoBehaviour
{
    public List<Toggle> interests = new List<Toggle>();

    void Start() {
        LoadInterestData();
    }

    public void LoadInterestData()
    {
        var bro = Backend.GameData.GetMyData("Interest", new Where());

        if (!bro.IsSuccess()) {
            Debug.Log("불러오기 실패");
            //Error(bro.GetErrorCode(), "loadData");
            return;
        } else if (bro.GetReturnValuetoJSON()["rows"].Count <= 0) {
            Debug.Log("조회할 데이터 없음");
            InsertInterestData();
            //Debug.Log(bro);
            return;
        }

        foreach (var tog in interests) {
            var data = bro.Rows()[0][tog.name][0];
            tog.isOn = (bool)data;
        }
    }

    public void InsertInterestData()
    {
        Param param = new Param();
        foreach (var tog in interests) {
            param.Add(tog.name, false);
        }
        
        ApplicationManager.Instance.InsertData("Interest", param);
    }

    public void UpdateInterestData()
    {
        Param param = new Param();
        foreach (var tog in interests) {
            if (tog.isOn) {
                param.Add(tog.name, true);
            } else {
                param.Add(tog.name, false);
            }
        }

        ApplicationManager.Instance.UpdateData("Interest", param);
    }

    public void LoadMainScene()
    {
        UpdateInterestData();
        
        string nextScene = ApplicationManager.Instance.isFromMyPage ? "MyPage" : "Main";
        ApplicationManager.Instance.LoadNextScene(nextScene);
    }
}

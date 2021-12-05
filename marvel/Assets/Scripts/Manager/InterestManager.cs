using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using BackEnd;

public class InterestManager : MonoBehaviour
{
    [Header("Interests")]
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
            Debug.Log("불러올 데이터 없음");
            //Debug.Log(bro);
            return;
        }

        foreach (var tog in interests) {
            var data = bro.Rows()[0][tog.name][0];
            if ((bool)data) {
                tog.isOn = true;
            } else {
                tog.isOn = false;
            }
        }
    }

    public void InsertInterestData()
    {
        Param param = new Param();
        foreach (var tog in interests) {
            if (tog.isOn) {
                param.Add(tog.name, true);
            } else {
                param.Add(tog.name, false);
            }
        }

        var bro = Backend.GameData.Insert("Interest", param);
        if (bro.IsSuccess()) {
            Debug.Log("Interset 데이터 추가 성공");
        } else {
            Error(bro.GetErrorCode(), "gameData");
        }
    }

    void Error(string errorCode, string type)
    {
        switch(errorCode) {
            case "PreconditionFailed":
                if (type == "gameData")
                    Debug.LogError("데이터를 추가할 수 없음");
                else if (type == "loadData")
                    Debug.LogError("데이터를 불러올 수 없음");
                break;
            default:
                break;
        }
    }

    public void LoadMainScene()
    {
        InsertInterestData();
        SceneManager.LoadScene(2);
    }
}

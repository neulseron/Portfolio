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

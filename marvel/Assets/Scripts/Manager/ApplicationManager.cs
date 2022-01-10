using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BackEnd;

public class ApplicationManager : MonoBehaviour
{
#region Singletone
    static ApplicationManager instance;
    public static ApplicationManager Instance => instance;
#endregion Singletone

    string[] menuBtns = { "Main", "WorldView", "Series", "Community", "MyPage" };

    public bool isFromMyPage = false;

    void Awake() {
        instance = this;
        DontDestroyOnLoad(gameObject);
        Screen.SetResolution(360, 720, false);
    }

    void Start() {
        Invoke("LoadLoginScene", 1.5f);
    }

#region Scene
    void LoadLoginScene() => SceneManager.LoadScene("Login");
    public void LoadNextScene(string nextScene) => SceneManager.LoadScene(nextScene);
    public void LoadNextScene(int idx) => SceneManager.LoadScene(menuBtns[idx]);
#endregion Scene

    public string GetNickName()
    {
        BackendReturnObject bro = Backend.BMember.GetUserInfo();
        string nickname = bro.GetReturnValuetoJSON()["row"]["nickname"].ToString();

        return nickname;
    }


#region Data
    public void UpdateData(string tableName, Param param)
    {
        var getInDate = Backend.GameData.GetMyData(tableName, new Where());
        var bro = Backend.GameData.UpdateV2(tableName, getInDate.Rows()[0]["inDate"][0].ToString(), Backend.UserInDate, param);

        if (bro.IsSuccess()) {
            Debug.Log(tableName + " 데이터 수정 성공");
        } else {
            Debug.Log(tableName + " 데이터 수정 실패");
        }
    }
    
    public void InsertData(string tableName, Param param)
    {
        var bro = Backend.GameData.Insert(tableName, param);

        if (bro.IsSuccess()) {
            Debug.Log(tableName + " 데이터 추가 성공");
        } else {
            Debug.LogError(tableName + " 데이터 추가 실패");
        }
    }
#endregion Data

}

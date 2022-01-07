using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using BackEnd;
using LitJson;

public class MyPageManager : MonoBehaviour
{
    public Text NicknameTxt;

    void Start() {
        BackendReturnObject bro = Backend.BMember.GetUserInfo();
        string nickname = bro.GetReturnValuetoJSON()["row"]["nickname"].ToString();
        NicknameTxt.text = nickname;
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene(2);
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
}

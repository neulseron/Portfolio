using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using BackEnd;

public class LoginManager : MonoBehaviour
{
    [Header("Login")]
    public InputField ID;
    public InputField PW;


    [Header("Login")]
    public GameObject JoinSet;
    public InputField JoinID;
    public InputField JoinPW;
    public InputField JoinNickname;


    void Start()
    {
        var bro = Backend.Initialize();
        if (bro.IsSuccess()) {
            Debug.Log("초기화 성공");
        } else {
            Debug.LogError("초기화 실패");
        }
    }

    void Error(string errorCode)
    {
        switch(errorCode) {
            case "DuplicatedParameterException":
                Debug.LogError("중복된 아이디입니다.");
                break;
            case "BadUnauthorizedException":
                Debug.LogError("잘못된 사용자 아이디 혹은 비밀번호입니다.");
                break;
            default:
                break;
        }
    }

    public void Login()
    {
        var bro = Backend.BMember.CustomLogin(ID.text, PW.text);

        if (bro.IsSuccess()) {
            Debug.Log("로그인 성공");

            var bro2 = Backend.GameData.GetMyData("ProfileImg", new Where());
            if (bro2.GetReturnValuetoJSON()["rows"].Count <= 0) {
                Debug.Log("프로필 없음");
                LoadDefaultProfileImg();
            }

            ApplicationManager.Instance.isFromMyPage = false;
            ApplicationManager.Instance.LoadNextScene("Interest");
        } else Error(bro.GetErrorCode());
    }

    public void ToJoin()
    {
        JoinSet.SetActive(true);
    }

    public void Join()
    {
        var bro = Backend.BMember.CustomSignUp(JoinID.text, JoinPW.text);

        if (bro.IsSuccess())    Debug.Log("동기 회원가입 성공");
        else Error(bro.GetErrorCode());

        Backend.BMember.CreateNickname(JoinNickname.text);

        JoinSet.SetActive(false);
    }

    void LoadDefaultProfileImg()
    {
        string path = "Assets/Resources/DefaultProfile.png";
        byte[] byteTexture = File.ReadAllBytes(path);

        Param param = new Param();
        param.Add("profile", byteTexture);

        /*
        var bro = Backend.GameData.Insert("ProfileImg", param);
        if (bro.IsSuccess()) {
            Debug.Log("ProfileImg 데이터 추가 성공");
        } else {
            Debug.LogError("Profile Img 추가 실패");
        }
        */
        ApplicationManager.Instance.InsertData("ProfileImg", param);
    }
}

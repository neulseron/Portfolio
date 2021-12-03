using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using BackEnd;

public class LoginManager : MonoBehaviour
{
    [Header("Login")]
    public InputField ID;
    public InputField PW;


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
        var Bro = Backend.BMember.CustomLogin(ID.text, PW.text);

        if (Bro.IsSuccess()) {
            Debug.Log("동기 로그인 성공");
            LoadInterestScene();
        } else Error(Bro.GetErrorCode());
    }

    public void Join()
    {
        var Bro = Backend.BMember.CustomSignUp(ID.text, PW.text);

        if (Bro.IsSuccess())    Debug.Log("동기 회원가입 성공");
        else Error(Bro.GetErrorCode());
    }

    void LoadInterestScene()
    {
        SceneManager.LoadScene(1);
    }
}

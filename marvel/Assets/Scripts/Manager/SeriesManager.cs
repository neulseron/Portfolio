using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BackEnd;

public class SeriesManager : MonoBehaviour
{

    public void LoadMainScene()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadWorldViewScene()
    {
        SceneManager.LoadScene(3);
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

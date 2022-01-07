using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WordviewManager : MonoBehaviour
{
    public void LoadMainScene()
    {
        SceneManager.LoadScene(2);
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

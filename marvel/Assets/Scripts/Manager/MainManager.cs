using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public void LoadWorldViewScene()
    {
        SceneManager.LoadScene(3);
    }

    public void LoadSeriesScene()
    {
        SceneManager.LoadScene(4);
    }
}

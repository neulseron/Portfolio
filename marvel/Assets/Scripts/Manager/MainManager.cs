using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public GameObject sideMenuPanel;

    void Update() {
        if (sideMenuPanel.activeSelf && Input.GetKey(KeyCode.Escape)) {
            sideMenuPanel.SetActive(false);
        }
    }

    public void OpenSideMenuPanel()
    {
        sideMenuPanel.SetActive(true);
    }

    public void LoadWorldViewScene()
    {
        SceneManager.LoadScene(3);
    }

    public void LoadSeriesScene()
    {
        SceneManager.LoadScene(4);
    }
}

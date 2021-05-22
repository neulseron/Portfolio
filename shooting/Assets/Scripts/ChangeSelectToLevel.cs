using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ChangeSelectToLevel : MonoBehaviour
{
    public GameObject level;

    LevelManager levelManager;

    private void Start() {
        level = GameObject.Find("LevelManager");
        levelManager = level.GetComponent<LevelManager>();
    }

    public void ChangeLevelScene()
    {
        string btnName = EventSystem.current.currentSelectedGameObject.name;
        switch (btnName) {
            case "BtnLevel1":
                levelManager.stageNum = 1;
                break;
            case "BtnLevel2":
                levelManager.stageNum = 2;
                break;
            case "BtnLevel3":
                levelManager.stageNum = 3;
                break;
        }

        SceneManager.LoadScene(1);
    }

    public void BackToMain()
    {
        SceneManager.LoadScene(0);
    }
}

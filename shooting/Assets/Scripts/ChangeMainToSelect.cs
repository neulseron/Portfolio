using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeMainToSelect : MonoBehaviour
{
    public GameObject level;

    LevelManager levelManager;

    private void Start() {
        level = GameObject.Find("LevelManager");
        levelManager = level.GetComponent<LevelManager>();
    }
    
    public void ChangeLevelSelectScene()
    {
        SceneManager.LoadScene(2);
    }

    public void ChangeUnlimitedScene()
    {
        levelManager.stageNum = 99;
                
        SceneManager.LoadScene(1);
    }
}

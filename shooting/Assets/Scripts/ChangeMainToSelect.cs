using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeMainToSelect : MonoBehaviour
{
    LevelManager levelManager;

    private void Start() {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
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

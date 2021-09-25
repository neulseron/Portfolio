using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSelectToLevel : MonoBehaviour
{
    LevelManager levelManager;

    private void Start() {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    public void ChangeLevelScene(int level)
    {
        levelManager.stageNum = level;
        SceneManager.LoadScene(1);
    }

    public void BackToMain()
    {
        SceneManager.LoadScene(0);
    }
}

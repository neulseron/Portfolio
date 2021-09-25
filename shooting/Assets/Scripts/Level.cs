using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    public int myLevel;
    public Image[] stars;

    LevelManager levelManager;

    private void Start() {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();

        for (int i = 0; i < levelManager.starCntArr[myLevel]; i++) {
            if (levelManager.allClearArr[myLevel] == true)
                stars[i].sprite = levelManager.clearStar;
            else
                stars[i].sprite = levelManager.starOn;
        }
    }
}

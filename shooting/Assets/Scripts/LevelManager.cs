using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int stageNum;
    public int maxStage;

    public Sprite starOn;
    public Sprite clearStar;

    public int[] starCntArr;
    public int[] highScoreArr;
    public bool[] allClearArr;

    public int unlimitScore;

    private void Awake() {
        var obj = FindObjectsOfType<LevelManager>();
        if (obj.Length == 1) {
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        starCntArr = new int[maxStage]; // 레벨 개수
        highScoreArr = new int[maxStage]; // 레벨 개수
        allClearArr = new bool[maxStage]; // 레벨 개수
    }
}

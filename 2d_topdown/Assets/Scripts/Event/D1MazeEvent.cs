using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D1MazeEvent : MonoBehaviour
{
    public GameManager gameManager;
    public SceneManager sceneManager;
    public SwitchManager switchManager;
    GameObject player;
    PlayerAction playerLogic;
    //--------------------------------------
    public GameObject Light;
    //--------------------------------------
    public Transform[] spawnPoints;
    //======================================
    void Start() {
        player = gameManager.player;
        playerLogic = player.GetComponent<PlayerAction>();

        Light.SetActive(true);
    }

    void Update() {
        if (switchManager.switchdata["HyeongSeoMaze"].on) {
            switchManager.switchdata["HyeongSeoMaze"].on = false;
            StartCoroutine(FadeAndChangeMap());
        }
    }

    IEnumerator FadeAndChangeMap()
    {
        gameManager.currTime = 0;
        gameManager.fadeEffect.nameStr = "서재이";
        gameManager.fadeEffect.timeStr = "2030-03-06(수)\n오후  2:10:00";
        gameManager.fadeEffect.OnFade(FadeState.FadeOut);
        yield return new WaitUntil(() => gameManager.fadeEffect.endFade == true);
        //=====================================================
        gameManager.OffPlayer(player);
        gameManager.ChangeMap(2, "P_Jaei", new Vector3(-2f, 16f, 0), "up", 1);
        //=====================================================
        switchManager.switchdata["HyeongSeoMaze"].off = true;
        gameManager.dontSave = false;
    }
}

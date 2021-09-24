using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstFloor : MonoBehaviour
{
    public GameManager gameManager;
    public SceneManager sceneManager;
    public SwitchManager switchManager;
    GameObject player;
    PlayerAction playerLogic;
    //--------------------------------------
    //--------------------------------------
    public Transform[] spawnPoints;
    //======================================
    void Start() {
        player = gameManager.player;
        playerLogic = player.GetComponent<PlayerAction>();
    }

    void Update() {

        if (switchManager.switchdata["FirstF_RecordRoom"].on) {
            switchManager.switchdata["FirstF_RecordRoom"].on = false;
            if (switchManager.switchdata["HR_ChkCalender"].off) {
                gameManager.ChangeMap(15, gameManager.player.GetComponent<PlayerAction>().myName, new Vector3(-14, 21.5f, 0), "left", 0);
            } else 
                StartCoroutine(CannotEnter(50));
        }

    }


    IEnumerator CannotEnter(int _obj)
    {
        sceneManager.PlayObj(_obj, 500, "UC");
        yield return new WaitUntil(() => gameManager.isInterAction == false);
    }
}

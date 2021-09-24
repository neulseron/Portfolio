using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D1Event : MonoBehaviour
{
    public GameManager gameManager;
    public SceneManager sceneManager;
    public SwitchManager switchManager;
    GameObject player;
    //--------------------------------------
    public GameObject sumin;
    NPCMove suminLogic;
    //--------------------------------------
    public Transform[] spawnPoints;
    //======================================
    void Start() {
        player = gameManager.player;

        if (!switchManager.D1_TalkSumin.off) {
            suminLogic = sumin.GetComponent<NPCMove>();
            suminLogic.Turn("up");
        }
    }

    void Update() {
        if (switchManager.D1_TalkSumin.on) {
            switchManager.D1_TalkSumin.on = false;
            StartCoroutine(talkSumin());
        }

        
    }

    IEnumerator talkSumin()
    {
        switchManager.D1_TalkSumin.ing = true;
        //=====================================================x
        suminLogic.Turn("down");
        //-----------------------------------------------------
        sceneManager.PlayTalk(0, 500);
        yield return new WaitUntil(() => gameManager.isTalking == false);
        //-----------------------------------------------------
        //=====================================================x
        switchManager.D1_TalkSumin.ing = false;
        switchManager.D1_TalkSumin.off = true;
    }

    
}

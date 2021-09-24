using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A1MazeEvent : MonoBehaviour
{
    public GameManager gameManager;
    public SceneManager sceneManager;
    public SwitchManager switchManager;
    GameObject player;
    PlayerAction playerLogic;
    //--------------------------------------
    string ans = "";
    int turn = 0;
    public Switch appearJH;

    GameObject Jihyeon;
    NPCMove JihyeonLogic;
    //--------------------------------------
    public Transform[] spawnPoints;
    //======================================
    void Start() {
        player = gameManager.player;
        playerLogic = player.GetComponent<PlayerAction>();

        Jihyeon = sceneManager.PlayNPC("N_Jihyeon", spawnPoints[0].position);
        JihyeonLogic = Jihyeon.GetComponent<NPCMove>();

        JihyeonLogic.Turn("right");

        appearJH.on = true;
    }

    void Update() {
        if (switchManager.switchdata["A1Maze_InitTalk"].on) {
            gameManager.dontSave = true;
            switchManager.switchdata["A1Maze_InitTalk"].on = false;
            playerLogic.Turn("left");
            StartCoroutine(InitTalk());
        }

        if (switchManager.switchdata["A1MazeOn"].on && ans != switchManager.A1MazeAns) {
            if (appearJH.on && !appearJH.ing) {
                gameManager.scanObj = null;
                appearJH.on = false;
                StartCoroutine(SMAppear());
            }
            
            if (JihyeonLogic.isOff) {
                Jihyeon.SetActive(false);
                Maze();
            }
        }

        if (ans == switchManager.A1MazeAns) {
            ans = "";
            gameManager.OffPlayer(player);
            gameManager.ChangeMap(5, "P_Jaeha", new Vector3(-0.5f, 0, 0), "up", 4);
            gameManager.dontSave = false;
        }
    }

    IEnumerator InitTalk() {
        switchManager.ing = true;
        //=====================================================
        playerLogic.Turn("up");
        yield return new WaitForSeconds(0.5f);
        playerLogic.Turn("right");
        yield return new WaitForSeconds(0.5f);
        playerLogic.Turn("left");
        //-----------------------------------------------------
        sceneManager.PlayTalk(0, 200);
        yield return new WaitUntil(() => gameManager.isTalking == false);
        //=====================================================
        switchManager.ing = false;
        switchManager.switchdata["A1Maze_InitTalk"].off = true;

        switchManager.switchdata["A1MazeOn"].on = true;
    }

    IEnumerator SMAppear()
    {
        if (switchManager.A1MazeAns[turn] == 'L') {
            Jihyeon.transform.position = spawnPoints[0].position;
            Jihyeon.SetActive(true);
            JihyeonLogic.SetWaypoint(2, spawnPoints[0].position, Jihyeon.transform.position + new Vector3(-5, 0, 0));
        } else if (switchManager.A1MazeAns[turn] == 'R') {
            Jihyeon.transform.position = spawnPoints[1].position;
            Jihyeon.SetActive(true);
            JihyeonLogic.SetWaypoint(2, spawnPoints[1].position, Jihyeon.transform.position + new Vector3(5, 0, 0));
        } else if (switchManager.A1MazeAns[turn] == 'U') {
            Jihyeon.transform.position = spawnPoints[2].position;
            Jihyeon.SetActive(true);
            JihyeonLogic.SetWaypoint(2, spawnPoints[2].position, Jihyeon.transform.position + new Vector3(0, 5, 0));
        }

        JihyeonLogic.speed = 5f;
        Jihyeon.SetActive(true);
        JihyeonLogic.isOn = true;
        yield return new WaitUntil(() => JihyeonLogic.isOff == true);
    }

    void Maze()
    {
        appearJH.ing = true;

        if (gameManager.scanObj != null && gameManager.scanObj.tag == "Maze") {
            JihyeonLogic.isOff = false;

            if (switchManager.A1MazeAns[turn] == gameManager.scanObj.name[0]) {
                ans += gameManager.scanObj.name[0];
                turn++;
            } else {
                turn = 0;
                ans = "";
            }

            gameManager.OffPlayer(player);
            gameManager.ChangeMap(6, "P_Jaeha", new Vector3(-0.5f, -3f, 0), "up", 4);
            gameManager.scanObj = null;
            
            appearJH.on = true;
        }

        appearJH.ing = false;
    }
}

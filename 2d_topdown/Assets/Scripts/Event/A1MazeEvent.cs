using System.Collections;
using UnityEngine;

public class A1MazeEvent : MapEvent
{
#region Variables
    string ans = "";
    int turn = 0;
    public Switch appearJH;

    GameObject Jihyeon;
    NPCMove JihyeonLogic;
    
    public Transform[] spawnPoints;
#endregion Variables


#region Unity Methods
    protected override void Start() {
        base.Start();

        Jihyeon = SceneManager.Instance.PlayNPC("N_Jihyeon", spawnPoints[0].position);
        JihyeonLogic = Jihyeon.GetComponent<NPCMove>();

        JihyeonLogic.Turn("right");

        appearJH.on = true;
    }

    void Update() {
        if (SwitchManager.Instance.switchdata["A1Maze_InitTalk"].on) {
            GameManager.Instance.dontSave = true;
            SwitchManager.Instance.switchdata["A1Maze_InitTalk"].on = false;
            playerLogic.Turn("left");
            StartCoroutine(InitTalk());
        }

        if (SwitchManager.Instance.switchdata["A1MazeOn"].on && ans != SwitchManager.Instance.A1MazeAns) {
            if (appearJH.on && !appearJH.ing) {
                GameManager.Instance.scanObj = null;
                appearJH.on = false;
                StartCoroutine(SMAppear());
            }
            
            if (JihyeonLogic.isOff) {
                Jihyeon.SetActive(false);
                Maze();
            }
        }

        if (ans == SwitchManager.Instance.A1MazeAns) {
            ans = "";
            GameManager.Instance.OffPlayer(player);
            GameManager.Instance.ChangeMap(5, "P_Jaeha", new Vector3(-0.5f, 0, 0), "up", 4);
            GameManager.Instance.dontSave = false;
        }
    }
#endregion Unity Methods


#region Event
    IEnumerator InitTalk() {
        SwitchManager.Instance.ing = true;
        //=====================================================
        playerLogic.Turn("up");
        yield return new WaitForSeconds(0.5f);
        playerLogic.Turn("right");
        yield return new WaitForSeconds(0.5f);
        playerLogic.Turn("left");
        //-----------------------------------------------------
        SceneManager.Instance.PlayTalk(0, 200);
        yield return new WaitUntil(() => GameManager.Instance.isTalking == false);
        //=====================================================
        SwitchManager.Instance.ing = false;
        SwitchManager.Instance.switchdata["A1Maze_InitTalk"].off = true;

        SwitchManager.Instance.switchdata["A1MazeOn"].on = true;
    }

    IEnumerator SMAppear()
    {
        if (SwitchManager.Instance.A1MazeAns[turn] == 'L') {
            Jihyeon.transform.position = spawnPoints[0].position;
            Jihyeon.SetActive(true);
            JihyeonLogic.SetWaypoint(2, spawnPoints[0].position, Jihyeon.transform.position + new Vector3(-5, 0, 0));
        } else if (SwitchManager.Instance.A1MazeAns[turn] == 'R') {
            Jihyeon.transform.position = spawnPoints[1].position;
            Jihyeon.SetActive(true);
            JihyeonLogic.SetWaypoint(2, spawnPoints[1].position, Jihyeon.transform.position + new Vector3(5, 0, 0));
        } else if (SwitchManager.Instance.A1MazeAns[turn] == 'U') {
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

        if (GameManager.Instance.scanObj != null && GameManager.Instance.scanObj.tag == "Maze") {
            JihyeonLogic.isOff = false;

            if (SwitchManager.Instance.A1MazeAns[turn] == GameManager.Instance.scanObj.name[0]) {
                ans += GameManager.Instance.scanObj.name[0];
                turn++;
            } else {
                turn = 0;
                ans = "";
            }

            GameManager.Instance.OffPlayer(player);
            GameManager.Instance.ChangeMap(6, "P_Jaeha", new Vector3(-0.5f, -3f, 0), "up", 4);
            GameManager.Instance.scanObj = null;
            
            appearJH.on = true;
        }

        appearJH.ing = false;
    }
#endregion Event
}

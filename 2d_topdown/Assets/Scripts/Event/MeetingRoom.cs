using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeetingRoom : MonoBehaviour
{
    public GameManager gameManager;
    public SceneManager sceneManager;
    public SwitchManager switchManager;
    GameObject player;
    PlayerAction playerLogic;
    //--------------------------------------
    public GameObject Hyeonseok;
    NPCMove HyeonseokLogic;
    public GameObject Sunwoo;
    NPCMove SunwooLogic;

    GameObject npc0, npc1, npc2, npc3, npc4, npc5, npc6, npc7, npc8;
    NPCMove npc0Logic, npc1Logic, npc2Logic, npc3Logic, npc4Logic, npc5Logic, npc6Logic, npc7Logic, npc8Logic;

    bool question1, question2, question3, question4;
    //--------------------------------------
    public Transform[] spawnPoints;
    //======================================
    void Start() {
        player = gameManager.player;
        playerLogic = player.GetComponent<PlayerAction>();

        HyeonseokLogic = Hyeonseok.GetComponent<NPCMove>();
        SunwooLogic = Sunwoo.GetComponent<NPCMove>();


    }

    void SpawnNpcs()
    {
        npc0 = sceneManager.spawnNPC("NPC0", spawnPoints[0].position);
        npc1 = sceneManager.spawnNPC("NPC1", spawnPoints[1].position);
        npc2 = sceneManager.spawnNPC("NPC2", spawnPoints[2].position);
        npc3 = sceneManager.spawnNPC("NPC3", spawnPoints[0].position + new Vector3(0, -1.5f, 0));
        npc4 = sceneManager.spawnNPC("NPC4", spawnPoints[1].position + new Vector3(0, -1.5f, 0));
        npc5 = sceneManager.spawnNPC("NPC5", spawnPoints[2].position + new Vector3(0, -1.5f, 0));
        npc6 = sceneManager.spawnNPC("NPC6", spawnPoints[0].position + new Vector3(0, -3f, 0));
        npc7 = sceneManager.spawnNPC("NPC7", spawnPoints[1].position + new Vector3(0, -3f, 0));
        npc8 = sceneManager.spawnNPC("NPC8", spawnPoints[2].position + new Vector3(0, -3f, 0));

        npc0Logic = npc0.GetComponent<NPCMove>();
        npc1Logic = npc1.GetComponent<NPCMove>();
        npc2Logic = npc2.GetComponent<NPCMove>();
        npc3Logic = npc3.GetComponent<NPCMove>();
        npc4Logic = npc4.GetComponent<NPCMove>();
        npc5Logic = npc5.GetComponent<NPCMove>();
        npc6Logic = npc6.GetComponent<NPCMove>();
        npc7Logic = npc7.GetComponent<NPCMove>();
        npc8Logic = npc8.GetComponent<NPCMove>();

        npc0Logic.Turn("up");
        npc1Logic.Turn("up");
        npc2Logic.Turn("up");
        npc3Logic.Turn("up");
        npc4Logic.Turn("up");
        npc5Logic.Turn("up");
        npc6Logic.Turn("up");
        npc7Logic.Turn("up");
        npc8Logic.Turn("up");
    }

    void Update() {


        if (switchManager.MR_turn2.on) {
            switchManager.MR_turn2.on = false;
            StartCoroutine(SecondExam());
        }

        if (switchManager.MR_turn3.on) {
            switchManager.MR_turn3.on = false;
            StartCoroutine(ThirdExam());
        }
    }

    IEnumerator StartExam()     // 1번째 사람 조사
    {
        switchManager.MR_StartExam.ing = true;
        //=====================================================
        playerLogic.Turn("left");

        StartCoroutine(Move(npc0, npc0Logic));
        StartCoroutine(Move(npc1, npc1Logic));
        StartCoroutine(Move(npc2, npc2Logic));
        StartCoroutine(Move(npc3, npc3Logic));
        StartCoroutine(Move(npc4, npc4Logic));
        StartCoroutine(Move(npc5, npc5Logic));
        StartCoroutine(Move(npc6, npc6Logic));
        StartCoroutine(Move(npc7, npc7Logic));
        StartCoroutine(Move(npc8, npc8Logic));
        //-----------------------------------------------------
        yield return new WaitForSeconds(1f);

        npc0Logic.Turn("right");
        npc1Logic.Turn("right");
        npc2Logic.Turn("right");

        while (!question1 || !question2 || !question3 || !question4) {
            sceneManager.SetSelectBtnTxt("이름", "생년월일", "거주지", "특이사항");
            yield return new WaitUntil(() => gameManager.selectIndex != -1);

            switch (gameManager.selectIndex) {
                case 1:
                    question1 = true;
                    sceneManager.PlayTalk(0, 800);
                    yield return new WaitUntil(() => gameManager.isTalking == false);
                    break;
                case 2:
                    question2 = true;
                    sceneManager.PlayTalk(10, 800);
                    yield return new WaitUntil(() => gameManager.isTalking == false);
                    break;
                case 3:
                    question3 = true;
                    sceneManager.PlayTalk(20, 800);
                    yield return new WaitUntil(() => gameManager.isTalking == false);
                    break;
                case 4:
                    question4 = true;
                    sceneManager.PlayTalk(30, 800);
                    yield return new WaitUntil(() => gameManager.isTalking == false);
                    break;
            }

            gameManager.selectIndex = -1;
        }

        sceneManager.PlayTalk(40, 800);
        yield return new WaitUntil(() => gameManager.isTalking == false);
        //-----------------------------------------------------
        npc0Logic.SetWaypoint(3, npc0.transform.position, npc0.transform.position + new Vector3(-1.5f, 0, 0), npc0.transform.position + new Vector3(-1, -5f, 0));
        npc1Logic.SetWaypoint(3, npc1.transform.position, npc1.transform.position + new Vector3(-1.5f, 0, 0), npc1.transform.position + new Vector3(-1, -5f, 0));
        npc2Logic.SetWaypoint(3, npc2.transform.position, npc2.transform.position + new Vector3(-1.5f, 0, 0), npc2.transform.position + new Vector3(-1, -5f, 0));
        npc0Logic.isOn = true;
        npc1Logic.isOn = true;
        npc2Logic.isOn = true;
        yield return new WaitUntil(() => npc0Logic.isOff == true);
        npc0Logic.isOff = false;
        npc1Logic.isOff = false;
        npc2Logic.isOff = false;

        npc0.SetActive(false);
        npc1.SetActive(false);
        npc2.SetActive(false);
        //-----------------------------------------------------
        question1 = false;
        question2 = false;
        question3 = false;
        question4 = false;
        //=====================================================
        switchManager.MR_StartExam.ing = false;
        switchManager.MR_StartExam.off = true;
        StopAllCoroutines();
        switchManager.MR_turn2.on = true;
    }

    IEnumerator SecondExam()
    {
        switchManager.MR_turn2.ing = true;
        //=====================================================
        npc0 = sceneManager.spawnNPC("NPC0", spawnPoints[0].position + new Vector3(0, -3.5f, 0));
        npc1 = sceneManager.spawnNPC("NPC1", spawnPoints[1].position + new Vector3(0, -3.5f, 0));
        npc2 = sceneManager.spawnNPC("NPC2", spawnPoints[2].position + new Vector3(0, -3.5f, 0));
        //-----------------------------------------------------
        playerLogic.Turn("left");

        StartCoroutine(Move(npc3, npc3Logic));
        StartCoroutine(Move(npc4, npc4Logic));
        StartCoroutine(Move(npc5, npc5Logic));
        StartCoroutine(Move(npc6, npc6Logic));
        StartCoroutine(Move(npc7, npc7Logic));
        StartCoroutine(Move(npc8, npc8Logic));
        StartCoroutine(Move(npc0, npc0Logic));
        StartCoroutine(Move(npc1, npc1Logic));
        StartCoroutine(Move(npc2, npc2Logic));
        //-----------------------------------------------------
        yield return new WaitForSeconds(1f);

        npc3Logic.Turn("right");
        npc4Logic.Turn("right");
        npc5Logic.Turn("right");

        while (!question1 || !question2 || !question3 || !question4) {
            sceneManager.SetSelectBtnTxt("이름", "생년월일", "거주지", "특이사항");
            yield return new WaitUntil(() => gameManager.selectIndex != -1);

            switch (gameManager.selectIndex) {
                case 1:
                    question1 = true;
                    sceneManager.PlayTalk(0, 900);
                    yield return new WaitUntil(() => gameManager.isTalking == false);
                    break;
                case 2:
                    question2 = true;
                    sceneManager.PlayTalk(10, 900);
                    yield return new WaitUntil(() => gameManager.isTalking == false);
                    break;
                case 3:
                    question3 = true;
                    sceneManager.PlayTalk(20, 900);
                    yield return new WaitUntil(() => gameManager.isTalking == false);
                    break;
                case 4:
                    question4 = true;
                    sceneManager.PlayTalk(30, 900);
                    yield return new WaitUntil(() => gameManager.isTalking == false);
                    break;
            }

            gameManager.selectIndex = -1;
        }

        sceneManager.PlayTalk(40, 900);
        yield return new WaitUntil(() => gameManager.isTalking == false);
        //-----------------------------------------------------
        npc3Logic.SetWaypoint(3, npc3.transform.position, npc3.transform.position + new Vector3(-1.5f, 0, 0), npc3.transform.position + new Vector3(-1, -5f, 0));
        npc4Logic.SetWaypoint(3, npc4.transform.position, npc4.transform.position + new Vector3(-1.5f, 0, 0), npc4.transform.position + new Vector3(-1, -5f, 0));
        npc5Logic.SetWaypoint(3, npc5.transform.position, npc5.transform.position + new Vector3(-1.5f, 0, 0), npc5.transform.position + new Vector3(-1, -5f, 0));
        npc3Logic.isOn = true;
        npc4Logic.isOn = true;
        npc5Logic.isOn = true;
        yield return new WaitUntil(() => npc3Logic.isOff == true);
        npc3Logic.isOff = false;
        npc4Logic.isOff = false;
        npc5Logic.isOff = false;

        npc3.SetActive(false);
        npc4.SetActive(false);
        npc5.SetActive(false);
        //-----------------------------------------------------
        question1 = false;
        question2 = false;
        question3 = false;
        question4 = false;
        //=====================================================
        switchManager.MR_turn2.ing = false;
        switchManager.MR_turn2.off = true;
        StopAllCoroutines();
        switchManager.MR_turn3.on = true;
    }

    IEnumerator ThirdExam()
    {
        switchManager.MR_turn3.ing = true;
        //=====================================================
        npc3 = sceneManager.spawnNPC("NPC3", spawnPoints[0].position + new Vector3(0, -3.5f, 0));
        npc4 = sceneManager.spawnNPC("NPC4", spawnPoints[1].position + new Vector3(0, -3.5f, 0));
        npc5 = sceneManager.spawnNPC("NPC5", spawnPoints[2].position + new Vector3(0, -3.5f, 0));
        //-----------------------------------------------------
        StartCoroutine(Move(npc6, npc6Logic));
        StartCoroutine(Move(npc7, npc7Logic));
        StartCoroutine(Move(npc8, npc8Logic));
        StartCoroutine(Move(npc0, npc0Logic));
        StartCoroutine(Move(npc1, npc1Logic));
        StartCoroutine(Move(npc2, npc2Logic));
        StartCoroutine(Move(npc3, npc3Logic));
        StartCoroutine(Move(npc4, npc4Logic));
        StartCoroutine(Move(npc5, npc5Logic));

        yield return new WaitForSeconds(5f);

        npc6Logic.SetWaypoint(3, npc6.transform.position, npc6.transform.position + new Vector3(-1.5f, 0, 0), npc6.transform.position + new Vector3(-1, -5f, 0));
        npc7Logic.SetWaypoint(3, npc7.transform.position, npc7.transform.position + new Vector3(-1.5f, 0, 0), npc7.transform.position + new Vector3(-1, -5f, 0));
        npc8Logic.SetWaypoint(3, npc8.transform.position, npc8.transform.position + new Vector3(-1.5f, 0, 0), npc8.transform.position + new Vector3(-1, -5f, 0));
        npc6Logic.isOn = true;
        npc7Logic.isOn = true;
        npc8Logic.isOn = true;
        yield return new WaitUntil(() => npc6Logic.isOff == true);
        npc6Logic.isOff = false;
        npc7Logic.isOff = false;
        npc8Logic.isOff = false;

        npc6.SetActive(false);
        npc7.SetActive(false);
        npc8.SetActive(false);
        //=====================================================
        switchManager.MR_turn3.ing = false;
        switchManager.MR_turn3.off = true;
        StopAllCoroutines();
    }

    
    IEnumerator Move(GameObject _who, NPCMove _logic)
    {
        _logic.SetWaypoint(2, _who.transform.position, _who.transform.position + new Vector3(0, 1.8f, 0));
        _logic.isOn = true;
        yield return new WaitUntil(() => _logic.isOff == true);
        _logic.isOff = false;
    }
}

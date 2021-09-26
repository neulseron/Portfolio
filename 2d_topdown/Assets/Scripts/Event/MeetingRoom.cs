using System.Collections;
using UnityEngine;

public class MeetingRoom : MapEvent
{
#region Variables
    public GameObject Hyeonseok;
    NPCMove HyeonseokLogic;
    public GameObject Sunwoo;
    NPCMove SunwooLogic;

    GameObject npc0, npc1, npc2, npc3, npc4, npc5, npc6, npc7, npc8;
    NPCMove npc0Logic, npc1Logic, npc2Logic, npc3Logic, npc4Logic, npc5Logic, npc6Logic, npc7Logic, npc8Logic;

    bool question1, question2, question3, question4;

    public Transform[] spawnPoints;
#endregion Variables


#region Unity Methods
    protected override void Start() {
        base.Start();

        HyeonseokLogic = Hyeonseok.GetComponent<NPCMove>();
        SunwooLogic = Sunwoo.GetComponent<NPCMove>();
    }

    void Update() {
        if (SwitchManager.Instance.MR_turn2.on) {
            SwitchManager.Instance.MR_turn2.on = false;
            StartCoroutine(SecondExam());
        }

        if (SwitchManager.Instance.MR_turn3.on) {
            SwitchManager.Instance.MR_turn3.on = false;
            StartCoroutine(ThirdExam());
        }
    }
#endregion Unity Methods


#region Event
    IEnumerator SecondExam()
    {
        SwitchManager.Instance.MR_turn2.ing = true;
        //=====================================================
        npc0 = SceneManager.Instance.spawnNPC("NPC0", spawnPoints[0].position + new Vector3(0, -3.5f, 0));
        npc1 = SceneManager.Instance.spawnNPC("NPC1", spawnPoints[1].position + new Vector3(0, -3.5f, 0));
        npc2 = SceneManager.Instance.spawnNPC("NPC2", spawnPoints[2].position + new Vector3(0, -3.5f, 0));
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
            SceneManager.Instance.SetSelectBtnTxt("이름", "생년월일", "거주지", "특이사항");
            yield return new WaitUntil(() => GameManager.Instance.selectIndex != -1);

            switch (GameManager.Instance.selectIndex) {
                case 1:
                    question1 = true;
                    SceneManager.Instance.PlayTalk(0, 900);
                    yield return new WaitUntil(() => GameManager.Instance.isTalking == false);
                    break;
                case 2:
                    question2 = true;
                    SceneManager.Instance.PlayTalk(10, 900);
                    yield return new WaitUntil(() => GameManager.Instance.isTalking == false);
                    break;
                case 3:
                    question3 = true;
                    SceneManager.Instance.PlayTalk(20, 900);
                    yield return new WaitUntil(() => GameManager.Instance.isTalking == false);
                    break;
                case 4:
                    question4 = true;
                    SceneManager.Instance.PlayTalk(30, 900);
                    yield return new WaitUntil(() => GameManager.Instance.isTalking == false);
                    break;
            }

            GameManager.Instance.selectIndex = -1;
        }

        SceneManager.Instance.PlayTalk(40, 900);
        yield return new WaitUntil(() => GameManager.Instance.isTalking == false);
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
        SwitchManager.Instance.MR_turn2.ing = false;
        SwitchManager.Instance.MR_turn2.off = true;
        StopAllCoroutines();
        SwitchManager.Instance.MR_turn3.on = true;
    }

    IEnumerator ThirdExam()
    {
        SwitchManager.Instance.MR_turn3.ing = true;
        //=====================================================
        npc3 = SceneManager.Instance.spawnNPC("NPC3", spawnPoints[0].position + new Vector3(0, -3.5f, 0));
        npc4 = SceneManager.Instance.spawnNPC("NPC4", spawnPoints[1].position + new Vector3(0, -3.5f, 0));
        npc5 = SceneManager.Instance.spawnNPC("NPC5", spawnPoints[2].position + new Vector3(0, -3.5f, 0));
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
        SwitchManager.Instance.MR_turn3.ing = false;
        SwitchManager.Instance.MR_turn3.off = true;
        StopAllCoroutines();
    }

    
    IEnumerator Move(GameObject _who, NPCMove _logic)
    {
        _logic.SetWaypoint(2, _who.transform.position, _who.transform.position + new Vector3(0, 1.8f, 0));
        _logic.isOn = true;
        yield return new WaitUntil(() => _logic.isOff == true);
        _logic.isOff = false;
    }
#endregion Event
}

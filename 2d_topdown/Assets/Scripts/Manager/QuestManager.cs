using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int questId;
    public int questActionIndex;
    public GameObject[] questObj;

    Dictionary<int, QuestData> questList;

    // Start is called before the first frame update
    void Awake()
    {
        questList = new Dictionary<int, QuestData>();
        GenerateData();
    }

    // Update is called once per frame
    void GenerateData()
    {
        questList.Add(10, new QuestData("첫 마을 방문", new int[] { 1000, 200 }));
        questList.Add(20, new QuestData("루도의 동전찾기", new int[] { 2000, 5000, 2000 }));
        questList.Add(30, new QuestData("퀘스트 끝", new int[] { 0 }));
    }

    public int GetQuestDialogIndex(int id)
    {
        return questId + questActionIndex;
    }

    public string CheckQuest(int id)
    {
        if (id == questList[questId].npcId[questActionIndex])
            questActionIndex++;

        ControlObj();

        if (questActionIndex == questList[questId].npcId.Length)
            NextQuest();

        return questList[questId].questName;
    }

    public string CheckQuest()
    {
        return questList[questId].questName;
    }

    void NextQuest()
    {
        questId += 10;
        questActionIndex = 0;
    }

    public void ControlObj()
    {
        switch(questId) {
            case 10:
                if (questActionIndex == 2)
                    questObj[0].SetActive(true);
                break;
            case 20:
                if (questActionIndex == 0)
                    questObj[0].SetActive(true);
                else if (questActionIndex == 2)
                    questObj[0].SetActive(false);
                break;
        }
    }
}

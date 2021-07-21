using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    static QuestManager instance;
    public static QuestManager Instance => instance;

    public QuestDBObject questDB;

    public event Action<QuestObject> OnCompletedQuest;


    void Awake() => instance = this;

    public void ProcessQuest(QuestType type, int targetId)
    {
        foreach (QuestObject quest in questDB.questObjects) {
            if (quest.status == QuestStatus.Accepted && quest.data.type == type &&
            quest.data.targetID == targetId) {
                quest.data.completeCnt++;

                if (quest.data.completeCnt >= quest.data.count) {
                    quest.status = QuestStatus.Completed;
                    OnCompletedQuest?.Invoke(quest);
                }
            }
        }
    }
}

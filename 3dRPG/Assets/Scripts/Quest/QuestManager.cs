using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class QuestManager : MonoBehaviour
{
    #region Singletone
    static QuestManager instance;
    public static QuestManager Instance => instance;
    #endregion Singletone

    public QuestDBObject questDB;

    public event Action<QuestObject> OnCompletedQuest;

    List<int> questIdList = new List<int>();
    List<QuestStatus> questStatusList = new List<QuestStatus>();


    void Awake() => instance = this;

    public void ProcessQuest(QuestType type, int targetId)
    {
        foreach (QuestObject quest in questDB.questObjects) {
            if (quest.status == QuestStatus.Accepted && quest.data.type == type &&
            quest.data.targetID == targetId) {
                quest.data.count++;

        Debug.Log("complete : " + quest.data.completeCnt + ", cnt : " + quest.data.count);
                if (quest.data.completeCnt <= quest.data.count) {
                    Debug.Log("클리어");
                    quest.status = QuestStatus.Completed;
                    OnCompletedQuest?.Invoke(quest);
                }
            }
        }
    }

#region Save/Load
    public string savePath;

    public void Save()
    {
        questIdList.Clear();    
        questStatusList.Clear();    
        foreach (QuestObject quest in questDB.questObjects) {
            questIdList.Add(quest.data.id);
            questStatusList.Add(quest.status);
        }

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath + "Id"),
            FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, questIdList);
        stream.Close();


        IFormatter formatter2 = new BinaryFormatter();
        Stream stream2 = new FileStream(string.Concat(Application.persistentDataPath, savePath + "Status"),
            FileMode.Create, FileAccess.Write);
        formatter2.Serialize(stream2, questStatusList);
        stream2.Close();
    }

    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath + "Id"))) {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath + "Id"),
                FileMode.Open, FileAccess.Read);

            List<int> newQuestIdList = (List<int>)formatter.Deserialize(stream);
            

        //}
        //if (File.Exists(string.Concat(Application.persistentDataPath, savePath + "Status"))) {
            IFormatter formatter2 = new BinaryFormatter();
            Stream stream2 = new FileStream(string.Concat(Application.persistentDataPath, savePath + "Status"),
                FileMode.Open, FileAccess.Read);

            List<QuestStatus> newQuestStatusList = (List<QuestStatus>)formatter2.Deserialize(stream2);
            for (int i = 0; i < questDB.questObjects.Length; i++) {
                if (questDB.questObjects[i].data.id == newQuestIdList[i] &&
                    questDB.questObjects[i].status != newQuestStatusList[i]) {
                        questDB.questObjects[i].status = newQuestStatusList[i];
                }
            }

            stream.Close();
            stream2.Close();
        }
    }
#endregion Save/Load

#region ContextMenu
    [ContextMenu("초기화")]
    public void Init()
    {
        foreach (QuestObject quest in questDB.questObjects) {
            quest.status = QuestStatus.None;
        }
    }
#endregion ContextMenu
}

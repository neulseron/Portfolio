using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestDB", menuName = "Quest System/Quests/New Quest Database")]
public class QuestDBObject : ScriptableObject
{
    public QuestObject[] questObjects;

    public void OnValidate() {
        for (int i = 0; i < questObjects.Length; i++) {
            questObjects[i].data.id = i;
        }
    }
}

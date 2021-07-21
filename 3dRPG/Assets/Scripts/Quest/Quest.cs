using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestType { DestroyEnemy, AcquireItem, }

[Serializable]
public class Quest
{
    public int id;
    public QuestType type;

    public int targetID;
    public int count;
    public int completeCnt;

    public int rewardExp;
    public int rewardGold;
    public int rewardItemId;

    public string title;
    public string description;
}

using System;

public enum QuestType { DestroyEnemy, AcquireItem, Main, }

[Serializable]
public class Quest
{
    public int id;
    public QuestType type;

    public int targetID;
    public int count;
    public int completeCnt;

    public int rewardItemId = -1;

    public string title;
    public string description;
}

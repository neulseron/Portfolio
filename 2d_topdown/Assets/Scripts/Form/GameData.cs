using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GameData
{
    // ** 플레이어 **
    public float playerX = 25f;
    public float playerY = 5f;
    public string playerName = "P_Jaei";
    // ** 맵 **
    public int mapIndex = 0;
    public int time = -1;
    // ** 아이템 **
    public List<int> itemList = new List<int>();
    // ** 스위치 **
    public List<SwitchData> switchList = new List<SwitchData>();
}

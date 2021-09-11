using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public int Health = 8;
    public int maxHealth = 8;

    public float posX = 0;
    public float posY = 1;
    public float posZ = -2;

    public List<int> chkGetItem = new List<int>();
}

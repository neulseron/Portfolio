using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class ItemBuff : IModifier
{
    public AttributeType stat;
    public int value;

    [SerializeField]
    int min, max;

    public int Min => min;
    public int Max => max;

    public ItemBuff(int _min, int _max) 
    {
        this.min = _min;
        this.max = _max + 1;

        GenerateValue();
    }

    public void GenerateValue()
    {
        value = UnityEngine.Random.Range(min, max);
        Debug.Log("[ItemBuff] value : " + value);
    }

    public void AddValue(ref int v)
    {
        Debug.Log("[ItemBuff/AddValue] Buff : " + stat + "/" + value);
        v += value;
    }
}

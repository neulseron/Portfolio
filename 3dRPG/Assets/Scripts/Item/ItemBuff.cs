using System;
using UnityEngine;


[Serializable]
public class ItemBuff : IModifier
{
#region Variables
    public AttributeType stat;
    public int value;

    [SerializeField]
    int min, max;

    public int Min => min;
    public int Max => max;
#endregion Variables


    public ItemBuff(int _min, int _max) 
    {
        this.min = _min;
        this.max = _max + 1;

        GenerateValue();
    }


#region Methods
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
#endregion Methods
}

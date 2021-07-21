using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory
{
    public InventorySlot[] slots = new InventorySlot[24];

    public void Clear()
    {
        foreach (InventorySlot slot in slots) {
            slot.RemoveItem();
        }
    }

    public bool IsContain(ItemObject _itemObject) 
    {
        return IsContain(_itemObject.data.id);
        //return Array.Find(slots, i => i.item.data.id == _itemObject.data.id) != null;
    }
    public bool IsContain(int id)
    {
        return slots.FirstOrDefault(i => i.item.id == id) != null;
    }
}

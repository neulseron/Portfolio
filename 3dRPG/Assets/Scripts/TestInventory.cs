using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInventory : MonoBehaviour
{
    public InventoryObject inventoryObject;
    public ItemObjectDB databaseObject;

    public void AddNewItem()
    {
        if (databaseObject.itemObjects.Length > 0) {
            ItemObject newItemObj = databaseObject.itemObjects[Random.Range(0, databaseObject.itemObjects.Length)];
            Item newItem = new Item(newItemObj);
            inventoryObject.AddItem(newItem, 1);
        }
    }

    public void ClearInventory()
    {
        inventoryObject.Clear();
    }
}

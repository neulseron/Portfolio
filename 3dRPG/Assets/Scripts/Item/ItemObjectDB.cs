using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item DB", menuName = "Inventory System/Items/DB")]
public class ItemObjectDB : ScriptableObject
{
    public ItemObject[] itemObjects;

    private void OnValidate() {
        for (int i = 0; i < itemObjects.Length; i++) {
            itemObjects[i].data.id = i;
        }
    }
}

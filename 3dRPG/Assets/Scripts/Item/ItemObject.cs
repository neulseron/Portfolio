using System.Collections.Generic;
using UnityEngine;

public enum ItemType: int   
{ 
    Helmet = 0, 
    Armor = 1, 
    Boots = 2, 
    LeftWeapon = 3,
    RightWeapon = 4,
    Gem,
    Food,
    Key,
    Default
    }

[CreateAssetMenu(fileName = "New Item", menuName ="Inventory System/Items/New Item")]
public class ItemObject : ScriptableObject
{
    public ItemType type;
    public bool stackable;

    public Sprite icon;
    public GameObject modelPrefab;

    public Item data = new Item();

    public int amount = 1;

    public List<string> boneNames = new List<string>();

    [TextArea(15, 20)]
    public string description;

    
    void OnValidate() {
        boneNames.Clear();

        if (modelPrefab == null || modelPrefab.GetComponentInChildren<SkinnedMeshRenderer>() == null) {
            return;
        }

        SkinnedMeshRenderer renderer = modelPrefab.GetComponentInChildren<SkinnedMeshRenderer>();
        Transform[] bones = renderer.bones;
        foreach (Transform t in bones) {
            boneNames.Add(t.name);
        }
    }

    public Item CreateItem()
    {
        Item newItem = new Item(this);
        return newItem;
    }
}

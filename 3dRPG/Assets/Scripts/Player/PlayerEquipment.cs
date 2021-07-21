using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public InventoryObject equipment;
    EquipmentCombiner combiner;
    ItemInstances[] itemInstances = new ItemInstances[5];
    public ItemObject[] defaultItemObjects = new ItemObject[5];
    public GameObject equipPoint;

    void Awake() {
        combiner = new EquipmentCombiner(gameObject);

        for (int i = 0; i < equipment.Slots.Length; i++) {
            equipment.Slots[i].OnPreUpdate += OnRemoveItem;
            equipment.Slots[i].OnPostUpdate += OnEquipItem;
        }
    }

    void Start()
    {
        foreach (InventorySlot slot in equipment.Slots) {
            OnEquipItem(slot);
        }
    }


    void OnDestroy() {
        foreach (ItemInstances item in itemInstances) {
            item?.Destroy();
        }
    }

    void OnEquipItem(InventorySlot slot)
    {
        ItemObject itemObject = slot.ItemObject;

        if (itemObject == null) {
            EquipDefaultItem(slot.allowedItems[0]);
            return;
        }

        int index = (int)slot.allowedItems[0];
        switch (slot.allowedItems[0]) {
            case ItemType.LeftWeapon:
            case ItemType.RightWeapon:
                itemInstances[index] = EquipMeshItem(itemObject);
                break;
        }
    }

    void EquipDefaultItem(ItemType type)
    {
        int index = (int)type;

        ItemObject itemObject = defaultItemObjects[index];
        switch (type) {
            case ItemType.LeftWeapon:
            case ItemType.RightWeapon:
                itemInstances[index] = EquipMeshItem(itemObject);
                break;
        }
    }

    ItemInstances EquipMeshItem(ItemObject itemObject)
    {
        if (itemObject == null)     return null;

        Transform itemTransform = combiner.AddMesh(itemObject.modelPrefab, itemObject.type);
        if (itemTransform != null) {
            ItemInstances instance = new GameObject().AddComponent<ItemInstances>();
            //instance.itemTransforms.AddRange(itemTransform.ToList<Transform>());
            instance.itemTransforms.Add(itemTransform);

            return instance;
        }

        return null;
    }

    void OnRemoveItem(InventorySlot slot)
    {
        ItemObject itemObject = slot.ItemObject;
        if (itemObject == null) {
            RemoveItemBy(slot.allowedItems[0]);
            return;
        }

        if (slot.ItemObject.modelPrefab != null) {
            RemoveItemBy(slot.allowedItems[0]);
        }
    }

    void RemoveItemBy(ItemType type)
    {
        int index = (int)type;
        if (itemInstances[index] != null) {
            itemInstances[index].Destroy();
            itemInstances[index] = null;
        }
    }
}

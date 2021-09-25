using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
#region Variables
    public InventoryObject equipment;
    EquipmentCombiner combiner;
    ItemInstances[] itemInstances = new ItemInstances[5];
    public ItemObject[] defaultItemObjects = new ItemObject[5];
#endregion Variables


#region Unity Methods
    void Awake() {
        combiner = new EquipmentCombiner(gameObject);
    }

    void Start()
    {
        for (int i = 0; i < equipment.Slots.Length; i++) {
            equipment.Slots[i].OnPreUpdate += OnRemoveItem;
            equipment.Slots[i].OnPostUpdate += OnEquipItem;
        }

        Debug.Log("==========[Player Equipment]==========");
        foreach (InventorySlot slot in equipment.Slots) {
            OnEquipItem(slot);
        }
        Debug.Log("==============================");
    }

    void OnDestroy() {
        foreach (ItemInstances item in itemInstances) {
            item?.Destroy();
        }
    }
#endregion Unity Methods


#region Methods
    void OnEquipItem(InventorySlot slot)
    {
        ItemObject itemObject = slot.ItemObject;

        if (itemObject == null)     return;

        int index = (int)slot.allowedItems[0];
        switch (slot.allowedItems[0]) {
            case ItemType.Helmet:
            case ItemType.Boots:
            case ItemType.Armor:
                equipment.OnUseItem(itemObject);
                break;
            case ItemType.LeftWeapon:   // 방패
                itemInstances[index] = EquipMeshItem(itemObject);
                equipment.OnUseItem(itemObject);
                break;
            case ItemType.RightWeapon:  // 무기
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
            equipment.OnRemoveItem(itemObject);
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
#endregion Methods
}

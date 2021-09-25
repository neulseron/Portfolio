using UnityEngine;
using System.Linq;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public enum InterfaceType { Inventory, Equipment, QuickSlot, Box, }

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public ItemObjectDB dB;
    public InterfaceType type;

    [SerializeField]
    Inventory container = new Inventory();
    public Inventory Container => container;

    [SerializeField]
    StatsObject playerStats;

    public Action<ItemObject> OnUseItem;
    public Action<ItemObject> OnRemoveItem;

    public InventorySlot[] Slots => container.slots;
    public int EmptySlotCnt 
    {
        get {
            int cnt = 0;
            foreach (InventorySlot slot in Slots) {
                if (slot.item.id < 0)   cnt++;
            }
            return cnt;
        }
    }


#region Methods
    public bool AddItem(Item _item, int _amount)
    {
        if (EmptySlotCnt <= 0)      return false;

        InventorySlot slot = FindItemInInventory(_item);
        if (!dB.itemObjects[_item.id].stackable || slot == null) {
            GetEmptySlot().AddItem(_item, _amount);
        } else {    // 여러개 가질 수 있는 아이템
            slot.AddAmount(_amount);
        }

        QuestManager.Instance.ProcessQuest(QuestType.AcquireItem, _item.id);

        return true;
    }

    public bool AddGem(Item item)
    {
        if (item.id == 14) {
            container.slots[0].AddItem(item, 1);
        } else if (item.id == 15) {
            container.slots[1].AddItem(item, 1);
        } else if (item.id == 7) {
            container.slots[2].AddItem(item, 1);
        }

        return true;
    }

    public InventorySlot FindItemInInventory(Item _item)
    {
        return Slots.FirstOrDefault(i => i.item.id == _item.id);
    }

    public InventorySlot GetEmptySlot()
    {
        return Slots.FirstOrDefault(i => i.item.id < 0);
    }

    public bool IsContainItem(ItemObject _itemObject)
    {
        return Slots.FirstOrDefault(i => i.item.id == _itemObject.data.id) != null;
    }

    public void SwapItems(InventorySlot itemSlotA, InventorySlot itemSlotB)
    {
        if (itemSlotA == itemSlotB)     return;

        if (itemSlotB.CanPlaceInSlot(itemSlotA.ItemObject) && itemSlotA.CanPlaceInSlot(itemSlotB.ItemObject)) {
            InventorySlot tmp = new InventorySlot(itemSlotB.item, itemSlotB.amount);

            itemSlotB.UpdateSlot(itemSlotA.item, itemSlotA.amount);
            itemSlotA.UpdateSlot(tmp.item, tmp.amount);
        }
    }

    public void UseItem(InventorySlot slotToUse)
    {
        if (slotToUse.ItemObject == null || slotToUse.item.id < 0)     return;

        ItemObject itemObject = slotToUse.ItemObject;
        slotToUse.UpdateSlot(slotToUse.item, slotToUse.amount - 1);

        OnUseItem.Invoke(itemObject);

        if (slotToUse.amount <= 0) {
            slotToUse.RemoveItem();
            return;
        }
    }

    [ContextMenu("Clear")]
    public void Clear() => container.Clear(playerStats);
#endregion Methods


#region Save/Load
    public string savePath;

    [ContextMenu("Save")]
    public void Save()
    {
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath),
            FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, container);
        stream.Close();
    }

    [ContextMenu("Load")]
    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath))) {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath),
                FileMode.Open, FileAccess.Read);
            Inventory newContainer = (Inventory)formatter.Deserialize(stream);
            for (int i = 0; i < Slots.Length; i++) {
                Slots[i]?.UpdateSlot(newContainer.slots[i].item, newContainer.slots[i].amount);
            }
            stream.Close();
        }
    }
#endregion Save/Load
}

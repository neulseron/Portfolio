using System;
using UnityEngine;

[Serializable]
public class InventorySlot
{
#region Variables
    public ItemType[] allowedItems = new ItemType[0];

    [NonSerialized]
    public InventoryObject parent;
    [NonSerialized]
    public GameObject slotUI;

    [NonSerialized]
    public Action<InventorySlot> OnPreUpdate;
    [NonSerialized]
    public Action<InventorySlot> OnPostUpdate;

    public Item item;
    public int amount;

    public ItemObject ItemObject
    {
        get {
            return item.id >= 0 ? parent?.dB.itemObjects[item.id] : null;
        }
    }
#endregion Variables


    public InventorySlot() => UpdateSlot(new Item(), 0);
    public InventorySlot(Item _item, int _amount) => UpdateSlot(_item, _amount);


#region Methods
    public void AddItem(Item _item, int _amount) => UpdateSlot(_item, _amount);
    
    public void RemoveItem() => UpdateSlot(new Item(), 0);

    public void AddAmount(int val) => UpdateSlot(item, amount += val);

    public void UpdateSlot(Item _item, int _amount)
    {
        if (amount <= 0) {
            item = new Item();
        }

        OnPreUpdate?.Invoke(this);      // <== 여기서 적용

        item = _item;
        amount = _amount;

        OnPostUpdate?.Invoke(this);
    }

    public bool CanPlaceInSlot(ItemObject _itemObject)
    {
        if (allowedItems.Length <= 0 || _itemObject == null || _itemObject.data.id < 0)     return true;

        foreach (ItemType type in allowedItems) {
            if (_itemObject.type == type) {
                return true;
            }
        }

        return false;
    }
#endregion Methods
}

using System;
using System.Linq;

[Serializable]
public class Inventory
{
    public InventorySlot[] slots = new InventorySlot[24];

#region Methods
    public void Clear(StatsObject playerStats)
    {
        foreach (InventorySlot slot in slots) {
            if (slot.item.buffs != null && slot.parent.type == InterfaceType.Equipment) {
                foreach (ItemBuff buff in slot.item.buffs) {
                    foreach (Attribute attribute in playerStats.attributes) {
                        if (attribute.type == buff.stat) {
                            attribute.value.RemoveModifier(buff);
                        }
                    }
                }
            }
        }

        foreach (InventorySlot slot in slots) {
            slot.RemoveItem();
        }
    }

    public bool IsContain(ItemObject _itemObject) 
    {
        return IsContain(_itemObject.data.id);
    }

    public bool IsContain(int id)
    {
        return slots.FirstOrDefault(i => i.item.id == id) != null;
    }
#endregion Methods
}

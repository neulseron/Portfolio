using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GemInventoryUI : InventoryUI
{
    [SerializeField]
    protected GameObject[] gemSlots;

    public override void CreateSlotUIs()
    {
        slotUIs = new Dictionary<GameObject, InventorySlot>();

        for (int i = 0; i < inventoryObject.Slots.Length; i++) {
            GameObject slot = gemSlots[i];

            inventoryObject.Slots[i].slotUI = slot;
            slotUIs.Add(slot, inventoryObject.Slots[i]);
        }
    }


    public override void OnPostUpdate(InventorySlot slot)
    {
        //base.OnPostUpdate(slot);
        slot.slotUI.transform.GetChild(0).GetComponent<Image>().sprite = slot.item.id < 0 ? null : slot.ItemObject.icon;
        slot.slotUI.transform.GetChild(0).GetComponent<Image>().color = slot.item.id < 0 ? new Color(1, 1, 1, 0) : new Color(1, 1, 1, 1);
    }
}

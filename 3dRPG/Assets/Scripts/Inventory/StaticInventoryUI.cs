using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StaticInventoryUI : InventoryUI
{
    [SerializeField]
    protected GameObject[] staticSlots;

    public override void CreateSlotUIs()
    {
        slotUIs = new Dictionary<GameObject, InventorySlot>();

        for (int i = 0; i < inventoryObject.Slots.Length; i++) {
            GameObject slot = staticSlots[i];

            AddEvent(slot, EventTriggerType.PointerEnter, delegate { OnEnterSlot(slot); });
            AddEvent(slot, EventTriggerType.PointerExit, delegate { OnExitSlot(slot); });
            AddEvent(slot, EventTriggerType.BeginDrag, delegate { OnStartDrag(slot); });
            AddEvent(slot, EventTriggerType.EndDrag, delegate { OnEndDrag(slot); });
            AddEvent(slot, EventTriggerType.Drag, delegate { OnDrag(slot); });

            inventoryObject.Slots[i].slotUI = slot;
            slotUIs.Add(slot, inventoryObject.Slots[i]);

            slot.name += ": " + i;
        }
    }

    public override void OnEndDrag(GameObject go)
    {
        //base.OnEndDrag(go);
        Destroy(MouseData.tmpItemBeingDragged);

        if (MouseData.interfaceMouseIsOver == null) {
            slotUIs[go].RemoveItem();
        } else if (MouseData.slotHoveredOver) {
            InventorySlot mouseHoverSlotData = MouseData.interfaceMouseIsOver.slotUIs[MouseData.slotHoveredOver];
            inventoryObject.SwapItems(slotUIs[go], mouseHoverSlotData);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicInventoryUI : InventoryUI
{
    [SerializeField]
    protected GameObject slotPrefab;

    [SerializeField]
    protected Vector2 start, size, space;

    [Min(1), SerializeField]
    protected int numOfCol = 4;

    public override void CreateSlotUIs()
    {
        slotUIs = new Dictionary<GameObject, InventorySlot>();

        for (int i = 0; i < inventoryObject.Slots.Length; i++) {
            GameObject slot = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, transform);
            slot.GetComponent<RectTransform>().anchoredPosition = CalculatePosition(i);

            AddEvent(slot, EventTriggerType.PointerEnter, delegate { OnEnterSlot(slot); });
            AddEvent(slot, EventTriggerType.PointerExit, delegate { OnExitSlot(slot); });
            AddEvent(slot, EventTriggerType.BeginDrag, delegate { OnStartDrag(slot); });
            AddEvent(slot, EventTriggerType.EndDrag, delegate { OnEndDrag(slot); });
            AddEvent(slot, EventTriggerType.Drag, delegate { OnDrag(slot); });
            AddEvent(slot, EventTriggerType.PointerClick, data => { OnClick(slot, (PointerEventData)data); });

            inventoryObject.Slots[i].slotUI = slot;
            slotUIs.Add(slot, inventoryObject.Slots[i]);

            slot.name += ": " + i;
        }
    }

    public Vector3 CalculatePosition(int i)
    {
        float x = start.x + ((space.x + size.x) * (i % numOfCol));
        float y = start.y + ((-1) * (space.y + size.y) * (i / numOfCol));

        return new Vector3(x, y, 0);
    }

    protected override void OnRightClick(InventorySlot slot)
    {
        inventoryObject.UseItem(slot);
    }
}

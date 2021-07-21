using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public static class MouseData
{
    public static InventoryUI interfaceMouseIsOver;
    public static GameObject slotHoveredOver;
    public static GameObject tmpItemBeingDragged;
}

[RequireComponent(typeof(EventTrigger))]
public abstract class InventoryUI : MonoBehaviour
{
    public InventoryObject inventoryObject;
    InventoryObject prevInventoryObject;

    public Dictionary<GameObject, InventorySlot> slotUIs = new Dictionary<GameObject, InventorySlot>();

    
    void Awake() {
        CreateSlotUIs();

        for (int i = 0; i < inventoryObject.Slots.Length; i++) {
            inventoryObject.Slots[i].parent = inventoryObject;
            inventoryObject.Slots[i].OnPostUpdate += OnPostUpdate;
        }

        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
    }

    protected virtual void Start()
    {
        for (int i = 0; i < inventoryObject.Slots.Length; i++) {
            inventoryObject.Slots[i].UpdateSlot(inventoryObject.Slots[i].item, inventoryObject.Slots[i].amount);
        }
    }

    public abstract void CreateSlotUIs();

    protected void AddEvent(GameObject go, EventTriggerType type, UnityAction<BaseEventData> action) 
    {
        EventTrigger trigger = go.GetComponent<EventTrigger>();
        if (!trigger) {
            Debug.LogWarning("No EventTrigger Component found!");
            return;
        }

        EventTrigger.Entry eventTrigger = new EventTrigger.Entry { eventID = type };
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnPostUpdate(InventorySlot slot)
    {
        slot.slotUI.transform.GetChild(0).GetComponent<Image>().sprite = slot.item.id < 0 ? null : slot.ItemObject.icon;
        slot.slotUI.transform.GetChild(0).GetComponent<Image>().color = slot.item.id < 0 ? new Color(1, 1, 1, 0) : new Color(1, 1, 1, 1);
        slot.slotUI.GetComponentInChildren<TextMeshProUGUI>().text = slot.item.id < 0 ? string.Empty : (slot.amount == 1 ? string.Empty : slot.amount.ToString());
    }


    public void OnEnterInterface(GameObject go)
    {
        MouseData.interfaceMouseIsOver = go.GetComponent<InventoryUI>();
    }

    public void OnExitInterface(GameObject go)
    {
        MouseData.interfaceMouseIsOver = null;
    }


    public void OnEnterSlot(GameObject go)
    {
        MouseData.slotHoveredOver = go;
    }

    public void OnExitSlot(GameObject go)
    {
        MouseData.slotHoveredOver = null;
    }


    public void OnStartDrag(GameObject go)
    {
        MouseData.tmpItemBeingDragged = CreateDragImage(go);
    }

    public void OnDrag(GameObject go)
    {
        if (MouseData.tmpItemBeingDragged == null)      return;

        MouseData.tmpItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
    }

    public virtual void OnEndDrag(GameObject go)
    {
        Destroy(MouseData.tmpItemBeingDragged);

        if (MouseData.interfaceMouseIsOver == null) {
            slotUIs[go].RemoveItem();
        } else if (MouseData.slotHoveredOver) {
            InventorySlot mouseHoverSlotData = MouseData.interfaceMouseIsOver.slotUIs[MouseData.slotHoveredOver];
            inventoryObject.SwapItems(slotUIs[go], mouseHoverSlotData);
        }
    }

    GameObject CreateDragImage(GameObject go)
    {
        if (slotUIs[go].item.id < 0)    return null;

        GameObject dragImageGO = new GameObject();

        RectTransform rectTransform = dragImageGO.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(50, 50);
        dragImageGO.transform.SetParent(transform.parent);  // Canvas

        Image img = dragImageGO.AddComponent<Image>();
        img.sprite = slotUIs[go].ItemObject.icon;
        img.raycastTarget = false;

        dragImageGO.name = "Drag Image";

        return dragImageGO;
    }


    public void OnClick(GameObject go, PointerEventData data)
    {
        InventorySlot slot = slotUIs[go];
        if (slot == null)   return;

        if (data.button == PointerEventData.InputButton.Left) {
            OnLeftClick(slot);
        }

        if (data.button == PointerEventData.InputButton.Right) {
            OnRightClick(slot);
        }
    }

    protected virtual void OnLeftClick(InventorySlot slot)
    {
        
    }

    protected virtual void OnRightClick(InventorySlot slot)
    {

    }
}

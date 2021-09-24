using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour
{
    public GameManager gameManager;
    public CSVManager csvManager;
    public ItemManager itemManager;

    public List<GameObject> allSlots;
    public List<int> itemIndexList;
    public GameObject originalSlot;
    public RectTransform invenRect;
    
    public GameObject infoWindow;
    public Image infoImage;
    public Text infoName;
    public Text infoContent;

    public float slotSize;              // 슬롯의 사이즈.
    public float slotGap;               // 슬롯간 간격.
    public float slotCountX;            // 슬롯의 가로 개수.
    public float slotCountY;            // 슬롯의 세로 개수

    float invenWidth;           // 인벤토리 가로길이.
    float invenHeight;          // 인밴토리 세로길이.
    float emptySlotNum;            // 빈 슬롯의 개수.

    public int clickCnt = 0;
    Slot selectedSlot = null;
    public bool nowUsing;
    public int selectedItemIndex = -1;

    void Awake() {
        invenWidth = (slotCountX * slotSize) + (slotCountX * slotGap) + slotGap;
        invenHeight = (slotCountY * slotSize) + (slotCountY * slotGap) + slotGap;

        invenRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, invenWidth);
        invenRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, invenHeight);

        for (int y = 0; y < slotCountY; y++) {
            for (int x = 0; x < slotCountX; x++) {
                GameObject slot = Instantiate(originalSlot) as GameObject;
                RectTransform slotRect = slot.GetComponent<RectTransform>();
                RectTransform defaultImg = slot.transform.Find("itemImage").GetComponent<RectTransform>();

                slot.name = "slot_" + y + "_" + x;
                slot.transform.SetParent(transform);

                slotRect.localPosition = new Vector3( (slotSize * x) + (slotGap * (x + 1)),
                -((slotSize * y) + (slotGap * (y + 1))), 0);

                slotRect.localScale = Vector3.one;
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);

                defaultImg.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize - slotSize * 0.3f);
                defaultImg.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize - slotSize * 0.3f);

                allSlots.Add(slot);
            }

            emptySlotNum = allSlots.Count;
        }

        itemIndexList = new List<int>();
    }

    public void InitInven(List<int> _list)
    {
        for (int i = 0; i < _list.Count; i++) {
            AddItem(_list[i]);
        }
    }

    public void TempClearInven()
    {
        for (int i = 0; i < allSlots.Count; i++) {
            Slot slot = allSlots[i].GetComponent<Slot>();

            if (slot.ChkEmpty())    // 비어있으면 다 검사한것(앞에서부터 채우니까)
                return;

            slot.UseItem();
        }
    }

    void OnEnable() {
        infoName.text = "-";
        infoImage.sprite = originalSlot.GetComponent<Slot>().defaultImage;
        infoContent.text = "(아이템을 선택해 주십시오.)";

        clickCnt = 0;
        if (selectedSlot != null)
            selectedSlot.borderImage.SetActive(false);
    }

    public bool AddItem(int _id)
    {
        Item item = itemManager.itemDic[_id];
        
        for (int i = 0; i < allSlots.Count; i++) {
            Slot slot = allSlots[i].GetComponent<Slot>();

            // 비어있지 않으면 통과
            if (!slot.ChkEmpty())    continue;

            slot.AddItem(item);
            itemIndexList.Add(item.itemIndex);

            return true;
        }

        return false;
    }

    public bool havingItem(int _itemIndex)
    {
        for (int i = 0; i < allSlots.Count; i++) {
            Slot slot = allSlots[i].GetComponent<Slot>();

            if (slot.ChkEmpty())
                continue;
            
            Item item = slot.ItemReturn();
            if (item.itemIndex == _itemIndex)
                return true;
        }

        return false;
    }

    public void ItemClick()
    {
        clickCnt++;
        GameObject btn = EventSystem.current.currentSelectedGameObject;
        Slot slot = btn.GetComponent<Slot>();

        if (selectedSlot != null && slot != selectedSlot) {
            selectedSlot.borderImage.SetActive(false);
            selectedSlot.itemMenu.SetActive(false);
            selectedSlot = slot;
            clickCnt = 1;
        }
        
        if (slot.ChkEmpty()) {
            clickCnt = 0;
            infoName.text = "-";
            infoImage.sprite = slot.defaultImage;
            infoContent.text = "(아이템을 선택해 주십시오.)";
            return;
        }
        
        slot.borderImage.SetActive(true);
        Item item = slot.ItemReturn();
        infoName.text = item.itemName;
        infoImage.sprite = item.itemImage;
        infoContent.text = csvManager.GetItemInfo(item.itemIndex).Replace("\\n", "\n");

        selectedSlot = slot;
        if (clickCnt == 2 && nowUsing) {
            selectedSlot.itemMenu.SetActive(true);
        }
    }

    public void ClickUse()
    {
        Item item = selectedSlot.ItemReturn();
        selectedItemIndex = item.itemIndex;
        clickCnt--;
        if (item.canUse) {
            itemIndexList.Remove(item.itemIndex);
            selectedSlot.UseItem();
        }
        selectedSlot.itemMenu.SetActive(false);
        gameObject.SetActive(false);
    }

    public void ClickCancel()
    {
        clickCnt--;
        selectedSlot.itemMenu.SetActive(false);
    }

    public void BtnClose()
    {
        gameManager.dontMove = false;
        gameObject.SetActive(false);
    }
}

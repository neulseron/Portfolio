using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Stack<Item> slot;
    public Sprite defaultImage;
    public GameObject borderImage;
    public GameObject itemMenu;

    Inventory inventory;
    Image itemImage;
    bool isEmpty;

    void Start() {
        slot = new Stack<Item>();
        isEmpty = true;

        itemImage = transform.Find("itemImage").GetComponent<Image>();
        inventory = transform.parent.gameObject.GetComponent<Inventory>();
    }

    public void AddItem(Item _item)
    {
        slot.Push(_item);
        UpdateInfo(false, _item.itemImage);
    }

    public void UseItem()
    {
        if (isEmpty)
            return;

        slot.Pop();
        UpdateInfo(true, defaultImage);
    }

    public void UpdateInfo(bool _isEmpty, Sprite _image)
    {
        SetSlot(_isEmpty);
        itemImage.sprite = _image;
        // 파일 데이터에 저장하는 부분
    }

    public Item ItemReturn()    {   return slot.Peek(); }
    public bool ChkEmpty()      {   return isEmpty;     }
    public void SetSlot(bool _isEmpty)  {   isEmpty = _isEmpty; }

}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
#region Variables
    public Stack<Item> slot;
    public Sprite defaultImage;
    public GameObject borderImage;
    public GameObject itemMenu;

    Inventory inventory;
    Image itemImage;
    bool isEmpty;
#endregion Variables


#region Unity Methods
    void Start() {
        slot = new Stack<Item>();
        isEmpty = true;

        itemImage = transform.Find("itemImage").GetComponent<Image>();
        inventory = transform.parent.gameObject.GetComponent<Inventory>();
    }
#endregion Unity Methods


#region Methods
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
    }

    public Item ItemReturn()    {   return slot.Peek(); }
    public bool ChkEmpty()      {   return isEmpty;     }
    public void SetSlot(bool _isEmpty)  {   isEmpty = _isEmpty; }
#endregion Methods
}

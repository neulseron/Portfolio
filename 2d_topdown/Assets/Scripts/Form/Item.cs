using UnityEngine;

public class Item
{
    public int itemIndex;
    public string itemName;
    public bool destroyable;
    public bool canUse;
    public Sprite itemImage;

    public Item(int _id, string _name, bool _destroy, bool _using, Sprite _image)
    {
        itemIndex = _id;
        itemName = _name;
        destroyable = _destroy;
        canUse = _using;
        itemImage = _image;
    }
}

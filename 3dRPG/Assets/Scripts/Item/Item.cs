using System;

[Serializable]
public class Item
{
    public int id = -1;
    public string name;

    public ItemBuff[] buffs;
    
    public Item()
    {
        id = -1;
        name = "";
    }

    public Item(ItemObject _itemObject)
    {
        name = _itemObject.name;
        id = _itemObject.data.id;

        buffs = new ItemBuff[_itemObject.data.buffs.Length];
        for (int i = 0; i < buffs.Length; i++) {
            buffs[i] = new ItemBuff(_itemObject.data.buffs[i].Min, _itemObject.data.buffs[i].Max) {
                stat = _itemObject.data.buffs[i].stat
            };
        }
    }
}

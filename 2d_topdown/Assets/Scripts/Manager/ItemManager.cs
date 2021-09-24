using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Sprite[] itemImages;
    
    public Dictionary<int, Item> itemDic;

    private void Awake() {
        itemDic = new Dictionary<int, Item>();
    }

    private void Start() {
        AddDic(new Item(1, "서재이의 ID", true, false, itemImages[0]));
        AddDic(new Item(2, "서재하의 ID", true, false, itemImages[1]));
        AddDic(new Item(3, "차현석의 ID", true, false, itemImages[2]));
        AddDic(new Item(4, "프로젝트 미아_378951.mia", false, true, itemImages[3]));
    }

    public void AddDic(Item _item)
    {
        bool chk = itemDic.ContainsKey(_item.itemIndex);

        if (!chk) {
            itemDic.Add(_item.itemIndex, _item);
        }
    }
}

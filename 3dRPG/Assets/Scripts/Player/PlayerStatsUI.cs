using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    public InventoryObject equipment;
    public StatsObject playerStats;

    public Text[] attributeTxt;

    void OnEnable() {
        playerStats.OnChangedStats += OnChangedStats;

        if (equipment != null && playerStats != null) {
            foreach (InventorySlot slot in equipment.Slots) {
                slot.OnPreUpdate += OnRemoveItem;
                slot.OnPostUpdate += OnEquipItem;
            }
        }

        UpdateAttributeTxts();
    }

    private void OnDestroy() {
        playerStats.OnChangedStats -= OnChangedStats;

        if (equipment != null && playerStats != null) {
            foreach (InventorySlot slot in equipment.Slots) {
                slot.OnPreUpdate -= OnRemoveItem;
                slot.OnPostUpdate -= OnEquipItem;
            }
        }
    }

    void UpdateAttributeTxts()
    {
        attributeTxt[0].text = playerStats.GetModifiedValue(AttributeType.Agility).ToString("n0");
        attributeTxt[1].text = playerStats.GetModifiedValue(AttributeType.Intellect).ToString("n0");
        attributeTxt[2].text = playerStats.GetModifiedValue(AttributeType.Stamina).ToString("n0");
        attributeTxt[3].text = playerStats.GetModifiedValue(AttributeType.Strength).ToString("n0");
    }

    void OnRemoveItem(InventorySlot slot)
    {
        if (slot.ItemObject == null)    return;

        foreach (ItemBuff buff in slot.item.buffs) {
            foreach (Attribute attribute in playerStats.attributes) {
                if (attribute.type == buff.stat) {
                    attribute.value.RemoveModifier(buff);
                }
            }
        }
    }

    void OnEquipItem(InventorySlot slot)
    {
        if (slot.ItemObject == null)    return;

        foreach (ItemBuff buff in slot.item.buffs) {
            foreach (Attribute attribute in playerStats.attributes) {
                if (attribute.type == buff.stat) {
                    attribute.value.AddModifier(buff);
                }
            }
        }
    }

    void OnChangedStats(StatsObject statsObject)
    {
        UpdateAttributeTxts();
    }
}

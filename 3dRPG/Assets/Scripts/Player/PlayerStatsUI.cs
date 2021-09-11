using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField]
    PlayerCharacter player;
    public InventoryObject equipment;
    public StatsObject playerStats;

    public Text[] attributeTxt;

    void OnEnable() {
        Debug.Log("## 스탯 유아이 Enable ##");
        playerStats.OnChangedStats += OnChangedStats;

        if (equipment != null && playerStats != null) {
            foreach (InventorySlot slot in equipment.Slots) {
                slot.OnPreUpdate += OnRemoveItem;
                slot.OnPostUpdate += OnEquipItem;
            }
        }

        UpdateAttributeTxts();
    }

    void OnDisable() {
        Debug.Log("## 스탯 유아이 Disable ##");
        playerStats.OnChangedStats -= OnChangedStats;

        if (equipment != null && playerStats != null) {
            foreach (InventorySlot slot in equipment.Slots) {
                slot.OnPreUpdate -= OnRemoveItem;
                slot.OnPostUpdate -= OnEquipItem;
            }
        }
    }

    public void UpdateAttributeTxts()
    {
        attributeTxt[0].text = playerStats.GetModifiedValue(AttributeType.Strength).ToString("n0");
        //attributeTxt[0].text = playerStats.Strength.ToString("n0");
    }

    public void OnRemoveItem(InventorySlot slot)
    {
        if (slot.ItemObject == null)    return;
            Debug.Log("^^^^^^^^^^^^^^^^^^^^^^^");

        if (slot.parent.type == InterfaceType.Equipment) {
            foreach (ItemBuff buff in slot.item.buffs) {
                foreach (Attribute attribute in playerStats.attributes) {
                    if (attribute.type == buff.stat) {
                        attribute.value.RemoveModifier(buff);

                        if (attribute.type == AttributeType.MaxHealth) {
                            playerStats.AddHealth(-(buff.value));
                        }
                        Debug.Log("~~~~OnRemoveItem에 입니다~~~~ 현재 MaxHealth : " + playerStats.GetModifiedValue(AttributeType.MaxHealth));
                    }
                }
            }
        }
    }

    void OnEquipItem(InventorySlot slot)
    {
        if (slot.ItemObject == null)    return;
            Debug.Log("^^********************^");

        if (slot.parent.type == InterfaceType.Equipment) {
            foreach (ItemBuff buff in slot.item.buffs) {
                foreach (Attribute attribute in playerStats.attributes) {
                    if (attribute.type == buff.stat) {
                        Debug.Log("item : " + slot.item.name + ", type : " + attribute.type + ", value : " + buff.value);
                        attribute.value.AddModifier(buff);

                        if (player.CurrentAttackBehaviour != null && attribute.type == AttributeType.Strength) {
                            player.CurrentAttackBehaviour.addDamage = buff.value;
                        } else if (attribute.type == AttributeType.MaxHealth) {
                            Debug.Log("~~~~OnEquipItem에 MaxHealth 입니다~~~~ 현재 MaxHealth : " + playerStats.GetModifiedValue(AttributeType.MaxHealth));
                            //playerStats.Health += buff.value;
                            playerStats.AddHealth(buff.value);
                            Debug.Log("~~~~OnEquipItem에 Health 입니다~~~~ 현재 Health : " + playerStats.GetModifiedValue(AttributeType.Health));
                        }
                    }
                }
            }
        }
    }

    void OnChangedStats(StatsObject statsObject)
    {
        UpdateAttributeTxts();
    }
}

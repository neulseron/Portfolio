using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
#region Variables
    [SerializeField]
    PlayerCharacter player;
    public InventoryObject equipment;
    public StatsObject playerStats;

    public Text[] attributeTxt;
#endregion Variables


#region Unity Methods
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
#endregion Unity Methods


#region Methods
    public void UpdateAttributeTxts()
    {
        attributeTxt[0].text = playerStats.GetModifiedValue(AttributeType.Strength).ToString("n0");
    }

    public void OnRemoveItem(InventorySlot slot)
    {
        if (slot.ItemObject == null)    return;

        if (slot.parent.type == InterfaceType.Equipment) {
            foreach (ItemBuff buff in slot.item.buffs) {
                foreach (Attribute attribute in playerStats.attributes) {
                    if (attribute.type == buff.stat) {
                        attribute.value.RemoveModifier(buff);

                        if (attribute.type == AttributeType.MaxHealth) {
                            playerStats.AddHealth(-(buff.value));
                        }
                    }
                }
            }
        }
    }

    void OnEquipItem(InventorySlot slot)
    {
        if (slot.ItemObject == null)    return;

        if (slot.parent.type == InterfaceType.Equipment) {
            foreach (ItemBuff buff in slot.item.buffs) {
                foreach (Attribute attribute in playerStats.attributes) {
                    if (attribute.type == buff.stat) {
                        attribute.value.AddModifier(buff);

                        if (player.CurrentAttackBehaviour != null && attribute.type == AttributeType.Strength) {
                            player.CurrentAttackBehaviour.addDamage = buff.value;
                        } else if (attribute.type == AttributeType.MaxHealth) {
                            playerStats.AddHealth(buff.value);
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
#endregion Methods
}

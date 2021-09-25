using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Stats", menuName ="Stats System/Stats/New Character Stats")]
public class StatsObject : ScriptableObject
{
#region Variables
    public Attribute[] attributes;

    public Action<StatsObject> OnChangedStats;

    public int Health { get; set; }  
    public int MaxHealth { get; set; }


    [NonSerialized]
    bool isInitialize = false;
#endregion Variables


#region Methods
    public void OnEnable()
    {
        InitializeAttribute();

        foreach (Attribute attribute in attributes) {
            if (attribute.type == AttributeType.MaxHealth) {
                MaxHealth = attribute.value.ModifiedValue;
            }
        }
    }

    public void InitializeAttribute()
    {
        if (isInitialize)   return;
        isInitialize = true;

        foreach (Attribute attribute in attributes) {
            attribute.value = new ModifiableInt(OnModifiedValue);
        }

        SetBaseValue(AttributeType.Strength, 5);
        SetBaseValue(AttributeType.Health, 8);
        SetBaseValue(AttributeType.MaxHealth, 8);

        Health = GetModifiedValue(AttributeType.Health);
    }

    public void RestartInitAttribute()
    {
        Health = GetBaseValue(AttributeType.Health);
    }

    void OnModifiedValue(ModifiableInt value)
    {
        OnChangedStats?.Invoke(this);
    }

    public int GetBaseValue(AttributeType type)
    {
        foreach (Attribute attribute in attributes) {
            if (attribute.type == type) {
                return attribute.value.BaseValue;
            }
        }

        return -1;
    }

    public void SetBaseValue(AttributeType type, int value)
    {
        foreach (Attribute attribute in attributes) {
            if (attribute.type == type) {
                attribute.value.BaseValue = value;
            }
        }
    }

    public int GetModifiedValue(AttributeType type)
    {
        foreach (Attribute attribute in attributes) {
            if (attribute.type == type) {
                return attribute.value.ModifiedValue;
            }
        }

        return -1;
    }

    public int AddHealth(int value)
    {
        Health += value;

        OnChangedStats?.Invoke(this);

        return Health;
    }

    public int AddMaxHealth(int value)
    {
        MaxHealth += value;

        OnChangedStats?.Invoke(this);

        return MaxHealth;
    }

    public int RemoveMaxHealth(int value)
    {
        MaxHealth -= value;

        OnChangedStats?.Invoke(this);

        return MaxHealth;
    }
#endregion Methods


#region Save/Load
    public string savePath;

    [ContextMenu("Save")]
    public void Save()
    {
        Debug.Log("[Save] HP : " + Health);
        GameManager.Instance.Data.Health = Health;
        //GameManager.Instance.Data.maxHealth = MaxHealth;
    }

    [ContextMenu("Load")]
    public void Load()
    {
        Health = GameManager.Instance.Data.Health;
        Debug.Log("[Load] HP : " + Health);
        //MaxHealth = GameManager.Instance.Data.maxHealth;
        
        OnChangedStats?.Invoke(this);
    }
#endregion Save/Load
}

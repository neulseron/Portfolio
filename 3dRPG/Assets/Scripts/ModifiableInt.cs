using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ModifiableInt
{
    [NonSerialized]
    int baseValue;

    [SerializeField]
    int modifiedValue;

    public int BaseValue
    {
        get => baseValue;
        set {
            baseValue = value;
            UpdateModifiedValue();
        }
    }

    public int ModifiedValue
    {
        get => modifiedValue;
        set => modifiedValue = value;
    }

    event Action<ModifiableInt> OnModifiedValue;
    List<IModifier> modifiers = new List<IModifier>();


    public ModifiableInt(Action<ModifiableInt> method = null) {
        ModifiedValue = baseValue;
        RegisterModEvent(method);
    }

    public void RegisterModEvent(Action<ModifiableInt> method)
    {
        if (method != null) {
            OnModifiedValue += method;
        }
    }

    public void UnregisterModEvent(Action<ModifiableInt> method)
    {
        if (method != null) {
            OnModifiedValue -= method;
        }
    }

    void UpdateModifiedValue()
    {
        int valueToAdd = 0;
        foreach (IModifier modifier in modifiers) {
            modifier.AddValue(ref valueToAdd);
        }

        ModifiedValue = baseValue + valueToAdd;

        OnModifiedValue.Invoke(this);
    }

    public void AddModifier(IModifier modifier)
    {
        modifiers.Add(modifier);
        UpdateModifiedValue();
    }

    public void RemoveModifier(IModifier modifier)
    {
        modifiers.Remove(modifier);
        UpdateModifiedValue();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ModifiableInt
{
    event Action<ModifiableInt> OnModifiedValue;
    List<IModifier> modifiers = new List<IModifier>();

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

    public void ResetModifier(IModifier modifier)
    {
        modifiedValue = baseValue;
    }

    void UpdateModifiedValue()
    {
        int valueToAdd = 0;
        foreach (IModifier modifier in modifiers) {
            modifier.AddValue(ref valueToAdd);
        }

        ModifiedValue = baseValue + valueToAdd;
        //Debug.Log(baseValue + ", " + valueToAdd + "//");

        OnModifiedValue.Invoke(this);
    }
}

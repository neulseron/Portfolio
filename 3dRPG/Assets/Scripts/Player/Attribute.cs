using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttributeType { Strength, Health, MaxHealth }

[Serializable]
public class Attribute
{
    public AttributeType type;
    public ModifiableInt value;
}

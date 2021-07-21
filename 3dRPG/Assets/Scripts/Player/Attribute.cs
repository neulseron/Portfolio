using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttributeType { Agility, Intellect, Stamina, Strength, Health, Mana, }

[Serializable]
public class Attribute
{
    public AttributeType type;
    public ModifiableInt value;
}

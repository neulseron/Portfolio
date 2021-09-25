using System;

public enum AttributeType { Strength, Health, MaxHealth }

[Serializable]
public class Attribute
{
    public AttributeType type;
    public ModifiableInt value;
}

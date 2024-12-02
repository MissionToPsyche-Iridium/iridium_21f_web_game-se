using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProbeComponentType
{
    Standard,
    Custom
}

public class ProbeComponent
{
    public string Id { get; private set; }
    public string Name { get; private set; }
    public ProbeComponentType Type { get; private set; }
    public string Description { get; private set; }
    public Sprite Sprite { get; private set; }

    public ProbeComponent(string name, ProbeComponentType type, string description, Sprite sprite)
    {
        Id = sprite.GetInstanceID().ToString();
        Type = type;
        Name = name;
        Description = description;
        Sprite = sprite;
    }
}

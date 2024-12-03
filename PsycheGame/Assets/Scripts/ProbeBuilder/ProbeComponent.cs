using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProbeComponentType
{
    Standard,
    Custom
}

[Serializable]
public class ProbeComponent
{
    public string Id;
    public string Name;
    public ProbeComponentType Type;
    public string Description;
    public string Sprite;

    public ProbeComponent(string id, string name, ProbeComponentType type, string description, string sprite)
    {
        Id = id;
        Type = type;
        Name = name;
        Description = description;
        Sprite = sprite;
    }
}

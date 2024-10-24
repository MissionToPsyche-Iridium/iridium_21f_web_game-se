using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbeComponent
{
    public string Name { get; }
    public string Description { get; }
    public int Type { get; }
    public Sprite Sprite { get; }

    public ProbeComponent(string name, string description, int type, Sprite sprite)
    {
        this.Name = name;
        this.Description = description;
        this.Type = type;
        this.Sprite = sprite;
    }

    public ProbeComponent Clone()
    {
        return new ProbeComponent(Name, Description, Type, Sprite);
    }
}

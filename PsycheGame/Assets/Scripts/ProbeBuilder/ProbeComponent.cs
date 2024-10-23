using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbeComponent
{
    public string Name { get; }
    public string Description { get; }
    public Sprite Sprite { get; }

    public ProbeComponent(string name, string description, Sprite sprite)
    {
        this.Name = name;
        this.Description = description;
        this.Sprite = sprite;
    }

    public ProbeComponent Clone()
    {
        return new ProbeComponent(Name, Description, Sprite);
    }
}

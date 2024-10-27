using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbeComponent
{
    public string Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int Type { get; private set; }
    public Sprite Sprite { get; private set; }

    public ProbeComponent(string id, string name, string description, int type, Sprite sprite)
    {
        Id = id;
        Name = name;
        Description = description;
        Type = type;
        Sprite = sprite;
    }

    public ProbeComponent Clone()
    {
        return new ProbeComponent(Id, Name, Description, Type, Sprite);
    }
}

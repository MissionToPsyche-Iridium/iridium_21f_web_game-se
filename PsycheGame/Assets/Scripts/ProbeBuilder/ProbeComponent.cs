using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbeComponent
{
    public string Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Sprite Sprite { get; private set; }

    public ProbeComponent(string name, string description, Sprite sprite)
    {
        Id = sprite.GetInstanceID().ToString();
        Name = name;
        Description = description;
        Sprite = sprite;
    }
}

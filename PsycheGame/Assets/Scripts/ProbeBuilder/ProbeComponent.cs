using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbeComponent
{
    private string name;
    private string description;
    private Sprite sprite;

    public ProbeComponent(string name, string description, Sprite sprite)
    {
        this.name = name;
        this.description = description;
        this.sprite = sprite;
    }

    public string GetName()
    {
        return name;
    }

    public string GetDescription()
    {
        return description;
    }

    public Sprite GetSprite()
    {
        return sprite;
    }

    public ProbeComponent Clone()
    {
        return new ProbeComponent(name, description, sprite);
    }
}

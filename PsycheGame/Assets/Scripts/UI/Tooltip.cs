using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tooltip
{
    public Transform Parent { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public Vector2 Position { get; private set; }

    public Tooltip() { }

    public Tooltip SetParent(Transform parent)
    {
        Parent = parent;
        return this;
    }

    public Tooltip SetTitle(string title)
    {
        Title = title;
        return this;
    }

    public Tooltip SetDescription(string description)
    {
        Description = description;
        return this;
    }

    public Tooltip SetPosition(float positionX, float positionY)
    {
        Position = new Vector2(positionX, positionY);
        return this;
    }

    public void Draw()
    {

    }
}

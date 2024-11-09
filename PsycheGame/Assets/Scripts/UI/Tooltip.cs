using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tooltip
{
    private static GameObject _tooltipTemplate = Resources.Load<GameObject>("UI/Tooltip");

    public string Title { get; private set; }
    public string Description { get; private set; }
    public Vector2 Position { get; private set; }

    public Tooltip() { }

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

    public Tooltip Draw()
    {
        Canvas masterCanvas = Utility.FindComponentInScene<Canvas>(SceneManager.GetActiveScene());
        if (masterCanvas != null)
        {
            throw new Exception("Could not find master canvas");
        }

        GameObject tooltip = GameObject.Instantiate(_tooltipTemplate);
        // TODO

        return this;
    }
}

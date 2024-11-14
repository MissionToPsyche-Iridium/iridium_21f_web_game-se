using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor.Playables;
using System.Runtime.CompilerServices;

public class TooltipBuilder
{
    private string _title, _description;
    private Vector3 _position;

    public TooltipBuilder() { }

    public TooltipBuilder SetTitle(string title)
    {
        _title = title;
        return this;
    }

    public TooltipBuilder SetDescription(string description)
    {
        _description = description;
        return this;
    }

    public TooltipBuilder SetPosition(float x, float y, float z)
    {
        _position = new Vector3(x, y, z);
        return this;
    }

    public TooltipBuilder SetPositionAtMouse()
    {
        _position = Input.mousePosition + new Vector3(1.0f, -1.0f, 0.0f);
        return this;
    }

    public Tooltip Build()
    {
        return new Tooltip(_title, _description, _position);
    }
}

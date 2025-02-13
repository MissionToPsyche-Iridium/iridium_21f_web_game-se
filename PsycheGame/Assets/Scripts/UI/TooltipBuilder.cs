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
    public const float OffsetXFromMouse = 2.0f;
    public const float OffsetYFromMouse = -2.0f;

    private Canvas _canvas;
    private string _title, _description, _credits;
    private Vector3? _position;

    public TooltipBuilder()
    {
        _canvas = null;
        _position = null;
    }

    public TooltipBuilder SetParentCanvas(Canvas canvas)
    {
        _canvas = canvas;
        return this;
    }

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

    public TooltipBuilder SetCredits(float credits) {
        _credits = credits.ToString() + " Credits";
        return this;
    }

    public TooltipBuilder SetPosition(float x, float y, float z)
    {
        _position = new Vector3(x, y, z);
        return this;
    }

    public Tooltip Build()
    {
        if (_canvas == null)
        {
            _canvas = Utility.FindComponentInScene<Canvas>(SceneManager.GetActiveScene());
        }

        if (_position == null)
        {
            _position = (Input.mousePosition + new Vector3(OffsetXFromMouse, OffsetYFromMouse, 0.0f)) / _canvas.scaleFactor;
        }

        return new Tooltip(_canvas.transform, _title, _description, _credits, (Vector3) _position);
    }
}

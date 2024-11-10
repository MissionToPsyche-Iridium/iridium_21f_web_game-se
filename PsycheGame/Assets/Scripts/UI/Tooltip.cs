using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor.Playables;

public class Tooltip
{
    private static GameObject _tooltipTemplate = Resources.Load<GameObject>("UI/Tooltip");

    private Canvas _masterCanvas;
    private GameObject _tooltip;

    public string Title { get; private set; }
    public string Description { get; private set; }
    public Vector3 Position { get; private set; }

    public Tooltip()
    {
        _masterCanvas = Utility.FindComponentInScene<Canvas>(SceneManager.GetActiveScene());
        _tooltip = null;
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

    public Tooltip SetPositionAtMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        Position = new Vector2(mousePosition.x, mousePosition.y);
        return this;
    }

    public void Draw()
    {
        _tooltip = GameObject.Instantiate(_tooltipTemplate);
        Transform contentTransform = _tooltip.transform.GetChild(0).GetChild(0);
        GameObject title = contentTransform.GetChild(0).gameObject;
        GameObject description = contentTransform.GetChild(1).gameObject;

        title.GetComponent<TextMeshProUGUI>().text = Title;
        description.GetComponent<TextMeshProUGUI>().text = Description;
        RectTransform rect = _tooltip.GetComponent<RectTransform>();
        _tooltip.transform.position = new Vector3(Position.x, Position.y, 10);

        _tooltip.transform.SetParent(_masterCanvas.transform);
    }

    public void Clear()
    {
        GameObject.Destroy(_tooltip);
        _tooltip = null;
    }
}

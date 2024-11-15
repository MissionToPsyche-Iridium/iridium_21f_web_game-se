using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor.Playables;

public class Tooltip
{
    private static GameObject _tooltipTemplate = Resources.Load<GameObject>("UI/Tooltip");

    private Canvas _masterCanvas;
    private GameObject _tooltip;

    public Tooltip(Transform parent, string title, string description, Vector3 position)
    {
        _tooltip = GameObject.Instantiate(_tooltipTemplate);

        _tooltip.SetActive(false);
        _tooltip.transform.SetParent(parent);

        RectTransform contentRect = _tooltip.transform.GetChild(0).GetChild(0).gameObject.transform as RectTransform;
        RectTransform titleRect = contentRect.GetChild(0) as RectTransform;
        RectTransform descriptionRect = contentRect.GetChild(1) as RectTransform;

        TextMeshProUGUI titleMesh = titleRect.gameObject.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI descriptionMesh = descriptionRect.gameObject.GetComponent<TextMeshProUGUI>();

        titleMesh.text = title;
        descriptionMesh.text = description;

        titleRect.sizeDelta = new Vector2(titleRect.rect.width, titleMesh.GetPreferredValues().y);
        descriptionRect.sizeDelta = new Vector2(descriptionRect.rect.width, descriptionMesh.GetPreferredValues().y);
        contentRect.sizeDelta = new Vector2(contentRect.rect.width, titleRect.rect.height + descriptionRect.rect.height + 10.0f);

        RectTransform tooltipRect = _tooltip.GetComponent<RectTransform>();
        tooltipRect.pivot = new Vector2(0.0f, 1.0f);
        tooltipRect.sizeDelta = new Vector3(150.0f, contentRect.rect.height + 50.0f);
        tooltipRect.anchoredPosition = position;
    }

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

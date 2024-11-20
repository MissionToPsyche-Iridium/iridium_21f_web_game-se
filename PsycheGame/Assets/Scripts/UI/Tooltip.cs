using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Tooltip
{
    private static GameObject _tooltipTemplate = Resources.Load<GameObject>("UI/Tooltip");

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
        contentRect.sizeDelta = new Vector2(Math.Max(titleRect.rect.width, descriptionRect.rect.width), titleRect.rect.height + descriptionRect.rect.height + 10.0f);

        RectTransform tooltipRect = _tooltip.GetComponent<RectTransform>();
        tooltipRect.pivot = new Vector2(0.0f, 1.0f);
        tooltipRect.sizeDelta = new Vector3(contentRect.rect.width + 30.0f, contentRect.rect.height + 30.0f);
        tooltipRect.anchoredPosition = position;
    }

    public void Enable()
    {
        _tooltip.SetActive(true);
    }

    public void Disable()
    {
        _tooltip.SetActive(false);
    }

    public void Destroy()
    {
        GameObject.Destroy(_tooltip);
        _tooltip = null;
    }
}
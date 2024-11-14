using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Tooltip
{
    private static GameObject _tooltipTemplate = Resources.Load<GameObject>("UI/Tooltip");

    private GameObject _tooltip;

    public Tooltip(string title, string description, Vector3 position)
    {
        _tooltip = GameObject.Instantiate(_tooltipTemplate);

        _tooltip.SetActive(false);

        Transform contentTransform = _tooltip.transform.GetChild(0).GetChild(0);
        contentTransform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = title;
        contentTransform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = description;

        Canvas masterCanvas = Utility.FindComponentInScene<Canvas>(SceneManager.GetActiveScene());
        _tooltip.transform.SetParent(masterCanvas.transform);

        RectTransform tooltipRect = _tooltip.GetComponent<RectTransform>();
        tooltipRect.pivot = new Vector2(0.0f, 1.0f);
        tooltipRect.sizeDelta = new Vector2(150.0f, 300.0f);
        tooltipRect.anchoredPosition = position / masterCanvas.scaleFactor;
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

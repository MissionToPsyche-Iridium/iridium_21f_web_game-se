using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ProbeComponentButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private GameObject _dragIcon;
    private RectTransform _dragPlane;

    public void OnBeginDrag(PointerEventData eventData)
    {
        _dragIcon = new GameObject();

        Image image = _dragIcon.AddComponent<Image>();
        image.preserveAspect = true;
        image.sprite = GetComponent<Image>().sprite;

        Transform canvasTransform = Utility.FindComponentInParents<Canvas>(gameObject).transform.parent;
        _dragIcon.transform.SetParent(canvasTransform);
        _dragPlane = canvasTransform as RectTransform;

        UpdateIconPosition(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_dragIcon != null)
        {
            UpdateIconPosition(eventData);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_dragIcon != null)
        {
            Destroy(_dragIcon);
        }
    }

    public void UpdateIconPosition(PointerEventData eventData)
    {
        RectTransform iconTransform = _dragIcon.GetComponent<RectTransform>();
        Vector3 mousePosition;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(iconTransform, eventData.position, eventData.pressEventCamera, out mousePosition))
        {
            iconTransform.position = mousePosition;
            iconTransform.rotation = _dragPlane.rotation;
        }
    }
}

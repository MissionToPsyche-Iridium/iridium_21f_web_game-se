using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ProbeComponentButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private GameObject _dragIcon;

    public void OnBeginDrag(PointerEventData eventData)
    {
        _dragIcon = new GameObject();

        Image image = _dragIcon.AddComponent<Image>();
        image.preserveAspect = true;
        image.sprite = GetComponent<Image>().sprite;

        _dragIcon.transform.SetParent(FindComponentInParents<Canvas>(gameObject).transform.parent);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_dragIcon != null)
        {
            // TODO
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_dragIcon != null)
        {
            Destroy(_dragIcon);
        }
    }

    public static T FindComponentInParents<T>(GameObject gameObject) where T : Component
    {
        if (gameObject == null)
        {
            return null;
        }

        T component = gameObject.GetComponent<T>();
        Transform parent = gameObject.transform.parent;

        while (parent != null && component == null)
        {
            component = parent.gameObject.GetComponent<T>();
            parent = parent.parent;
        }

        return component;
    }
}

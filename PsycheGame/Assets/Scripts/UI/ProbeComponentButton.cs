using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ProbeComponentButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private ProbeComponent _probeComponent;

    private GameObject _dragIcon;
    private RectTransform _dragPlane;

    private Tooltip _tooltip;

    public void Start()
    {
        _probeComponent = ProbeComponentInventory.GetInstance().GetProbeComponent(gameObject);

        _dragIcon = null;
        _dragPlane = null;

        _tooltip = null;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _dragIcon = new GameObject();

        Image image = _dragIcon.AddComponent<Image>();
        image.preserveAspect = true;
        image.sprite = GetComponent<Image>().sprite;

        _dragIcon.GetComponent<RectTransform>().sizeDelta = GetComponent<RectTransform>().sizeDelta;

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
            _dragIcon = null;
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_tooltip != null)
        {
            return;
        }

        _tooltip = new Tooltip()
                    .SetTitle(_probeComponent.Name)
                    .SetDescription(_probeComponent.Description)
                    .SetPositionAtMouse();

        // _tooltip.Draw(); FIX
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_tooltip == null)
        {
            return;
        }

        // _tooltip.Clear();
        _tooltip = null;
    }
}

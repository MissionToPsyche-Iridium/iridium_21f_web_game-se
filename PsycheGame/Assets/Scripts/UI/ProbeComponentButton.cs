using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ProbeComponentButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public ProbeComponent ProbeComponent { get; set; }
    public ProbeComponentInventory ProbeComponentInventory { get; set; }
    public GameObject SpawnArea { get; set; }

    private GameObject _dragIcon;
    private RectTransform _dragPlane;

    private Tooltip _tooltip;

    public void Awake()
    {
        _dragIcon = null;
        _dragPlane = null;

        _tooltip = null;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (gameObject.tag.Equals("Inactive"))
        {
            return;
        }

        _dragIcon = new GameObject();
        _dragIcon.name = ProbeComponent.Name;

        _dragIcon.AddComponent<BoxCollider2D>().isTrigger = true;
        _dragIcon.AddComponent<Rigidbody2D>().gravityScale = 0;
        _dragIcon.GetComponent<BoxCollider2D>().size = new Vector2(10, 10);
        _dragIcon.AddComponent<SpriteDragDrop>();
        _dragIcon.layer = 9;
        _dragIcon.tag = "ProbePart";

        Image image = _dragIcon.AddComponent<Image>();
        image.preserveAspect = true;
        image.sprite = GetComponent<Image>().sprite;

        RectTransform rect = (RectTransform) transform;
        _dragIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(rect.rect.width, rect.rect.height);

        Transform canvasTransform = Utility.FindComponentInParents<Canvas>(gameObject).transform.parent;
        _dragIcon.transform.SetParent(SpawnArea.transform);
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
            (int cellX, int cellY) cellPos = GameObject.Find("ContainerPanel").GetComponent<ContainerManager>().FindGridPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            if (cellPos.cellX != -1 && cellPos.cellY != -1)
            {
                if (!GameObject.Find("ContainerPanel").GetComponent<ContainerManager>().CheckGridOccupied(cellPos.cellX, cellPos.cellY))
                {
                    GameObject.Find("ContainerPanel").GetComponent<ContainerManager>().AssignToGridPosition(cellPos.cellX, cellPos.cellY, this.gameObject);
                    Debug.Log("+++Assigned Grid position: [" + cellPos.cellX + ", " + cellPos.cellY + "] +++");
                    (float x, float y) cell = GameObject.Find("ContainerPanel").GetComponent<ContainerManager>().GetBeaconPositionGrid(cellPos.cellX, cellPos.cellY);
                    _dragIcon.transform.position = new Vector3(cell.x, cell.y, -0.01f);
                                    
                    if (this.gameObject.layer <= 9)
                    {
                        this.gameObject.layer = 10;
                    }
                }
                else
                {
                    Debug.Log("---Grid position is occupied: " + cellPos.cellX + ", " + cellPos.cellY + "---");
                    Destroy(_dragIcon);
                }
            }
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

        _tooltip = new TooltipBuilder()
                    .SetTitle(ProbeComponent.Name)
                    .SetDescription(ProbeComponent.Description)
                    .Build();

        _tooltip.Enable();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_tooltip == null)
        {
            return;
        }

        _tooltip.Destroy();

        _tooltip = null;
    }
}

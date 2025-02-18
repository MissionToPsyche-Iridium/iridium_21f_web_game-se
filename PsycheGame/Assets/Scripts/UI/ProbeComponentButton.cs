using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ProbeComponentButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public BuildManager BuildManager { get; set; }
    public ProbeComponent ProbeComponent { get; set; }
    public ProbeComponentInventory ProbeComponentInventory { get; set; }
    public GameObject ComponentPanel { get; set; }
    public GameObject SpawnArea { get; set; }
    private ContainerManager _containerManager;
    private GameObject _dragIcon;
    private Material _boundMaterial;
    private Material _sparkMaterial;
    private RectTransform _dragPlane;
    private AudioClip _snapSound;
    private Tooltip _tooltip;

    public void Awake()
    {
        _dragIcon = null;
        _dragPlane = null;
        _tooltip = null;

        _containerManager = GameObject.Find("ContainerPanel").GetComponent<ContainerManager>();
        _snapSound = Resources.Load<AudioClip>("Audio/SnapClick");
        _boundMaterial = Resources.Load<Material>("EFX/OrangeRecolor");
        _sparkMaterial = Resources.Load<Material>("EFX/SparkMaterial2");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (gameObject.tag.Equals("Inactive") || eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        _dragIcon = new GameObject();
        _dragIcon.name = ProbeComponent.Name;

        _dragIcon.AddComponent<BoxCollider2D>().isTrigger = true;
        _dragIcon.AddComponent<Rigidbody2D>().gravityScale = 0;

        Image image = _dragIcon.AddComponent<Image>();
        image.preserveAspect = true;
        image.sprite = GetComponent<Image>().sprite;
        //image.material = _boundMaterial;

        _dragIcon.AddComponent<AudioSource>();

        RectTransform rect = (RectTransform)transform;
        Vector2 size = new Vector2(rect.rect.width, rect.rect.height);
        _dragIcon.GetComponent<RectTransform>().sizeDelta = size;
        _dragIcon.GetComponent<BoxCollider2D>().size = size;

        Transform canvasTransform = Utility.FindComponentInParents<Canvas>(gameObject).transform.parent;
        _dragIcon.transform.SetParent(SpawnArea.transform);
        _dragPlane = canvasTransform as RectTransform;

        _dragIcon.AddComponent<SpriteDragDrop>();
        _dragIcon.GetComponent<SpriteDragDrop>().BuildManager = BuildManager;
        _dragIcon.GetComponent<SpriteDragDrop>().ComponentPanel = ComponentPanel;
        _dragIcon.layer = 9;
        _dragIcon.tag = "ProbePart";

        UpdateIconPosition(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_dragIcon != null)
        {
            UpdateIconPosition(eventData);
            Debug.Log(" <PCB> +++Dragging Probe Component: " + ProbeComponent.Name + "+++");
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_dragIcon != null)
        {
            (int cellX, int cellY) cellPos = _containerManager.GetCellAtWorldPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (BuildManager.GetAvailableCredits() >= ProbeComponent.Credits && (cellPos.cellX != -1 && cellPos.cellY != -1))
            {
                if (_containerManager.CanOccupyCell(cellPos.cellX, cellPos.cellY))
                {
                    BuildManager.SpawnProbeComponent(new Tuple<ProbeComponent, GameObject>(ProbeComponent, _dragIcon));

                    _containerManager.AssignToGridPosition(cellPos.cellX, cellPos.cellY, _dragIcon);

                    (float x, float y) cell = _containerManager.GetBeaconPositionGrid(cellPos.cellX, cellPos.cellY);
                    _dragIcon.transform.position = new Vector3(cell.x, cell.y, -0.01f);

                    _dragIcon.GetComponent<SpriteDragDrop>().CurrentCell = new Tuple<int, int>(cellPos.cellX, cellPos.cellY);

                    _dragIcon.GetComponent<AudioSource>().PlayOneShot(_snapSound, 1.0f);
                    Image image = _dragIcon.GetComponent<Image>();

                    if (_containerManager.IsInteriorTile(cellPos.cellX, cellPos.cellY))
                    {
                        image.material = _sparkMaterial;
                    }
                    else
                    {
                        image.material = _boundMaterial;
                    }

                    if (this.gameObject.layer <= 9)
                    {
                        this.gameObject.layer = 10;
                    }
                }
                else
                {
                    Destroy(_dragIcon);
                }
            }
            else
            {
                Destroy(_dragIcon);
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
                    .Build();

        _tooltip.Enable();

        //set info panel
        ProbeComponent probeComponent = ProbeComponentInventory.GetProbeComponent(gameObject);

        GameObject.Find("PartName").GetComponentInChildren<TMP_Text>().text = probeComponent.Name;
        GameObject.Find("PartDescription").GetComponentInChildren<TMP_Text>().text = probeComponent.Description;
        GameObject.Find("PartImage").GetComponentInChildren<Image>().sprite = GetComponent<Image>().sprite;
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

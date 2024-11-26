using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    private ContainerManager _containerManager;
    private GameObject _dragIcon;
    private Material _boundMaterial;
    private Material _sparkMaterial;
    private RectTransform _dragPlane;

    private AudioClip _snapSound;

    private Tooltip _tooltip;

    private String _itemSeed;

    public void Awake()
    {
        _dragIcon = null;
        _dragPlane = null;
        _tooltip = null;

        _containerManager = GameObject.Find("ContainerPanel").GetComponent<ContainerManager>();
        _snapSound = Resources.Load<AudioClip>("Audio/SnapClick");
        _boundMaterial = Resources.Load<Material>("EFX/BlueRecolor");
        _sparkMaterial = Resources.Load<Material>("EFX/SparkMaterial");
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

        Image image = _dragIcon.AddComponent<Image>();
        image.preserveAspect = true;
        image.sprite = GetComponent<Image>().sprite;
        image.material = _boundMaterial;

        _dragIcon.AddComponent<AudioSource>();

        RectTransform rect = (RectTransform) transform;
        _dragIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(rect.rect.width, rect.rect.height);

        Transform canvasTransform = Utility.FindComponentInParents<Canvas>(gameObject).transform.parent;
        _dragIcon.transform.SetParent(SpawnArea.transform);
        _dragPlane = canvasTransform as RectTransform;
        
        _dragIcon.AddComponent<SpriteDragDrop>();
        _itemSeed = GameObject.Find("ContainerPanel").GetComponent<ContainerManager>().SeedUniquId();
        _dragIcon.GetComponent<SpriteDragDrop>().internalId = _itemSeed;
        _dragIcon.layer = 9;
        _dragIcon.tag = "ProbePart";

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
            (int cellX, int cellY) cellPos = _containerManager.FindGridPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            if (cellPos.cellX != -1 && cellPos.cellY != -1)
            {
                if (_containerManager.CheckGridOccupied(cellPos.cellX, cellPos.cellY) == "")
                {
                    BuildManager.GetInstance().SpawnProbeComponent(new Tuple<ProbeComponent, GameObject>(ProbeComponent, _dragIcon));

                    _containerManager.AssignToGridPosition(cellPos.cellX, cellPos.cellY, _itemSeed);
                    Debug.Log(" <PCB> +++Assigned Grid position: [" + cellPos.cellX + ", " + cellPos.cellY + "] +++");
                    (float x, float y) cell = _containerManager.GetBeaconPositionGrid(cellPos.cellX, cellPos.cellY);
                    _dragIcon.transform.position = new Vector3(cell.x, cell.y, -0.01f);
                    _dragIcon.GetComponent<AudioSource>().PlayOneShot(_snapSound, 1.0f);
                    Image image = _dragIcon.GetComponent<Image>();
                    image.material = _sparkMaterial;
                    
                    if (this.gameObject.layer <= 9)
                    {
                        this.gameObject.layer = 10;
                    }
                }
                else
                {
                    Debug.Log(" <PCB> ---Grid position is occupied: " + cellPos.cellX + ", " + cellPos.cellY + "---");
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
                    .Build();

        _tooltip.Enable();

         //set info panel
                ProbeComponent probeComponent = ProbeComponentInventory.GetProbeComponent(gameObject);

                GameObject.Find("PartName").GetComponentInChildren<TMP_Text>().text = probeComponent.Name;
                GameObject.Find("PartDescription").GetComponentInChildren<TMP_Text>().text = InfoPanelData.getDescription(probeComponent.Name);
                GameObject.Find("PartImage").GetComponentInChildren<Image>().sprite = probeComponent.Sprite;
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

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ProbeComponentButton : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public BuildManager BuildManager { get; set; }
    public ProbeComponent ProbeComponent { get; set; }
    public ProbeComponentInventory ProbeComponentInventory { get; set; }
    public GameObject MasterCanvas { get; set; }
    public GameObject ForegroundCanvas { get; set; }
    public GameObject DraggingBox { get; set; }
    public GameObject InfoPanel { get; set; }
    public GameObject InfoPartName { get; set; }
    public GameObject InfoPartDescription { get; set; }
    public GameObject InfoPartCredits { get; set; }
    public GameObject InfoPartImage { get; set; }
    public GameObject SpawnArea { get; set; }
    public GameObject NotificationPrefab { get; set; }
    public GameObject TooltipPrefab { get; set; }
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

        SpriteDragDrop spriteDragDrop = _dragIcon.AddComponent<SpriteDragDrop>();
        spriteDragDrop.MasterCanvas = MasterCanvas;
        spriteDragDrop.BuildManager = BuildManager;
        spriteDragDrop.ProbeComponent = ProbeComponent;
        spriteDragDrop.DraggingBox = DraggingBox;
        spriteDragDrop.NotificationPrefab = NotificationPrefab;
        _dragIcon.layer = 9;
        _dragIcon.tag = "ProbePart";

        UpdateIconPosition();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_dragIcon != null)
        {
            UpdateIconPosition();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_dragIcon != null)
        {
            (int cellX, int cellY) cellPos = _containerManager.GetCellAtWorldPosition(_dragIcon.transform.position);
            if (cellPos.cellX != -1 && cellPos.cellY != -1)
            {
                bool mountTypeMatch = ProbeComponent.MountType == ProbeComponentMountType.Any || _containerManager.IsInteriorTile(cellPos.cellX, cellPos.cellY) == (ProbeComponent.MountType == ProbeComponentMountType.Interior),
                     enoughCredits = BuildManager.GetAvailableCredits() >= ProbeComponent.Credits,
                     canOccupy = _containerManager.CanOccupyCell(cellPos.cellX, cellPos.cellY);

                if (!(mountTypeMatch && enoughCredits))
                {
                    NotificationService.Create((!mountTypeMatch) ? "Cannot mount there" : "Insufficient credits");
                }
                else if (canOccupy)
                {
                    BuildManager.SpawnProbeComponent(new Tuple<ProbeComponent, GameObject>(ProbeComponent, _dragIcon));

                    _containerManager.AssignToGridPosition(cellPos.cellX, cellPos.cellY, _dragIcon);

                    (float x, float y) cell = _containerManager.GetBeaconPositionGrid(cellPos.cellX, cellPos.cellY);
                    _dragIcon.transform.position = new Vector3(cell.x, cell.y, -0.01f);

                    Rect tileRect = (_containerManager.GetTileAtCell(cellPos.cellX, cellPos.cellY).gameObject.transform as RectTransform).rect;
                    (_dragIcon.transform as RectTransform).sizeDelta = new Vector2(tileRect.width, tileRect.height);

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

                    _dragIcon = null;
                    return;
                }
            }

            Destroy(_dragIcon);
            _dragIcon = null;
        }
    }

    public void UpdateIconPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane));

        Vector3[] worldCorners = new Vector3[4];
        (MasterCanvas.transform as RectTransform).GetWorldCorners(worldCorners);

        RectTransform iconTransform = _dragIcon.transform as RectTransform;
        iconTransform.position = new Vector3(
            Mathf.Clamp(worldPos.x, worldCorners[0].x + iconTransform.rect.width / 2, worldCorners[2].x - iconTransform.rect.width / 2),
            Mathf.Clamp(worldPos.y, worldCorners[0].y + iconTransform.rect.height / 2, worldCorners[2].y - iconTransform.rect.height / 2),
            worldPos.z
        );
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_tooltip != null)
        {
            return;
        }
        _tooltip = TooltipService.Create(ProbeComponent.Name, "Click for more info", transform.position, TooltipPivot.TopLeft);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_tooltip == null)
        {
            return;
        }
        _tooltip.Delete();
        _tooltip = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            InfoPartName.GetComponent<TextMeshProUGUI>().text = ProbeComponent.Name;
            InfoPartDescription.GetComponent<TextMeshProUGUI>().text = ProbeComponent.Description;
            InfoPartCredits.GetComponent<TextMeshProUGUI>().text = ProbeComponent.MountType + " - " + ProbeComponent.Credits.ToString() + " Credits";
            InfoPartImage.GetComponent<Image>().sprite = GetComponent<Image>().sprite;

            InfoPanel.transform.GetChild(0).GetChild(0).GetChild(1).gameObject.GetComponent<Scrollbar>().value = 1;

            if (!InfoPanel.activeSelf)
            {
                InfoPanel.SetActive(true);
            }
        }
    }
}

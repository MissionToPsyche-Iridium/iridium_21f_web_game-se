using System;
using UnityEngine;

/*
    Probe Builder :: SpriteDragDrop.cs
    Date: Oct. 2024
    Description: this script provides the drag-and-drop behavior for the probe parts. It also contains the logic to snap the probe part 
    to the grid tile when the probe part is in contact with the tile.

    version 1.0 candidate (Jan 21)
    :: 1.0 candidate - Jan 21 - refactored code to meet C# convention for performance and readability

    version 1.1 (Feb 14)
    :: updated the logic to apply different shader materials to the probe part when it is placed on the interior or exterior tile

    version 1.2 (Apr 16)
    :: review code and perform minor code cleanup.  No functional changes.
*/

public class SpriteDragDrop : MonoBehaviour
{
    private ContainerManager containerManager;
    public BuildManager BuildManager;
    public ProbeComponent ProbeComponent;
    public GameObject MasterCanvas;
    public GameObject DraggingBox;
    public GameObject NotificationPrefab;
    public bool Selected { get; private set; }
    public Tuple<int, int> CurrentCell { get; set; }

    private AudioClip snapSound;
    private Material exteriorlMaterial;
    private Material sparkMaterial;
    private Vector3 offset;
    private AudioSource audioSource;
    private UnityEngine.UI.Image image;

    private void Start()
    {
        Selected = false;

        containerManager = GameObject.Find("ContainerPanel").GetComponent<ContainerManager>();
        snapSound = Resources.Load<AudioClip>("Audio/SnapClick");
        audioSource = gameObject.AddComponent<AudioSource>();
        exteriorlMaterial = Resources.Load<Material>("EFX/OrangeRecolor");
        sparkMaterial = Resources.Load<Material>("EFX/SparkMaterial2");
        image = GetComponent<UnityEngine.UI.Image>();
    }
    private void OnMouseDown()
    {
        Selected = true;
        offset = transform.position - MouseWorldPosition();
        gameObject.layer = 9;
        DraggingBox.SetActive(true);
    }

    private void OnMouseDrag()
    {
        UpdatePosition();
    }

    private void OnMouseUp()
    {
        if (Selected)
        {
            RectTransform draggingBox = DraggingBox.transform as RectTransform;

            (int cellX, int cellY) cellPos = containerManager.GetCellAtWorldPosition(transform.position);
            if (cellPos.cellX != -1 && cellPos.cellY != -1)
            {
                bool mountTypeMatch = ProbeComponent.MountType == ProbeComponentMountType.Any || containerManager.IsInteriorTile(cellPos.cellX, cellPos.cellY) == (ProbeComponent.MountType == ProbeComponentMountType.Interior),
                     canOccupy = containerManager.CanOccupyCell(cellPos.cellX, cellPos.cellY);

                if (!mountTypeMatch)
                {
                    NotificationService.Create("Cannot mount there");
                }
                else
                {
                    if (canOccupy)
                    {
                        AttemptToRelease();

                        CurrentCell = new Tuple<int, int>(cellPos.cellX, cellPos.cellY);
                        containerManager.AssignToGridPosition(CurrentCell.Item1, CurrentCell.Item2, gameObject);
                        audioSource.PlayOneShot(snapSound, 1.0f);

                        if (containerManager.IsInteriorTile(cellPos.cellX, cellPos.cellY))
                        {
                            Debug.Log("set to [Spark material]");
                            image.material = sparkMaterial;
                        }
                        else
                        {
                            Debug.Log("set to [Original material]");
                            image.material = exteriorlMaterial;
                        }
                    }
                    else
                    {
                        containerManager.SwapOccupants(CurrentCell.Item1, CurrentCell.Item2, cellPos.cellX, cellPos.cellY);
                    }
                }
            }
            else if (Math.Abs(draggingBox.position.x - transform.position.x) <= Math.Abs(draggingBox.rect.width) / 2 && Math.Abs(draggingBox.position.y - transform.position.y) <= Math.Abs(draggingBox.rect.height) / 2)
            {
                BuildManager.DespawnProbeComponent(gameObject);
            }

            (float x, float y) cell = containerManager.GetBeaconPositionGrid(CurrentCell.Item1, CurrentCell.Item2);
            transform.position = new Vector3(cell.x, cell.y, -0.01f);

            DraggingBox.SetActive(false);

            Selected = false;
        }
    }

    public void UpdatePosition()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane)) + offset;

        Vector3[] worldCorners = new Vector3[4];
        (MasterCanvas.transform as RectTransform).GetWorldCorners(worldCorners);

        RectTransform rectTransform = transform as RectTransform;
        rectTransform.position = new Vector3(
            Mathf.Clamp(worldPos.x, worldCorners[0].x + rectTransform.rect.width / 2, worldCorners[2].x - rectTransform.rect.width / 2),
            Mathf.Clamp(worldPos.y, worldCorners[0].y + rectTransform.rect.height / 2, worldCorners[2].y - rectTransform.rect.height / 2),
            worldPos.z
        );
    }

    public bool AttemptToRelease()
    {
        if (!containerManager.CanOccupyCell(CurrentCell.Item1, CurrentCell.Item2))
        {
            containerManager.ReleaseFromGridPosition(CurrentCell.Item1, CurrentCell.Item2, gameObject);
            return true;
        }
        return false;
    }

    public bool AttemptToReoccupy()
    {
        if (containerManager.CanOccupyCell(CurrentCell.Item1, CurrentCell.Item2))
        {
            containerManager.AssignToGridPosition(CurrentCell.Item1, CurrentCell.Item2, gameObject);
            return true;
        }
        return false;
    }

    Vector3 MouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        return Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane));
    }
}


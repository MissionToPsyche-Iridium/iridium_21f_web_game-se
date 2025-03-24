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
*/

public class SpriteDragDrop : MonoBehaviour
{
    private ContainerManager containerManager;
    public BuildManager BuildManager;
    public ProbeComponent ProbeComponent;
    public GameObject DraggingBox;
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
        ProbeComponent probeComponent = GetComponent<ProbeComponent>();
        gameObject.layer = 9;
        DraggingBox.SetActive(true);
    }

    private void OnMouseDrag()
    {
        ProbeComponent probeComponent = GetComponent<ProbeComponent>();
        transform.position = MouseWorldPosition() + offset;
    }

    private void OnMouseUp()
    {
        if (Selected)
        {
            RectTransform draggingBox = DraggingBox.transform as RectTransform;

            (int cellX, int cellY) cellPos = containerManager.GetCellAtWorldPosition(transform.position);

            if (cellPos.cellX != -1 && cellPos.cellY != -1)
            {
                ProbeComponent comp = GetComponent<ProbeComponent>();
                if (containerManager.CanOccupyCell(comp, cellPos.cellX, cellPos.cellY))
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

                    if (gameObject.layer <= 9)
                    {
                        gameObject.layer = 10;
                    }
                }
                else
                {
                    containerManager.SwapOccupants(CurrentCell.Item1, CurrentCell.Item2, cellPos.cellX, cellPos.cellY);
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

    public bool AttemptToRelease()
    {
        ProbeComponent comp = GetComponent<ProbeComponent>();
        if (!containerManager.CanOccupyCell(comp, CurrentCell.Item1, CurrentCell.Item2))
        {
            containerManager.ReleaseFromGridPosition(CurrentCell.Item1, CurrentCell.Item2, gameObject);
            return true;
        }
        return false;
    }

    public bool AttemptToReoccupy()
    {
        if (containerManager.CanOccupyCell(ProbeComponent, CurrentCell.Item1, CurrentCell.Item2))
        {
            containerManager.AssignToGridPosition(CurrentCell.Item1, CurrentCell.Item2, gameObject);
            return true;
        }
        return false;
    }

    Vector3 MouseWorldPosition()
    {
        var mouseScreenPos = Input.mousePosition;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }
}


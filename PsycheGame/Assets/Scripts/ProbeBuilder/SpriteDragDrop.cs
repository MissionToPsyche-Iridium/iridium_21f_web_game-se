using System;
using UnityEngine;

/*
    Probe Builder :: SpriteDragDrop.cs
    Date: Oct. 2024
    Description: this script provides the drag-and-drop behavior for the probe parts. It also contains the logic to snap the probe part 
    to the grid tile when the probe part is in contact with the tile.

    version 1.0 candidate (Jan 21)
    :: 1.0 candidate - Jan 21 - refactored code to meet C# convention for performance and readability
*/

public class SpriteDragDrop : MonoBehaviour
{
    private ContainerManager containerManager;
    public BuildManager BuildManager;
    public GameObject ComponentPanel;
    public bool Selected { get; private set; }
    public string InternalId { get; set; }
    public Tuple<int, int> CurrentCell { get; set; }

    private AudioClip snapSound;
    private Material originalMaterial;
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
        image = GetComponent<UnityEngine.UI.Image>();

        Debug.Log(" <SDD> +++Probe part internal ID: " + InternalId + "+++");
    }
    private void OnMouseDown()
    {
        Selected = true;
        offset = transform.position - MouseWorldPosition();
        gameObject.layer = 9;
    }

    private void OnMouseDrag()
    {
        transform.position = MouseWorldPosition() + offset;
    }

    private void OnMouseUp()
    {
        if (Selected)
        {
            Vector3 mousePos = MouseWorldPosition();
            RectTransform panelRect = ComponentPanel.transform as RectTransform;

            (int cellX, int cellY) cellPos = containerManager.GetCellAtWorldPosition(mousePos);

            if (cellPos.cellX != -1 && cellPos.cellY != -1)
            {
                AttemptToRelease();

                if (containerManager.CheckOccupationEligibility(cellPos.cellX, cellPos.cellY))
                {
                    CurrentCell = new Tuple<int, int>(cellPos.cellX, cellPos.cellY);

                    containerManager.AssignToGridPosition(CurrentCell.Item1, CurrentCell.Item2, InternalId);

                    audioSource.PlayOneShot(snapSound, 1.0f);
                    image.material = sparkMaterial;

                    if (gameObject.layer <= 9)
                    {
                        gameObject.layer = 10;
                    }
                }
                else
                {
                    AttemptToReoccupy();
                }
            }
            else if (Math.Abs(panelRect.position.x - mousePos.x) <= Math.Abs(panelRect.rect.width) / 2 && Math.Abs(panelRect.position.y - mousePos.y) <= Math.Abs(panelRect.rect.height) / 2)
            {
                BuildManager.DespawnProbeComponent(gameObject);
            }

            (float x, float y) cell = containerManager.GetBeaconPositionGrid(CurrentCell.Item1, CurrentCell.Item2);
            transform.position = new Vector3(cell.x, cell.y, -0.01f);

            Selected = false;
        }
    }

    public bool AttemptToRelease()
    {
        if (!containerManager.CheckOccupationEligibility(CurrentCell.Item1, CurrentCell.Item2))
        {
            containerManager.ReleaseFromGridPosition(CurrentCell.Item1, CurrentCell.Item2, InternalId);
            return true;
        }
        return false;
    }

    public bool AttemptToReoccupy()
    {
        if (containerManager.CheckOccupationEligibility(CurrentCell.Item1, CurrentCell.Item2))
        {
            containerManager.AssignToGridPosition(CurrentCell.Item1, CurrentCell.Item2, InternalId);
            return true;
        }
        return false;
    }

    Vector3 MouseWorldPosition()
    {
        var mouseScreenPos = Input.mousePosition;
        // mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }
}


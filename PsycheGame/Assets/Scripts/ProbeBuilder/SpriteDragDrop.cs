using System;
using Unity.VisualScripting;
using UnityEngine;

/*
    Probe Builder :: SpriteDragDrop.cs

    Date: Oct. 2024
    Description: this script provides the drag-and-drop behavior for the probe parts.  It also contains the logic to snap the probe part 
    to the grid tile when the probe part is in contact with the tile.
       
*/

public class SpriteDragDrop : MonoBehaviour
{
    private ContainerManager containerManager;
    public bool selected;
    public String internalId;
    public Tuple<int, int> currentCell;

    private AudioClip snapSound;
    private Material originalMaterial;
    private Material sparkMaterial;

    Vector3 offset;

    private void Start()
    {
        selected = false;

        containerManager = GameObject.Find("ContainerPanel").GetComponent<ContainerManager>();
        snapSound = Resources.Load<AudioClip>("Audio/SnapClick");
        this.AddComponent<AudioSource>();
        UnityEngine.UI.Image image = GetComponent<UnityEngine.UI.Image>();
        originalMaterial = image.material;
        sparkMaterial = Resources.Load<Material>("EFX/SparkMaterial2");

        Debug.Log(" <SDD> +++Probe part internal ID: " + internalId + "+++");
    }
    private void OnMouseDown()
    {
        selected = true;
        offset = transform.position - MouseWorldPosition();
        gameObject.layer = 9;
    }

    private void OnMouseDrag()
    {
        transform.position = MouseWorldPosition() + offset;
    }

    private void OnMouseUp()
    {
        if (selected)
        {
            Vector3 newPos = MouseWorldPosition();
            (int cellX, int cellY) cellPos = containerManager.FindGridPosition(newPos);

            if (cellPos.cellX != -1 && cellPos.cellY != -1)
            {
                AttemptToRelease();

                if (containerManager.CheckOccupationEligibility(cellPos.cellX, cellPos.cellY))
                {
                    currentCell = new Tuple<int, int>(cellPos.cellX, cellPos.cellY);

                    containerManager.AssignToGridPosition(currentCell.Item1, currentCell.Item2, internalId);

                    GetComponent<AudioSource>().PlayOneShot(snapSound, 1.0f);
                    UnityEngine.UI.Image image = GetComponent<UnityEngine.UI.Image>();
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

            (float x, float y) cell = containerManager.GetBeaconPositionGrid(currentCell.Item1, currentCell.Item2);
            transform.position = new Vector3(cell.x, cell.y, -0.01f);

            selected = false;
        }
    }

    public bool AttemptToRelease()
    {
        if (!containerManager.CheckOccupationEligibility(currentCell.Item1, currentCell.Item2))
        {
            containerManager.ReleaseFromGridPosition(currentCell.Item1, currentCell.Item2, internalId);
            return true;
        }
        return false;
    }

    public bool AttemptToReoccupy()
    {
        if (containerManager.CheckOccupationEligibility(currentCell.Item1, currentCell.Item2))
        {
            containerManager.AssignToGridPosition(currentCell.Item1, currentCell.Item2, internalId);
            return true;
        }
        return false;
    }

    Vector3 MouseWorldPosition()
    {
        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }
}


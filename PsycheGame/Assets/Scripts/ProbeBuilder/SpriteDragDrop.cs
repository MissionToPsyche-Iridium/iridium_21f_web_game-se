using System;
using Unity.VisualScripting;
using UnityEngine;

/*
    Author: Hannah M.
    Date: 10/??/2024
    Description: this script provides the drag-and-drop behavior for the probe parts.  It also contains the logic to snap the probe part 
    to the grid tile when the probe part is in contact with the tile.

    v 1.1 - Shawn (11/6)
    - Updated MouseUp() method to take the tile position for placement of the game object (probe part)

    v 1.2 - Shawn (11/10)
    - Refactored OnMouseUp method to simplify the probe item placement check logic, including validation of grid position

    v [1.3] - Shawn (11/11) in process to add logic to handle removal and update of grid status and visual indicator

    v [2] - Shawn (11/22) updated snap logic to add and remove from a grid position.  still need to associate each occupying grid position
    - with a unique probe item id to differentiate between the probe items
       
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
        this.gameObject.layer = 9;
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
                if (containerManager.CheckGridOccupied(cellPos.cellX, cellPos.cellY) == "")
                {
                    if (containerManager.CheckGridOccupied(currentCell.Item1, currentCell.Item2) == internalId)
                    {
                        containerManager.ReleaseFromGridPosition(currentCell.Item1, currentCell.Item2, internalId);
                    }

                    currentCell = new Tuple<int, int>(cellPos.cellX, cellPos.cellY);

                    containerManager.AssignToGridPosition(currentCell.Item1, currentCell.Item2, internalId);

                    GetComponent<AudioSource>().PlayOneShot(snapSound, 1.0f);
                    UnityEngine.UI.Image image = GetComponent<UnityEngine.UI.Image>();
                    image.material = sparkMaterial;
                                    
                    if (this.gameObject.layer <= 9)
                    {
                        this.gameObject.layer = 10;
                    }
                }
            }

            (float x, float y) cell = containerManager.GetBeaconPositionGrid(currentCell.Item1, currentCell.Item2);
            transform.position = new Vector3(cell.x, cell.y, -0.01f);

            selected = false;
        }
    }

    public bool AttemptToRelease()
    {
        if (containerManager.CheckGridOccupied(currentCell.Item1, currentCell.Item2) == internalId)
        {
            containerManager.ReleaseFromGridPosition(currentCell.Item1, currentCell.Item2, internalId);
            return true;
        }
        return false;
    }

    public bool AttemptToReoccupy()
    {
        if (containerManager.CheckGridOccupied(currentCell.Item1, currentCell.Item2) == "")
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


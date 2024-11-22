using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Text.RegularExpressions;
using UnityEditor.Callbacks;
using UnityEngine;
using TMPro;

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
    private UnityEngine.Vector2 mousePosition;
    private float offsetX, offsetY;
    private UnityEngine.Vector2 initialPos;
    public static bool selected;

    public String internalId;

    Collider2D col;

    Vector3 offset;

    private void Start()
    {
        selected = false;
        Debug.Log(" <SDD> +++Probe part internal ID: " + internalId + "+++");
    }

    private void OnMouseDown()
    {
        selected = true;
        offset = transform.position - MouseWorldPosition();
        Vector3 newPos = MouseWorldPosition();

        (int cellX, int cellY) cellPos = GameObject.Find("ContainerPanel").GetComponent<ContainerManager>().FindGridPosition(newPos);
        if (cellPos.cellX != -1 && cellPos.cellY != -1)
        {
            if (GameObject.Find("ContainerPanel").GetComponent<ContainerManager>().CheckGridOccupied(cellPos.cellX, cellPos.cellY) == internalId)
            {
                GameObject.Find("ContainerPanel").GetComponent<ContainerManager>().ReleaseFromGridPosition(cellPos.cellX, cellPos.cellY, internalId);
                Debug.Log(" <SDD> ~~~Released Grid position: [" + cellPos.cellX + ", " + cellPos.cellY + "]  id: {" + internalId +"}~~~");

                this.gameObject.layer = 9;
            }
        }
    }

    private void OnMouseDrag()
    {
        transform.position = MouseWorldPosition() + offset;
    }

    private void OnMouseUp()
    {
        // check if a probe part is being dragged
        if (selected)
        {
            Vector3 newPos = MouseWorldPosition();
            (int cellX, int cellY) cellPos = GameObject.Find("ContainerPanel").GetComponent<ContainerManager>().FindGridPosition(newPos);

            if (cellPos.cellX != -1 && cellPos.cellY != -1)
            {
                if (GameObject.Find("ContainerPanel").GetComponent<ContainerManager>().CheckGridOccupied(cellPos.cellX, cellPos.cellY) == "")
                {
                    GameObject.Find("ContainerPanel").GetComponent<ContainerManager>().AssignToGridPosition(cellPos.cellX, cellPos.cellY, internalId);
                    Debug.Log(" <SDD> +++Assigned Grid position: [" + cellPos.cellX + ", " + cellPos.cellY + "] with {" + internalId + "} +++");
                    (float x, float y) cell = GameObject.Find("ContainerPanel").GetComponent<ContainerManager>().GetBeaconPositionGrid(cellPos.cellX, cellPos.cellY);
                    transform.position = new Vector3(cell.x, cell.y, -0.01f);
                                    
                    if (this.gameObject.layer <= 9)
                    {
                        this.gameObject.layer = 10;
                    }
                }
                else
                {
                    Debug.Log(" <SDD> ---Grid position is occupied: " + cellPos.cellX + ", " + cellPos.cellY + "---");
                }
            }
        }
        selected = false;
    }

    Vector3 MouseWorldPosition()
    {
        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }
}


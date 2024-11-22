using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Text.RegularExpressions;
using UnityEditor.Callbacks;
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

*/

public class SpriteDragDrop : MonoBehaviour
{
    private UnityEngine.Vector2 mousePosition;
    private float offsetX, offsetY;
    private UnityEngine.Vector2 initialPos;
    public static bool selected;

    Collider2D col;

    Vector3 offset;

    private void OnMouseDown()
    {
        selected = true;
        offset = transform.position - MouseWorldPosition();
        Vector3 newPos = MouseWorldPosition();
        (int cellX, int cellY) cellPos = GameObject.Find("ContainerPanel").GetComponent<ContainerManager>().FindGridPosition(newPos);

        if (cellPos.cellX != -1 && cellPos.cellY != -1)
        {
            Debug.Log("This game object reference: " + this.gameObject);
            GameObject otherobj = GameObject.Find("ContainerPanel").GetComponent<ContainerManager>().CheckGridOccupied(cellPos.cellX, cellPos.cellY);
            if (this.gameObject == otherobj)
            {
                GameObject.Find("ContainerPanel").GetComponent<ContainerManager>().ReleaseFromGridPosition(cellPos.cellX, cellPos.cellY, this.gameObject);
                Debug.Log("~~~Released Grid position: [" + cellPos.cellX + ", " + cellPos.cellY + "]~~~");

                if (this.gameObject.layer >= 10)
                {
                    this.gameObject.layer = 9;
                }
            }
        }
    }

    private void OnMouseDrag()
    {
        //Debug.Log("MouseDrag");
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
                if (!GameObject.Find("ContainerPanel").GetComponent<ContainerManager>().CheckGridOccupied(cellPos.cellX, cellPos.cellY))
                {
                    GameObject.Find("ContainerPanel").GetComponent<ContainerManager>().AssignToGridPosition(cellPos.cellX, cellPos.cellY, this.gameObject);
                    Debug.Log("+++Assigned Grid position: [" + cellPos.cellX + ", " + cellPos.cellY + "] +++");
                    (float x, float y) cell = GameObject.Find("ContainerPanel").GetComponent<ContainerManager>().GetBeaconPositionGrid(cellPos.cellX, cellPos.cellY);
                    transform.position = new Vector3(cell.x, cell.y, -0.01f);
                                    
                    if (this.gameObject.layer <= 9)
                    {
                        this.gameObject.layer = 10;
                    }
                }
                else
                {
                    Debug.Log("---Grid position is occupied: " + cellPos.cellX + ", " + cellPos.cellY + "---");
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


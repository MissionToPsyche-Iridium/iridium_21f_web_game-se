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

*/

public class SpriteDragDrop : MonoBehaviour
{
    private UnityEngine.Vector2 mousePosition;
    private float offsetX, offsetY;
    private UnityEngine.Vector2 initialPos;
    public static bool selected;

    Collider2D col;

    Vector3 offset;
    
    private void OnMouseDown() {
        //Debug.Log("MouseDown");
        selected = true;
        offset = transform.position - MouseWorldPosition();

        // if the tile is in the in place layer -- move into layer 9 (ProbePart)
        if (this.gameObject.layer == 10) {
            this.gameObject.layer = 9;
            // code logic here to change the object's appearance to indicate it is not in place
        }

    }

    private void OnMouseDrag() {
        //Debug.Log("MouseDrag");
        transform.position = MouseWorldPosition() + offset;
    }

    private void OnMouseUp() {
        selected = false;
        Vector3 newPos = MouseWorldPosition();

        // last refactor - 11/10: Shawn -- fix index out of bounds error <> additionally clean up the Tile collision detection logic
        // based on the last collision detection (on a particular grid x,y position), the target of the probe item will go there
        (int cellX, int cellY) cellPos = GameObject.Find("ContainerPanel").GetComponent<ContainerManager>().FindGridPosition(newPos);

        if (cellPos.cellX != -1 && cellPos.cellY != -1) {
            Debug.Log("~~ Target Grid position: " + cellPos.cellX + ", " + cellPos.cellY + " ~~");
            (float x, float y) cell = GameObject.Find("ContainerPanel").GetComponent<ContainerManager>().GetBeaconPositionGrid(cellPos.cellX, cellPos.cellY);
            transform.position = new Vector3(cell.x, cell.y, -0.01f);

            // if the tile is in the initailized layer -- before in place -- move into layer 10 (ProbePartInPlace)
            if (this.gameObject.layer <= 9) {
                this.gameObject.layer = 10;
                // code logic here to change the object's appearance to indicate it is in place
            }

            /* 
            // Future direction - grid layout integration on the canvas panel for auto-responsive scaling -- sprint 3 or 4
            Debug.Log("~~ Targeting center of grid position ~~");
            GridLayout gridLayout = transform.parent.GetComponent<GridLayout>();
            Vector3Int cellPosition = gridLayout.WorldToCell(transform.position);
            if (cellPosition.x < 0 || cellPosition.y < 0) {
                Debug.Log("Invalid grid position");
                return;
            } else {
                Debug.Log("~ Asserting object to grid position: [" + cellPosition.x + ", " + cellPosition.y + "]");
                transform.position = gridLayout.CellToWorld(cellPosition);
            }
            */
        }
    }

    Vector3 MouseWorldPosition()
    {
        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }
}


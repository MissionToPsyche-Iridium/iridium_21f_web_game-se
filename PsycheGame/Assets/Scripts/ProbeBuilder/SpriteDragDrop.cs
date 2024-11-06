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
    }

    private void OnMouseDrag() {
        //Debug.Log("MouseDrag");
        transform.position = MouseWorldPosition() + offset;
    }

    private void OnMouseUp() {
        selected = false;

        // based on the last collision detection (on a particular grid x,y position), the target of the probe item will go there
        if (GameObject.Find("ContainerPanel").GetComponent<ContainerManager>().getTrigger()) {
            Debug.Log("---Beacon location found---");
            (float x, float y) pos= GameObject.Find("ContainerPanel").GetComponent<ContainerManager>().GetBeaconPosition();
            transform.position = new Vector3(pos.x, pos.y, 0);
        }
    }

    Vector3 MouseWorldPosition()
    {
        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProbeItemController : MonoBehaviour
{
    Vector3 offset;
    Collider2D col;
    ChassisController TilemapController;
    public string probeTag = "ProbeMap";


    void Awake()
    {
        col = GetComponent<Collider2D>();
    }
    void Start()
    {
        //Screen.SetResolution(2560, 1440, true);
        TilemapController = GameObject.Find("HappyTilemap").GetComponent<ChassisController>();
    }

    void OnMouseDown()
    {
        offset = transform.position - MouseWorldPosition();
    }

    void OnMouseDrag()
    {
        transform.position = MouseWorldPosition() + offset;
        // Debug.Log("Mouse Dragging - Position: " + transform.position);
    }

    void OnMouseUp()
    {
        // debug raycast location
        col.enabled = false;

        var rayOrigin = Camera.main.transform.position;
        var rayDirection = MouseWorldPosition() - Camera.main.transform.position;

        RaycastHit2D hit;
        
        if (hit = Physics2D.Raycast(rayOrigin, rayDirection))
        {
            if (hit.transform.tag == probeTag)
            {
                transform.position = hit.transform.position + new Vector3(0, 0, -0.01f);
                Debug.Log("collider name: " + hit.transform.name + " position: " + hit.transform.position); 

                // pass the probe item to the tilemap controller and place it to the appropriate grid location
                TilemapController.MoveProbeItemToGridPosition(this.gameObject, MouseWorldPosition());
            }
        }
    
        col.enabled = true;
        // transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }


    Vector3 MouseWorldPosition()
    {
        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }

}
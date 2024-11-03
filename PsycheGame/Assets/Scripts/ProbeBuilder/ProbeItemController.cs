using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProbeItemController : MonoBehaviour
{
    Vector3 offset;
    Collider2D col;
    ChassisController TilemapController;
    public string probeTag = "ProbeMap";

    public ChassisController TimeMapController { get; private set; }

    void Awake()
    {
        col = GetComponent<Collider2D>();
        TilemapController = GameObject.Find("HappyTilemap").GetComponent<ChassisController>();
    }
    void Start()
    {
        //Screen.SetResolution(2560, 1440, true);
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
        
        if (hit = Physics2D.Raycast(rayOrigin, rayDirection, Mathf.Infinity))
        {
            if (hit.transform.tag == probeTag)
            {
                // transform.position = hit.transform.position + new Vector3(0, 0, -0.01f);
                Debug.Log("collider name: " + hit.transform.name + " position: " + hit.transform.position);

                GridLayout gridLayout = TilemapController.tilemap.GetComponentInParent<GridLayout>();
                Vector3Int cellPosition = gridLayout.LocalToCell(transform.position);
                transform.position = gridLayout.CellToLocalInterpolated(cellPosition);

                Vector3Int cell = TilemapController.tilemap.LocalToCell(hit.point);
                Vector3 tilepos = TilemapController.tilemap.GetCellCenterLocal(cell);

                Debug.Log("Cell Position: " + cell + " tilemap get cell center to local: " + tilepos);
                int x = cell.x;
                int y = cell.y;
                //Vector3Int gridPosition = TimeMapController.gridInformation[x, y].GridPosition;
            }
        }
    
        col.enabled = true;
    }


    Vector3 MouseWorldPosition()
    {
        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }

}
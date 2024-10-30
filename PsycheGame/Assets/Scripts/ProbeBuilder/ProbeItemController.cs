using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProbeItemController : MonoBehaviour
{
    Vector3 offset;
    Collider2D col;
    public string probeTag = "ProbeMap";

    void Awake()
    {
        col = GetComponent<Collider2D>();
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
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == probeTag)
        {
            Vector3 tilePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            tilePosition.z = 0;

            GridLayout gridLayout = transform.parent.GetComponentInParent<GridLayout>();
            Vector3Int cellPosition = gridLayout.WorldToCell(tilePosition);
            Debug.Log(">> cell Position: " + cellPosition);

            transform.position = cellPosition + new Vector3(0, 0, -0.01f);

            // transform.position = collision.transform.position + new Vector3(0, 0, -0.01f);
        }
    }

    void OnMouseUp()
    {

        col.enabled = false;
        var rayOrigin = Camera.main.transform.position;
        var rayDirection = MouseWorldPosition() - Camera.main.transform.position;
        /* 
        RaycastHit2D hit;

        if (hit = Physics2D.Raycast(rayOrigin, rayDirection))
        {
            if (hit.transform.tag == probeTag)
            {
                transform.position = hit.transform.position + new Vector3(0, 0, -0.01f);
                Debug.Log("collider name: " + hit.transform.name);
            }
        } */
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        col.enabled = true;
    }


    Vector3 MouseWorldPosition()
    {
        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }

}
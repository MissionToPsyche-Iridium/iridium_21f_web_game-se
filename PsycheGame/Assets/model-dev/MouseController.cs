using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    Vector3 offset;
    Collider2D col;

    public string chassisTag = "Chassis";

    void Awake()
    {
        col = GetComponent<Collider2D>();
    }
    void Start()
    {
        Screen.SetResolution(1920, 1080, true);
    }
 
    void OnMouseDown()
    {
        offset = transform.position - MouseWorldPosition();
    }

    void OnMouseDrag()
    {
        if (gameObject.layer == 8)
        {
            transform.position = MouseWorldPosition() + offset;
        }
    }

    void OnMouseUp()
    {
        col.enabled = false;
        var rayOrigin = Camera.main.transform.position;
        var rayDirection = MouseWorldPosition() - Camera.main.transform.position;
        RaycastHit2D hit;
        
        if (hit = Physics2D.Raycast(rayOrigin, rayDirection))
        {
            if (hit.transform.tag == chassisTag)
            {
                transform.position = hit.transform.position + new Vector3(0, 0, -0.01f);
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

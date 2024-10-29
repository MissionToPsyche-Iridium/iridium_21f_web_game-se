using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor.Callbacks;
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
        Screen.SetResolution(2560, 1440, true);
    }

    void OnMouseDown()
    {
        if (gameObject.layer == 8)
        {
            offset = transform.position - MouseWorldPosition();
        }
    }

    void OnMouseDrag()
    {
        if (gameObject.layer == 8)
        {
            transform.position = MouseWorldPosition() + offset;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == chassisTag)
        {
            transform.position = collision.transform.position + new Vector3(0, 0, -0.01f);
        }
    }

    void OnMouseUp()
    {
        if (gameObject.layer == 8)
        {

            col.enabled = false;
            var rayOrigin = Camera.main.transform.position;
            var rayDirection = MouseWorldPosition() - Camera.main.transform.position;
            RaycastHit2D hit;
            // String pattern = "(Square)(.*)";

            if (hit = Physics2D.Raycast(rayOrigin, rayDirection))
            {
                if (hit.transform.tag == chassisTag)
                {
                    transform.position = hit.transform.position + new Vector3(0, 0, -0.01f);
                    Debug.Log("collider name: " + hit.transform.name);
                }
            }
            col.enabled = true;

        }
    }

    Vector3 MouseWorldPosition()
    {
        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }

}
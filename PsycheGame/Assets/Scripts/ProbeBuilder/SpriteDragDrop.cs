using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Text.RegularExpressions;
using UnityEditor.Callbacks;
using UnityEngine;


public class SpriteDragDrop : MonoBehaviour
{
    private UnityEngine.Vector2 mousePosition;
    private float offsetX, offsetY;
    private UnityEngine.Vector2 initialPos;
    public static bool selected;

    Collider2D col;

    Vector3 offset;
    
    private void OnMouseDown() {
        Debug.Log("MouseDown");
        selected = true;
        offset = transform.position - MouseWorldPosition();

        /*
        initialPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offsetX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;
        offsetY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y;
        */

    }

    private void OnMouseDrag() {
        Debug.Log("MouseDrag");

        transform.position = MouseWorldPosition() + offset;
        //mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //transform.position = new UnityEngine.Vector2(mousePosition.x - offsetX, mousePosition.y - offsetY);

        
    }

    private void OnMouseUp() {
        selected = false;
        Debug.Log("MouseUp");

        /*  
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
        */
    }

    Vector3 MouseWorldPosition()
    {
        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }
   

    //This function snaps parts into their assigned containers 
    // private void OnTriggerStay2D(Collider2D collision) {
    //     string thisGameObjectName;
    //     string thisGameObjectTag;
    //     string collisionGameObjectName;
    //     string collisionGameObjectTag;

    //     thisGameObjectName = gameObject.name.Substring(0, name.IndexOf("_")); //i.e. GammaRaySpectrometer_Part -->GammaRaySpectrometer
    //     thisGameObjectTag = gameObject.tag;
    //     collisionGameObjectName = collision.gameObject.name.Substring(0, name.IndexOf("_")); //i.e. GammaRaySpectrometer_Container-->GammaRaySpectrometer
    //     collisionGameObjectTag = collision.gameObject.tag;

    //     if(mouseReleased && thisGameObjectTag == "Part" && collisionGameObjectTag == "Container" && thisGameObjectName == collisionGameObjectName) {
    //         gameObject.SetActive(false); //snap gameObject into place
    //         selected = false;

    //     } else {
    //        //return gameObject to original position
    //        gameObject.transform.position = initialPos;
    //     }
    // }
}


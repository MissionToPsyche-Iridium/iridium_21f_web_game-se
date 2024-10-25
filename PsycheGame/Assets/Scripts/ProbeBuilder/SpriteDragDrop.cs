using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;


public class SpriteDragDrop : MonoBehaviour
{
    private UnityEngine.Vector2 mousePosition;
    private float offsetX, offsetY;
    private UnityEngine.Vector2 initialPos;
    public static bool selected;
    
    private void OnMouseDown() {
        Debug.Log("MouseDown");
        selected = true;
        initialPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offsetX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;
        offsetY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y;
    }

    private void OnMouseDrag() {
        Debug.Log("MouseDrag");
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new UnityEngine.Vector2(mousePosition.x - offsetX, mousePosition.y - offsetY);
    }

    private void OnMouseUp() {
        selected = false;
        Debug.Log("MouseUp");
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


using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Tuturial: https://www.youtube.com/watch?v=9-ok9Cn3d90
//Allows you to drag and drop 2 sprites of the same kind onto each other to create a new second kind of sprite.

public class SpriteButton : MonoBehaviour
{
    private Vector2 mousePosition;
    private float offsetX, offsetY;
    
    public static bool mouseReleased;

    private void OnMouseDown() {
        Debug.Log("MouseDown");
        mouseReleased = false;
        offsetX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;
        offsetY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y;
    }

    private void OnMouseDrag() {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(mousePosition.x - offsetX, mousePosition.y - offsetY);
    }

    private void OnMouseUp() {
        mouseReleased = true;
        Debug.Log("MouseUp");
    }

    private void OnTriggerStay2D(Collider2D collision) {
        string thisGameObjectName;
        string collisionGameObjectName;

        thisGameObjectName = gameObject.name.Substring(0, name.IndexOf("_"));
        collisionGameObjectName =collision.gameObject.name.Substring(0, name.IndexOf("_"));

        if(mouseReleased && thisGameObjectName == "part" && thisGameObjectName == collisionGameObjectName) {
            Instantiate(Resources.Load("whole_object"), transform.position, Quaternion.identity);
            mouseReleased = false; //reset
            Destroy(collision.gameObject);
            Destroy(gameObject);

        } else if(mouseReleased && thisGameObjectName == "whole" && thisGameObjectName == collisionGameObjectName) {
            Instantiate(Resources.Load("another_object"), transform.position, Quaternion.identity);
            mouseReleased = false; //reset
            Destroy(collision.gameObject);
            Destroy(gameObject);

        }
    }
}

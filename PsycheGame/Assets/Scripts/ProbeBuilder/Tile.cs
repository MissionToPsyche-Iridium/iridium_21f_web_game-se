using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
    Author: Hannah M.
    Date: 10/??/2024
    Description: this script is a stereotype for the specific tile instantiated.  Including the coordinate
    and cell index attributes on the grid.

    v 1.1 - Shawn
    - Added cell and positional attributes purpose of optimizing identification of the cell, including in an 
    event of collision detection to associate with the probe item in contact with. 

    v 1.2 - Shawn (11/6)
    - Modified OnTriggerEnter2D to detect collision with probe part and set the tile as occupied --> used by the probe game object to position itself

*/


public class Tile : MonoBehaviour
{
    [SerializeField] private Color color1, color2;
    [SerializeField] private SpriteRenderer render;
    [SerializeField] private GameObject highlight;

    private int cellX, cellY;
    private float xPosition, yPosition;
    bool isOccupied;

    //paints tile
    public void Init(bool isOffset, int x, int y, float xP, float yP) {
        isOccupied = false;
        cellX = x;
        cellY = y;
        xPosition = xP;
        yPosition = yP;
       render.color = isOffset ? color1 : color2;
    }

    //setters and getters
    public int GetCellX() {
        return cellX;
    }
    public int GetCellY() {
        return cellY;
    }

    public float GetXPosition() {
        return xPosition;
    }
    public float GetYPosition() {
        return yPosition;
    }

    public (int, int) GetCell() {
        return (cellX, cellY);
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "ProbePart") {
            isOccupied = true;

            Debug.Log("Probe part collided on the following: " + collision.gameObject.transform.position);
            Debug.Log(">> Probe part collision detected on tile [" + cellX + ", " + cellY + "] <<");
            
            // beacon the last collision position to the container manager (temporary data structure for probe parts tracking)
            this.gameObject.GetComponentInParent<ContainerManager>().SetTrigger(true);
            this.gameObject.GetComponentInParent<ContainerManager>().SetBeaconPosition(cellX, cellY, xPosition, yPosition);
            Debug.Log(">> Beaconing on tile [" + cellX + ", " + cellY + "] <<");
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        Debug.Log("Collision exit");
        if (collision.gameObject.tag == "ProbePart") {
            //Debug.Log("Probe part collision detected");
            isOccupied = false;
        }
    }

    //Highlights tile on hover
    void OnMouseEnter() {
        //Debug.Log("tile active: [" + cellX + ", " + cellY + "]");
        highlight.SetActive(true);
    }

    void OnMouseExit() {
        //Debug.Log("tile inactive");
        highlight.SetActive(false);
    }

}

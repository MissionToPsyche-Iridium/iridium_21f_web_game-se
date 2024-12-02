using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/*
    Probe Builder:: Tile.cs
    
    Date: Oct. 2024
    Description: this script is a stereotype for the specific tile instantiated.  Including the coordinate
    and cell index attributes on the grid.
*/


public class Tile : MonoBehaviour
{
    [SerializeField] private Color color1, color2;
    [SerializeField] private SpriteRenderer render;

    private int cellX, cellY;
    private float xPosition, yPosition;

    private Color defaultColor;

    public void Init(bool isOffset, int x, int y, float xP, float yP) {
        cellX = x;
        cellY = y;
        xPosition = xP;
        yPosition = yP;
        render.color = isOffset ? color1 : color2;
        defaultColor = render.color;
    }

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
        if (collision.gameObject.tag == "ProbePart") 
        {
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "ProbePart") 
        {
        }
    }

    void OnMouseEnter() {
        String occupied = gameObject.GetComponentInParent<ContainerManager>().CheckGridOccupied(cellX, cellY);
        if (occupied != "") {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        } else {
            gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    void OnMouseExit() {

        gameObject.GetComponent<SpriteRenderer>().color = defaultColor;
    }

}

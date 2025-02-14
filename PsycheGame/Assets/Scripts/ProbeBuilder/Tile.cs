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

    v1.0 - Jan 21
    :: 1.0 candidate - Jan 21 - refactored code to meet C# convention for performance and readability
    v1.1 - Feb 1
    :: 1.1 - Feb 1 - refactored code to use the color scheme defined in the Constants class. Move handle to 
    the ContainerManager class.
    v1.2 - Feb 6
    :: 1.2 - Feb 6 - refactored code to use the color scheme set in the ContainerManager class.  Eliminated
    code and stack mem required to hold the color scheme in the tile class.
*/


public class Tile : MonoBehaviour
{
    [SerializeField] private Color color1;
    [SerializeField] private Color color2;
    [SerializeField] private Color openTileColor;
    [SerializeField] private Color occupiedTileColor;
    [SerializeField] private SpriteRenderer render;

    private int cellX;
    private int cellY;
    private float xPosition;
    private float yPosition;

    private Color defaultColor;

    public void Init(bool isOffset, int x, int y, float xP, float yP)
    {
        (color1, color2, openTileColor, occupiedTileColor) = GameObject.Find("ContainerPanel").GetComponent<ContainerManager>().GetTileColors();

        cellX = x;
        cellY = y;
        xPosition = xP;
        yPosition = yP;
        render.color = isOffset ? color1 : color2;
        defaultColor = render.color;
    }

    public int GetCellX()
    {
        return cellX;
    }
    public int GetCellY()
    {
        return cellY;
    }

    public float GetXPosition()
    {
        return xPosition;
    }
    public float GetYPosition()
    {
        return yPosition;
    }

    public (int, int) GetCell()
    {
        return (cellX, cellY);
    }

    void OnMouseEnter()
    {
        (color1, color2, openTileColor, occupiedTileColor) = GameObject.Find("ContainerPanel").GetComponent<ContainerManager>().GetTileColors();
        String occupied = gameObject.GetComponentInParent<ContainerManager>().CheckGridOccupied(cellX, cellY);
        if (occupied != String.Empty) {
            gameObject.GetComponent<SpriteRenderer>().color = occupiedTileColor;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = openTileColor;
        }
    }

    void OnMouseExit()
    {
        gameObject.GetComponent<SpriteRenderer>().color = defaultColor;
    }
}

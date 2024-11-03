 using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GridInformation
{
    public Vector3Int GridPosition { get; private set; }
    public Vector3 WorldPosition { get; private set; }
    public Vector3 CellToCamPositioon { get; private set; }
    public bool IsOccupied { get; private set; }
    public GameObject OccupyingObject { get; private set; }

    public GridInformation(Vector3Int gridPosition, Vector3 worldPosition, Vector3 celltoCamPosition, bool isOccupied, GameObject occupyingObject)
    {
        GridPosition = gridPosition;
        WorldPosition = worldPosition;
        CellToCamPositioon = celltoCamPosition;
        IsOccupied = isOccupied;
        OccupyingObject = occupyingObject;
    }

    public void SetOccupyingObject(GameObject occupyingObject)
    {
        OccupyingObject = occupyingObject;
        IsOccupied = true;
    }

    public Vector3Int GetGridPosition()
    {
        return GridPosition;
    }

    public void RemoveOccupyingObject()
    {
        OccupyingObject = null;
        IsOccupied = false;
    }

}
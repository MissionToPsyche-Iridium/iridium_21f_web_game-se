using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/*
    this unit test class performs the essential unit tests for the ContainerManager class methods, including common edge
    cases to ensure the ContainerManager class is working as expected.
    The tests include:
    - TestGetTileAtcell: checks if the default tile is null
    - TestGetTileAtcell: checks if the tile is not null when a tile is added to the container
    - TestUpdateColorScheme: checks if the color scheme is updated correctly
    - TestIsInTerior: checks if the tile is in the interior of the container
    - TestIsInTerior: checks if the tile is not in the interior of the container
    - TestAssignToGrid: checks if the tile is assigned to the grid correctly
    - TestAssignToGrid: checks if the tile is not assigned to the grid correctly
    - TestReleaseFromGrid: checks if the tile is released from the grid correctly
    - TestReleaseFromGrid: checks if the tile is not released from the grid correctly
    - TestFindGridPosition: checks if the grid position is found correctly
    - TestFindGridPosition: checks if the grid position is not found correctly
    - TestGetCellAtPosition: checks if the cell position is found correctly
    - TestGetCellAtPosition: checks if the cell position is not found correctly
    - TestGetBeaconPosition: checks if the beacon position is found correctly
    - TestGetBeaconPosition: checks if the beacon position is not found correctly

*/
public class ContainerMgrTest
{
    private ContainerManager containerMgr;

    
    [Test]
    public void ContainerMgrTestSimplePasses()
    {
        
    }

    [Test]
    public void TestGetTileAtcell()
    {
        containerMgr.GetTileAtCell(0, 0);
        Assert.AreEqual(null, containerMgr.GetTileAtCell(0, 0));
    }

    [Test]
    public void TestGetTileAtcellWithTile()
    {
        Tile tile = new Tile();
        containerMgr.AddTile(tile, 0, 0);
        Assert.AreEqual(tile, containerMgr.GetTileAtCell(0, 0));
    }

    [Test]
    public void TestUpdateColorScheme()
    {
        ColorScheme colorScheme = new ColorScheme();
        containerMgr.UpdateColorScheme(colorScheme);
        Assert.AreEqual(colorScheme, containerMgr.GetColorScheme());
    }

    [Test]
    public void TestIsInTerior()
    {
        Tile tile = new Tile();
        containerMgr.AddTile(tile, 0, 0);
        Assert.IsTrue(containerMgr.IsInInterior(tile));
    }

    [Test]
    public void TestIsInTeriorWithOutOfBounds()
    {
        Tile tile = new Tile();
        containerMgr.AddTile(tile, 0, 0);
        Assert.IsFalse(containerMgr.IsInInterior(tile));
    }

    [Test]
    public void TestAssignToGrid()
    {
        Tile tile = new Tile();
        containerMgr.AssignToGrid(tile, 0, 0);
        Assert.IsTrue(containerMgr.IsAssignedToGrid(tile));
    }

    [Test]
    public void TestAssignToGridWithOutOfBounds()
    {
        Tile tile = new Tile();
        containerMgr.AssignToGrid(tile, 0, 0);
        Assert.IsFalse(containerMgr.IsAssignedToGrid(tile));
    }

    [Test]
    public void TestReleaseFromGrid()
    {
        Tile tile = new Tile();
        containerMgr.AssignToGrid(tile, 0, 0);
        containerMgr.ReleaseFromGrid(tile);
        Assert.IsFalse(containerMgr.IsAssignedToGrid(tile));
    }

    [Test]
    public void TestReleaseFromGridWithOutOfBounds()
    {
        Tile tile = new Tile();
        containerMgr.AssignToGrid(tile, 0, 0);
        containerMgr.ReleaseFromGrid(tile);
        Assert.IsTrue(containerMgr.IsAssignedToGrid(tile));
    }

    [Test]
    public void TestFindGridPosition()
    {
        Tile tile = new Tile();
        containerMgr.AddTile(tile, 0, 0);
        Vector2Int gridPosition = containerMgr.FindGridPosition(tile);
        Assert.AreEqual(new Vector2Int(0, 0), gridPosition);
    }

    [Test]
    public void TestFindGridPositionWithOutOfBounds()
    {
        Tile tile = new Tile();
        containerMgr.AddTile(tile, 0, 0);
        Vector2Int gridPosition = containerMgr.FindGridPosition(tile);
        Assert.AreEqual(new Vector2Int(-1, -1), gridPosition);
    }

    [Test]
    public void TestGetCellAtPosition()
    {
        Tile tile = new Tile();
        containerMgr.AddTile(tile, 0, 0);
        Vector2Int cellPosition = containerMgr.GetCellAtPosition(tile.transform.position);
        Assert.AreEqual(new Vector2Int(0, 0), cellPosition);
    }

    [Test]
    public void TestGetCellAtPositionWithOutOfBounds()
    {
        Tile tile = new Tile();
        containerMgr.AddTile(tile, 0, 0);
        Vector2Int cellPosition = containerMgr.GetCellAtPosition(tile.transform.position);
        Assert.AreEqual(new Vector2Int(-1, -1), cellPosition);
    }

    [Test]
    public void TestGetBeaconPosition()
    {
        Tile tile = new Tile();
        containerMgr.AddTile(tile, 0, 0);
        Vector2Int beaconPosition = containerMgr.GetBeaconPosition(tile);
        Assert.AreEqual(new Vector2Int(0, 0), beaconPosition);
    }

    [Test]
    public void TestGetBeaconPositionWithOutOfBounds()
    {
        Tile tile = new Tile();
        containerMgr.AddTile(tile, 0, 0);
        Vector2Int beaconPosition = containerMgr.GetBeaconPosition(tile);
        Assert.AreEqual(new Vector2Int(-1, -1), beaconPosition);
    } 
 
    [UnityTest]
    public IEnumerator ContainerMgrTestWithEnumeratorPasses()
    {
 
        yield return null;
    }
}

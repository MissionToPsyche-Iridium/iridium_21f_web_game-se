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
    - TestIsInTeriorWithOutOfBounds: checks if the tile is not in the interior of the container when it is out of bounds
    - TestAssignToGrid: checks if the tile is assigned to the grid correctly
    - TestAssignToGridWithOutOfBounds: checks if the tile is not assigned to the grid when it is out of bounds
    - TestReleaseFromGrid: checks if the tile is released from the grid correctly
    - TestReleaseFromGridWithOutOfBounds: checks if the tile is not released from the grid when it is out of bounds

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
    // Test if color scheme update is functioning correctly (1 of 2)
    public void TestUpdateColorScheme()
    {
        TileColorScheme colorScheme = new TileAltScheme();
        int targetScheme = 2;
        containerMgr.SetColorScheme(targetScheme);
        Assert.AreEqual(colorScheme, containerMgr.GetColorScheme());
    }

    [Test]
    // Test if color scheme update is functioning correctly (2 of 2)
    public void TestUpdateColorScheme2()
    {
        TileColorScheme colorScheme = new TileStdScheme();
        int targetScheme = 1;
        containerMgr.SetColorScheme(targetScheme);
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
        GameObject tileObj = new GameObject();
        containerMgr.AssignToGridPosition(0,0,tileObj);
        Assert.IsTrue(containerMgr.IsAssignedToGrid(0,0));
    }

    [Test]
    public void TestAssignToGridWithOutOfBounds()
    {
        GameObject tileObj = new GameObject();
        containerMgr.AssignToGridPosition(-1, -1, tileObj);
        Assert.IsFalse(containerMgr.IsAssignedToGrid(-1, -1));
    }

    [Test]
    public void TestReleaseFromGrid()
    {
        GameObject tileObj = new GameObject();
        containerMgr.AssignToGridPosition(0,0,tileObj);
        containerMgr.ReleaseFromGridPosition(0,0,tileObj);
        Assert.IsFalse(containerMgr.IsAssignedToGrid(0,0));
    }

    [Test]
    public void TestReleaseFromGridWithOutOfBounds()
    {
        GameObject tileObj = new GameObject();
        containerMgr.AssignToGridPosition(-1, -1, tileObj);
        containerMgr.ReleaseFromGridPosition(-1, -1, tileObj);
        Assert.IsFalse(containerMgr.IsAssignedToGrid(-1, -1));
    }
 
    [UnityTest]
    public IEnumerator ContainerMgrTestWithEnumeratorPasses()
    {
        yield return null;
    }
}

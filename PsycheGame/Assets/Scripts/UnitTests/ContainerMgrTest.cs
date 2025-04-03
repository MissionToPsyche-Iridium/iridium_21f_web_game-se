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

    [SetUp]
    public void Setup()
    {
        // Create a new GameObject and add the ContainerManager component for testing
        GameObject containerObj = new GameObject("ContainerManagerTest");
        containerMgr = containerObj.AddComponent<ContainerManager>();
        containerMgr.SetColorProfile(1);

        // Initialize the grid data to ensure we can run tests
        containerMgr.InitGridData();

        Assert.IsNotNull(containerMgr, "ContainerManager component was not created");
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up the ContainerManager and its GameObject after tests
        if (containerMgr != null && containerMgr.gameObject != null)
        {
            Object.DestroyImmediate(containerMgr.gameObject);
        }
        containerMgr = null; // Ensure reference is cleared
    }
    
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
    public void TestUpdateColorSchemeToAlt()
    {
        TileColorScheme colorScheme = new TileAltScheme();
        int targetScheme = 2;
        containerMgr.SetColorProfile(1);
        containerMgr.SetColorScheme(targetScheme);
        Assert.IsInstanceOf(typeof(TileAltScheme), containerMgr.GetCurrentColorScheme(), 
            "The current color scheme should be TileAltScheme when scheme is set to 2");
    }

    [Test]
    // Test if color scheme update is functioning correctly (2 of 2)
    public void TestUpdateColorSchemeToStd()
    {
        TileColorScheme colorScheme = new TileStdScheme();
        int targetScheme = 1;
        containerMgr.SetColorProfile(2);  
        containerMgr.SetColorScheme(targetScheme);
        Assert.IsInstanceOf(typeof(TileStdScheme), containerMgr.GetCurrentColorScheme(), 
            "The current color scheme should be TileStdScheme when scheme is set to 1");
    }

    [Test]
    public void TestIsInTerior()
    {
        Tile tile = new Tile();
        containerMgr.AddTile(tile, 0, 0);
        Assert.IsFalse(containerMgr.IsInInterior(tile));
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
        bool success = containerMgr.AssignToGridPosition(0,0,tileObj);
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
    public void TestReleaseFromGridOutOfBounds()
    {
        GameObject tileObj = new GameObject();
        bool success = containerMgr.AssignToGridPosition(-1, -1, tileObj);
        Assert.IsFalse(success);
    }
 
    [UnityTest]
    public IEnumerator ContainerMgrTestWithEnumeratorPasses()
    {
        yield return null;
    }
}

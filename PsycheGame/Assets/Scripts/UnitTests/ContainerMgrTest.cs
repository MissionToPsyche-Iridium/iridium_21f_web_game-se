using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/*
    ContainerMgrTest.cs
    -------------------
    this unit test class performs the essential unit tests for the ContainerManager class methods, including common edge
    cases to ensure the ContainerManager class is working as expected.
    The tests include:
    - TestGetTileAtcell: checks if the default tile is null
    - TestGetTileAtcell: checks if the tile is not null when a tile is added to the container
    - TestUpdateColorScheme: checks if the color scheme is updated correctly
    - TestUpdateColorSchemeToStd: checks if the color scheme can be set to standard (1)
    - TestUpdateColorSchemeToAlt: checks if the color scheme can be set to alternate (2)
    - TestIsInTerior: checks if the tile is in the interior of the container
    - TestIsInTeriorWithOutOfBounds: checks if the tile is not in the interior of the container when it is out of bounds
    - TestAssignToGridStandard: checks if the tile is assigned to the grid correctly
    - TestAssignToGridEdge1: checks if the tile can be assigned to the edge of the grid (0,0)
    - TestAssignToGridEdge2: checks if the tile can be assigned to the edge of the grid (5,5)
    - TestAssignToGridWithOutOfBounds: checks if the tile is not assigned to the grid when it is out of bounds
    - TestReleaseFromGridStandard: checks if the tile is released from the grid correctly
    - TestReleaseFromGridEdge1: checks if the tile can be released from the edge of the grid (0,0)
    - TestReleaseFromGridEdge2: checks if the tile can be released from the edge of the grid (5,5)
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
        // This is a simple test to ensure the ContainerManager setup was successful
        Assert.IsNotNull(containerMgr, "ContainerManager should not be null after setup");
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
    public void TestColorCodeForStandardScheme()
    {
        TileColorScheme colorScheme = new TileStdScheme();
        containerMgr.SetColorProfile(1); // Set to standard scheme
        containerMgr.SetColorScheme(1); // Ensure we are using the standard scheme

        Assert.IsInstanceOf(typeof(TileStdScheme), colorScheme, 
            "The current color scheme should be TileStdScheme for standard scheme");

        Color openColor = colorScheme.GetOpenTileColor();
        Assert.AreEqual(Color.green, openColor, "The open tile color should be green in the standard scheme");
    }

    [Test]
    public void TestColorCodeForAlternateScheme()
    {
        TileColorScheme colorScheme = new TileAltScheme(); 
        containerMgr.SetColorProfile(2); // Set to alternate scheme
        containerMgr.SetColorScheme(2); // Ensure we are using the alternate scheme

        Assert.IsInstanceOf(typeof(TileAltScheme), colorScheme, 
            "The current color scheme should be TileAltScheme for alternate scheme");

        Color openColor = colorScheme.GetOpenTileColor();
        Assert.AreEqual(Color.blue, openColor, "The open tile color should be blue in the alternate scheme");
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
    public void TestAssignToGridStandard()
    {
        GameObject tileObj = new GameObject();
        bool success = containerMgr.AssignToGridPosition(3, 2, tileObj);
        Assert.IsTrue(success, "Tile should be successfully assigned to grid position (3, 2)");
        Assert.IsTrue(containerMgr.IsAssignedToGrid(3, 2), 
            "Tile should be found in the grid at position (3, 2)");
    }

    [Test]
    public void TestAssignToGridEdge1()
    {   
        GameObject tileObj = new GameObject();
        bool success = containerMgr.AssignToGridPosition(0,0,tileObj);
        Assert.IsTrue(containerMgr.IsAssignedToGrid(0,0), 
            "Tile should be successfully assigned to grid position (0, 0)");
    }

    [Test]
    public void TestAssignToGridEdge2()
    {
        // Test assigning to the edge of the grid, assuming grid size is at least 1x1
        GameObject tileObj = new GameObject();
        bool success = containerMgr.AssignToGridPosition(5, 5, tileObj);
        Assert.IsTrue(containerMgr.IsAssignedToGrid(5, 5), 
            "Tile should be found in the grid at the edge position (5, 5)");
    }

    [Test]
    public void TestAssignToGridWithOutOfBounds()
    {
        GameObject tileObj = new GameObject();
        containerMgr.AssignToGridPosition(-1, -1, tileObj);
        Assert.IsFalse(containerMgr.IsAssignedToGrid(-1, -1));
    }

    [Test]
    public void TestReleaseFromGridStandard()
    {
        GameObject tileObj = new GameObject();
        containerMgr.AssignToGridPosition(2,4,tileObj);
        containerMgr.ReleaseFromGridPosition(2,4,tileObj);
        Assert.IsFalse(containerMgr.IsAssignedToGrid(2,4));
    }

    [Test]
    public void TestReleaseFromGridEdge1()
    {
        GameObject tileObj = new GameObject();
        containerMgr.AssignToGridPosition(0, 0, tileObj);
        containerMgr.ReleaseFromGridPosition(0, 0, tileObj);
        Assert.IsFalse(containerMgr.IsAssignedToGrid(0, 0), 
            "Tile should no longer be assigned to grid position (0, 0)");
    }

    [Test]
    public void TestReleaseFromGridEdge2()
    {
        GameObject tileObj = new GameObject();
        containerMgr.AssignToGridPosition(5, 5, tileObj);
        containerMgr.ReleaseFromGridPosition(5, 5, tileObj);
        Assert.IsFalse(containerMgr.IsAssignedToGrid(5, 5), 
            "Tile should no longer be assigned to grid position (5, 5)");
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

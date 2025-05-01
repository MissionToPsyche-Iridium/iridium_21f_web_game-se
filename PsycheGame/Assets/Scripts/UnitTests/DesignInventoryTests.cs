using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[TestFixture]
public class DesignInventoryTests
{
    private DesignInventory designInventory;
    private List<ProbeDesign> designs;
    private GameObject dummyUiDesignObject;

    [SetUp]
    public void Setup()
    {
        GameObject diObj = new GameObject("DesignInventory");
        designInventory = diObj.AddComponent<DesignInventory>();
        designs = new List<ProbeDesign>();
        designInventory.designs = designs;

        var indexField = typeof(DesignInventory).GetField("index", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var maxIndexField = typeof(DesignInventory).GetField("maxIndex", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        indexField.SetValue(designInventory, 0);
        maxIndexField.SetValue(designInventory, designs.Count);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(designInventory.gameObject);
        Object.DestroyImmediate(dummyUiDesignObject);
        designInventory = null;
        designs = null;
        dummyUiDesignObject = null;
    }

    [Test]
    public void DesignInventory_InitializesWithEmptyDesigns()
    {
        Assert.IsNotNull(designInventory);
        Assert.IsNotNull(designInventory.designs);
        Assert.AreEqual(0, designInventory.designs.Count);
    }

    [Test]
    public void AddDesign_IncreasesDesignCount()
    {
        ProbeDesign design = new ProbeDesign(null, "Test Design", "list of parts", new List<GameObject>(), new ProbeAttributeTotals(), "names");
        designs.Add(design);
        Assert.AreEqual(1, designInventory.designs.Count);

        var maxIndexField = typeof(DesignInventory).GetField("maxIndex", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        maxIndexField.SetValue(designInventory, designs.Count);
    }

    [Test]
    public void SelectShipDesign_ReturnsPartsJson()
    {
        ProbeDesign design = new ProbeDesign(null, "Test Design", "list of parts", new List<GameObject>(), new ProbeAttributeTotals(), "names");
        designs.Add(design);
        
        var maxIndexField = typeof(DesignInventory).GetField("maxIndex", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        maxIndexField.SetValue(designInventory, designs.Count);

        string result = designInventory.selectShipDesign();
        Assert.AreEqual("list of parts", result);
    }
}
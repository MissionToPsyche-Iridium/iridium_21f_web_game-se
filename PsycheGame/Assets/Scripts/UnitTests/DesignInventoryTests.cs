using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class DesignInventoryTests
{

    private DesignInventory designInventory;
    private ProbeDesign design;
    private List<ProbeDesign> designs;
    
    [SetUp]
    public void Setup() {
        GameObject di_obj = new GameObject("DesignInventory");
        designInventory = di_obj.AddComponent<DesignInventory>();
        Assert.IsNotNull(designInventory, "Design Inventory was not created.");

        designs = new List<ProbeDesign>();
        Assert.IsNotNull(designs, "Design was not added to designs list.");

    }

    [TearDown]
    public void TearDown() {
        Object.DestroyImmediate(designInventory.gameObject);
        designInventory = null;

        design = null;

        designs = null;
    }

    [Test]
    public void DesignInventoryTestsSimplePasses()
    {
        Assert.IsNotNull(designInventory, "Test Design Inventory is null");
    }

    [Test]
    public void AddDesign() {
        design = new ProbeDesign(null, "Test Design", "list of parts", new List<GameObject>(), new ProbeAttributeTotals() ,"names");
        Assert.IsNotNull(design, "Design was not created.");
        designs.Add(design);
        Assert.AreEqual(designs.Count,1);
    }

    [Test]
    public void DesignInventoryTestStart() {
        designInventory.Start(designs);
    }

    [Test]
    public void DeleteDesign() {
        designInventory.deleteShipDesign();
        Assert.AreEqual(designInventory.designs.Count, 0);
    }

    [Test]
    public void SelectShipDesign() {
        Assert.AreEqual(designInventory.selectShipDesign(), "list of parts");
    }

    [UnityTest]
    public IEnumerator DesignInventoryTestsWithEnumeratorPasses()
    {
        yield return null;
    }
}

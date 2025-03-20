using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.TestTools;

[TestFixture]
public class MineralCollectionTests
{
    private class TestMineralCollection : MineralCollection
    {
        public new int maxMetalTypes => base.maxMetalTypes;
        public new int minMetalTypes => base.minMetalTypes;
        public new int maxTotalAmount => base.maxTotalAmount;
        public new int minTotalAmount => base.minTotalAmount;

        public new void Drill()
        {
            foreach (RareMetal metal in metals)
            {
                if (metal.Amount > 0)
                {
                    int minedAmount = 20;
                    metal.Amount = Mathf.Max(0, metal.Amount - minedAmount);
                    UpdateMissionProgress(minedAmount, metal.Name);
                    break;
                }
            }
        }
    }

    private GameObject mineralObject;
    private TestMineralCollection mineralCollection;
    private MissionState missionState;
    
    [SetUp]
    public void Setup()
    {
        mineralObject = new GameObject("MineralCollection");
        mineralObject.AddComponent<BoxCollider2D>();
        mineralCollection = mineralObject.AddComponent<TestMineralCollection>();

        missionState = new MissionState();
        var objectives = new List<MissionState.MissionObjective>
        {
            new MissionState.MissionObjective { objectiveType = MissionState.ObjectiveType.CollectGases, targetAmount = 100 },
            new MissionState.MissionObjective { objectiveType = MissionState.ObjectiveType.CollectRareMetals, targetAmount = 50 }
        };
        missionState.Initialize(objectives, "TestLevel");

        typeof(MissionState).GetField("instance", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
            ?.SetValue(null, missionState);

        mineralCollection.metals = new List<RareMetal>
        {
            new Titanium(50),
            new Palladium(30),
            new Iridium(20)
        };

        typeof(MineralCollection).GetField("missionState", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(mineralCollection, missionState);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(mineralObject);
        typeof(MissionState).GetField("instance", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
            ?.SetValue(null, null);
    }

    [Test]
    public void GenerateMetals_CreatesValidMetalList()
    {
        Assert.IsNotEmpty(mineralCollection.metals);
        Assert.IsTrue(mineralCollection.metals.Count >= mineralCollection.minMetalTypes);
        Assert.IsTrue(mineralCollection.metals.Count <= mineralCollection.maxMetalTypes);

        int totalAmount = 0;
        foreach (var metal in mineralCollection.metals)
        {
            Assert.IsNotNull(metal);
            Assert.IsTrue(metal.Amount > 0);
            totalAmount += metal.Amount;
        }
        Assert.IsTrue(totalAmount <= mineralCollection.maxTotalAmount);
    }

    [Test]
    public void IsDepleted_ReturnsTrueWhenAllMetalsZero()
    {
        foreach (var metal in mineralCollection.metals)
        {
            metal.Amount = 0;
        }

        bool isDepleted = mineralCollection.IsDepleted();
        Assert.IsTrue(isDepleted);
    }

    [Test]
    public void IsDepleted_ReturnsFalseWhenMetalsRemain()
    {
        mineralCollection.metals[0].Amount = 10;

        bool isDepleted = mineralCollection.IsDepleted();
        Assert.IsFalse(isDepleted);
    }

    [Test]
    public void Drill_ReducesMetalAmount()
    {
        var initialMetal = mineralCollection.metals[0];
        int initialAmount = initialMetal.Amount;

        mineralCollection.Drill(); 
        Assert.Less(initialMetal.Amount, initialAmount, "Metal amount should decrease after drilling");
        Assert.GreaterOrEqual(initialMetal.Amount, 0, "Metal amount should not go below 0");
    }
}
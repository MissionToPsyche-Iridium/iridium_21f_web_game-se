using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using UnityEngine.SceneManagement;

[TestFixture]
public class SpaceGamePlayModeTests
{
    private GameObject levelManagerGO;
    private LevelManager levelManager;
    private GameObject shipGO;
    private ShipMovement shipMovement;

    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("TestScene"); 
        
        levelManagerGO = new GameObject("LevelManager");
        levelManager = levelManagerGO.AddComponent<LevelManager>();
        
        shipGO = new GameObject("Ship");
        shipMovement = shipGO.AddComponent<ShipMovement>();
        shipGO.AddComponent<Rigidbody2D>();
        
        levelManager.SetLevels(new System.Collections.Generic.List<LevelConfig>
        {
            ScriptableObject.CreateInstance<LevelConfig>()
        });
        levelManager.GetLevels()[0].missionTimer = 180f;
        
        ShipManager.Fuel = 100f; 
        ShipManager.Health = 100; 
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(levelManagerGO);
        Object.Destroy(shipGO);
    }

    [UnityTest]
    public IEnumerator Test_ShipMovement_ConsumesFuel()
    {
        float initialFuel = ShipManager.Fuel;
        shipMovement.moveSpeed = 7.5f;
        shipMovement.fuelConsumptionRate = 1f;

        shipMovement.Update();
        yield return new WaitForFixedUpdate();

        Assert.Less(ShipManager.Fuel, initialFuel, "Fuel should decrease when ship moves");
        Assert.GreaterOrEqual(ShipManager.Fuel, 0f, "Fuel should not go below 0");
    }

    [UnityTest]
    public IEnumerator Test_DrillAsteroid_CollectsResources()
    {
        GameObject drillGO = new GameObject("Drill");
        drillGO.AddComponent<DrillController>();
        BoxCollider2D drillCollider = drillGO.AddComponent<BoxCollider2D>();
        drillCollider.isTrigger = true;

        GameObject asteroidGO = new GameObject("Asteroid");
        MineralCollection asteroid = asteroidGO.AddComponent<MineralCollection>();
        BoxCollider2D asteroidCollider = asteroidGO.AddComponent<BoxCollider2D>();
        asteroidCollider.isTrigger = true;

        drillGO.transform.position = Vector3.zero;
        asteroidGO.transform.position = Vector3.zero;

        yield return new WaitForFixedUpdate();
        Assert.Less(asteroid.metals[0].Amount, 50, "Metal amount should decrease after drilling");
        Assert.Greater(MissionState.Instance.GetObjectiveProgress(MissionState.ObjectiveType.CollectRareMetals), 0, "Mission progress should increase");
    }

    [UnityTest]
    public IEnumerator Test_LevelProgression_OnSuccess()
    {
        int initialLevel = levelManager.getCurrentLevel();
        MissionState.Instance.Initialize(new System.Collections.Generic.List<MissionState.MissionObjective>
        {
            new MissionState.MissionObjective { objectiveType = MissionState.ObjectiveType.CollectRareMetals, targetAmount = 10 }
        }, "TestLevel");
        MissionState.Instance.UpdateObjectiveProgress(MissionState.ObjectiveType.CollectRareMetals, 10);

        levelManager.EndLevel(true);
        yield return new WaitForSeconds(4f);

        Assert.AreEqual(initialLevel + 1, levelManager.getCurrentLevel(), "Level should increment on success");
        Assert.Greater(levelManager.PlayerScore, 0, "Score should increase on level completion");
    }

    [UnityTest]
    public IEnumerator Test_MissionObjective_Completion()
    {
        MissionState.Instance.Initialize(new System.Collections.Generic.List<MissionState.MissionObjective>
        {
            new MissionState.MissionObjective { objectiveType = MissionState.ObjectiveType.CollectGases, targetAmount = 50 },
            new MissionState.MissionObjective { objectiveType = MissionState.ObjectiveType.CollectRareMetals, targetAmount = 50 }
        }, "TestLevel");

        MissionState.Instance.UpdateObjectiveProgress(MissionState.ObjectiveType.CollectGases, 50);
        MissionState.Instance.UpdateObjectiveProgress(MissionState.ObjectiveType.CollectRareMetals, 50);
        yield return null;

        Assert.IsTrue(MissionState.Instance.IsMissionComplete, "Mission should be complete when all objectives are met");
    }
}
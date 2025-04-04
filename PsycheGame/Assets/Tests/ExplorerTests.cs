using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

[TestFixture]
public class ExplorerPlayModeTests
{
    private GameObject levelManagerGO;
    private LevelManager levelManager;
    private GameObject shipGO;
    private ShipMovement shipMovement;
    private GameObject gasGO;

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
        
        gasGO = new GameObject("GasCloud");
        ParticleSystem particleSystem = gasGO.AddComponent<ParticleSystem>();
        BoxCollider2D gasCollider = gasGO.AddComponent<BoxCollider2D>();
        gasCollider.isTrigger = true;

        var renderer = gasGO.GetComponent<ParticleSystemRenderer>();
        if (renderer.sharedMaterial == null)
        {
            renderer.sharedMaterial = new Material(Shader.Find("Particles/Standard Unlit"));
        }

        ShipManager.Fuel = 100f; 
        ShipManager.Health = 100; 
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(levelManagerGO);
        Object.Destroy(gasGO);
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

    [UnityTest]
    public IEnumerator Test_ShipCollision_ReducesHealth()
    {
        GameObject collisionHandlerGO = new GameObject("ShipCollisionHandler");
        ShipCollisionHandler collisionHandler = collisionHandlerGO.AddComponent<ShipCollisionHandler>();
      
        GameObject shipGO = new GameObject("Ship");
        GameObject modalPanelGO = new GameObject("ModalPanel");
        HealthBar healthBar = new GameObject("HealthBar").AddComponent<HealthBar>();

        collisionHandler.InitializeForTest(shipGO, modalPanelGO, healthBar);

        int initialHealth = (int) ShipManager.Health;

        collisionHandler.HandleAsteroidCollision(Vector2.right * 5f, Vector2.left);

        Assert.Less(ShipManager.Health, initialHealth, "Health should decrease after collision");

        yield return null;
    }

    [UnityTest]
    public IEnumerator Test_AlertNotification_FlashesOnAsteroid()
    {
        GameObject alertGO = new GameObject("AlertNotification");
        AlertNotification alert = alertGO.AddComponent<AlertNotification>();
        alert.alertPanel = new GameObject("AlertPanel");
        alert.flashInterval = 0.5f;
        BoxCollider2D alertCollider = alertGO.AddComponent<BoxCollider2D>();
        alertCollider.isTrigger = true;

        GameObject asteroidGO = new GameObject("Asteroid");
        asteroidGO.tag = "Asteroid";
        BoxCollider2D asteroidCollider = asteroidGO.AddComponent<BoxCollider2D>();
        asteroidCollider.isTrigger = true;

        alertGO.transform.position = Vector3.zero;
        asteroidGO.transform.position = Vector3.zero;

        yield return new WaitForFixedUpdate(); 
        yield return new WaitForSeconds(0.6f); 

        Assert.IsTrue(alert.alertPanel.activeSelf, "Alert panel should activate with nearby asteroid");
        yield return new WaitForSeconds(0.5f);
        Assert.IsFalse(alert.alertPanel.activeSelf, "Alert panel should flash off");

        asteroidGO.transform.position = Vector3.one * 10f;
        yield return new WaitForFixedUpdate();
        Assert.IsFalse(alert.alertPanel.activeSelf, "Alert panel should deactivate when asteroid leaves");
    }

    [UnityTest]
    public IEnumerator Test_HealthBar_UpdatesAndFlashes()
    {
        GameObject healthBarGO = new GameObject("HealthBar");
        HealthBar healthBar = healthBarGO.AddComponent<HealthBar>();
        healthBar.healthBarColor = new GameObject("HealthBarColor");
        healthBar.healthBarImage = healthBar.healthBarColor.AddComponent<Image>();
        healthBar.healthBar = healthBarGO.AddComponent<Slider>();
        healthBar.textDisplay = new GameObject("Text").AddComponent<TextMeshProUGUI>();

        ShipManager.Health = 75f;
        healthBar.UpdateIndicator();
        Assert.AreEqual(Color.green, healthBar.healthBarImage.color, "Health bar should be green at high health");

        ShipManager.Health = 40f;
        healthBar.UpdateIndicator();
        Assert.AreEqual(Color.yellow, healthBar.healthBarImage.color, "Health bar should be yellow at mid health");

        ShipManager.Health = 10f;
        healthBar.UpdateIndicator();
        yield return new WaitForSeconds(0.6f);
        Assert.AreEqual(Color.red, healthBar.healthBarImage.color, "Health bar should be red at low health");
        yield return new WaitForSeconds(0.5f);
        Assert.AreEqual(Color.white, healthBar.healthBarImage.color, "Health bar should flash white at low health");

        yield return null;
    }

    [UnityTest]
    public IEnumerator Test_FuelBar_UpdatesAndFlashes()
    {
        GameObject fuelBarGO = new GameObject("FuelBar");
        FuelBar fuelBar = fuelBarGO.AddComponent<FuelBar>();
        fuelBar.fuelBarColor = new GameObject("FuelBarColor");
        fuelBar.fuelBarImage = fuelBar.fuelBarColor.AddComponent<Image>();
        fuelBar.fuelBar = fuelBarGO.AddComponent<Slider>();
        fuelBar.textDisplay = new GameObject("Text").AddComponent<TextMeshProUGUI>();

        ShipManager.Fuel = 75f;
        fuelBar.UpdateIndicator(ShipManager.Fuel);
        Assert.AreEqual(Color.green, fuelBar.fuelBarImage.color, "Fuel bar should be green at high fuel");
        Assert.AreEqual("75", fuelBar.textDisplay.text, "Text should show current fuel");

        ShipManager.Fuel = 40f;
        fuelBar.UpdateIndicator(ShipManager.Fuel);
        Assert.AreEqual(Color.yellow, fuelBar.fuelBarImage.color, "Fuel bar should be yellow at mid fuel");
        Assert.AreEqual("40", fuelBar.textDisplay.text, "Text should show current fuel");

        ShipManager.Fuel = 10f;
        fuelBar.UpdateIndicator(ShipManager.Fuel);
        yield return new WaitForSeconds(0.6f);
        Assert.AreEqual(Color.red, fuelBar.fuelBarImage.color, "Fuel bar should be red at low fuel");
        yield return new WaitForSeconds(0.5f); 
        Assert.AreEqual(Color.white, fuelBar.fuelBarImage.color, "Fuel bar should flash white at low fuel");
        Assert.AreEqual("10", fuelBar.textDisplay.text, "Text should show current fuel");

        ShipManager.Fuel = 30f;
        fuelBar.UpdateIndicator(ShipManager.Fuel);
        Assert.AreEqual(Color.yellow, fuelBar.fuelBarImage.color, "Fuel bar should return to yellow above low threshold");

        yield return null;
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
    public IEnumerator Test_HeilumGas_Collection_ReducesFuel()
    {
        HeilumGas heilumGas = gasGO.AddComponent<HeilumGas>();
        float initialFuel = ShipManager.Fuel;

        shipGO.transform.position = Vector3.zero;
        gasGO.transform.position = Vector3.zero;

        yield return new WaitForFixedUpdate();

        heilumGas.OnCollect(10);

        Assert.AreEqual(initialFuel - 5f, ShipManager.Fuel, "HeilumGas should reduce fuel when collected");
    }

    [UnityTest]
    public IEnumerator Test_HydrogenGas_Collection_IncreasesFuel()
    {
        HydrogenGas hydrogenGas = gasGO.AddComponent<HydrogenGas>();
        float initialFuel = ShipManager.Fuel;

        shipGO.transform.position = Vector3.zero;
        gasGO.transform.position = Vector3.zero;

        yield return new WaitForFixedUpdate();

        hydrogenGas.OnCollect(10);

        Assert.AreEqual(initialFuel + 10f, ShipManager.Fuel, "HydrogenGas should increase fuel when collected");
    }

    [UnityTest]
    public IEnumerator Test_RareMetalCollectionStatusBar_UpdatesProgress()
    {
        GameObject metalBarGO = new GameObject("RareMetalBar");
        RareMetalCollectionStatusBar metalBar = metalBarGO.AddComponent<RareMetalCollectionStatusBar>();
        metalBar.rareMetalCollectionBarColor = new GameObject("BarColor");
        metalBar.rareMetalCollectBarImage = metalBar.rareMetalCollectionBarColor.AddComponent<Image>();
        metalBar.rareMetalCollectBar = metalBarGO.AddComponent<Slider>();
        metalBar.textDisplay = new GameObject("Text").AddComponent<TextMeshProUGUI>();

        MissionState.Instance.Initialize(new System.Collections.Generic.List<MissionState.MissionObjective>
        {
            new MissionState.MissionObjective { objectiveType = MissionState.ObjectiveType.CollectRareMetals, targetAmount = 100 }
        }, "TestLevel");
        metalBar.ResetStatusBar();

        Assert.AreEqual(Color.red, metalBar.rareMetalCollectBarImage.color, "Bar should be red at 0 progress");
        Assert.AreEqual("0/100", metalBar.textDisplay.text, "Text should show 0 progress");
        Assert.AreEqual(0f, metalBar.rareMetalCollectBar.value, "Slider should be at 0");

        metalBar.UpdateIndicator(70);
        Assert.AreEqual(Color.yellow, metalBar.rareMetalCollectBarImage.color, "Bar should be yellow at mid progress");
        Assert.AreEqual("70/100", metalBar.textDisplay.text, "Text should show 70 progress");
        Assert.AreEqual(70f, metalBar.rareMetalCollectBar.value, "Slider should be at 70");

        metalBar.UpdateIndicator(30); 
        Assert.AreEqual(Color.green, metalBar.rareMetalCollectBarImage.color, "Bar should be green when target met");
        Assert.AreEqual("100/100", metalBar.textDisplay.text, "Text should show 100 progress");
        Assert.AreEqual(100f, metalBar.rareMetalCollectBar.value, "Slider should be at 100");

        yield return null;
    }

    [UnityTest]
    public IEnumerator Test_GasCollectionStatusBar_UpdatesProgress()
    {
        GameObject gasBarGO = new GameObject("GasBar");
        GasCollectionStatusBar gasBar = gasBarGO.AddComponent<GasCollectionStatusBar>();
        gasBar.gasCollectionBarColor = new GameObject("BarColor");
        gasBar.gasCollectBarImage = gasBar.gasCollectionBarColor.AddComponent<Image>();
        gasBar.gasCollectBar = gasBarGO.AddComponent<Slider>();
        gasBar.textDisplay = new GameObject("Text").AddComponent<TextMeshProUGUI>();

        MissionState.Instance.Initialize(new System.Collections.Generic.List<MissionState.MissionObjective>
        {
            new MissionState.MissionObjective { objectiveType = MissionState.ObjectiveType.CollectGases, targetAmount = 50 }
        }, "TestLevel");
        gasBar.ResetStatusBar();

        Assert.AreEqual(Color.red, gasBar.gasCollectBarImage.color, "Bar should be red at 0 progress");
        Assert.AreEqual("0/50", gasBar.textDisplay.text, "Text should show 0 progress");
        Assert.AreEqual(0f, gasBar.gasCollectBar.value, "Slider should be at 0");

        gasBar.UpdateIndicator(35);
        Assert.AreEqual(Color.yellow, gasBar.gasCollectBarImage.color, "Bar should be yellow at mid progress");
        Assert.AreEqual("35/50", gasBar.textDisplay.text, "Text should show 35 progress");
        Assert.AreEqual(35f, gasBar.gasCollectBar.value, "Slider should be at 35");

        gasBar.UpdateIndicator(15); 
        Assert.AreEqual(Color.green, gasBar.gasCollectBarImage.color, "Bar should be green when target met");
        Assert.AreEqual("50/50", gasBar.textDisplay.text, "Text should show 50 progress");
        Assert.AreEqual(50f, gasBar.gasCollectBar.value, "Slider should be at 50");

        yield return null;
    }
}
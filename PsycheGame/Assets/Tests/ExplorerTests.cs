using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

[TestFixture]
public class ExplorerPlayModeTests
{
    private LevelManager levelManager;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        Debug.Log("Load PlayMode Scene");
        SceneManager.LoadScene("ExplorationPlayMode");
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "ExplorationPlayMode");

        GameObject gameStateManager = GameObject.Find("GameStateManager");
        levelManager = gameStateManager.GetComponent<LevelManager>();

        PlayerPrefs.SetString("PlayerName", "TestPlayer");

        levelManager.StartGame();
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Setup complete");
    }

    [TearDown]
    public void TearDown(){
        PlayerPrefs.DeleteKey("PlayerName");
        Debug.Log("TearDown completed");
    }

   [UnityTest]
    public IEnumerator Test_AlertNotification_FlashesOnAsteroid()
    {
        ShipMovement ship = GameObject.FindObjectOfType<ShipMovement>();
        Assert.IsNotNull(ship, "ShipMovement not found in scene");
        Debug.Log($"Ship found at {ship.transform.position}");

        Transform proximityBoundaryTransform = ship.transform.Find("ProximityBoundary");
        Assert.IsNotNull(proximityBoundaryTransform, "ProximityBoundary not found under Ship");
        AlertNotification alert = proximityBoundaryTransform.GetComponent<AlertNotification>();
        Assert.IsNotNull(alert, "AlertNotification not found on ProximityBoundary");
        Debug.Log("ProximityBoundary and AlertNotification found");

        Collider2D boundaryCollider = proximityBoundaryTransform.GetComponent<Collider2D>();
        Assert.IsNotNull(boundaryCollider, "ProximityBoundary must have a Collider2D");
        Assert.IsTrue(boundaryCollider.isTrigger, "ProximityBoundary collider must be a trigger");
        Debug.Log($"ProximityBoundary collider: {boundaryCollider.GetType().Name}, size: {(boundaryCollider as BoxCollider2D)?.size}, trigger: {boundaryCollider.isTrigger}");

        if (alert.alertPanel == null)
        {
            Debug.LogWarning("AlertPanel is null, assigning a mock panel");
            alert.alertPanel = new GameObject("AlertPanelMock");
            alert.alertPanel.SetActive(false);
        }
        Debug.Log($"Alert panel: {alert.alertPanel.name}, initially active: {alert.alertPanel.activeSelf}");

        GameObject asteroidGO = GameObject.FindWithTag("Asteroid");
        if (asteroidGO == null)
        {
            Debug.LogWarning("No Asteroid found, creating one");
            asteroidGO = new GameObject("Asteroid");
            asteroidGO.tag = "Asteroid";
            asteroidGO.AddComponent<Asteroid>();
            BoxCollider2D asteroidCollider = asteroidGO.AddComponent<BoxCollider2D>();
            asteroidCollider.isTrigger = true;
            asteroidCollider.size = new Vector2(2f, 2f);
            Rigidbody2D asteroidRb = asteroidGO.AddComponent<Rigidbody2D>();
            asteroidRb.isKinematic = true;
        }
        Debug.Log($"Asteroid at {asteroidGO.transform.position}, tag: {asteroidGO.tag}");

        asteroidGO.transform.position = ship.transform.position;
        Debug.Log($"Asteroid moved to {asteroidGO.transform.position}");

        yield return new WaitForFixedUpdate();
        yield return new WaitForSeconds(0.6f); 

        Debug.Log($"After wait, alert panel active: {alert.alertPanel.activeSelf}");
        Assert.IsTrue(alert.alertPanel.activeSelf, "Alert panel should activate with nearby asteroid");

        yield return new WaitForSeconds(0.5f); 
        Debug.Log($"After flash wait, alert panel active: {alert.alertPanel.activeSelf}");
        Assert.IsFalse(alert.alertPanel.activeSelf, "Alert panel should flash off");

        asteroidGO.transform.position = ship.transform.position + Vector3.one * 10f;
        Debug.Log($"Asteroid moved to {asteroidGO.transform.position}");
        yield return new WaitForFixedUpdate();
        yield return new WaitForSeconds(0.1f);

        Debug.Log($"After move, alert panel active: {alert.alertPanel.activeSelf}");
        Assert.IsFalse(alert.alertPanel.activeSelf, "Alert panel should deactivate when asteroid leaves");
    }

        [UnityTest]
    public IEnumerator Test_DrillAsteroid_CollectsResources()
    {
        ShipMovement ship = GameObject.FindObjectOfType<ShipMovement>();
        Assert.IsNotNull(ship, "ShipMovement not found in scene");
        Debug.Log($"Ship found at {ship.transform.position}");

        DrillController drill = ship.GetComponentInChildren<DrillController>();
        Assert.IsNotNull(drill, "DrillController not found on Ship or its children");
        Debug.Log($"DrillController found at {drill.transform.position}");

        GameObject asteroidGO = GameObject.FindWithTag("Mineral");
        Assert.IsNotNull(asteroidGO, "No rare mineral asteroid with tag 'Mineral' found in scene");
        MineralCollection mineralCollection = asteroidGO.GetComponent<MineralCollection>();
        Assert.IsNotNull(mineralCollection, "Asteroid must have MineralCollection component");
        Debug.Log($"Asteroid found at {asteroidGO.transform.position}, metals: {mineralCollection.metals.Count}");

        if (mineralCollection.metals.Count == 0)
        {
            Debug.LogWarning("Asteroid has no metals, adding Titanium for test");
            mineralCollection.metals.Add(new Titanium(100));
        }
        int initialMetalAmount = mineralCollection.metals[0].Amount;
        Assert.Greater(initialMetalAmount, 0, "Asteroid must have some metal to drill");
        Debug.Log($"Initial metal amount: {initialMetalAmount}");

        Vector3 drillPosition = drill.transform.position;
        Vector3 shipForward = ship.transform.up;
        asteroidGO.transform.position = drillPosition + shipForward * 1f;
        Debug.Log($"Asteroid moved to {asteroidGO.transform.position}");

        Collider2D asteroidCollider = asteroidGO.GetComponent<Collider2D>();
        Assert.IsNotNull(asteroidCollider, "Asteroid must have a Collider2D");
        drill.OnTriggerEnter2D(asteroidCollider);
        drill.ActivateLaser();
        Debug.Log("Drill activated");

        yield return new WaitForSeconds(2.1f);

        int finalMetalAmount = mineralCollection.metals[0].Amount;
        Debug.Log($"Final metal amount: {finalMetalAmount}");
        Assert.Less(finalMetalAmount, initialMetalAmount, "Metal amount should decrease after drilling");

        float missionProgress = MissionState.Instance?.GetObjectiveProgress(MissionState.ObjectiveType.CollectRareMetals) ?? 0f;
        Debug.Log($"Mission progress: {missionProgress}");
        Assert.Greater(missionProgress, 0f, "Mission progress should increase");
    }

    [UnityTest]
    public IEnumerator Test_FuelBar_UpdatesAndFlashes()
    {
        FuelBar fuelBar = GameObject.FindObjectOfType<FuelBar>();
        Assert.IsNotNull(fuelBar.fuelBarImage, "FuelBar’s fuelBarImage is null");
        Assert.IsNotNull(fuelBar.textDisplay, "FuelBar’s textDisplay is null");
        Debug.Log("FuelBar found");

        ShipManager.Fuel = 75f;
        fuelBar.UpdateIndicator(ShipManager.Fuel);
        yield return null; 
        Debug.Log($"Fuel: {ShipManager.Fuel}, Color: {fuelBar.fuelBarImage.color}, Text: {fuelBar.textDisplay.text}");
        Assert.AreEqual(Color.green, fuelBar.fuelBarImage.color, "Fuel bar should be green at high fuel");
        Assert.AreEqual("75", fuelBar.textDisplay.text.Trim(), "Text should show current fuel");

        ShipManager.Fuel = 40f;
        fuelBar.UpdateIndicator(ShipManager.Fuel);
        yield return null;
        Debug.Log($"Fuel: {ShipManager.Fuel}, Color: {fuelBar.fuelBarImage.color}, Text: {fuelBar.textDisplay.text}");
        Assert.AreEqual(Color.yellow, fuelBar.fuelBarImage.color, "Fuel bar should be yellow at mid fuel");
        Assert.AreEqual("40", fuelBar.textDisplay.text.Trim(), "Text should show current fuel");

        ShipManager.Fuel = 10f;
        fuelBar.UpdateIndicator(ShipManager.Fuel);
        yield return new WaitForSeconds(0.2f); 
        Debug.Log($"Fuel: {ShipManager.Fuel}, Color: {fuelBar.fuelBarImage.color}, Text: {fuelBar.textDisplay.text}");
        Assert.AreEqual(Color.red, fuelBar.fuelBarImage.color, "Fuel bar should be red at low fuel");

        yield return new WaitForSeconds(0.5f); 
        Debug.Log($"Fuel: {ShipManager.Fuel}, Color: {fuelBar.fuelBarImage.color}, Text: {fuelBar.textDisplay.text}");
        Assert.AreEqual(Color.white, fuelBar.fuelBarImage.color, "Fuel bar should flash white at low fuel");
        Assert.AreEqual("10", fuelBar.textDisplay.text.Trim(), "Text should show current fuel");

        ShipManager.Fuel = 30f;
        fuelBar.UpdateIndicator(ShipManager.Fuel);
        yield return null;
        Debug.Log($"Fuel: {ShipManager.Fuel}, Color: {fuelBar.fuelBarImage.color}, Text: {fuelBar.textDisplay.text}");
        Assert.AreEqual(Color.yellow, fuelBar.fuelBarImage.color, "Fuel bar should return to yellow above low threshold");
        yield return null;
    }

    [UnityTest]
    public IEnumerator Test_GasCollectionStatusBar_UpdatesProgress()
    {
        GasCollectionStatusBar gasBar = GameObject.FindObjectOfType<GasCollectionStatusBar>();
        if (gasBar == null)
        {
            Debug.LogWarning("GasCollectionStatusBar not found, creating one");
            GameObject gasBarGO = new GameObject("GasBar");
            gasBar = gasBarGO.AddComponent<GasCollectionStatusBar>();
            gasBar.gasCollectionBarColor = new GameObject("BarColor");
            gasBar.gasCollectBarImage = gasBar.gasCollectionBarColor.AddComponent<Image>();
            gasBar.gasCollectBar = gasBarGO.AddComponent<Slider>();
            gasBar.textDisplay = new GameObject("Text").AddComponent<TextMeshProUGUI>();
        }

        MissionState.Instance.Initialize(new List<MissionState.MissionObjective>
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

        Object.DestroyImmediate(gasBar.gameObject);
    }

    [UnityTest]
    public IEnumerator Test_HeilumGas_Collection_ReducesFuel()
    {
        ShipMovement ship = GameObject.FindObjectOfType<ShipMovement>();
        Assert.IsNotNull(ship, "ShipMovement not found in scene");
        Debug.Log($"Ship found at {ship.transform.position}");

        HeilumGas heliumGas = GameObject.FindObjectOfType<HeilumGas>();
        Assert.IsNotNull(heliumGas, "No HeliumGas object found in scene");
        GameObject gasGO = heliumGas.gameObject;
        Debug.Log($"HeliumGas found at {gasGO.transform.position}");

        CircleCollider2D gasCollider = gasGO.GetComponent<CircleCollider2D>();
        Assert.IsNotNull(gasCollider, "HeliumGas must have a CircleCollider2D");

        float initialFuel = ShipManager.Fuel;
        Debug.Log($"Initial fuel: {initialFuel}");

        ship.transform.position = gasGO.transform.position;
        Debug.Log($"Ship moved to {ship.transform.position} for gas collection");

        yield return new WaitForFixedUpdate();
        yield return new WaitForSeconds(0.1f);

        float finalFuel = ShipManager.Fuel;
        Assert.Less(finalFuel, initialFuel, "HeliumGas should reduce fuel when collected");
    }

    [UnityTest]
    public IEnumerator Test_HydrogenGas_Collection_IncreasesFuel()
    {
        ShipMovement ship = GameObject.FindObjectOfType<ShipMovement>();
        Assert.IsNotNull(ship, "ShipMovement not found in scene");
        Debug.Log($"Ship found at {ship.transform.position}");

        HydrogenGas hydrogenGas = GameObject.FindObjectOfType<HydrogenGas>();
        Assert.IsNotNull(hydrogenGas, "No HydrogenGas object found in scene");
        GameObject gasGO = hydrogenGas.gameObject;
        Debug.Log($"HydrogenGas found at {gasGO.transform.position}");

        CircleCollider2D gasCollider = gasGO.GetComponent<CircleCollider2D>();
        Assert.IsNotNull(gasCollider, "HydrogenGas must have a CircleCollider2D");

        float initialFuel = ShipManager.Fuel;
        Debug.Log($"Initial fuel: {initialFuel}");

        ship.transform.position = gasGO.transform.position;
        Debug.Log($"Ship moved to {ship.transform.position} for gas collection");

        yield return new WaitForFixedUpdate();
        yield return new WaitForSeconds(0.1f);

        float finalFuel = ShipManager.Fuel;
        Assert.Greater(finalFuel, initialFuel, "HydrogenGas should increase fuel when collected");
    }

    [UnityTest]
    public IEnumerator Test_RareMetalCollectionStatusBar_UpdatesProgress()
    {
        Debug.Log("Test rare metal status bar starting...");
        RareMetalCollectionStatusBar metalBar = GameObject.FindObjectOfType<RareMetalCollectionStatusBar>();
        if (metalBar == null)
        {
            Debug.LogWarning("RareMetalCollectionStatusBar not found, creating one");
            GameObject metalBarGO = new GameObject("RareMetalBar");
            metalBar = metalBarGO.AddComponent<RareMetalCollectionStatusBar>();
            metalBar.rareMetalCollectionBarColor = new GameObject("BarColor");
            metalBar.rareMetalCollectBarImage = metalBar.rareMetalCollectionBarColor.AddComponent<Image>();
            metalBar.rareMetalCollectBar = metalBarGO.AddComponent<Slider>();
            metalBar.textDisplay = new GameObject("Text").AddComponent<TextMeshProUGUI>();
        }

        MissionState.Instance.Initialize(new List<MissionState.MissionObjective>
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

        Object.DestroyImmediate(metalBar.gameObject);
    }


    [UnityTest]
    public IEnumerator Test_PauseHandler_PauseGame()
    {
        Debug.Log("Test pause handler pause starting...");
        PauseHandler pauseHandler = GameObject.FindObjectOfType<PauseHandler>();
        Assert.IsNotNull(pauseHandler, "PauseHandler not found in scene");

        Assert.IsFalse(PauseHandler.IsGamePaused, "Game should start unpaused");
        Assert.AreEqual(1f, Time.timeScale, "Time should be normal");

        pauseHandler.PauseGame();
        yield return null;

        Assert.IsTrue(PauseHandler.IsGamePaused, "Game should be paused");
        Assert.AreEqual(0f, Time.timeScale, "Time should be frozen");
        Assert.IsTrue(pauseHandler.missionObjectivePanel.activeSelf, "Mission panel should be visible");
    }

    [UnityTest]
    public IEnumerator Test_PauseHandler_ResumeGame()
    {
        Debug.Log("Test pause handler resume starting...");
        PauseHandler pauseHandler = GameObject.FindObjectOfType<PauseHandler>();
        Assert.IsNotNull(pauseHandler, "PauseHandler not found in scene");
        pauseHandler.PauseGame();

        pauseHandler.ResumeGame();
        yield return null;

        Assert.IsFalse(PauseHandler.IsGamePaused, "Game should be unpaused");
        Assert.AreEqual(1f, Time.timeScale, "Time should be normal");
        Assert.IsFalse(pauseHandler.missionObjectivePanel.activeSelf, "Mission panel should be hidden");
    }

    [UnityTest]
    public IEnumerator Test_PauseHandler_QuitGame()
    {
        Debug.Log("Test pause handler quit starting...");
        PauseHandler pauseHandler = GameObject.FindObjectOfType<PauseHandler>();
        Assert.IsNotNull(pauseHandler, "PauseHandler not found in scene");
        pauseHandler.PauseGame();

        pauseHandler.QuitGame();
        yield return null;

        Assert.AreEqual("MainMenu", SceneManager.GetActiveScene().name, "Should load MainMenu scene");
        Assert.AreEqual(1f, Time.timeScale, "Time should be reset to normal");
    }

    [UnityTest]
    public IEnumerator Test_PauseHandler_RestartGame()
    {
        Debug.Log("Test pause handler restart starting...");
        PauseHandler pauseHandler = GameObject.FindObjectOfType<PauseHandler>();
        Assert.IsNotNull(pauseHandler, "PauseHandler not found in scene");
        string initialScene = SceneManager.GetActiveScene().name;

        pauseHandler.RestartGame();
        yield return new WaitForSecondsRealtime(0.1f);

        Assert.AreEqual(initialScene, SceneManager.GetActiveScene().name, "Scene should reload to current scene");
        Assert.AreEqual(0f, Time.timeScale, "Time should be paused at game start");
        pauseHandler = GameObject.FindObjectOfType<PauseHandler>();
        Assert.IsTrue(pauseHandler.missionObjectivePanel.activeSelf, "Mission panel should be displayed");
    }

    [UnityTest]
    public IEnumerator Test_PauseHandler_UpdateButtonText()
    {
        Debug.Log("Test pause handler update button text starting...");
        PauseHandler pauseHandler = GameObject.FindObjectOfType<PauseHandler>();
        Assert.IsNotNull(pauseHandler, "PauseHandler not found in scene");

        TextMeshProUGUI textComponent = pauseHandler.missionObjectivePanel.transform.Find("BeginResumeText")?.GetComponent<TextMeshProUGUI>();
        if (textComponent == null)
        {
            Debug.LogWarning("BeginResumeText not found, creating one");
            GameObject textObj = new GameObject("BeginResumeText");
            textObj.transform.SetParent(pauseHandler.missionObjectivePanel.transform);
            textComponent = textObj.AddComponent<TextMeshProUGUI>();
        }

        pauseHandler.UpdateButtonText(false);
        Assert.AreEqual("Begin", textComponent.text, "Button should say 'Begin' when unpaused");

        pauseHandler.UpdateButtonText(true);
        Assert.AreEqual("Resume", textComponent.text, "Button should say 'Resume' when paused");

        yield return null;
    }

    [UnityTest]
    public IEnumerator Test_UpdateMissionObjectives_UpdateUI()
    {
        Debug.Log("Test update mission objectives starting...");
        PauseHandler pauseHandler = GameObject.FindObjectOfType<PauseHandler>();
        Assert.IsNotNull(pauseHandler, "PauseHandler not found in scene");
        pauseHandler.PauseGame();
        UpdateMissionObjectives ui = GameObject.FindObjectOfType<UpdateMissionObjectives>();
        Assert.IsNotNull(ui, "UpdateMissionObjectives not found in scene");

        MissionState.Instance.Initialize(new List<MissionState.MissionObjective>
        {
            new MissionState.MissionObjective { objectiveType = MissionState.ObjectiveType.CollectRareMetals, targetAmount = 50, description = "Collect Rare Metals" },
            new MissionState.MissionObjective { objectiveType = MissionState.ObjectiveType.CollectGases, targetAmount = 30, description = "Collect Gases" }
        }, "TestLevel");

        ui.UpdateUI();
        yield return null;

        string expectedText = "Level: TestLevel\nCollect Rare Metals: 50\nCollect Gases: 30\n";
        Assert.AreEqual(expectedText, ui.textMeshProUGUI.text, "UI should display level and objectives");
        Assert.AreEqual(30, ui.textMeshProUGUI.fontSize, "Font size should be 30");
        Assert.AreEqual(TextAlignmentOptions.Center, ui.textMeshProUGUI.alignment, "Text should be centered");
    }
}
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
        SceneManager.LoadScene("ExplorationPlayMode");
        
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "ExplorationPlayMode");
        yield return new WaitForSeconds(0.5f);

        levelManager = GameObject.FindObjectOfType<LevelManager>();
        Assert.IsNotNull(levelManager, "LevelManager must be present in the test scene");

        PlayerNameHandler nameHandler = GameObject.FindObjectOfType<PlayerNameHandler>();
        Assert.IsNotNull(nameHandler, "PlayerNameHandler must be present in the test scene");
    
        GameObject panel = GameObject.Find("PlayerNamePanel");
        Assert.IsNotNull(panel, "Panel not found in scene");

        Transform playerNameTransform = panel.transform.Find("PlayerName");
        Assert.IsNotNull(playerNameTransform, "PlayerName not found under Panel");

        Transform nameInputFieldTransform = playerNameTransform.Find("NameInputField");
        Assert.IsNotNull(nameInputFieldTransform, "NameInputField not found under PlayerName");

        InputField nameInput = nameInputFieldTransform.GetComponent<InputField>();
        Assert.IsNotNull(nameInput, "InputField component not found on NameInputField");

        nameInput.text = "TestPlayer";

        Button beginButton = GameObject.Find("BeginButton")?.GetComponent<Button>();
        Assert.IsNotNull(beginButton, "BeginButton not found in scene");

        beginButton.onClick.Invoke();

        yield return new WaitUntil(() => !nameInput.gameObject.activeSelf); 
        yield return new WaitForSeconds(0.5f); 
        Assert.AreEqual("TestPlayer", PlayerPrefs.GetString("PlayerName", ""), "Player name should be saved in PlayerPrefs");
        }

    [TearDown]
    public void TearDown(){
        Debug.Log("TearDown completed");
    }

   [UnityTest]
    public IEnumerator Test_AlertNotification_FlashesOnAsteroid()
    {
        Debug.Log("Test alert notification starting...");

        GameObject alertGO = new GameObject("AlertNotification");
        AlertNotification alert = alertGO.AddComponent<AlertNotification>();
        alert.alertPanel = new GameObject("AlertPanel");
        alert.flashInterval = 0.5f;
        BoxCollider2D alertCollider = alertGO.AddComponent<BoxCollider2D>();
        alertCollider.isTrigger = true;
        alertCollider.size = new Vector2(2f, 2f);
        Rigidbody2D alertRb = alertGO.AddComponent<Rigidbody2D>();
        alertRb.gravityScale = 0f;
        alertRb.freezeRotation = true;
        alertRb.isKinematic = false;
        alertRb.velocity = Vector2.up * 0.01f;

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

        alertGO.transform.position = Vector3.zero;
        asteroidGO.transform.position = Vector3.zero;

 
        yield return new WaitForFixedUpdate();
        yield return new WaitForSeconds(0.6f);

        Assert.IsTrue(alert.alertPanel.activeSelf, "Alert panel should activate with nearby asteroid");
        yield return new WaitForSeconds(0.5f);
        Assert.IsFalse(alert.alertPanel.activeSelf, "Alert panel should flash off");

        alertRb.velocity = Vector2.zero;
        asteroidGO.transform.position = Vector3.one * 10f;
        yield return new WaitForFixedUpdate();

        Assert.IsFalse(alert.alertPanel.activeSelf, "Alert panel should deactivate when asteroid leaves");
        Debug.Log("Test completed");

        Object.DestroyImmediate(alertGO);
        Object.DestroyImmediate(asteroidGO);
    }

    [UnityTest]
    public IEnumerator Test_FuelBar_UpdatesAndFlashes()
    {
        Debug.Log("Test fuel bar starting...");
        FuelBar fuelBar = GameObject.FindObjectOfType<FuelBar>();
        Assert.IsNotNull(fuelBar, "FuelBar not found in scene");

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
        Debug.Log("Test drill asteroid starting...");
        DrillController drill = GameObject.FindObjectOfType<DrillController>();
        Assert.IsNotNull(drill, "DrillController not found in scene");

        GameObject asteroidGO = GameObject.FindWithTag("Asteroid");
        if (asteroidGO == null || !asteroidGO.TryGetComponent<MineralCollection>(out _))
        {
            Debug.LogWarning("No MineralCollection asteroid found, creating one");
            asteroidGO = new GameObject("Asteroid");
            MineralCollection asteroid = asteroidGO.AddComponent<MineralCollection>();
            asteroid.fragmentParticles = new GameObject("Fragments").AddComponent<ParticleSystem>();
            BoxCollider2D asteroidCollider = asteroidGO.AddComponent<BoxCollider2D>();
            asteroidCollider.isTrigger = true;
            asteroidCollider.size = new Vector2(2f, 2f);
            asteroidGO.AddComponent<Rigidbody2D>().isKinematic = true;
        }
        MineralCollection mineralCollection = asteroidGO.GetComponent<MineralCollection>();
        Assert.IsNotNull(mineralCollection, "Asteroid must have MineralCollection");

        drill.transform.position = Vector3.zero;
        asteroidGO.transform.position = Vector3.zero;

        drill.OnTriggerEnter2D(asteroidGO.GetComponent<BoxCollider2D>());
        drill.ActivateLaser();

        yield return new WaitForSeconds(2.1f);

        Assert.Less(mineralCollection.metals[0].Amount, 50, "Metal amount should decrease after drilling");
        Assert.Greater(MissionState.Instance.GetObjectiveProgress(MissionState.ObjectiveType.CollectRareMetals), 0, "Mission progress should increase");

        Object.DestroyImmediate(asteroidGO);
    }

    [UnityTest]
    public IEnumerator Test_HeilumGas_Collection_ReducesFuel()
    {
        Debug.Log("Test helium gas starting...");
        GameObject gasGO = new GameObject("HeliumGasTest");
        HeilumGas heliumGas = gasGO.AddComponent<HeilumGas>();
        BoxCollider2D gasCollider = gasGO.AddComponent<BoxCollider2D>();
        gasCollider.isTrigger = true;
        gasCollider.size = new Vector2(1f, 1f);
        ParticleSystem gasPS = gasGO.AddComponent<ParticleSystem>();
        ParticleSystemRenderer gasPSRenderer = gasPS.GetComponent<ParticleSystemRenderer>();
        if (gasPSRenderer.material == null)
            gasPSRenderer.material = new Material(Shader.Find("Particles/Standard Unlit"));

        ShipMovement ship = GameObject.FindObjectOfType<ShipMovement>();
        Assert.IsNotNull(ship, "ShipMovement not found in scene");
        gasPS.trigger.SetCollider(0, ship.GetComponent<BoxCollider2D>());

        float initialFuel = ShipManager.Fuel;
        ship.transform.position = Vector3.zero;
        gasGO.transform.position = Vector3.zero;

        yield return new WaitForFixedUpdate();
        heliumGas.OnCollect(10);

        Assert.AreEqual(initialFuel - 5f, ShipManager.Fuel, "HeilumGas should reduce fuel when collected");

        Object.DestroyImmediate(gasGO);
    }

    [UnityTest]
    public IEnumerator Test_HydrogenGas_Collection_IncreasesFuel()
    {
        Debug.Log("Test hydrogen gas starting...");
        GameObject gasGO = new GameObject("HydrogenGasTest");
        HydrogenGas hydrogenGas = gasGO.AddComponent<HydrogenGas>();
        BoxCollider2D gasCollider = gasGO.AddComponent<BoxCollider2D>();
        gasCollider.isTrigger = true;
        gasCollider.size = new Vector2(1f, 1f);
        ParticleSystem gasPS = gasGO.AddComponent<ParticleSystem>();
        ParticleSystemRenderer gasPSRenderer = gasPS.GetComponent<ParticleSystemRenderer>();
        if (gasPSRenderer.material == null)
            gasPSRenderer.material = new Material(Shader.Find("Particles/Standard Unlit"));

        ShipMovement ship = GameObject.FindObjectOfType<ShipMovement>();
        Assert.IsNotNull(ship, "ShipMovement not found in scene");
        gasPS.trigger.SetCollider(0, ship.GetComponent<BoxCollider2D>());

        float initialFuel = ShipManager.Fuel;
        ship.transform.position = Vector3.zero;
        gasGO.transform.position = Vector3.zero;

        yield return new WaitForFixedUpdate();
        hydrogenGas.OnCollect(10);

        Assert.AreEqual(initialFuel + 10f, ShipManager.Fuel, "HydrogenGas should increase fuel when collected");

        Object.DestroyImmediate(gasGO);
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
    public IEnumerator Test_GasCollectionStatusBar_UpdatesProgress()
    {
        Debug.Log("Test gas status bar starting...");
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
        Assert.AreEqual(1f, Time.timeScale, "Time should be normal");
        Assert.IsFalse(pauseHandler.missionObjectivePanel.activeSelf, "Mission panel should be hidden");
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
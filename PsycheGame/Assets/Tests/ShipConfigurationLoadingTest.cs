using NUnit.Framework;
using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

public class ShipConfigurationLoadingTest 
{
    private string scene_path = "Assets/Scenes/ExplorationLevel.unity";
    private ShipConfig defaultShipConfig;

    [SetUp]
    public void Setup() {
        EditorSceneManager.LoadScene(scene_path);

        defaultShipConfig = ScriptableObject.CreateInstance<ShipConfig>();

        defaultShipConfig.tetherConfig.resolution = 1;
        defaultShipConfig.tetherConfig.launchSpeed = 2;
        defaultShipConfig.tetherConfig.probeObjectDistance = 3;
        defaultShipConfig.tetherConfig.straightLineSpeed = 4;
        defaultShipConfig.tetherConfig.startWaveSize = 5;
        defaultShipConfig.tetherConfig.progressionSpeed = 6;

        defaultShipConfig.scanConfig.distance = 1;
        defaultShipConfig.scanConfig.resolution = 2;
        defaultShipConfig.scanConfig.arcAngle = 3;

        defaultShipConfig.shipMoveConfig.fuel = 1;
        defaultShipConfig.shipMoveConfig.health = 2;
        defaultShipConfig.shipMoveConfig.moveSpeed = 3;
        defaultShipConfig.shipMoveConfig.fuelConsumptionRate = 4;
        defaultShipConfig.shipMoveConfig.boostMultiplier = 5;
        defaultShipConfig.shipMoveConfig.boostChangeRate = 6;
    }

    [TearDown]
    public void TearDown()
    {
        EditorSceneManager.UnloadSceneAsync(scene_path);
    }

    [UnityTest]
    public IEnumerator PlayModeTestWithEnumeratorPasses()
    {
        Assert.NotNull(ShipManager.Ship);
        yield return null;
    }

    [Test]
    public void Json_ship_config_load_success()
    {
        // ignore error messages relating to ui components which
        // have not been initialized in this test
        LogAssert.ignoreFailingMessages = true;
        ShipConfigLoader loader = new GameObject("Loader", typeof(ShipConfigLoader)).GetComponent<ShipConfigLoader>();
        ShipConfig config = loader.LoadBuilderConfig(ShipConfigLoader.DATA_PATH, defaultShipConfig);
        Assert.NotNull(config);
    }

    [Test]
    public void Invaild_ship_config_path_load_default_config()
    {
        ShipConfigLoader loader = new ShipConfigLoader();
        ShipConfig config = loader.LoadBuilderConfig("Invaild Path", defaultShipConfig);
        LogAssert.Expect(LogType.Error, "ERROR: builder '.json' save data not found using default 'editor' variables for ship config");

        // Default editor config should have been loaded
        Assert.NotNull(config);
        Assert.AreEqual(config.tetherConfig.resolution, 1);
        Assert.AreEqual(config.tetherConfig.launchSpeed, 2);
        Assert.AreEqual(config.tetherConfig.probeObjectDistance, 3);
        Assert.AreEqual(config.tetherConfig.straightLineSpeed, 4);
        Assert.AreEqual(config.tetherConfig.startWaveSize, 5);
        Assert.AreEqual(config.tetherConfig.progressionSpeed, 6);

        Assert.AreEqual(config.scanConfig.distance, 1);
        Assert.AreEqual(config.scanConfig.resolution, 2);
        Assert.AreEqual(config.scanConfig.arcAngle, 3);

        Assert.AreEqual(config.shipMoveConfig.fuel, 1);
        Assert.AreEqual(config.shipMoveConfig.health, 2);
        Assert.AreEqual(config.shipMoveConfig.moveSpeed, 3);
        Assert.AreEqual(config.shipMoveConfig.fuelConsumptionRate, 4);
        Assert.AreEqual(config.shipMoveConfig.boostMultiplier, 5);
        Assert.AreEqual(config.shipMoveConfig.boostChangeRate, 6);
    }
}

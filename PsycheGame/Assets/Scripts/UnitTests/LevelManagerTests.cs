using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections.Generic;
using TMPro;

[TestFixture]
public class LevelManagerTests
{
    private GameObject levelManagerObject;
    private LevelManager levelManager;
    private GameObject missionTimerObject;
    private GameObject leaderBoardObject;
[SetUp]
    public void Setup()
    {
        levelManagerObject = new GameObject("LevelManager");
        levelManager = levelManagerObject.AddComponent<LevelManager>();

        missionTimerObject = new GameObject("MissionTimer");
        var missionTimer = missionTimerObject.AddComponent<MissionTimerMock>();
        levelManager.SetFieldValue("missionTimer", missionTimer);

        var objectives = new List<MissionState.MissionObjective>
        {
            new MissionState.MissionObjective { objectiveType = MissionState.ObjectiveType.CollectGases, targetAmount = 100 },
            new MissionState.MissionObjective { objectiveType = MissionState.ObjectiveType.CollectRareMetals, targetAmount = 50 }
        };
        MissionState.Instance.Initialize(objectives, "TestLevel");

        var levelConfig = ScriptableObject.CreateInstance<LevelConfig>();
        levelConfig.missionTimer = 180f;
        levelConfig.levelName = "TestLevel";
        levelConfig.gasSpawnerConfig = new ObjectSpawner.ObjectSpawnerConfig();
        levelConfig.rareMetalSpawnerConfig = new ObjectSpawner.ObjectSpawnerConfig();
        levelConfig.asteroidSpawnerConfig = new ObjectSpawner.ObjectSpawnerConfig();
        levelConfig.objectives = objectives;

        var boundingArea = new GameObject("BoundingArea");
        boundingArea.AddComponent<MeshRenderer>();

        var gasSpawner = new GameObject("GasSpawner").AddComponent<ObjectSpawnerMock>();
        gasSpawner.SetFieldValue("boundingArea", boundingArea);
        
        var rareMetalSpawner = new GameObject("RareMetalSpawner").AddComponent<ObjectSpawnerMock>();
        rareMetalSpawner.SetFieldValue("boundingArea", boundingArea);
        
        var asteroidSpawner = new GameObject("AsteroidSpawner").AddComponent<ObjectSpawnerMock>();
        asteroidSpawner.SetFieldValue("boundingArea", boundingArea);
        levelManager.SetFieldValue("gasSpawner", gasSpawner);
        levelManager.SetFieldValue("rareMetalSpawner", rareMetalSpawner);
        levelManager.SetFieldValue("asteroidSpawner", asteroidSpawner);
        levelManager.SetFieldValue("levels", new List<LevelConfig> { levelConfig });
        levelManager.SetFieldValue("currentLevelIndex", 0);
        levelManager.SetFieldValue("missionObjectivePanel", new GameObject("MissionObjectivePanel"));
        levelManager.SetFieldValue("loadingScreen", new GameObject("LoadingScreen"));
        levelManager.SetFieldValue("loadingTime", 0.1f);

        var missionObjectivePanel = new GameObject("MissionObjectivePanel");
        levelManager.SetFieldValue("missionObjectivePanel", missionObjectivePanel);

        var loadingScreen = new GameObject("LoadingScreen");
        var loadingText = new GameObject("LoadingText").AddComponent<TextMeshProUGUI>();
        levelManager.SetFieldValue("loadingScreen", loadingScreen);
        levelManager.SetFieldValue("loadingText", loadingText);
        levelManager.SetFieldValue("loadingTime", 0.1f);

        var leaderboardText = new GameObject("LeaderboardText").AddComponent<TextMeshProUGUI>();
        levelManager.SetFieldValue("leaderboardText", leaderboardText);

        leaderBoardObject = new GameObject("LeaderBoard");
        var leaderBoard = leaderBoardObject.AddComponent<LeaderBoardMock>();
        typeof(LeaderBoard).GetField("Instance", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
            ?.SetValue(null, leaderBoard);

        levelManager.Invoke("Awake");
        levelManager.SetFieldValue("missionTimeRemaining", levelConfig.missionTimer);
        Time.timeScale = 0f;

        LogAssert.ignoreFailingMessages = true;
    }
    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(levelManagerObject);
        Object.DestroyImmediate(missionTimerObject);
        typeof(MissionState).GetField("instance", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
            ?.SetValue(null, null);
        typeof(LeaderBoard).GetField("Instance", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
            ?.SetValue(null, null);
    }

    [Test]
    public void Singleton_Instance_SetsCorrectly()
    {
        var instance1 = LevelManager.Instance;
        var newObject = new GameObject("LevelManager2");
        newObject.AddComponent<LevelManager>();
        Assert.AreEqual(instance1, LevelManager.Instance);
        Object.DestroyImmediate(newObject);
    }

    [Test]
    public void CalculateScore_ReturnsCorrectValues()
    {
        float timeRemaining = 90f;
        float totalTime = 180f;
        int score = (int)levelManager.Invoke("CalculateScore", timeRemaining, totalTime);
        Assert.AreEqual(200, score);
    }
}

public class MissionTimerMock : MissionTimer
{
    public new void UpdateTimerUI(float timeRemaining)
    {
        // Mock implementation - do nothing
    }
}

public class ObjectSpawnerMock : ObjectSpawner
{
    public new void InitWithConfig(ObjectSpawnerConfig config)
    {
        // Mock implementation - do nothing
    }
}

public class LoadingProgressBarMock : LoadingProgressBar
{
    public float LastProgressValue { get; private set; }

    public new void UpdateProgress(float progress)
    {
        LastProgressValue = progress;
    }
}

public class LeaderBoardMock : LeaderBoard
{
    public bool SaveScoreCalled { get; private set; }

    public new void SaveScore(int levelScore, int level)
    {
        SaveScoreCalled = true;
    }
}

public static class TestExtensions
{
    public static void SetFieldValue<T>(this object obj, string fieldName, T value)
    {
        var field = obj.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        field?.SetValue(obj, value);
    }

    public static T GetFieldValue<T>(this object obj, string fieldName)
    {
        var field = obj.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return (T)field?.GetValue(obj);
    }

    public static object Invoke(this object obj, string methodName, params object[] args)
    {
        var method = obj.GetType().GetMethod(methodName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return method?.Invoke(obj, args);
    }
}
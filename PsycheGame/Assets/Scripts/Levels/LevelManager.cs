
using UnityEngine;

using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private List<LevelConfig> levels; 
    private int currentLevelIndex = 0;

    [SerializeField] private RareMetalAsteroidSpawner spawner;
    [SerializeField] private ObjectSpawner asteroidSpawner; 
    [SerializeField] private MissionTimer missionTimerUI;
    private MissionState missionState; 
    private float missionTimeRemaining;
    private float missionDuration = 180f;
    private bool isTimerRunning;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadLevel(currentLevelIndex);
    }

    private void UpdateMissionTimer()
    {
        missionTimeRemaining -= Time.deltaTime;
        missionTimeRemaining = Mathf.Max(missionTimeRemaining, 0);

        missionTimerUI.UpdateTimerUI(missionTimeRemaining);
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            UpdateMissionTimer();

            if (missionTimeRemaining <= 0)
            {
                EndLevel(false);
            }
        }

        if (missionState.IsMissionComplete)
        {
            EndLevel(true);
        }
    }

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex >= levels.Count)
        {
            Debug.Log("All levels completed!");
            return;
        }

        currentLevelIndex = levelIndex;
        LevelConfig config = levels[levelIndex];

        spawner.rareAsteroidCount = config.rareAsteroidCount;
        spawner.scaleMin = config.RMscaleMin;
        spawner.scaleMax = config.RMscaleMax;

        spawner.SpawnRareAsteroids();

        asteroidSpawner.spawnInterval = config.asteroidSpawnInterval;
        asteroidSpawner.objectLimit = config.asteroidObjectLimit;
        asteroidSpawner.initialPopulation = config.ateroidInitialPopulation;
        asteroidSpawner.scaleMax = config.asteroidScaleMax;
        asteroidSpawner.scaleMin = config.asteroidScaleMax;
        asteroidSpawner.velocityMax = config.asteroidVelocityMax;
        asteroidSpawner.velocityMin = config.asteroidVelocityMin;

        asteroidSpawner.Start();

        missionState.objectives = new List<MissionState.MissionObjective>(config.objectives);
        missionState.ResetObjectives();

        missionTimeRemaining = config.missionTimer;

        Debug.Log($"Loaded Level: {config.levelName}");
    }

    private void EndLevel(bool success)
    {
        if (success)
        {
            Debug.Log($"Level {currentLevelIndex} Complete!");
            LoadLevel(currentLevelIndex + 1);
        }
        else
        {
            Debug.Log($"Level {currentLevelIndex} Failed.");
            RestartLevel();
        }
    }

    private void RestartLevel()
    {
        LoadLevel(currentLevelIndex);
    }
}

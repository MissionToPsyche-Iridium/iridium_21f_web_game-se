
using UnityEngine;

using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private List<LevelConfig> levels; 
    private int currentLevelIndex = 0;

    [SerializeField] private RareMetalAsteroidSpawner spawner;
    [SerializeField] private ObjectSpawner asteroidSpawner; 
    private MissionState missionState; 
    private MissionTimer missionTimeLeft;


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

    private void Update()
    {
        if (missionTimeLeft.timeRemaining > 0)
        {
            missionTimeLeft.timeRemaining -= Time.deltaTime;
            if (missionTimeLeft.timeRemaining <= 0)
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

        missionTimeLeft.timeRemaining = config.missionTimer;

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

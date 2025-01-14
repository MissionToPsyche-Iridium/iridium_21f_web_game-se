
using UnityEngine;

using System.Collections.Generic;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private List<LevelConfig> levels; 
    private int currentLevelIndex = 0;

    [SerializeField] private RareMetalAsteroidSpawner spawner;
    [SerializeField] private ObjectSpawner asteroidSpawner; 
    private MissionState missionState; 
    private float missionTimeRemaining = 180f;
    private bool isTimerRunning = false;
    private bool isPaused = false;
    private MissionTimer missionTimer;

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
        missionState = MissionState.Instance;

        if (missionState == null)
        {
            Debug.LogError("Instance of MissionState is not set! MissionState must be initialized for use by LevelManager.");
            return;
        }
        LoadLevel(currentLevelIndex);
    }

    public void StartMissionTimer()
    {
        UpdateMissionTimer();
    }

    private void UpdateMissionTimer()
    {
        missionTimeRemaining -= Time.deltaTime;
        missionTimeRemaining = Mathf.Max(missionTimeRemaining, 0);

        missionTimer.UpdateTimerUI(missionTimeRemaining);
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

        if (MissionState.Instance.IsMissionComplete)
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

        MissionState.Instance.Initialize(config.objectives);

        missionTimeRemaining = config.missionTimer;

        Debug.Log($"Loaded Level: {config.levelName}");
    }

    public void PauseGame()
    {
        if (isPaused) return;

        isPaused = true;
        Time.timeScale = 0f; 
        Debug.Log("Game Paused");
    }

    public void ResumeGame()
    {
        if (!isPaused) return;

        isPaused = false;
        Time.timeScale = 1f;
        Debug.Log("Game Resumed");
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

    public void RestartLevel()
    {
        LoadLevel(currentLevelIndex);
    }
}

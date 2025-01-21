
using UnityEngine;

using System.Collections.Generic;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private List<LevelConfig> levels; 
    [SerializeField] private ObjectSpawner gasSpawner;
    [SerializeField] private RareMetalAsteroidSpawner spawner;
    [SerializeField] private ObjectSpawner asteroidSpawner;
    [SerializeField] private GameObject objectiveText;
    private MissionState missionState; 
    private float missionTimeRemaining = 180f;
    private bool isTimerRunning = false;
    private bool isPaused = false;
    private MissionTimer missionTimer;
    private int currentLevelIndex = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // NOTE: spawners should be:
        // 1. Disabled at 'Awake'
        // 2. Spawner configuration set
        // 3. Endabled
        // This ensures that ONLY values from the given level config are used to
        // spawn objects rather than also spawning objects using default inspector
        // values. These spawners are renabled in 'LoadLevel'
        gasSpawner.enabled = false;
        asteroidSpawner.enabled = false;
    }

    private void Start()
    {
        LevelConfig config = levels[currentLevelIndex];
        MissionState.Instance.Initialize(config.objectives);
        missionState = MissionState.Instance;

        if (missionState == null)
        {
            Debug.LogError("Instance of MissionState is not set! MissionState must be initialized for use by LevelManager.");
            return;
        }
        missionTimer = FindObjectOfType<MissionTimer>();
        if (missionTimer == null)
        {
            Debug.LogError("MissionTimer is not found in the scene! Make sure a GameObject with the MissionTimer component exists.");
            return;
        }
        LoadLevel(currentLevelIndex);
    }

    public void StartMissionTimer()
    {
        isTimerRunning = true;
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
        MissionState.Instance.Initialize(config.objectives);
        string objectivesText = "";

        foreach (var objective in missionState.Objectives)
        {
            Debug.Log("Mission Objectve: " + objective.description + " : " + objective.targetAmount);
            objectivesText += $"{config.levelName}\n";
            objectivesText += $"{objective.description}.\n Amount to gather: {objective.targetAmount}\n";
        }
        TextMeshPro text = GameObject.Find("MissionObjectiveContent").GetComponent<TextMeshPro>();
        text.SetText(objectivesText);

        spawner.rareAsteroidCount = config.rareAsteroidCount;
        spawner.scaleMin = config.RMscaleMin;
        spawner.scaleMax = config.RMscaleMax;

        spawner.SpawnRareAsteroids();

        asteroidSpawner.InitWithConfig(config.asteroidSpawnerConfig);
        asteroidSpawner.enabled = true;

        gasSpawner.InitWithConfig(config.gasSpawnerConfig);
        gasSpawner.enabled = true;


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

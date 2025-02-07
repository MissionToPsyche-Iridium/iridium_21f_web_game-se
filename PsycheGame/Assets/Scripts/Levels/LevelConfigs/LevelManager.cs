
using UnityEngine;

using System.Collections.Generic;
using TMPro;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public delegate void OnLevelLoadedHandler(LevelConfig config);
    public static event OnLevelLoadedHandler OnLevelLoaded;

    [SerializeField] private List<LevelConfig> levels; 
    [SerializeField] private ObjectSpawner gasSpawner;
    [SerializeField] private ObjectSpawner rareMetalSpawner;
    [SerializeField] private ObjectSpawner asteroidSpawner;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private float minLoadingTime = 2f;
    [SerializeField] private GameObject missionObjectivePanel;
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
        initializeByConfig(config);
        
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
    }

    private void initializeByConfig(LevelConfig config){
        MissionState.Instance.Initialize(config.objectives, config.levelName);
        missionState = MissionState.Instance;

        missionTimeRemaining = config.missionTimer;

        rareMetalSpawner.InitWithConfig(config.rareMetalSpawnerConfig);
        rareMetalSpawner.enabled = true;

        asteroidSpawner.InitWithConfig(config.asteroidSpawnerConfig);
        asteroidSpawner.enabled = true;

        gasSpawner.InitWithConfig(config.gasSpawnerConfig);
        gasSpawner.enabled = true;
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

        if (ShipManager.Health <= 0 || ShipManager.Fuel <= 0)
        {
            Debug.Log("Ship health or fuel reached 0. Game over!");
            EndLevel(false);
        }

        if (MissionState.Instance.IsMissionComplete)
        {
            Debug.Log("Level Complete - loading next level...");
            EndLevel(true);
        }
    }
 
    public void LoadLevel(int levelIndex)
    {
        StartCoroutine(LoadLevelAsync(levelIndex));
    }

    public IEnumerator LoadLevelAsync(int levelIndex){
        if (levelIndex >= levels.Count)
        {
            Debug.Log("All levels completed!");
            loadingText.text = "All levels completed!";
            loadingScreen.SetActive(true);
            EnableAllChildren(loadingScreen.transform);
            yield break;
        }

        if (loadingScreen != null){
            loadingText.text = "Loading Next Level...";
            loadingScreen.SetActive(true);
            EnableAllChildren(loadingScreen.transform);

        }
        LevelConfig config = levels[levelIndex];
        initializeByConfig(config);
        float startTime = Time.time;
        yield return new WaitForSeconds(minLoadingTime);
        
        OnLevelLoaded?.Invoke(config);

        Debug.Log($"Loaded Level: {config.levelName}");

        if (loadingScreen != null)
            loadingScreen.SetActive(false);

        missionObjectivePanel.SetActive(true);
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

    private void EnableAllChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            child.gameObject.SetActive(true);
        }
    }
}

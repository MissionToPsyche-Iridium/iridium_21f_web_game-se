using UnityEngine;

using System.Collections.Generic;
using TMPro;
using System.Collections;
using System;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public delegate void OnLevelLoadedHandler(LevelConfig config);
    public static event OnLevelLoadedHandler OnLevelLoaded;

    [SerializeField] private List<LevelConfig> levels;

    private int currentLevel = 1;
    private int currentLevelIndex = 0;

    [SerializeField] private ObjectSpawner gasSpawner;
    [SerializeField] private ObjectSpawner rareMetalSpawner;
    [SerializeField] private ObjectSpawner asteroidSpawner;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private TextMeshProUGUI leaderboardText;
    [SerializeField] private float loadingTime = 3f;
    [SerializeField] private GameObject missionObjectivePanel;
    [SerializeField] private LoadingProgressBar loadingProgressBar;
    private LeaderBoard leaderBoard;
    private MissionState missionState; 
    private float missionTimeRemaining = 180f;
    private bool isTimerRunning = false;
    private bool isPaused = false;
    private MissionTimer missionTimer;
    public static bool isLoading = false;
    private int playerScore = 0;
    public int PlayerScore => playerScore;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

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
        InitializeByConfig(levels[currentLevelIndex]);
        Time.timeScale = 0f;

        missionTimer = FindObjectOfType<MissionTimer>();
        if (missionTimer == null)
        {
            Debug.LogError("MissionTimer is missing in the scene.");
            return;
        }
    }

    public void StartGame()
    {
        isLoading = false;
        missionObjectivePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        StartMissionTimer();
    }

    public void SetLevels(List<LevelConfig> newLevels)
    {
        levels = newLevels;
    }

    public List<LevelConfig> GetLevels()
    {
        return levels;
    }

    public int getCurrentLevel(){
        return currentLevel;
    }

    private void InitializeByConfig(LevelConfig config){
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
        if (isLoading) return;

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
            Debug.Log("Level Complete - loading next level...");
            
            EndLevel(true);
        }
    }
 
    public void LoadLevel(bool success)
    {
        if (isLoading) return;
        StartCoroutine(LoadLevelAsync(success));
    }

    private IEnumerator LoadLevelAsync(bool success)
    {
        SetLoadingState(true);
        if(success){
            leaderboardText.text = leaderBoard.DisplayLeaderboardByLevel(currentLevel);
            loadingText.text = $"You scored: {playerScore}\n\n\n Loading Level {currentLevel + 1}...";
        } else {
            loadingText.text = $"Better luck this time! Loading Level {currentLevel}";
        }

        yield return new WaitForSeconds(0.5f);

        float elapsedTime = 0f;
        while (elapsedTime < loadingTime)
        {
            elapsedTime += Time.deltaTime;
            loadingProgressBar.UpdateProgress(Mathf.Clamp01(elapsedTime / loadingTime) * 100);
            yield return new WaitForEndOfFrame();
        }

        loadingProgressBar.UpdateProgress(100);
        yield return new WaitForSeconds(0.5f);

        if (success)
        {
            currentLevel++;
            currentLevelIndex++;
            if (currentLevelIndex < levels.Count)
            {
                InitializeByConfig(levels[currentLevelIndex]);
                OnLevelLoaded?.Invoke(levels[currentLevelIndex]);
            }
            else
            {
                Debug.Log("All levels completed.");
            }
        }
        else
        {
            InitializeByConfig(levels[currentLevelIndex]);
        }

        ShipManager.ResetShip();
        ShipConfig shipConfig = null;
        if (shipConfig != null) {
            ShipManager.SetShipConfig(shipConfig);
        } else {
            Debug.LogWarning("No ship configuration found for explorer level with 'levelIndex': " + currentLevelIndex + "\n"
                            + "Using default ship configuration with editor defaults");
        }

        yield return new WaitForSeconds(0.5f);

        SetLoadingState(false);
    }

    public void EndLevel(bool success)
    {
        if(isLoading) return;
        if (success)
        {
            leaderBoard = LeaderBoard.Instance;
            Debug.Log($"Level {currentLevel} Complete!");
            float totalTime = levels[currentLevelIndex].missionTimer;
            int pointsEarned = CalculateScore(missionTimeRemaining, totalTime);
            playerScore += pointsEarned;
            leaderBoard.SaveScore(playerScore, currentLevel); 
            LoadLevel(true);
        }
        else
        {
            Debug.Log($"Level {currentLevel} Failed.");
            RestartLevel();
        }
    }

    public void RestartLevel()
    {
        if (isLoading) return;
        ShipManager.ResetShip();
        StopAllCoroutines();
        LoadLevel(false);
    }

    private void SetLoadingState(bool state)
    {
        isLoading = state;
        loadingScreen.SetActive(state);
        ToggleChildren(loadingScreen.transform, state);
        missionObjectivePanel.SetActive(!state);
    }

    private void ToggleChildren(Transform parent, bool state)
    {
        foreach (Transform child in parent)
        {
            child.gameObject.SetActive(state);
        }
    }

    private int CalculateScore(float timeRemaining, float totalTime)
    {
        int basePoints = 100;

        float timeRatio = timeRemaining / totalTime;
        int timeBonus = Mathf.RoundToInt(timeRatio * 200);

        return basePoints + timeBonus;
    }
}

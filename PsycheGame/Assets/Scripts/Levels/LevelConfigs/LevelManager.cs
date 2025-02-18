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
    private int currentLevelIndex = 0;

    [SerializeField] private ObjectSpawner gasSpawner;
    [SerializeField] private ObjectSpawner rareMetalSpawner;
    [SerializeField] private ObjectSpawner asteroidSpawner;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private float loadingTime = 2f;
    [SerializeField] private GameObject missionObjectivePanel;
    [SerializeField] private ProgressBarWrapper progressBarWrapper;

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
        InitializeByConfig(config);

        missionTimer = FindObjectOfType<MissionTimer>();
        if (missionTimer == null)
        {
            Debug.LogError("MissionTimer is missing in the scene.");
            return;
        }
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
 
    public void LoadLevel(int levelIndex)
    {
        if (isLoading) return;
        if (levelIndex < 0 || levelIndex >= levels.Count)
        {
            Debug.LogWarning($"Invalid level index {levelIndex}. Returning to first level.");
            levelIndex = 0;
        }
        StartCoroutine(LoadLevelAsync(levelIndex));
    }

private IEnumerator LoadLevelAsync(int levelIndex)
    {
        SetLoadingState(true);
        loadingText.text = $"You scored: {playerScore}\n\nLoading Level {levelIndex + 1}...";

        yield return new WaitForSeconds(0.5f);

        float elapsedTime = 0f;
        while (elapsedTime < loadingTime)
        {
            elapsedTime += Time.deltaTime;
            progressBarWrapper.UpdateProgress(Mathf.Clamp01(elapsedTime / loadingTime) * 100);
            yield return null;
        }

        progressBarWrapper.UpdateProgress(100);
        yield return new WaitForSeconds(0.5f);

        currentLevelIndex = levelIndex;
        LevelConfig config = levels[levelIndex];
        InitializeByConfig(config);
        OnLevelLoaded?.Invoke(config);

        yield return new WaitForSeconds(0.5f);

        SetLoadingState(false);
    }

    private void EndLevel(bool success)
    {
        if(isLoading) return;
        if (success)
        {
            Debug.Log($"Level {currentLevelIndex} Complete!");
            float totalTime = levels[currentLevelIndex].missionTimer;
            int pointsEarned = CalculateScore(missionTimeRemaining, totalTime);
            playerScore += pointsEarned;
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
        if (isLoading) return;
        ShipManager.ResetShip();
        StopAllCoroutines();
        LoadLevel(currentLevelIndex);
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
